<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.txt_ver = New System.Windows.Forms.TextBox()
        Me.txt_samples = New System.Windows.Forms.TextBox()
        Me.txt_dur = New System.Windows.Forms.TextBox()
        Me.txt_bpm = New System.Windows.Forms.TextBox()
        Me.chk_34 = New System.Windows.Forms.CheckBox()
        Me.pnl_ieditor = New System.Windows.Forms.Panel()
        Me.btn_tool_idur50 = New System.Windows.Forms.Button()
        Me.btn_tool_idur30 = New System.Windows.Forms.Button()
        Me.btn_tool_idur20 = New System.Windows.Forms.Button()
        Me.btn_tool_idur15 = New System.Windows.Forms.Button()
        Me.btn_tool_idur10 = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btn_tool_itimeminus = New System.Windows.Forms.Button()
        Me.btn_tool_itimeplus = New System.Windows.Forms.Button()
        Me.btn_tool_slowclear = New System.Windows.Forms.Button()
        Me.btn_tool_setslow = New System.Windows.Forms.Button()
        Me.num_idur = New System.Windows.Forms.NumericUpDown()
        Me.txt_ia = New System.Windows.Forms.TextBox()
        Me.cmb_itype = New System.Windows.Forms.ComboBox()
        Me.num_itime = New System.Windows.Forms.NumericUpDown()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.pnl_oeditor = New System.Windows.Forms.Panel()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.btn_tool_odur50 = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.btn_tool_odur30 = New System.Windows.Forms.Button()
        Me.btn_tool_odur20 = New System.Windows.Forms.Button()
        Me.btn_tool_odur15 = New System.Windows.Forms.Button()
        Me.btn_tool_odur10 = New System.Windows.Forms.Button()
        Me.btn_tool_otimeminus = New System.Windows.Forms.Button()
        Me.btn_tool_otimeplus = New System.Windows.Forms.Button()
        Me.chk_odur = New System.Windows.Forms.CheckBox()
        Me.num_odur = New System.Windows.Forms.NumericUpDown()
        Me.cmb_otype = New System.Windows.Forms.ComboBox()
        Me.num_otime = New System.Windows.Forms.NumericUpDown()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.men_new = New System.Windows.Forms.ToolStripMenuItem()
        Me.men_open = New System.Windows.Forms.ToolStripMenuItem()
        Me.men_open_recent = New System.Windows.Forms.ToolStripMenuItem()
        Me.men_open_appdata = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.men_save = New System.Windows.Forms.ToolStripMenuItem()
        Me.men_save_as = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.men_exit = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UndoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RedoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.men_add_beat = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_add_intensity = New System.Windows.Forms.ToolStripMenuItem()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.AxWindowsMediaPlayer1 = New AxWMPLib.AxWindowsMediaPlayer()
        Me.btn_LoadAudio = New System.Windows.Forms.Button()
        Me.txt_audio_samples = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.btn_audio_stop = New System.Windows.Forms.Button()
        Me.lst_obstacles = New Melody_s_Maker.ListBoxColour()
        Me.lst_intensities = New Melody_s_Maker.ListBoxColour()
        Me.pnl_ieditor.SuspendLayout()
        CType(Me.num_idur, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.num_itime, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnl_oeditor.SuspendLayout()
        CType(Me.num_odur, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.num_otime, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuStrip1.SuspendLayout()
        CType(Me.AxWindowsMediaPlayer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txt_ver
        '
        Me.txt_ver.Location = New System.Drawing.Point(12, 43)
        Me.txt_ver.Name = "txt_ver"
        Me.txt_ver.Size = New System.Drawing.Size(48, 20)
        Me.txt_ver.TabIndex = 0
        '
        'txt_samples
        '
        Me.txt_samples.Location = New System.Drawing.Point(66, 43)
        Me.txt_samples.Name = "txt_samples"
        Me.txt_samples.Size = New System.Drawing.Size(48, 20)
        Me.txt_samples.TabIndex = 1
        '
        'txt_dur
        '
        Me.txt_dur.Location = New System.Drawing.Point(120, 43)
        Me.txt_dur.Name = "txt_dur"
        Me.txt_dur.Size = New System.Drawing.Size(48, 20)
        Me.txt_dur.TabIndex = 2
        '
        'txt_bpm
        '
        Me.txt_bpm.Location = New System.Drawing.Point(174, 43)
        Me.txt_bpm.Name = "txt_bpm"
        Me.txt_bpm.Size = New System.Drawing.Size(48, 20)
        Me.txt_bpm.TabIndex = 3
        '
        'chk_34
        '
        Me.chk_34.AutoSize = True
        Me.chk_34.Location = New System.Drawing.Point(228, 46)
        Me.chk_34.Name = "chk_34"
        Me.chk_34.Size = New System.Drawing.Size(54, 17)
        Me.chk_34.TabIndex = 4
        Me.chk_34.Text = "Is 3/4"
        Me.chk_34.UseVisualStyleBackColor = True
        '
        'pnl_ieditor
        '
        Me.pnl_ieditor.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnl_ieditor.Controls.Add(Me.btn_tool_idur50)
        Me.pnl_ieditor.Controls.Add(Me.btn_tool_idur30)
        Me.pnl_ieditor.Controls.Add(Me.btn_tool_idur20)
        Me.pnl_ieditor.Controls.Add(Me.btn_tool_idur15)
        Me.pnl_ieditor.Controls.Add(Me.btn_tool_idur10)
        Me.pnl_ieditor.Controls.Add(Me.Label4)
        Me.pnl_ieditor.Controls.Add(Me.Label3)
        Me.pnl_ieditor.Controls.Add(Me.Label2)
        Me.pnl_ieditor.Controls.Add(Me.btn_tool_itimeminus)
        Me.pnl_ieditor.Controls.Add(Me.btn_tool_itimeplus)
        Me.pnl_ieditor.Controls.Add(Me.btn_tool_slowclear)
        Me.pnl_ieditor.Controls.Add(Me.btn_tool_setslow)
        Me.pnl_ieditor.Controls.Add(Me.num_idur)
        Me.pnl_ieditor.Controls.Add(Me.txt_ia)
        Me.pnl_ieditor.Controls.Add(Me.cmb_itype)
        Me.pnl_ieditor.Controls.Add(Me.num_itime)
        Me.pnl_ieditor.Controls.Add(Me.Label1)
        Me.pnl_ieditor.Location = New System.Drawing.Point(351, 70)
        Me.pnl_ieditor.Name = "pnl_ieditor"
        Me.pnl_ieditor.Size = New System.Drawing.Size(437, 177)
        Me.pnl_ieditor.TabIndex = 7
        Me.pnl_ieditor.Visible = False
        '
        'btn_tool_idur50
        '
        Me.btn_tool_idur50.Location = New System.Drawing.Point(229, 94)
        Me.btn_tool_idur50.Name = "btn_tool_idur50"
        Me.btn_tool_idur50.Size = New System.Drawing.Size(27, 23)
        Me.btn_tool_idur50.TabIndex = 19
        Me.btn_tool_idur50.Text = "50"
        Me.btn_tool_idur50.UseVisualStyleBackColor = True
        '
        'btn_tool_idur30
        '
        Me.btn_tool_idur30.Location = New System.Drawing.Point(203, 94)
        Me.btn_tool_idur30.Name = "btn_tool_idur30"
        Me.btn_tool_idur30.Size = New System.Drawing.Size(27, 23)
        Me.btn_tool_idur30.TabIndex = 18
        Me.btn_tool_idur30.Text = "30"
        Me.btn_tool_idur30.UseVisualStyleBackColor = True
        '
        'btn_tool_idur20
        '
        Me.btn_tool_idur20.Location = New System.Drawing.Point(177, 94)
        Me.btn_tool_idur20.Name = "btn_tool_idur20"
        Me.btn_tool_idur20.Size = New System.Drawing.Size(27, 23)
        Me.btn_tool_idur20.TabIndex = 17
        Me.btn_tool_idur20.Text = "20"
        Me.btn_tool_idur20.UseVisualStyleBackColor = True
        '
        'btn_tool_idur15
        '
        Me.btn_tool_idur15.Location = New System.Drawing.Point(151, 94)
        Me.btn_tool_idur15.Name = "btn_tool_idur15"
        Me.btn_tool_idur15.Size = New System.Drawing.Size(27, 23)
        Me.btn_tool_idur15.TabIndex = 16
        Me.btn_tool_idur15.Text = "15"
        Me.btn_tool_idur15.UseVisualStyleBackColor = True
        '
        'btn_tool_idur10
        '
        Me.btn_tool_idur10.Location = New System.Drawing.Point(125, 94)
        Me.btn_tool_idur10.Name = "btn_tool_idur10"
        Me.btn_tool_idur10.Size = New System.Drawing.Size(27, 23)
        Me.btn_tool_idur10.TabIndex = 15
        Me.btn_tool_idur10.Text = "10"
        Me.btn_tool_idur10.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(0, 118)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(51, 13)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "Flags (-A)"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(0, 81)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(47, 13)
        Me.Label3.TabIndex = 13
        Me.Label3.Text = "Duration"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(0, 43)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(31, 13)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "Type"
        '
        'btn_tool_itimeminus
        '
        Me.btn_tool_itimeminus.Location = New System.Drawing.Point(147, 19)
        Me.btn_tool_itimeminus.Name = "btn_tool_itimeminus"
        Me.btn_tool_itimeminus.Size = New System.Drawing.Size(22, 23)
        Me.btn_tool_itimeminus.TabIndex = 10
        Me.btn_tool_itimeminus.Text = "-"
        Me.btn_tool_itimeminus.UseVisualStyleBackColor = True
        '
        'btn_tool_itimeplus
        '
        Me.btn_tool_itimeplus.Location = New System.Drawing.Point(125, 19)
        Me.btn_tool_itimeplus.Name = "btn_tool_itimeplus"
        Me.btn_tool_itimeplus.Size = New System.Drawing.Size(22, 23)
        Me.btn_tool_itimeplus.TabIndex = 9
        Me.btn_tool_itimeplus.Text = "+"
        Me.btn_tool_itimeplus.UseVisualStyleBackColor = True
        '
        'btn_tool_slowclear
        '
        Me.btn_tool_slowclear.Location = New System.Drawing.Point(182, 130)
        Me.btn_tool_slowclear.Name = "btn_tool_slowclear"
        Me.btn_tool_slowclear.Size = New System.Drawing.Size(40, 23)
        Me.btn_tool_slowclear.TabIndex = 8
        Me.btn_tool_slowclear.Text = "Clear"
        Me.btn_tool_slowclear.UseVisualStyleBackColor = True
        '
        'btn_tool_setslow
        '
        Me.btn_tool_setslow.Location = New System.Drawing.Point(125, 130)
        Me.btn_tool_setslow.Name = "btn_tool_setslow"
        Me.btn_tool_setslow.Size = New System.Drawing.Size(57, 23)
        Me.btn_tool_setslow.TabIndex = 7
        Me.btn_tool_setslow.Text = "Set Slow"
        Me.btn_tool_setslow.UseVisualStyleBackColor = True
        '
        'num_idur
        '
        Me.num_idur.Location = New System.Drawing.Point(3, 95)
        Me.num_idur.Maximum = New Decimal(New Integer() {9999999, 0, 0, 0})
        Me.num_idur.Name = "num_idur"
        Me.num_idur.Size = New System.Drawing.Size(121, 20)
        Me.num_idur.TabIndex = 2
        '
        'txt_ia
        '
        Me.txt_ia.Location = New System.Drawing.Point(3, 131)
        Me.txt_ia.Name = "txt_ia"
        Me.txt_ia.Size = New System.Drawing.Size(121, 20)
        Me.txt_ia.TabIndex = 6
        '
        'cmb_itype
        '
        Me.cmb_itype.FormattingEnabled = True
        Me.cmb_itype.Items.AddRange(New Object() {"Walk", "Jog", "Run", "Fly"})
        Me.cmb_itype.Location = New System.Drawing.Point(3, 57)
        Me.cmb_itype.Name = "cmb_itype"
        Me.cmb_itype.Size = New System.Drawing.Size(121, 21)
        Me.cmb_itype.TabIndex = 1
        '
        'num_itime
        '
        Me.num_itime.Location = New System.Drawing.Point(3, 20)
        Me.num_itime.Maximum = New Decimal(New Integer() {9999999, 0, 0, 0})
        Me.num_itime.Name = "num_itime"
        Me.num_itime.Size = New System.Drawing.Size(121, 20)
        Me.num_itime.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(0, 6)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(30, 13)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "Time"
        '
        'pnl_oeditor
        '
        Me.pnl_oeditor.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnl_oeditor.Controls.Add(Me.Label5)
        Me.pnl_oeditor.Controls.Add(Me.btn_tool_odur50)
        Me.pnl_oeditor.Controls.Add(Me.Label6)
        Me.pnl_oeditor.Controls.Add(Me.btn_tool_odur30)
        Me.pnl_oeditor.Controls.Add(Me.btn_tool_odur20)
        Me.pnl_oeditor.Controls.Add(Me.btn_tool_odur15)
        Me.pnl_oeditor.Controls.Add(Me.btn_tool_odur10)
        Me.pnl_oeditor.Controls.Add(Me.btn_tool_otimeminus)
        Me.pnl_oeditor.Controls.Add(Me.btn_tool_otimeplus)
        Me.pnl_oeditor.Controls.Add(Me.chk_odur)
        Me.pnl_oeditor.Controls.Add(Me.num_odur)
        Me.pnl_oeditor.Controls.Add(Me.cmb_otype)
        Me.pnl_oeditor.Controls.Add(Me.num_otime)
        Me.pnl_oeditor.Location = New System.Drawing.Point(351, 253)
        Me.pnl_oeditor.Name = "pnl_oeditor"
        Me.pnl_oeditor.Size = New System.Drawing.Size(437, 160)
        Me.pnl_oeditor.TabIndex = 8
        Me.pnl_oeditor.Visible = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(0, 43)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(31, 13)
        Me.Label5.TabIndex = 21
        Me.Label5.Text = "Type"
        '
        'btn_tool_odur50
        '
        Me.btn_tool_odur50.Location = New System.Drawing.Point(229, 100)
        Me.btn_tool_odur50.Name = "btn_tool_odur50"
        Me.btn_tool_odur50.Size = New System.Drawing.Size(27, 23)
        Me.btn_tool_odur50.TabIndex = 26
        Me.btn_tool_odur50.Text = "50"
        Me.btn_tool_odur50.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(0, 6)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(30, 13)
        Me.Label6.TabIndex = 20
        Me.Label6.Text = "Time"
        '
        'btn_tool_odur30
        '
        Me.btn_tool_odur30.Location = New System.Drawing.Point(203, 100)
        Me.btn_tool_odur30.Name = "btn_tool_odur30"
        Me.btn_tool_odur30.Size = New System.Drawing.Size(27, 23)
        Me.btn_tool_odur30.TabIndex = 25
        Me.btn_tool_odur30.Text = "30"
        Me.btn_tool_odur30.UseVisualStyleBackColor = True
        '
        'btn_tool_odur20
        '
        Me.btn_tool_odur20.Location = New System.Drawing.Point(177, 100)
        Me.btn_tool_odur20.Name = "btn_tool_odur20"
        Me.btn_tool_odur20.Size = New System.Drawing.Size(27, 23)
        Me.btn_tool_odur20.TabIndex = 24
        Me.btn_tool_odur20.Text = "20"
        Me.btn_tool_odur20.UseVisualStyleBackColor = True
        '
        'btn_tool_odur15
        '
        Me.btn_tool_odur15.Location = New System.Drawing.Point(151, 100)
        Me.btn_tool_odur15.Name = "btn_tool_odur15"
        Me.btn_tool_odur15.Size = New System.Drawing.Size(27, 23)
        Me.btn_tool_odur15.TabIndex = 23
        Me.btn_tool_odur15.Text = "15"
        Me.btn_tool_odur15.UseVisualStyleBackColor = True
        '
        'btn_tool_odur10
        '
        Me.btn_tool_odur10.Location = New System.Drawing.Point(125, 100)
        Me.btn_tool_odur10.Name = "btn_tool_odur10"
        Me.btn_tool_odur10.Size = New System.Drawing.Size(27, 23)
        Me.btn_tool_odur10.TabIndex = 22
        Me.btn_tool_odur10.Text = "10"
        Me.btn_tool_odur10.UseVisualStyleBackColor = True
        '
        'btn_tool_otimeminus
        '
        Me.btn_tool_otimeminus.Location = New System.Drawing.Point(147, 18)
        Me.btn_tool_otimeminus.Name = "btn_tool_otimeminus"
        Me.btn_tool_otimeminus.Size = New System.Drawing.Size(22, 23)
        Me.btn_tool_otimeminus.TabIndex = 21
        Me.btn_tool_otimeminus.Text = "-"
        Me.btn_tool_otimeminus.UseVisualStyleBackColor = True
        '
        'btn_tool_otimeplus
        '
        Me.btn_tool_otimeplus.Location = New System.Drawing.Point(125, 18)
        Me.btn_tool_otimeplus.Name = "btn_tool_otimeplus"
        Me.btn_tool_otimeplus.Size = New System.Drawing.Size(22, 23)
        Me.btn_tool_otimeplus.TabIndex = 20
        Me.btn_tool_otimeplus.Text = "+"
        Me.btn_tool_otimeplus.UseVisualStyleBackColor = True
        '
        'chk_odur
        '
        Me.chk_odur.AutoSize = True
        Me.chk_odur.Location = New System.Drawing.Point(3, 84)
        Me.chk_odur.Name = "chk_odur"
        Me.chk_odur.Size = New System.Drawing.Size(91, 17)
        Me.chk_odur.TabIndex = 7
        Me.chk_odur.Text = "Hold Duration"
        Me.chk_odur.UseVisualStyleBackColor = True
        '
        'num_odur
        '
        Me.num_odur.Location = New System.Drawing.Point(3, 101)
        Me.num_odur.Maximum = New Decimal(New Integer() {9999999, 0, 0, 0})
        Me.num_odur.Name = "num_odur"
        Me.num_odur.Size = New System.Drawing.Size(121, 20)
        Me.num_odur.TabIndex = 5
        '
        'cmb_otype
        '
        Me.cmb_otype.FormattingEnabled = True
        Me.cmb_otype.Items.AddRange(New Object() {"Down", "Left", "Up", "Right"})
        Me.cmb_otype.Location = New System.Drawing.Point(3, 57)
        Me.cmb_otype.Name = "cmb_otype"
        Me.cmb_otype.Size = New System.Drawing.Size(121, 21)
        Me.cmb_otype.TabIndex = 4
        '
        'num_otime
        '
        Me.num_otime.Location = New System.Drawing.Point(3, 19)
        Me.num_otime.Maximum = New Decimal(New Integer() {9999999, 0, 0, 0})
        Me.num_otime.Name = "num_otime"
        Me.num_otime.Size = New System.Drawing.Size(121, 20)
        Me.num_otime.TabIndex = 3
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.EditToolStripMenuItem, Me.ToolsToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(800, 24)
        Me.MenuStrip1.TabIndex = 16
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.men_new, Me.men_open, Me.men_open_recent, Me.men_open_appdata, Me.ToolStripSeparator2, Me.men_save, Me.men_save_as, Me.ToolStripSeparator1, Me.men_exit})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'men_new
        '
        Me.men_new.Name = "men_new"
        Me.men_new.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.N), System.Windows.Forms.Keys)
        Me.men_new.Size = New System.Drawing.Size(195, 22)
        Me.men_new.Text = "New"
        '
        'men_open
        '
        Me.men_open.Name = "men_open"
        Me.men_open.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.men_open.Size = New System.Drawing.Size(195, 22)
        Me.men_open.Text = "Open..."
        '
        'men_open_recent
        '
        Me.men_open_recent.Name = "men_open_recent"
        Me.men_open_recent.Size = New System.Drawing.Size(195, 22)
        Me.men_open_recent.Text = "Open Recent"
        '
        'men_open_appdata
        '
        Me.men_open_appdata.Name = "men_open_appdata"
        Me.men_open_appdata.Size = New System.Drawing.Size(195, 22)
        Me.men_open_appdata.Text = "Open AppData"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(192, 6)
        '
        'men_save
        '
        Me.men_save.Name = "men_save"
        Me.men_save.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.men_save.Size = New System.Drawing.Size(195, 22)
        Me.men_save.Text = "Save"
        '
        'men_save_as
        '
        Me.men_save_as.Name = "men_save_as"
        Me.men_save_as.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Shift) _
            Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.men_save_as.Size = New System.Drawing.Size(195, 22)
        Me.men_save_as.Text = "Save As..."
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(192, 6)
        '
        'men_exit
        '
        Me.men_exit.Name = "men_exit"
        Me.men_exit.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.F4), System.Windows.Forms.Keys)
        Me.men_exit.Size = New System.Drawing.Size(195, 22)
        Me.men_exit.Text = "Exit"
        '
        'EditToolStripMenuItem
        '
        Me.EditToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.UndoToolStripMenuItem, Me.RedoToolStripMenuItem})
        Me.EditToolStripMenuItem.Name = "EditToolStripMenuItem"
        Me.EditToolStripMenuItem.Size = New System.Drawing.Size(39, 20)
        Me.EditToolStripMenuItem.Text = "Edit"
        '
        'UndoToolStripMenuItem
        '
        Me.UndoToolStripMenuItem.Name = "UndoToolStripMenuItem"
        Me.UndoToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Z), System.Windows.Forms.Keys)
        Me.UndoToolStripMenuItem.Size = New System.Drawing.Size(174, 22)
        Me.UndoToolStripMenuItem.Text = "Undo"
        '
        'RedoToolStripMenuItem
        '
        Me.RedoToolStripMenuItem.Name = "RedoToolStripMenuItem"
        Me.RedoToolStripMenuItem.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Shift) _
            Or System.Windows.Forms.Keys.Z), System.Windows.Forms.Keys)
        Me.RedoToolStripMenuItem.Size = New System.Drawing.Size(174, 22)
        Me.RedoToolStripMenuItem.Text = "Redo"
        '
        'ToolsToolStripMenuItem
        '
        Me.ToolsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.men_add_beat, Me.menu_add_intensity})
        Me.ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem"
        Me.ToolsToolStripMenuItem.Size = New System.Drawing.Size(41, 20)
        Me.ToolsToolStripMenuItem.Text = "Add"
        '
        'men_add_beat
        '
        Me.men_add_beat.Name = "men_add_beat"
        Me.men_add_beat.Size = New System.Drawing.Size(147, 22)
        Me.men_add_beat.Text = "Beat Pattern..."
        '
        'menu_add_intensity
        '
        Me.menu_add_intensity.Name = "menu_add_intensity"
        Me.menu_add_intensity.Size = New System.Drawing.Size(147, 22)
        Me.menu_add_intensity.Text = "Intensity"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(9, 30)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(42, 13)
        Me.Label7.TabIndex = 20
        Me.Label7.Text = "Version"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(63, 30)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(47, 13)
        Me.Label8.TabIndex = 21
        Me.Label8.Text = "Samples"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(117, 30)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(47, 13)
        Me.Label9.TabIndex = 22
        Me.Label9.Text = "Duration"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(172, 30)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(40, 13)
        Me.Label10.TabIndex = 23
        Me.Label10.Text = "Tempo"
        '
        'AxWindowsMediaPlayer1
        '
        Me.AxWindowsMediaPlayer1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AxWindowsMediaPlayer1.Enabled = True
        Me.AxWindowsMediaPlayer1.Location = New System.Drawing.Point(544, 22)
        Me.AxWindowsMediaPlayer1.Name = "AxWindowsMediaPlayer1"
        Me.AxWindowsMediaPlayer1.OcxState = CType(resources.GetObject("AxWindowsMediaPlayer1.OcxState"), System.Windows.Forms.AxHost.State)
        Me.AxWindowsMediaPlayer1.Size = New System.Drawing.Size(244, 45)
        Me.AxWindowsMediaPlayer1.TabIndex = 24
        '
        'btn_LoadAudio
        '
        Me.btn_LoadAudio.Location = New System.Drawing.Point(288, 42)
        Me.btn_LoadAudio.Name = "btn_LoadAudio"
        Me.btn_LoadAudio.Size = New System.Drawing.Size(75, 23)
        Me.btn_LoadAudio.TabIndex = 25
        Me.btn_LoadAudio.Text = "Load Audio"
        Me.btn_LoadAudio.UseVisualStyleBackColor = True
        '
        'txt_audio_samples
        '
        Me.txt_audio_samples.Location = New System.Drawing.Point(369, 44)
        Me.txt_audio_samples.Name = "txt_audio_samples"
        Me.txt_audio_samples.ReadOnly = True
        Me.txt_audio_samples.Size = New System.Drawing.Size(125, 20)
        Me.txt_audio_samples.TabIndex = 26
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(366, 31)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(128, 13)
        Me.Label11.TabIndex = 27
        Me.Label11.Text = "Audio Position in Samples"
        '
        'btn_audio_stop
        '
        Me.btn_audio_stop.Location = New System.Drawing.Point(500, 44)
        Me.btn_audio_stop.Name = "btn_audio_stop"
        Me.btn_audio_stop.Size = New System.Drawing.Size(40, 23)
        Me.btn_audio_stop.TabIndex = 28
        Me.btn_audio_stop.Text = "Stop"
        Me.btn_audio_stop.UseVisualStyleBackColor = True
        '
        'lst_obstacles
        '
        Me.lst_obstacles.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lst_obstacles.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.lst_obstacles.FormattingEnabled = True
        Me.lst_obstacles.IntegralHeight = False
        Me.lst_obstacles.Location = New System.Drawing.Point(175, 70)
        Me.lst_obstacles.Name = "lst_obstacles"
        Me.lst_obstacles.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lst_obstacles.Size = New System.Drawing.Size(170, 368)
        Me.lst_obstacles.TabIndex = 6
        '
        'lst_intensities
        '
        Me.lst_intensities.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lst_intensities.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.lst_intensities.FormattingEnabled = True
        Me.lst_intensities.IntegralHeight = False
        Me.lst_intensities.Location = New System.Drawing.Point(12, 70)
        Me.lst_intensities.Name = "lst_intensities"
        Me.lst_intensities.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lst_intensities.Size = New System.Drawing.Size(157, 368)
        Me.lst_intensities.TabIndex = 5
        '
        'Form1
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.btn_audio_stop)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.txt_audio_samples)
        Me.Controls.Add(Me.btn_LoadAudio)
        Me.Controls.Add(Me.AxWindowsMediaPlayer1)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.pnl_oeditor)
        Me.Controls.Add(Me.pnl_ieditor)
        Me.Controls.Add(Me.lst_obstacles)
        Me.Controls.Add(Me.lst_intensities)
        Me.Controls.Add(Me.chk_34)
        Me.Controls.Add(Me.txt_bpm)
        Me.Controls.Add(Me.txt_dur)
        Me.Controls.Add(Me.txt_samples)
        Me.Controls.Add(Me.txt_ver)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.pnl_ieditor.ResumeLayout(False)
        Me.pnl_ieditor.PerformLayout()
        CType(Me.num_idur, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.num_itime, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnl_oeditor.ResumeLayout(False)
        Me.pnl_oeditor.PerformLayout()
        CType(Me.num_odur, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.num_otime, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        CType(Me.AxWindowsMediaPlayer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txt_ver As TextBox
    Friend WithEvents txt_samples As TextBox
    Friend WithEvents txt_dur As TextBox
    Friend WithEvents txt_bpm As TextBox
    Friend WithEvents chk_34 As CheckBox
    Friend WithEvents lst_intensities As ListBoxColour
    Friend WithEvents lst_obstacles As ListBoxColour
    Friend WithEvents pnl_ieditor As Panel
    Friend WithEvents pnl_oeditor As Panel
    Friend WithEvents cmb_itype As ComboBox
    Friend WithEvents num_itime As NumericUpDown
    Friend WithEvents num_idur As NumericUpDown
    Friend WithEvents chk_odur As CheckBox
    Friend WithEvents txt_ia As TextBox
    Friend WithEvents num_odur As NumericUpDown
    Friend WithEvents cmb_otype As ComboBox
    Friend WithEvents num_otime As NumericUpDown
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents FileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents men_new As ToolStripMenuItem
    Friend WithEvents men_open As ToolStripMenuItem
    Friend WithEvents men_open_recent As ToolStripMenuItem
    Friend WithEvents men_open_appdata As ToolStripMenuItem
    Friend WithEvents EditToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents UndoToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RedoToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents men_exit As ToolStripMenuItem
    Friend WithEvents men_add_beat As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents men_save As ToolStripMenuItem
    Friend WithEvents men_save_as As ToolStripMenuItem
    Friend WithEvents btn_tool_setslow As Button
    Friend WithEvents btn_tool_slowclear As Button
    Friend WithEvents btn_tool_itimeminus As Button
    Friend WithEvents btn_tool_itimeplus As Button
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents btn_tool_idur10 As Button
    Friend WithEvents btn_tool_idur50 As Button
    Friend WithEvents btn_tool_idur30 As Button
    Friend WithEvents btn_tool_idur20 As Button
    Friend WithEvents btn_tool_idur15 As Button
    Friend WithEvents Label5 As Label
    Friend WithEvents btn_tool_odur50 As Button
    Friend WithEvents Label6 As Label
    Friend WithEvents btn_tool_odur30 As Button
    Friend WithEvents btn_tool_odur20 As Button
    Friend WithEvents btn_tool_odur15 As Button
    Friend WithEvents btn_tool_odur10 As Button
    Friend WithEvents btn_tool_otimeminus As Button
    Friend WithEvents btn_tool_otimeplus As Button
    Friend WithEvents menu_add_intensity As ToolStripMenuItem
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents AxWindowsMediaPlayer1 As AxWMPLib.AxWindowsMediaPlayer
    Friend WithEvents btn_LoadAudio As Button
    Friend WithEvents txt_audio_samples As TextBox
    Friend WithEvents Label11 As Label
    Friend WithEvents btn_audio_stop As Button
End Class
