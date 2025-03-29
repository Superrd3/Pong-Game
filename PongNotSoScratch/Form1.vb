Public Class Form1
    Inherits System.Windows.Forms.Form

    ' Deklaration der Buttons
    Private btnStart As Button
    Private btnSettings As Button
    Private btnExit As Button
    Private focusTimer As Timer ' Timer zum Entfernen des Fokus
    Public Sub New()
        ' Fenster initialisieren
        Me.Text = "Pong - Startmenü"
        Me.ClientSize = New Size(400, 300)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False

        ' Start-Button
        btnStart = New Button()
        btnStart.Text = "Start"
        btnStart.Size = New Size(200, 50)
        btnStart.Location = New Point(100, 50)
        AddHandler btnStart.Click, AddressOf btnStart_Click

        ' Einstellungen-Button
        btnSettings = New Button()
        btnSettings.Text = "Einstellungen"
        btnSettings.Size = New Size(200, 50)
        btnSettings.Location = New Point(100, 120)
        AddHandler btnSettings.Click, AddressOf btnSettings_Click

        ' Beenden-Button
        btnExit = New Button()
        btnExit.Text = "Beenden"
        btnExit.Size = New Size(200, 50)
        btnExit.Location = New Point(100, 190)
        AddHandler btnExit.Click, AddressOf btnExit_Click

        ' Buttons zum Formular hinzufügen
        Me.Controls.Add(btnStart)
        Me.Controls.Add(btnSettings)
        Me.Controls.Add(btnExit)
        'TabStop für alle Steuerelemente deaktivieren
        DisableTabStop(Me)
        ' SetStyle für das Formular setzen
        Me.SetStyle(ControlStyles.Selectable, False)

    End Sub




    ' 'Event-Handler für Buttons
    Private Sub btnStart_Click(sender As Object, e As EventArgs)
        Dim gameForm As New frmPong()
        gameForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnSettings_Click(sender As Object, e As EventArgs)
        Dim settingsForm As New frmSettings()
        settingsForm.ShowDialog()
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs)
        Application.Exit()
    End Sub

    ' Methode zum Deaktivieren von TabStop für alle Steuerelemente
    Private Sub DisableTabStop(control As Control)
        For Each ctrl As Control In control.Controls
            ctrl.TabStop = False
            If ctrl.HasChildren Then
                DisableTabStop(ctrl)
            End If
        Next
    End Sub

    ' Methode zum Entfernen des Fokus
    Private Sub RemoveFocus(sender As Object, e As EventArgs)
        Me.ActiveControl = Nothing
        focusTimer.Stop()
    End Sub

End Class
