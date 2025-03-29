Public Class frmSettings
    Inherits System.Windows.Forms.Form
    ' Deklaration der UI-Elemente
    Private lblScoreToWin As Label
    Private cboScoreToWin As ComboBox
    Private lblBallSpeed As Label
    Private chkBallSpeedOnServe As CheckBox
    Private lblControlSensitivity As Label
    Private cboControlSensitivity As ComboBox
    Private lblWhoStarts As Label
    Private cboWhoStarts As ComboBox
    Private lblSingleplayerControl As Label
    Private cboSingleplayerControl As ComboBox
    Private lblBotDifficulty As Label
    Private cboBotDifficulty As ComboBox
    Private lblBallSpeedIncreaseRate As Label
    Private cboBallSpeedIncreaseRate As ComboBox
    Private btnSave As Button
    Private focusTimer As Timer ' Timer zum Entfernen des Fokus
    Private lblGameMode As Label
    Private cboGameMode As ComboBox
    Private btnReset As Button
    Private Sub frmSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Einstellungen laden
        SettingsManager.LoadSettings()

        ' Vorhandene Einstellungen laden
        cboScoreToWin.SelectedItem = SettingsManager.ScoreToWin
        chkBallSpeedOnServe.Checked = SettingsManager.BallSpeedOnServe
        cboControlSensitivity.SelectedItem = SettingsManager.ControlSensitivity
        cboWhoStarts.SelectedItem = SettingsManager.WhoStarts
        cboSingleplayerControl.SelectedItem = SettingsManager.SingleplayerControl
        cboBotDifficulty.SelectedItem = SettingsManager.BotDifficulty
        cboBallSpeedIncreaseRate.SelectedItem = SettingsManager.BallSpeedIncreaseRate
        cboGameMode.SelectedItem = SettingsManager.GameMode

        ' Verhindert, dass die Anwendung abstürzt, weil keine Werte in der ComboBox ausgewählt sind
        If cboScoreToWin.SelectedItem Is Nothing Then
            cboScoreToWin.SelectedIndex = cboScoreToWin.SelectedIndex = 2
        End If
        If cboControlSensitivity.SelectedItem Is Nothing Then
            cboControlSensitivity.SelectedIndex = cboControlSensitivity.SelectedIndex = 1
        End If
        If cboWhoStarts.SelectedItem Is Nothing Then
            cboWhoStarts.SelectedIndex = cboWhoStarts.SelectedIndex = 1
        End If
        If cboSingleplayerControl.SelectedItem Is Nothing Then
            cboSingleplayerControl.SelectedIndex = cboSingleplayerControl.SelectedIndex = 0
        End If
        If cboBotDifficulty.SelectedItem Is Nothing Then
            cboBotDifficulty.SelectedIndex = cboBotDifficulty.SelectedIndex = 2
        End If
        If cboBallSpeedIncreaseRate.SelectedItem Is Nothing Then
            cboBallSpeedIncreaseRate.SelectedIndex = cboBallSpeedIncreaseRate.SelectedIndex = 0
        End If

        If cboGameMode.SelectedIndex = -1 Then cboGameMode.SelectedIndex = 0 ' Standard: Singleplayer


    End Sub





    Public Sub New()
        ' Fenster-Einstellungen
        Me.Text = "Einstellungen"
        Me.ClientSize = New Size(435, 440)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False

        ' Punktelimit
        lblScoreToWin = New Label() With {.Text = "Punktelimit:", .Location = New Point(30, 20), .AutoSize = True}
        cboScoreToWin = New ComboBox() With {.Location = New Point(250, 20), .Size = New Size(100, 20), .DropDownStyle = ComboBoxStyle.DropDownList}
        cboScoreToWin.Items.AddRange(New Object() {3, 5, 10, 15, 20})

        ' Ballgeschwindigkeit nach Serve
        lblBallSpeed = New Label() With {.Text = "Langsame Aufschläge?", .Location = New Point(30, 60), .AutoSize = True}
        chkBallSpeedOnServe = New CheckBox() With {.Location = New Point(290, 60)}

        ' Steuerungssensitivität
        lblControlSensitivity = New Label() With {.Text = "(Tastatur) Steuerungsempfinglichkeit:", .Location = New Point(30, 100), .AutoSize = True}
        cboControlSensitivity = New ComboBox() With {.Location = New Point(250, 100), .Size = New Size(100, 20), .DropDownStyle = ComboBoxStyle.DropDownList}
        cboControlSensitivity.Items.AddRange(New Object() {"Niedrig", "Normal", "Hoch"})

        ' Wer serviert?
        lblWhoStarts = New Label() With {.Text = "Wer kriegt Aufschlag nach Punkt?", .Location = New Point(30, 140), .AutoSize = True}
        cboWhoStarts = New ComboBox() With {.Location = New Point(250, 140), .Size = New Size(100, 20), .DropDownStyle = ComboBoxStyle.DropDownList}
        cboWhoStarts.Items.AddRange(New Object() {"Torschießer", "Wechselt ab", "Zufall"})

        ' Singleplayer Steuerung
        lblSingleplayerControl = New Label() With {.Text = "Steuerung Einzelspieler:", .Location = New Point(30, 180), .AutoSize = True}
        cboSingleplayerControl = New ComboBox() With {.Location = New Point(250, 180), .Size = New Size(100, 20), .DropDownStyle = ComboBoxStyle.DropDownList}
        cboSingleplayerControl.Items.AddRange(New Object() {"Maus", "Tastatur"})

        ' Bot Schwierigkeit
        lblBotDifficulty = New Label() With {.Text = "Bot Schwierigkeit:", .Location = New Point(30, 220), .AutoSize = True}
        cboBotDifficulty = New ComboBox() With {.Location = New Point(250, 220), .Size = New Size(100, 20), .DropDownStyle = ComboBoxStyle.DropDownList}
        cboBotDifficulty.Items.AddRange(New Object() {"Einfach", "Mittel", "Hart", "Unmöglich"})

        ' Ball Speed Zuwachs
        lblBallSpeedIncreaseRate = New Label() With {.Text = "Ball Geschwindigkeitszunahme:", .Location = New Point(30, 260), .AutoSize = True}
        cboBallSpeedIncreaseRate = New ComboBox() With {.Location = New Point(250, 260), .Size = New Size(100, 20), .DropDownStyle = ComboBoxStyle.DropDownList}
        cboBallSpeedIncreaseRate.Items.AddRange(New Object() {1, 2, 3, 4, 5})

        ' Spielmodus (Einzelspieler / Mehrspieler)
        lblGameMode = New Label() With {.Text = "Spielmodus:", .Location = New Point(30, 300)}
        cboGameMode = New ComboBox() With {.Location = New Point(250, 300), .Size = New Size(100, 20), .DropDownStyle = ComboBoxStyle.DropDownList}
        cboGameMode.Items.AddRange(New Object() {"Singleplayer", "Multiplayer"})


        ' Speichern-Button
        btnSave = New Button() With {.Text = "Speichern", .Location = New Point(205, 360), .Size = New Size(150, 50)}
        AddHandler btnSave.Click, AddressOf btnSave_Click

        ' Zurücksetzen-Button
        btnReset = New Button() With {.Text = "Zurücksetzen", .Location = New Point(30, 360), .Size = New Size(100, 35)}
        AddHandler btnReset.Click, AddressOf btnReset_Click


        ' Elemente hinzufügen
        Me.Controls.AddRange(New Control() {lblScoreToWin, cboScoreToWin, lblBallSpeed, chkBallSpeedOnServe,
                                        lblControlSensitivity, cboControlSensitivity, lblWhoStarts, cboWhoStarts,
                                        lblSingleplayerControl, cboSingleplayerControl, lblBotDifficulty, cboBotDifficulty,
                                        lblBallSpeedIncreaseRate, cboBallSpeedIncreaseRate, btnSave, lblGameMode, cboGameMode, btnReset})
        ' TabStop für alle Steuerelemente deaktivieren
        DisableTabStop(Me)
        ' SetStyle für das Formular setzen
        Me.SetStyle(ControlStyles.Selectable, False)
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs)

        ' Werte speichern
        SettingsManager.ScoreToWin = Integer.Parse(cboScoreToWin.SelectedItem.ToString())
        SettingsManager.BallSpeedOnServe = chkBallSpeedOnServe.Checked
        SettingsManager.ControlSensitivity = cboControlSensitivity.SelectedItem.ToString()
        SettingsManager.WhoStarts = cboWhoStarts.SelectedItem.ToString()
        SettingsManager.SingleplayerControl = cboSingleplayerControl.SelectedItem.ToString()
        SettingsManager.BotDifficulty = cboBotDifficulty.SelectedItem.ToString()
        SettingsManager.BallSpeedIncreaseRate = Integer.Parse(cboBallSpeedIncreaseRate.SelectedItem.ToString())
        SettingsManager.GameMode = cboGameMode.SelectedItem.ToString()
        ' Speichern der Einstellungen
        SettingsManager.SaveSettings()

        ' Fenster schließen
        Me.Close()
    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs)
        ' Einstellungen zurücksetzen
        ResetSettings()
    End Sub

    ' Methode zum Deaktivieren von TabStop für alle Steuerelemente (replaced)
    Private Sub DisableTabStop(control As Control)
        For Each ctrl As Control In control.Controls
            ctrl.TabStop = False
            If ctrl.HasChildren Then
                DisableTabStop(ctrl)
            End If
        Next
    End Sub
    Private Sub RemoveFocus(sender As Object, e As EventArgs)
        Me.ActiveControl = Nothing
        focusTimer.Stop()
    End Sub

    Private Sub ResetSettings()
        ' Einstellungen zurücksetzen
        cboScoreToWin.SelectedIndex = 2
        chkBallSpeedOnServe.Checked = True
        cboControlSensitivity.SelectedIndex = 1
        cboWhoStarts.SelectedIndex = 0
        cboSingleplayerControl.SelectedIndex = 1
        cboBotDifficulty.SelectedIndex = 1
        cboBallSpeedIncreaseRate.SelectedIndex = 0
        cboGameMode.SelectedIndex = 0
    End Sub

End Class