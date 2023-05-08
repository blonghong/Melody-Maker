Imports System.IO
Imports System.Environment
Public Class Form1
    Private ReadOnly TRACK As New MelodyTrack()
    Private ReadOnly HISTORY As New HistoryManager(Of MelodyTrack)(TRACK)
    Private FILENAME As String

    Private ReadOnly DIALOG_SAVE As New SaveFileDialog With {.Title = "Save ME2 TRACK Cache File", .Filter = "ME2 TRACK Cache Files|*.txt"}
    Private ReadOnly DIALOG_OPEN As New OpenFileDialog With {.Title = "Open ME2 TRACK Cache File", .Filter = "ME2 TRACK Cache Files|*.txt"}

    Public Const SAMPLES_PER_SECOND = 44100 / 1024
    Protected Overrides ReadOnly Property CreateParams() As CreateParams
        ' Seems to reduce draw flickering
        Get
            Dim cp As CreateParams = MyBase.CreateParams
            cp.ExStyle = cp.ExStyle Or &H2000000
            Return cp
        End Get
    End Property
    Public Sub UNDO()
        HISTORY.Undo(TRACK.Clone())
        PopulateData()
    End Sub
    Public Sub REDO()
        HISTORY.Redo(TRACK.Clone())
        PopulateData()
    End Sub
    Public Sub ADD_HISTORY(Description As String)
        HISTORY.Add(TRACK.Clone(), Description)
    End Sub
    Function ToSamples(seconds As Decimal) As Integer
        Return seconds * SAMPLES_PER_SECOND
    End Function
    Function ToSeconds(samples As Integer) As Decimal
        Return samples / SAMPLES_PER_SECOND
    End Function
    Function QBeat() As Decimal
        Return 60 / TRACK.Tempo * SAMPLES_PER_SECOND
    End Function

    Sub PopulateComboBoxTypes()
        Dim itypes = IntensityType.Select(Function(i) i.Value).ToArray()
        Dim otypes = ObstacleType.Select(Function(o) o.Value).ToArray()

        cmb_itype.Items.Clear()
        cmb_itypefrom.Items.Clear()
        cmb_otype.Items.Clear()

        cmb_itype.Items.AddRange(itypes)
        cmb_itypefrom.Items.AddRange(itypes)
        cmb_otype.Items.AddRange(otypes)
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Application.CurrentCulture = Globalization.CultureInfo.InvariantCulture

        SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint, True)
        pnl_ieditor.Dock = DockStyle.Fill
        pnl_oeditor.Dock = DockStyle.Fill
        RECENT_Load()
        PopulateComboBoxTypes()
        PopulateData()
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
    Sub RECENT_Clear()
        If My.Settings.recent_files Is Nothing Then Exit Sub
        My.Settings.recent_files.Clear()
        My.Settings.Save()
        RECENT_Load()
    End Sub
    Sub RECENT_Load()
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
                        RECENT_Load()
                    End If
                End Sub
            men_open_recent.DropDownItems.Add(men)
        Next
        Dim clear As New ToolStripMenuItem With {.Text = "Clear"}
        AddHandler clear.Click, Sub() RECENT_Clear()
        men_open_recent.DropDownItems.Add(clear)

        If men_open_recent.DropDownItems.Count > 1 Then
            men_open_recent.Visible = True
        Else
            men_open_recent.Visible = False
        End If
    End Sub

    Sub RECENT_Add(f As String)
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
        RECENT_Load()
    End Sub
    Sub LoadFile(f As String)
        SetData(File.ReadAllText(f))
        FILENAME = f
        RECENT_Add(f)
    End Sub
    Sub SetData(data As String)
        Try
            TRACK.Parse(data)
        Catch ex As Exception
            MsgBox("Failed to parse this file as a MelodyTrack")
            Throw ex
            Exit Sub
        End Try

        LoadData()
    End Sub

    Sub LoadData()
        HISTORY.Clear()

        lst_intensities.MelodyTrack = TRACK
        lst_obstacles.MelodyTrack = TRACK

        PopulateData()
    End Sub
    Sub PopulateData()
        UpdatingSelection = True
        txt_ver.Text = TRACK.Version
        txt_samples.Text = TRACK.Samples
        txt_dur.Text = TRACK.Duration
        txt_bpm.Text = TRACK.Tempo
        chk_34.Checked = TRACK.Is34TimeSignature
        UpdatingSelection = False

        pnl_ieditor.Visible = False
        pnl_oeditor.Visible = False

        PopulateLists()
    End Sub
    Sub PopulateLists(Optional RememberSelections As Boolean = False, Optional isel As Integer = -1, Optional osel As Integer = -1)
        ' this memory feature is a hacky solution
        ' ideally, we do not re-make the whole list on every edit, just edit the single listbox item.
        ' it shouldn't be too hard but i am quite lazy right now
        ' ironic that i went through the trouble of getting and setting the scroll position through wndproc
        If isel = -1 Then isel = lst_intensities.SelectedIndex
        If osel = -1 Then osel = lst_obstacles.SelectedIndex
        Dim iscroll = lst_intensities.VerticalScrollValue
        Dim oscroll = lst_obstacles.VerticalScrollValue

        TRACK.SortLists()
        lst_intensities.PopulateData()
        lst_obstacles.PopulateData()

        If RememberSelections Then
            Try
                ' set scroll first, so that if the new index is out of range, it will then scroll to it automatically
                lst_intensities.VerticalScrollValue = iscroll
                lst_obstacles.VerticalScrollValue = oscroll
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
        ' for some reason this isnt happenning automatically, even though it SHOULD be...? will have to look into it.
        pnl_oeditor.Visible = False

        If lst_intensities.SelectedIndices.Count > 1 Then
            pnl_ieditor.Visible = False
            Exit Sub
        End If

        Dim i = lst_intensities.SelectedIndex
        If TRACK.Intensities.Count <= i Then
            pnl_ieditor.Visible = False
            Exit Sub
        End If
        Dim int = TRACK.Intensities(i)

        ' Update Controls
        UpdatingSelection = True

        ' time
        num_itime.Value = int.Time

        ' type
        Dim ValidateType As New Action(Of Char, ComboBox)(
            Sub(t As Char, c As ComboBox)
                If IntensityType.Keys.Contains(t) Then
                    Dim itype = IntensityType.Item(t)
                    If c.Items.Contains(itype) Then
                        c.SelectedItem = itype
                    Else
                        'this should theoretically never happen, but just in case
                        c.Items.Add(itype)
                        c.SelectedItem = itype
                    End If
                Else
                    c.Text = t
                End If
            End Sub)
        ValidateType(int.Type, cmb_itype)


        ' duration
        num_idur.Value = int.TransDuration

        ' ME1 and ME2 specific
        If int.IsME1 Then
            ' Angel Jump is an obstacle in ME1
            chk_iangeljump.Visible = False
            ' ME1 has a From Type
            LBL_ITypeFrom.Visible = True
            cmb_itypefrom.Visible = True
            ValidateType(int.ME1_StartType, cmb_itypefrom)
        Else
            ' Angel Jump is an intensity in ME2
            chk_iangeljump.Checked = int.IsAngelJump
            chk_iangeljump.Visible = True
            ' There is no From Type in ME2
            LBL_ITypeFrom.Visible = False
            cmb_itypefrom.Visible = False
        End If

        UpdatingSelection = False
        ' Finished Updating

        ' temp music sync
        If Not String.IsNullOrWhiteSpace(AxWindowsMediaPlayer1.URL) Then
            AxWindowsMediaPlayer1.Ctlcontrols.stop()
            AxWindowsMediaPlayer1.Ctlcontrols.currentPosition = ToSeconds(int.Time)
            AxWindowsMediaPlayer1.Ctlcontrols.play()
        End If

        ' set editor as visible
        pnl_ieditor.Visible = True
    End Sub
    Private Sub lst_obstacles_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lst_obstacles.SelectedIndexChanged
        If lst_obstacles.SelectedIndex = -1 Then
            pnl_oeditor.Visible = False
            Exit Sub
        End If
        lst_intensities.SelectedIndex = -1
        ' for some reason this isnt happenning automatically, even though it SHOULD be...? will have to look into it.
        pnl_ieditor.Visible = False

        If lst_obstacles.SelectedIndices.Count > 1 Then
            pnl_oeditor.Visible = False
            Exit Sub
        End If

        Dim i = lst_obstacles.SelectedIndex
        If TRACK.Obstacles.Count <= i Then
            pnl_oeditor.Visible = False
            Exit Sub
        End If
        Dim o = TRACK.Obstacles(i)

        ' Updating Controls
        UpdatingSelection = True

        ' time
        num_otime.Value = o.Time

        ' type
        If ObstacleType.Keys.Contains(o.Type) Then
            Dim otype = ObstacleType.Item(o.Type)
            If cmb_otype.Items.Contains(otype) Then
                cmb_otype.SelectedItem = otype
            Else
                'this should theoretically never happen, but just in case
                cmb_otype.Items.Add(otype)
                cmb_otype.SelectedItem = otype
            End If
        Else
            cmb_otype.Text = o.Type
        End If

        ' is hold & hold duration
        If o.IsHold Then
            chk_odur.Checked = True
            num_odur.Value = o.HoldDuration
        Else
            chk_odur.Checked = False
            num_odur.Value = 0
        End If

        ' ME1 uses type A to determine angel jump, it is not an additional property

        UpdatingSelection = False
        ' Finished Updating

        ' temp music sync
        If Not String.IsNullOrWhiteSpace(AxWindowsMediaPlayer1.URL) Then
            AxWindowsMediaPlayer1.Ctlcontrols.stop()
            AxWindowsMediaPlayer1.Ctlcontrols.currentPosition = ToSeconds(o.Time)
            AxWindowsMediaPlayer1.Ctlcontrols.play()
        End If

        ' set editor as visible
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
                Dim ilist As New List(Of Integer)()
                For Each index In lst_obstacles.SelectedIndices
                    ilist.Add(index)
                Next
                ilist.Sort()

                ADD_HISTORY(String.Format("Delete {0} Obstacle{1}", ilist.Count, If(ilist.Count = 1, "", "s")))

                For i = 0 To ilist.Count - 1
                    TRACK.Obstacles.RemoveAt(ilist(i) - i)
                Next

                PopulateLists()
            End If
        End If
    End Sub
    Private Sub lst_intensities_KeyDown(sender As Object, e As KeyEventArgs) Handles lst_intensities.KeyDown
        If e.KeyCode = Keys.Delete Then
            If MessageBox.Show("Are you sure you want to delete selected intensities?", "Confirm Delete", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                Dim ilist As New List(Of Integer)()
                For Each index In lst_intensities.SelectedIndices
                    ilist.Add(index)
                Next
                ilist.Sort()

                ADD_HISTORY(String.Format("Delete {0} Intensit{1}", ilist.Count, If(ilist.Count = 1, "y", "ies")))

                For i = 0 To ilist.Count - 1
                    TRACK.Intensities.RemoveAt(ilist(i) - i)
                Next

                PopulateLists(True)
            End If
        End If
    End Sub

    Private Sub num_itime_ValueChanged(sender As Object, e As EventArgs) Handles num_itime.ValueChanged
        If UpdatingSelection Then Exit Sub
        ADD_HISTORY("Change Intensity Time")
        Dim i = TRACK.Intensities(lst_intensities.SelectedIndex)
        i.Time = num_itime.Value
        TRACK.SortLists()
        PopulateLists(True, TRACK.Intensities.IndexOf(i))
    End Sub

    Private Sub cmb_itype_TextChanged(sender As Object, e As EventArgs) Handles cmb_itype.TextChanged
        If UpdatingSelection Then Exit Sub
        ADD_HISTORY("Change Intensity Type")

        Dim itype = MelodyIntensity.ValidateType(cmb_itype.Text)

        TRACK.Intensities(lst_intensities.SelectedIndex).Type = itype
        Dim sel = cmb_itype.SelectionStart
        PopulateLists(True)
        cmb_itype.SelectionStart = sel
    End Sub

    Private Sub cmb_itypefrom_TextChanged(sender As Object, e As EventArgs) Handles cmb_itypefrom.TextChanged
        If UpdatingSelection Then Exit Sub
        ADD_HISTORY("Change Intensity From Type")

        Dim itype = MelodyIntensity.ValidateType(cmb_itypefrom.Text)
        TRACK.Intensities(lst_intensities.SelectedIndex).ME1_StartType = itype
        Dim sel = cmb_itypefrom.SelectionStart
        PopulateLists(True)
        cmb_itypefrom.SelectionStart = sel
    End Sub

    Private Sub num_idur_ValueChanged(sender As Object, e As EventArgs) Handles num_idur.ValueChanged
        If UpdatingSelection Then Exit Sub
        ADD_HISTORY("Change Intensity Duration")
        TRACK.Intensities(lst_intensities.SelectedIndex).TransDuration = num_idur.Value
        PopulateLists(True)
    End Sub

    Private Sub chk_iangeljump_CheckedChanged(sender As Object, e As EventArgs) Handles chk_iangeljump.CheckedChanged
        If UpdatingSelection Then Exit Sub
        ADD_HISTORY("Toggle Intensity Angel Jump")
        TRACK.Intensities(lst_intensities.SelectedIndex).IsAngelJump = chk_iangeljump.Checked
        PopulateLists(True)
    End Sub

    Private Sub num_otime_ValueChanged(sender As Object, e As EventArgs) Handles num_otime.ValueChanged
        If UpdatingSelection Then Exit Sub
        ADD_HISTORY("Change Obstacle Time")
        Dim o = TRACK.Obstacles(lst_obstacles.SelectedIndex)
        o.Time = num_otime.Value
        TRACK.SortLists()
        PopulateLists(True, osel:=TRACK.Obstacles.IndexOf(o))
    End Sub

    Private Sub cmb_otype_TextChanged(sender As Object, e As EventArgs) Handles cmb_otype.TextChanged
        If UpdatingSelection Then Exit Sub
        ADD_HISTORY("Change Obstacle Type")

        Dim otype = MelodyObstacle.ValidateType(cmb_otype.Text)

        TRACK.Obstacles(lst_obstacles.SelectedIndex).Type = otype
        Dim sel = cmb_otype.SelectionStart
        PopulateLists(True)
        cmb_otype.SelectionStart = sel
    End Sub

    Private Sub chk_odur_CheckedChanged(sender As Object, e As EventArgs) Handles chk_odur.CheckedChanged
        If UpdatingSelection Then Exit Sub
        ADD_HISTORY("Toggle Obstacle Hold Duration")
        If chk_odur.Checked Then
            TRACK.Obstacles(lst_obstacles.SelectedIndex).IsHold = True
            TRACK.Obstacles(lst_obstacles.SelectedIndex).HoldDuration = num_odur.Value
        Else
            TRACK.Obstacles(lst_obstacles.SelectedIndex).IsHold = False
        End If
        PopulateLists(True)
    End Sub

    Private Sub num_odur_ValueChanged(sender As Object, e As EventArgs) Handles num_odur.ValueChanged
        If UpdatingSelection Then Exit Sub
        If chk_odur.Checked Then
            ADD_HISTORY("Change Obstacle Hold Duration")
            TRACK.Obstacles(lst_obstacles.SelectedIndex).HoldDuration = num_odur.Value
            PopulateLists(True)
        End If
    End Sub

    Sub AddTrackCacheFiles(MenuItem As ToolStripMenuItem, Dir As String)
        MenuItem.DropDownItems.Clear()
        If Directory.Exists(Dir) Then
            Dim files = Directory.GetFiles(Dir)
            If files.Length > 0 Then MenuItem.Visible = True
            For Each f In Directory.GetFiles(Dir)
                Dim menu As New ToolStripMenuItem With {.Text = Path.GetFileNameWithoutExtension(f)}
                AddHandler menu.Click,
                    Sub()
                        LoadFile(f)
                    End Sub
                MenuItem.DropDownItems.Add(menu)
            Next
        Else
            MenuItem.Visible = False
        End If
    End Sub
    Sub UpdateAppDataFiles()
        Dim appdata = GetFolderPath(SpecialFolder.LocalApplicationData) + "Low"
        Dim p = Path.Combine(appdata, "Icetesy", "Melody's Escape 2", "Tracks Cache")
        AddTrackCacheFiles(men_open_appdata, p)
        appdata = GetFolderPath(SpecialFolder.ApplicationData)
        p = Path.Combine(appdata, "MelodyEscape", "TracksCache")
        AddTrackCacheFiles(men_open_appdata_me1, p)
    End Sub
    Private Sub FileToolStripMenuItem_DropDownOpening(sender As Object, e As EventArgs) Handles MenuFile.DropDownOpening
        UpdateAppDataFiles()
    End Sub

    Private Sub men_exit_Click(sender As Object, e As EventArgs) Handles men_exit.Click
        Close()
    End Sub

    Private Sub men_add_beat_Click(sender As Object, e As EventArgs) Handles men_add_beat.Click
        Dim Queries = {
            "How many qbeats is this beat playing for? 4 is 1 beat.",
            "How many obstacles do you want? (make this the same as before for a qbeat pattern)",
            "When should it start? (In samples)",
            "Pattern? Leave Blank will CANCEL, type 8 for default"
        }
        Dim Answers As New List(Of String)()
        For Each q In Queries
            Dim a = InputBox(q)
            If String.IsNullOrWhiteSpace(a) Then Exit Sub
            Answers.Add(a)
        Next
        Dim length = Answers(0)
        Dim num = Answers(1)
        Dim start = Answers(2)
        Dim pattern = Answers(3)

        Dim beat_dist = length / num * QBeat()

        Dim pi = 0
        For i = 0 To num - 1
            Dim o As New MelodyObstacle With {
                .Type = pattern(pi),
                .Time = start + beat_dist * i
            }
            TRACK.Obstacles.Add(o)
            pi = (pi + 1) Mod pattern.Length
        Next
        PopulateLists()
    End Sub
    Sub save()
        If String.IsNullOrWhiteSpace(FILENAME) Then
            save_as()
            Exit Sub
        End If

        save_file(FILENAME)
    End Sub

    Sub save_file(f As String)
        File.WriteAllText(f, TRACK.ToString())
    End Sub
    Sub save_as()
        If DIALOG_SAVE.ShowDialog() = DialogResult.OK Then
            save_file(DIALOG_SAVE.FileName)
            FILENAME = DIALOG_SAVE.FileName
            RECENT_Add(FILENAME)
        End If
    End Sub
    Private Sub men_save_Click(sender As Object, e As EventArgs) Handles men_save.Click
        save()
    End Sub
    Private Sub men_save_as_Click(sender As Object, e As EventArgs) Handles men_save_as.Click
        save_as()
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
        If Not EnsureTextBoxDecimal(txt_ver, "TRACK Version", TRACK.Version) Then Exit Sub
        ADD_HISTORY("Change Version")
        TRACK.Version = txt_ver.Text
    End Sub

    Private Sub txt_samples_TextChanged(sender As Object, e As EventArgs) Handles txt_samples.TextChanged
        If UpdatingSelection Then Exit Sub
        If Not EnsureTextBoxDigits(txt_samples, "TRACK Samples", TRACK.Samples) Then Exit Sub
        TRACK.Samples = txt_samples.Text
    End Sub

    Private Sub txt_dur_TextChanged(sender As Object, e As EventArgs) Handles txt_dur.TextChanged
        If UpdatingSelection Then Exit Sub
        If Not EnsureTextBoxDecimal(txt_dur, "TRACK Duration", TRACK.Duration) Then Exit Sub
        TRACK.Duration = txt_dur.Text
    End Sub

    Private Sub txt_bpm_TextChanged(sender As Object, e As EventArgs) Handles txt_bpm.TextChanged
        If UpdatingSelection Then Exit Sub
        If Not EnsureTextBoxDigits(txt_bpm, "TRACK Tempo (BPM)", TRACK.Tempo) Then Exit Sub
        TRACK.Tempo = txt_bpm.Text
    End Sub

    Private Sub chk_34_CheckedChanged(sender As Object, e As EventArgs) Handles chk_34.CheckedChanged
        If UpdatingSelection Then Exit Sub
        TRACK.Is34TimeSignature = chk_34.Checked
    End Sub

    Private Sub MenuEdit_Undo_Click(sender As Object, e As EventArgs) Handles MenuEdit_Undo.Click
        UNDO()
    End Sub

    Private Sub MenuEdit_Redo_Click(sender As Object, e As EventArgs) Handles MenuEdit_Redo.Click
        REDO()
    End Sub

    Private Sub btn_tool_itimeplus_Click(sender As Object, e As EventArgs) Handles btn_tool_itimeplus.Click
        num_itime.Value = Math.Min(num_itime.Maximum, num_itime.Value + QBeat())
    End Sub

    Private Sub btn_tool_itimeminus_Click(sender As Object, e As EventArgs) Handles btn_tool_itimeminus.Click
        num_itime.Value = Math.Max(num_itime.Minimum, num_itime.Value - QBeat())
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
        num_otime.Value = Math.Min(num_otime.Maximum, num_otime.Value + QBeat())
    End Sub

    Private Sub btn_tool_otimeminus_Click(sender As Object, e As EventArgs) Handles btn_tool_otimeminus.Click
        num_otime.Value = Math.Max(num_otime.Minimum, num_otime.Value - QBeat())
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
        If DIALOG_OPEN.ShowDialog() = DialogResult.OK Then
            LoadFile(DIALOG_OPEN.FileName)
        End If
    End Sub

    Private Sub menu_add_intensity_Click(sender As Object, e As EventArgs) Handles menu_add_intensity.Click
        TRACK.Intensities.Add(New MelodyIntensity With {.Type = "L", .Time = TRACK.Intensities.Last().Time + 1, .TransDuration = 20})
        PopulateLists()
    End Sub

    Dim wmpTimer As Timer
    Sub LoadMusic(Filename As String)
        wmpTimer = New Timer With {.Interval = 100}
        AddHandler wmpTimer.Tick,
            Sub()
                'txt_audio_samples.Text = ToSamples(AxWindowsMediaPlayer1.Ctlcontrols.currentPosition)
            End Sub
        AxWindowsMediaPlayer1.settings.autoStart = False
        AxWindowsMediaPlayer1.URL = Filename
        AxWindowsMediaPlayer1.Ctlcontrols.play()
    End Sub
    Sub UnloadMusic()
        AxWindowsMediaPlayer1.Ctlcontrols.stop()
        AxWindowsMediaPlayer1.URL = ""
        AxWindowsMediaPlayer1.currentPlaylist.clear()
        wmpTimer = Nothing
    End Sub
    Private Sub AxWindowsMediaPlayer1_StatusChange(sender As Object, e As EventArgs) Handles AxWindowsMediaPlayer1.StatusChange
        If AxWindowsMediaPlayer1.playState = WMPLib.WMPPlayState.wmppsPlaying Then
            wmpTimer.Start()
        Else
            wmpTimer.Stop()
        End If
    End Sub
    Private Sub MenuEdit_DropDownOpening(sender As Object, e As EventArgs) Handles MenuEdit.DropDownOpening
        Dim u = HISTORY.GetUndoDescriptions()
        Dim r = HISTORY.GetRedoDescriptions()
        If u.Length > 0 Then
            MenuEdit_Undo.Text = String.Format("Undo '{0}'", u(0))
            MenuEdit_Undo.Enabled = True
        Else
            MenuEdit_Undo.Text = "Undo"
            MenuEdit_Undo.Enabled = False
        End If
        If r.Length > 0 Then
            MenuEdit_Redo.Text = String.Format("Redo '{0}'", r(0))
            MenuEdit_Redo.Enabled = True
        Else
            MenuEdit_Redo.Text = "Redo"
            MenuEdit_Redo.Enabled = False
        End If
    End Sub

    Private Sub MenuMusic_SmartOpen_Click(sender As Object, e As EventArgs) Handles MenuMusic_SmartOpen.Click
        Dim song = Path.GetFileNameWithoutExtension(FILENAME)
        If song.EndsWith("_2") Then song = song.Substring(0, song.Length - 2)
        If song.StartsWith("EP") Then song = song.Substring(5, song.Length - 5)
        If song.StartsWith("TEMPO") Then song = song.Substring(8, song.Length - 5)

        Dim FoundFile As String = ""
        Dim SearchDirectory As New Action(Of String)(
            Sub(SearchString As String)
                Dim files As String()
                Try
                    files = Directory.GetFiles(SearchString)
                Catch ex As Exception
                    Exit Sub
                End Try
                Dim filtered = files.Where(Function(a) a.Contains(song) AndAlso Not a.ToLower().EndsWith(".txt") AndAlso Not a.ToLower().EndsWith(".track")).ToArray()
                If filtered.Length > 0 Then
                    FoundFile = filtered(0)
                    Exit Sub
                End If
                For Each d In Directory.GetDirectories(SearchString)
                    SearchDirectory(d)
                Next
            End Sub)

        Dim dir = GetFolderPath(SpecialFolder.MyMusic)
        If Directory.Exists(dir) Then
            SearchDirectory(dir)

        End If

        If Not String.IsNullOrWhiteSpace(FoundFile) AndAlso File.Exists(FoundFile) Then
            LoadMusic(FoundFile)
            Exit Sub
        End If

        MsgBox("Song wasn't found in your Music folder, we're going to scan your whole god dang file system now heheheeh")
        Dim FullSearch As New Action(
            Sub()
                For Each d In My.Computer.FileSystem.Drives
                    SearchDirectory(d.Name)
                Next
            End Sub)

        Dim bgw As New ComponentModel.BackgroundWorker
        AddHandler bgw.DoWork, Sub() FullSearch()
        AddHandler bgw.RunWorkerCompleted,
            Sub()
                Cursor = Cursors.Default
                If Not String.IsNullOrWhiteSpace(FoundFile) AndAlso File.Exists(FoundFile) Then
                    LoadMusic(FoundFile)
                    Exit Sub
                End If

                MsgBox("Could not find this file, ANYWHERE, on your system. Is it on an external drive that isn't connected?")
            End Sub

        Cursor = Cursors.WaitCursor
        bgw.RunWorkerAsync()
    End Sub

    Private Sub MenuMusic_DropDownOpening(sender As Object, e As EventArgs) Handles MenuMusic.DropDownOpening
        MenuMusic_SmartOpen.Visible = Not String.IsNullOrWhiteSpace(FILENAME)
        MenuMusic_Unload.Visible = Not String.IsNullOrWhiteSpace(AxWindowsMediaPlayer1.URL) AndAlso File.Exists(AxWindowsMediaPlayer1.URL)
    End Sub

    Private Sub MenuMusic_Unload_Click(sender As Object, e As EventArgs) Handles MenuMusic_Unload.Click
        UnloadMusic()
    End Sub

    Private Sub MenuFile_New_ME2_Click(sender As Object, e As EventArgs) Handles MenuFile_New_ME2.Click
        TRACK.ParseNewME2()
        LoadData()
        FILENAME = Nothing
    End Sub
    Private Sub MenuFile_New_ME1_Click(sender As Object, e As EventArgs) Handles MenuFile_New_ME1.Click
        TRACK.ParseNewME1()
        LoadData()
        FILENAME = Nothing
    End Sub


End Class
