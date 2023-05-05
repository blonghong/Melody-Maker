Imports System.IO
Imports AxWMPLib

Public Class Form1
    Public track As MelodyTrack
    Public history As New List(Of MelodyTrack)()
    Public redohistory As New List(Of MelodyTrack)()
    Public ProjectFileName As String

    Dim save_as_dialog As New SaveFileDialog With {.Title = "Save ME2 Track Cache File", .Filter = "ME2 Track Cache Files|*.txt"}
    Dim open_dialog As New OpenFileDialog With {.Title = "Open ME2 Track Cache File", .Filter = "ME2 Track Cache Files|*.txt"}
    Public Sub undo()
        If history.Count = 0 Then Exit Sub
        redohistory.Insert(0, track.Clone())
        track = history(0).Clone()
        history.RemoveAt(0)

        LoadLists()
    End Sub
    Public Sub redo()
        If redohistory.Count = 0 Then Exit Sub
        history.Insert(0, track.Clone())
        track = redohistory(0).Clone()
        redohistory.RemoveAt(0)

        LoadLists()
    End Sub
    Public Sub addhistory()
        redohistory.Clear()
        history.Insert(0, track.Clone())
    End Sub
    Public Sub clearhistory()
        history.Clear()
        redohistory.Clear()
    End Sub
    Function ToSamples(seconds As Decimal) As Integer
        If track Is Nothing Then
            Throw New ArgumentException("Can't convert Seconds to Samples without a track loaded")
            Return 0
        End If
        Return seconds * (track.samples / track.duration)
    End Function
    Function ToSeconds(samples As Integer) As Decimal
        If track Is Nothing Then
            Throw New ArgumentException("Can't convert Samples to Seconds without a track loaded.")
            Return 0
        End If
        Return samples / (track.samples / track.duration)
    End Function
    Function QuarterBeat() As Decimal
        If track Is Nothing Then Return 0
        Return 60 / track.tempo * (track.samples / track.duration)
    End Function

    Dim rnd As New Random
    Public Function RandomColor() As Color
        Return Color.FromArgb(255, rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255))
    End Function
    ' Help reduce jittering/flickering in the form
    Protected Overrides ReadOnly Property CreateParams() As CreateParams
        Get
            Dim cp As CreateParams = MyBase.CreateParams
            cp.ExStyle = cp.ExStyle Or &H2000000
            Return cp
        End Get
    End Property
    Sub set_types()
        cmb_itype.Items.Clear()
        For Each itype In MelodyIntensity.TypeEnum
            cmb_itype.Items.Add(itype.Value)
        Next

        cmb_otype.Items.Clear()
        For Each otype In MelodyObstacle.TypeEnum
            cmb_otype.Items.Add(otype.Value)
        Next
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Application.CurrentCulture = New Globalization.CultureInfo("EN-US")

        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint, True)
        DoubleBuffered = True
        pnl_oeditor.Location = pnl_ieditor.Location
        set_recent_files()
        set_types()
    End Sub
    Private Sub Form1_DragDrop(sender As Object, e As DragEventArgs) Handles Me.DragDrop
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            Dim d = e.Data.GetData(DataFormats.FileDrop)
            If TypeOf d Is Array Then
                Dim files As String() = d
                If files.Length >= 1 Then
                    Dim f = files(0)
                    LoadFile(f)
                End If
            End If
        End If
    End Sub
    Sub clear_recent_files()
        If My.Settings.recent_files Is Nothing Then Exit Sub
        My.Settings.recent_files.Clear()
        My.Settings.Save()
        set_recent_files()
    End Sub
    Sub set_recent_files()
        men_open_recent.DropDownItems.Clear()
        If My.Settings.recent_files Is Nothing Then
            men_open_recent.Visible = False
            Exit Sub
        End If
        For Each f In My.Settings.recent_files
            Dim men As New ToolStripMenuItem With {.Text = Path.GetFileNameWithoutExtension(f)}
            AddHandler men.Click, Sub() If men_open_recent.DropDownItems.Contains(men) Then LoadFile(f)
            AddHandler men.MouseDown,
                Sub(s As Object, e As MouseEventArgs)
                    If e.Button = MouseButtons.Middle Then
                        While My.Settings.recent_files.Contains(f)
                            My.Settings.recent_files.Remove(f)
                        End While
                        My.Settings.Save()
                        set_recent_files()
                    End If
                End Sub
            men_open_recent.DropDownItems.Add(men)
        Next
        Dim clear As New ToolStripMenuItem With {.Text = "Clear"}
        AddHandler clear.Click, Sub() clear_recent_files()
        men_open_recent.DropDownItems.Add(clear)

        If men_open_recent.DropDownItems.Count > 1 Then
            men_open_recent.Visible = True
        Else
            men_open_recent.Visible = False
        End If
    End Sub

    Sub add_recent_file(f As String)
        If My.Settings.recent_files Is Nothing Then
            My.Settings.recent_files = New Specialized.StringCollection
        End If
        While My.Settings.recent_files.Contains(f)
            My.Settings.recent_files.Remove(f)
        End While
        My.Settings.recent_files.Insert(0, f)
        If My.Settings.recent_files.Count > 10 Then
            For i = 10 To My.Settings.recent_files.Count - 1
                My.Settings.recent_files.RemoveAt(i)
            Next
        End If
        My.Settings.Save()
        set_recent_files()
    End Sub
    Sub LoadFile(f As String)
        SetData(File.ReadAllText(f))
        ProjectFileName = f
        add_recent_file(f)
    End Sub
    Sub SetData(data As String)
        Try
            track = MelodyTrack.Parse(data)
        Catch ex As Exception
            MsgBox("Failed to parse this file as a MelodyTrack")
            Exit Sub
        End Try

        SetDataFromCurrentTrack()
    End Sub

    Sub SetDataFromCurrentTrack()
        clearhistory()

        UpdatingSelection = True
        txt_ver.Text = track.version
        txt_samples.Text = track.samples
        txt_dur.Text = track.duration
        txt_bpm.Text = track.tempo
        chk_34.Checked = track.is_3_4
        UpdatingSelection = False

        LoadLists()
        pnl_ieditor.Visible = False
        pnl_oeditor.Visible = False
    End Sub
    Function GetIntensityGroup(o As MelodyObstacle) As MelodyIntensity
        If track Is Nothing Then Return Nothing
        If track.intensities.Count = 0 Then Return Nothing

        Dim ints = track.intensities.Where(Function(x) x.time < o.time)

        Return ints.OrderByDescending(Function(x) x.time).FirstOrDefault()
    End Function
    Sub LoadLists(Optional RememberSelections As Boolean = False, Optional isel As Integer = -1, Optional osel As Integer = -1)
        If isel = -1 Then isel = lst_intensities.SelectedIndex
        If osel = -1 Then osel = lst_obstacles.SelectedIndex

        lst_intensities.Items.Clear()
        lst_obstacles.Items.Clear()
        lst_intensities.Colours.Clear()
        lst_obstacles.Colours.Clear()

        track.SortLists()

        Dim _i = 0
        For Each i In track.intensities
            lst_intensities.Items.Add(i.ToNiceString())
            lst_intensities.Colours.Add(_i, i.GetColour())
            _i += 1
        Next

        _i = 0
        For Each o In track.obstacles
            lst_obstacles.Items.Add(o.ToNiceString)
            Dim i = GetIntensityGroup(o)
            If i IsNot Nothing Then
                lst_obstacles.Colours.Add(_i, lst_intensities.Colours.Item(track.intensities.IndexOf(i)))
            End If
            _i += 1
        Next

        If RememberSelections Then
            Try
                lst_intensities.SelectedIndex = isel
                lst_obstacles.SelectedIndex = osel
            Catch ex As Exception
                MsgBox("Failed to remember selections..." + vbNewLine + ex.ToString())
            End Try
        End If
    End Sub

    Private Sub Form1_DragEnter(sender As Object, e As DragEventArgs) Handles Me.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Dim UpdatingSelection = False
    Private Sub lst_intensities_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lst_intensities.SelectedIndexChanged
        If lst_intensities.SelectedIndex = -1 Then
            pnl_ieditor.Visible = False
            Exit Sub
        End If
        lst_obstacles.SelectedIndex = -1

        If lst_intensities.SelectedIndices.Count > 1 Then
            pnl_ieditor.Visible = False
            Exit Sub
        End If

        Dim i = lst_intensities.SelectedIndex
        If track Is Nothing OrElse track.intensities.Count <= i Then Exit Sub
        Dim int = track.intensities(i)

        UpdatingSelection = True
        num_itime.Value = int.time

        If MelodyIntensity.TypeEnum.Keys.Contains(int.type) Then
            Dim itype = MelodyIntensity.TypeEnum.Item(int.type)
            If cmb_itype.Items.Contains(itype) Then
                cmb_itype.SelectedItem = itype
            Else
                'this should theoretically never happen, but just in case
                cmb_itype.Items.Add(itype)
                cmb_itype.SelectedItem = itype
            End If
        Else
            cmb_itype.Text = int.type
        End If

        num_idur.Value = int.trans_duration

        If Not String.IsNullOrWhiteSpace(int.has_a) Then
            txt_ia.Text = int.has_a
        Else
            txt_ia.Text = ""
        End If
        UpdatingSelection = False

        If Not String.IsNullOrWhiteSpace(AxWindowsMediaPlayer1.URL) Then
            AxWindowsMediaPlayer1.Ctlcontrols.stop()
            AxWindowsMediaPlayer1.Ctlcontrols.currentPosition = ToSeconds(int.time)
            AxWindowsMediaPlayer1.Ctlcontrols.play()
        End If

        pnl_ieditor.Visible = True
    End Sub
    Dim aaa = 0
    Private Sub lst_obstacles_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lst_obstacles.SelectedIndexChanged
        If lst_obstacles.SelectedIndex = -1 Then
            pnl_oeditor.Visible = False
            Exit Sub
        End If
        lst_intensities.SelectedIndex = -1

        If lst_obstacles.SelectedIndices.Count > 1 Then
            pnl_oeditor.Visible = False
            Exit Sub
        End If

        Dim i = lst_obstacles.SelectedIndex
        If track Is Nothing OrElse track.obstacles.Count <= i Then Exit Sub
        Dim o = track.obstacles(i)

        UpdatingSelection = True
        num_otime.Value = o.time

        If MelodyObstacle.TypeEnum.Keys.Contains(o.type) Then
            Dim otype = MelodyObstacle.TypeEnum.Item(o.type)
            If cmb_otype.Items.Contains(otype) Then
                cmb_otype.SelectedItem = otype
            Else
                'this should theoretically never happen, but just in case
                cmb_otype.Items.Add(otype)
                cmb_otype.SelectedItem = otype
            End If
        Else
            cmb_otype.Text = o.type
        End If

        If o.duration > -1 Then
            chk_odur.Checked = True
            num_odur.Value = o.duration
        Else
            chk_odur.Checked = False
            num_odur.Value = 0
        End If
        UpdatingSelection = False

        If Not String.IsNullOrWhiteSpace(AxWindowsMediaPlayer1.URL) Then
            AxWindowsMediaPlayer1.Ctlcontrols.stop()
            AxWindowsMediaPlayer1.Ctlcontrols.currentPosition = ToSeconds(o.time)
            AxWindowsMediaPlayer1.Ctlcontrols.play()
        End If



        pnl_oeditor.Visible = True
    End Sub

    Private Sub lst_intensities_MouseDown(sender As Object, e As MouseEventArgs) Handles lst_intensities.MouseDown
        If e.Button = MouseButtons.Left Then
            Try
                lst_intensities.SelectedIndex = lst_intensities.IndexFromPoint(e.Location)
            Catch ex As Exception
                lst_intensities.SelectedIndex = -1
            End Try
        End If
    End Sub

    Private Sub lst_obstacles_MouseDown(sender As Object, e As MouseEventArgs) Handles lst_obstacles.MouseDown
        If e.Button = MouseButtons.Left Then
            Try
                lst_obstacles.SelectedIndex = lst_obstacles.IndexFromPoint(e.Location)
            Catch ex As Exception
                lst_obstacles.SelectedIndex = -1
            End Try
        End If
    End Sub


    Private Sub lst_obstacles_KeyDown(sender As Object, e As KeyEventArgs) Handles lst_obstacles.KeyDown
        If e.KeyCode = Keys.Delete Then
            If MessageBox.Show("Are you sure you want to delete selected obstacles?", "Confirm Delete", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                addhistory()

                Dim ilist As New List(Of Integer)()
                For Each index In lst_obstacles.SelectedIndices
                    ilist.Add(index)
                Next
                ilist.Sort()

                For i = 0 To ilist.Count - 1
                    track.obstacles.RemoveAt(ilist(i) - i)
                Next

                LoadLists()
            End If
        End If
    End Sub
    Private Sub lst_intensities_KeyDown(sender As Object, e As KeyEventArgs) Handles lst_intensities.KeyDown
        If e.KeyCode = Keys.Delete Then
            If MessageBox.Show("Are you sure you want to delete selected intensities?", "Confirm Delete", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                addhistory()

                Dim ilist As New List(Of Integer)()
                For Each index In lst_intensities.SelectedIndices
                    ilist.Add(index)
                Next
                ilist.Sort()

                For i = 0 To ilist.Count - 1
                    track.intensities.RemoveAt(ilist(i) - i)
                Next

                LoadLists(True)
            End If
        End If
    End Sub

    Private Sub num_itime_ValueChanged(sender As Object, e As EventArgs) Handles num_itime.ValueChanged
        If UpdatingSelection Then Exit Sub
        addhistory()
        Dim i = track.intensities(lst_intensities.SelectedIndex)
        i.time = num_itime.Value
        track.SortLists()
        LoadLists(True, track.intensities.IndexOf(i))
    End Sub

    Private Sub cmb_itype_TextChanged(sender As Object, e As EventArgs) Handles cmb_itype.TextChanged
        If UpdatingSelection Then Exit Sub
        addhistory()

        Dim itype = cmb_itype.Text
        Dim i = MelodyIntensity.TypeEnum.Values.ToList().IndexOf(itype)
        If i <> -1 Then
            itype = MelodyIntensity.TypeEnum.Keys(i)
        End If

        track.intensities(lst_intensities.SelectedIndex).type = itype
        Dim sel = cmb_itype.SelectionStart
        LoadLists(True)
        cmb_itype.SelectionStart = sel
    End Sub

    Private Sub num_idur_ValueChanged(sender As Object, e As EventArgs) Handles num_idur.ValueChanged
        If UpdatingSelection Then Exit Sub
        addhistory()
        track.intensities(lst_intensities.SelectedIndex).trans_duration = num_idur.Value
        LoadLists(True)
    End Sub

    Private Sub txt_ia_TextChanged(sender As Object, e As EventArgs) Handles txt_ia.TextChanged
        If UpdatingSelection Then Exit Sub
        addhistory()
        track.intensities(lst_intensities.SelectedIndex).has_a = txt_ia.Text
        Dim sel = txt_ia.SelectionStart
        LoadLists(True)
        txt_ia.SelectionStart = sel
    End Sub

    Private Sub num_otime_ValueChanged(sender As Object, e As EventArgs) Handles num_otime.ValueChanged
        If UpdatingSelection Then Exit Sub
        addhistory()
        Dim o = track.obstacles(lst_obstacles.SelectedIndex)
        o.time = num_otime.Value
        track.SortLists()
        LoadLists(True, osel:=track.obstacles.IndexOf(o))
    End Sub

    Private Sub cmb_otype_TextChanged(sender As Object, e As EventArgs) Handles cmb_otype.TextChanged
        If UpdatingSelection Then Exit Sub
        addhistory()

        Dim otype = cmb_otype.Text
        Dim o = MelodyObstacle.TypeEnum.Values.ToList().IndexOf(otype)
        If o <> -1 Then
            otype = MelodyObstacle.TypeEnum.Keys(o)
        End If

        track.obstacles(lst_obstacles.SelectedIndex).type = otype
        Dim sel = cmb_otype.SelectionStart
        LoadLists(True)
        cmb_otype.SelectionStart = sel
    End Sub

    Private Sub chk_odur_CheckedChanged(sender As Object, e As EventArgs) Handles chk_odur.CheckedChanged
        If UpdatingSelection Then Exit Sub
        addhistory()
        If chk_odur.Checked Then
            track.obstacles(lst_obstacles.SelectedIndex).duration = num_odur.Value
        Else
            track.obstacles(lst_obstacles.SelectedIndex).duration = -1
        End If
        LoadLists(True)
    End Sub

    Private Sub num_odur_ValueChanged(sender As Object, e As EventArgs) Handles num_odur.ValueChanged
        If UpdatingSelection Then Exit Sub
        If chk_odur.Checked Then
            addhistory()
            track.obstacles(lst_obstacles.SelectedIndex).duration = num_odur.Value
            LoadLists(True)
        End If
    End Sub

    Sub UpdateAppDataFiles()
        men_open_appdata.DropDownItems.Clear()

        Dim appdata = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "Low"
        Dim p = Path.Combine(appdata, "Icetesy", "Melody's Escape 2", "Tracks Cache")
        If Not Directory.Exists(p) Then
            men_open_appdata.Visible = False
            Exit Sub
        End If
        men_open_appdata.Visible = True
        For Each f In Directory.GetFiles(p)
            Dim menu As New ToolStripMenuItem With {.Text = Path.GetFileNameWithoutExtension(f)}
            AddHandler menu.Click,
                    Sub()
                        LoadFile(f)
                    End Sub
            men_open_appdata.DropDownItems.Add(menu)
        Next
    End Sub

    Private Sub FileToolStripMenuItem_DropDownOpening(sender As Object, e As EventArgs) Handles FileToolStripMenuItem.DropDownOpening
        UpdateAppDataFiles()
    End Sub

    Private Sub men_exit_Click(sender As Object, e As EventArgs) Handles men_exit.Click
        Close()
    End Sub

    Private Sub men_add_beat_Click(sender As Object, e As EventArgs) Handles men_add_beat.Click
        If track Is Nothing Then
            MsgBox("No track created yet.")
            Exit Sub
        End If

        Dim num = InputBox("How many qbeats do you want? (4 for every beat)")
        Dim start = InputBox("When should it start? (In samples)")
        Dim pattern = InputBox("Pattern?")

        If String.IsNullOrWhiteSpace(pattern) Then pattern = "4"

        Dim pi = 0
        For i = 0 To num - 1
            Dim o As New MelodyObstacle With {
                .type = pattern(pi),
                .time = start + QuarterBeat() * i,
                .duration = -1
            }
            track.obstacles.Add(o)
            'lst_obstacles.Items.Add(o.ToNiceString())
            pi = (pi + 1) Mod pattern.Length
        Next
        LoadLists()
    End Sub
    Sub save()
        If track Is Nothing OrElse String.IsNullOrWhiteSpace(ProjectFileName) Then
            save_as()
            Exit Sub
        End If

        save_file(ProjectFileName)
    End Sub

    Sub save_file(f As String)
        File.WriteAllText(f, track.ToString())
    End Sub
    Sub save_as()
        If track Is Nothing Then
            MsgBox("There is nothing to save.")
            Exit Sub
        End If
        If save_as_dialog.ShowDialog() = DialogResult.OK Then
            save_file(save_as_dialog.FileName)
            ProjectFileName = save_as_dialog.FileName
            add_recent_file(ProjectFileName)
        End If
    End Sub
    Private Sub men_save_Click(sender As Object, e As EventArgs) Handles men_save.Click
        save()
    End Sub
    Private Sub men_save_as_Click(sender As Object, e As EventArgs) Handles men_save_as.Click
        save_as()
    End Sub

    Private Sub men_new_Click(sender As Object, e As EventArgs) Handles men_new.Click
        track = New MelodyTrack With {
            .version = "1.13"
        }
        SetDataFromCurrentTrack()
        ProjectFileName = Nothing
    End Sub
    Function EnsureTextBoxDigits(txt As TextBox, property_name As String, og_value As Object) As Boolean
        If Integer.TryParse(txt.Text, Nothing) Then Return True
        txt.Text = og_value
        MsgBox(String.Format("{0} must be a number (whole or decimal).", property_name))
        Return False
    End Function
    Function EnsureTextBoxDecimal(txt As TextBox, property_name As String, og_value As Object) As Boolean
        If Decimal.TryParse(txt.Text, Nothing) Then Return True
        txt.Text = og_value
        MsgBox(String.Format("{0} must be a number (whole or decimal).", property_name))
        Return False
    End Function

    Private Sub txt_ver_TextChanged(sender As Object, e As EventArgs) Handles txt_ver.TextChanged
        If UpdatingSelection Then Exit Sub
        If track Is Nothing Then Exit Sub
        If Not EnsureTextBoxDecimal(txt_ver, "Track Version", track.version) Then Exit Sub
        track.version = txt_ver.Text
    End Sub

    Private Sub txt_samples_TextChanged(sender As Object, e As EventArgs) Handles txt_samples.TextChanged
        If UpdatingSelection Then Exit Sub
        If track Is Nothing Then Exit Sub
        If Not EnsureTextBoxDigits(txt_samples, "Track Samples", track.samples) Then Exit Sub
        track.samples = txt_samples.Text
    End Sub

    Private Sub txt_dur_TextChanged(sender As Object, e As EventArgs) Handles txt_dur.TextChanged
        If UpdatingSelection Then Exit Sub
        If track Is Nothing Then Exit Sub
        If Not EnsureTextBoxDecimal(txt_dur, "Track Duration", track.duration) Then Exit Sub
        track.duration = txt_dur.Text
    End Sub

    Private Sub txt_bpm_TextChanged(sender As Object, e As EventArgs) Handles txt_bpm.TextChanged
        If UpdatingSelection Then Exit Sub
        If track Is Nothing Then Exit Sub
        If Not EnsureTextBoxDigits(txt_bpm, "Track Tempo (BPM)", track.tempo) Then Exit Sub
        track.tempo = txt_bpm.Text
    End Sub

    Private Sub chk_34_CheckedChanged(sender As Object, e As EventArgs) Handles chk_34.CheckedChanged
        If UpdatingSelection Then Exit Sub
        If track Is Nothing Then Exit Sub
        track.is_3_4 = chk_34.Checked
    End Sub

    Private Sub UndoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UndoToolStripMenuItem.Click
        undo()
    End Sub

    Private Sub RedoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RedoToolStripMenuItem.Click
        redo()
    End Sub

    Private Sub btn_tool_setslow_Click(sender As Object, e As EventArgs) Handles btn_tool_setslow.Click
        txt_ia.Text = "A"
    End Sub

    Private Sub btn_tool_slowclear_Click(sender As Object, e As EventArgs) Handles btn_tool_slowclear.Click
        txt_ia.Text = ""
    End Sub

    Private Sub btn_tool_itimeplus_Click(sender As Object, e As EventArgs) Handles btn_tool_itimeplus.Click
        num_itime.Value = Math.Min(num_itime.Maximum, num_itime.Value + QuarterBeat())
    End Sub

    Private Sub btn_tool_itimeminus_Click(sender As Object, e As EventArgs) Handles btn_tool_itimeminus.Click
        num_itime.Value = Math.Max(num_itime.Minimum, num_itime.Value - QuarterBeat())
    End Sub

    Private Sub btn_tool_idur10_Click(sender As Object, e As EventArgs) Handles btn_tool_idur10.Click
        num_idur.Value = 10
    End Sub

    Private Sub btn_tool_idur15_Click(sender As Object, e As EventArgs) Handles btn_tool_idur15.Click
        num_idur.Value = 15
    End Sub

    Private Sub btn_tool_idur20_Click(sender As Object, e As EventArgs) Handles btn_tool_idur20.Click
        num_idur.Value = 20
    End Sub

    Private Sub btn_tool_idur30_Click(sender As Object, e As EventArgs) Handles btn_tool_idur30.Click
        num_idur.Value = 30
    End Sub

    Private Sub btn_tool_idur50_Click(sender As Object, e As EventArgs) Handles btn_tool_idur50.Click
        num_idur.Value = 50
    End Sub

    Private Sub btn_tool_otimeplus_Click(sender As Object, e As EventArgs) Handles btn_tool_otimeplus.Click
        num_otime.Value = Math.Min(num_otime.Maximum, num_otime.Value + QuarterBeat())
    End Sub

    Private Sub btn_tool_otimeminus_Click(sender As Object, e As EventArgs) Handles btn_tool_otimeminus.Click
        num_otime.Value = Math.Max(num_otime.Minimum, num_otime.Value - QuarterBeat())
    End Sub

    Private Sub btn_tool_odur10_Click(sender As Object, e As EventArgs) Handles btn_tool_odur10.Click
        chk_odur.Checked = True
        num_odur.Value = 10
    End Sub

    Private Sub btn_tool_odur15_Click(sender As Object, e As EventArgs) Handles btn_tool_odur15.Click
        chk_odur.Checked = True
        num_odur.Value = 15
    End Sub

    Private Sub btn_tool_odur20_Click(sender As Object, e As EventArgs) Handles btn_tool_odur20.Click
        chk_odur.Checked = True
        num_odur.Value = 20
    End Sub

    Private Sub btn_tool_odur30_Click(sender As Object, e As EventArgs) Handles btn_tool_odur30.Click
        chk_odur.Checked = True
        num_odur.Value = 30
    End Sub

    Private Sub btn_tool_odur50_Click(sender As Object, e As EventArgs) Handles btn_tool_odur50.Click
        chk_odur.Checked = True
        num_odur.Value = 50
    End Sub

    Private Sub men_open_Click(sender As Object, e As EventArgs) Handles men_open.Click
        If open_dialog.ShowDialog() = DialogResult.OK Then
            LoadFile(open_dialog.FileName)
        End If
    End Sub

    Private Sub menu_add_intensity_Click(sender As Object, e As EventArgs) Handles menu_add_intensity.Click
        If track Is Nothing Then Exit Sub
        If track.intensities Is Nothing Then Exit Sub

        track.intensities.Add(New MelodyIntensity With {.type = "L", .time = track.intensities.Last().time + 1, .trans_duration = 20})
        LoadLists()
    End Sub

    Dim wmpTimer As Timer
    Private Sub btn_LoadAudio_Click(sender As Object, e As EventArgs) Handles btn_LoadAudio.Click
        Dim fn = InputBox("Filename")
        If Not File.Exists(fn) Then Exit Sub

        wmpTimer = New Timer With {.Interval = 100}
        AddHandler wmpTimer.Tick,
            Sub()
                txt_audio_samples.Text = ToSamples(AxWindowsMediaPlayer1.Ctlcontrols.currentPosition)
            End Sub
        AxWindowsMediaPlayer1.settings.autoStart = False
        AxWindowsMediaPlayer1.URL = fn
        AxWindowsMediaPlayer1.Ctlcontrols.play()

    End Sub

    Private Sub AxWindowsMediaPlayer1_StatusChange(sender As Object, e As EventArgs) Handles AxWindowsMediaPlayer1.StatusChange
        If AxWindowsMediaPlayer1.playState = WMPLib.WMPPlayState.wmppsPlaying Then
            wmpTimer.Start()
        Else
            wmpTimer.Stop()
        End If
    End Sub

    Private Sub btn_audio_stop_Click(sender As Object, e As EventArgs) Handles btn_audio_stop.Click
        AxWindowsMediaPlayer1.Ctlcontrols.stop()
    End Sub
End Class
