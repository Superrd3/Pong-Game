Public Class frmPong
    Inherits System.Windows.Forms.Form

    ' UI-Elemente für das Spiel
    Private player1Bat As PictureBox
    Private player2Bat As PictureBox
    Private ballControl As PictureBox
    Private player1Score As Label
    Private player2Score As Label
    Private gameTimer As Timer
    Private focusTimer As Timer ' Timer zum Entfernen des Fokus

    ' Ballbewegung
    Private ballSpeedX As Integer = 5
    Private ballSpeedY As Integer = 3

    ' Spielerbewegung
    Private playerSpeed As Integer = 7

    ' Punkte
    Private scorePlayer1 As Integer = 0
    Private scorePlayer2 As Integer = 0

    ' Steuerungsflags
    Private moveUpP1 As Boolean = False
    Private moveDownP1 As Boolean = False
    Private moveUpP2 As Boolean = False
    Private moveDownP2 As Boolean = False

    ' Pausenmenü (Panel mit Buttons)
    Private pauseMenu As Panel
    Private btnResume As Button
    Private btnRestart As Button
    Private btnSettings As Button
    Private btnMainMenu As Button
    Private btnExit As Button
    Private isPaused As Boolean = False
    Private btnOpenPauseMenu As Button
    ' Spielstatus
    Private gameOver As Boolean = False
    Public Sub New()
        ' Fenster-Einstellungen
        Me.Text = "Pong"
        Me.ClientSize = New Size(800, 600)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.BackColor = Color.Black
        Me.DoubleBuffered = True
        Me.KeyPreview = True ' Soll verhindern dass die Steuerung nicht funktioniert wenn ein Steuerelement fokussiert ist oder das Formular nicht fokussiert ist

        ' Spieler 1 Schläger
        player1Bat = New PictureBox() With {
            .Size = New Size(10, 50),
            .BackColor = Color.White,
            .Location = New Point(30, 250)
        }

        ' Spieler 2 Schläger
        player2Bat = New PictureBox() With {
            .Size = New Size(10, 50),
            .BackColor = Color.White,
            .Location = New Point(760, 250)
        }

        ' Ball
        ballControl = New PictureBox() With {
            .Size = New Size(10, 10),
            .BackColor = Color.White,
            .Location = New Point(395, 295)
        }

        ' Spieler 1 Score
        player1Score = New Label() With {
            .Text = "0",
            .ForeColor = Color.White,
            .Font = New Font("Arial", 16, FontStyle.Bold),
            .Location = New Point(350, 10),
            .AutoSize = True
        }

        ' Spieler 2 Score
        player2Score = New Label() With {
            .Text = "0",
            .ForeColor = Color.White,
            .Font = New Font("Arial", 16, FontStyle.Bold),
            .Location = New Point(400, 10),
            .AutoSize = True
        }

        ' Spiel-Timer
        gameTimer = New Timer() With {.Interval = 20}
        AddHandler gameTimer.Tick, AddressOf GameLoop
        gameTimer.Start()

        ' Key-Events für Steuerung
        AddHandler Me.KeyDown, AddressOf frmPong_KeyDown
        '( AddHandler Me.KeyUp, AddressOf frmPong_KeyUp)

        ' Fokus-Timer
        focusTimer = New Timer() With {.Interval = 100} ' Fokus nach 100ms entfernen
        AddHandler focusTimer.Tick, AddressOf RemoveFocus

        ' **Pausenmenü erstellen**
        pauseMenu = New Panel() With {
            .Size = New Size(300, 320),
            .Location = New Point(250, 150),
            .BackColor = Color.Gray,
            .Visible = False 'Unsichtbar bis es aufgerufen wird
        }

        ' Titel-Label für das Pausenmenü
        Dim lblPauseTitle As New Label() With {
            .Text = "Pausenmenü",
            .ForeColor = Color.White,
            .Font = New Font("Arial", 20, FontStyle.Bold),
            .Size = New Size(200, 40), ' Breite an die Buttons angepasst
            .Location = New Point(50, 20), ' Zentriert im Panel
            .TextAlign = ContentAlignment.MiddleCenter, ' Text zentriert
            .BackColor = Color.DarkGray ' Hintergrundfarbe hinzugefügt
        }

        ' Resume-Button
        btnResume = New Button() With {
        .Text = "Fortsetzen",
        .ForeColor = Color.White,
        .Size = New Size(200, 40),
        .Location = New Point(50, 70)
        }
        AddHandler btnResume.Click, AddressOf ResumeGame

        ' Restart-Button
        btnRestart = New Button() With {
        .Text = "Neustarten",
        .ForeColor = Color.White,
        .Size = New Size(200, 40),
        .Location = New Point(50, 120)
        }
        AddHandler btnRestart.Click, AddressOf RestartGame

        ' Einstellungen-Button
        btnSettings = New Button() With {
        .Text = "Einstellungen",
        .ForeColor = Color.White,
        .Size = New Size(200, 40),
        .Location = New Point(50, 170)
        }
        AddHandler btnSettings.Click, AddressOf OpenSettings

        ' Hauptmenü-Button
        btnMainMenu = New Button() With {
        .Text = "Zum Hauptmenü",
        .ForeColor = Color.White,
        .Size = New Size(200, 40),
        .Location = New Point(50, 220)
        }
        AddHandler btnMainMenu.Click, AddressOf ReturnToMainMenu

        ' Beenden-Button
        btnExit = New Button() With {
        .Text = "Spiel Beenden",
        .ForeColor = Color.White,
        .Size = New Size(200, 40),
        .Location = New Point(50, 270)
        }
        AddHandler btnExit.Click, AddressOf ExitGame

        ' Button zum Öffnen des Pausenmenüs
        btnOpenPauseMenu = New Button() With {
            .Text = "Pausenmenü",
            .ForeColor = Color.Gray,
            .Font = New Font("Arial", 8.5, FontStyle.Italic),
            .Location = New Point(10, 10),
            .Size = New Size(200, 30),
            .FlatStyle = FlatStyle.Flat,
            .BackColor = Color.Black,
            .TabStop = False
        }
        AddHandler btnOpenPauseMenu.Click, AddressOf PauseGame

        AddHandler btnExit.Click, AddressOf ExitGame

        ' Buttons zum Panel hinzufügen
        pauseMenu.Controls.Add(btnResume)
        pauseMenu.Controls.Add(lblPauseTitle)
        pauseMenu.Controls.Add(btnRestart)
        pauseMenu.Controls.Add(btnSettings)
        pauseMenu.Controls.Add(btnMainMenu)
        pauseMenu.Controls.Add(btnExit)

        ' Elemente zum Formular hinzufügen
        Me.Controls.Add(player1Bat)
        Me.Controls.Add(player2Bat)
        Me.Controls.Add(ballControl)
        Me.Controls.Add(player1Score)
        Me.Controls.Add(player2Score)
        Me.Controls.Add(pauseMenu)
        Me.Controls.Add(btnOpenPauseMenu)
        ' SetStyle für das Formular setzen
        Me.SetStyle(ControlStyles.Selectable, False)

        ' Key-Events für Steuerung
        AddHandler Me.KeyDown, AddressOf frmPong_KeyDown
        AddHandler Me.PreviewKeyDown, AddressOf frmPong_PreviewKeyDown

        ' TabStop für alle Steuerelemente deaktivieren
        DisableTabStop(Me)
    End Sub

    'Methode zum Deaktivieren des Schließen-Buttons da das den Programm nicht vollständig beendet
    Protected Overrides ReadOnly Property CreateParams() As CreateParams
        Get
            Dim cp As CreateParams = MyBase.CreateParams
            ' CS_NOCLOSE hat den Wert &H200: schaltet die Close-Schaltfläche ab.
            cp.ClassStyle = cp.ClassStyle Or &H200
            Return cp
        End Get
    End Property


    Private Sub frmPong_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SettingsManager.LoadSettings()
        ApplyGameSettings()
    End Sub


    Private Sub ApplyGameSettings()
        ' Punktestand zurücksetzen
        player1Score.Text = "0"
        player2Score.Text = "0"

        ' Wer serviert zuerst?
        Select Case SettingsManager.WhoStarts
            Case "Torschießer"
            ' Der Spieler, der den letzten Punkt gemacht hat, serviert.
            Case "Wechselt ab"
            ' Nach jedem Punkt wechselt der Aufschlag
            Case "Zufall"
                ' Zufällige Auswahl des Aufschlägers
        End Select

        ' Ballgeschwindigkeit nach einem Punkt setzen
        If SettingsManager.BallSpeedOnServe = True Then
            Dim baseSpeed As Integer = If(SettingsManager.BallSpeedOnServe, 3, 6)
            Dim randomDirection As Integer = If(Rnd() > 0.5, -1, 1)

            ballSpeedX = baseSpeed * randomDirection
            ballSpeedY = baseSpeed * (If(Rnd() > 0.5, 1, -1))
        End If

        ' Steuerungssensitivität setzen
        Select Case SettingsManager.ControlSensitivity
            Case "Niedrig"
                playerSpeed = 6
            Case "Normal"
                playerSpeed = 10
            Case "Hoch"
                playerSpeed = 14
        End Select

        ' Punktelimit setzen
        Select Case SettingsManager.ScoreToWin
            Case 3
            ' Punktelimit auf 3 setzen
            Case 5
            ' Punktelimit auf 5 setzen
            Case 10
            ' Punktelimit auf 10 setzen
            Case 15
            ' Punktelimit auf 15 setzen
            Case 20
                ' Punktelimit auf 20 setzen
        End Select

        ' Einzelspieler Steuerung setzen
        Select Case SettingsManager.SingleplayerControl
            Case "Maus"
            ' Steuerung auf Maus setzen
            Case "Tastatur"
                ' Steuerung auf Tastatur setzen
        End Select

        ' Bot Schwierigkeit setzen
        Select Case SettingsManager.BotDifficulty
            Case "Einfach"
            ' Bot auf einfach setzen
            Case "Mittel"
            ' Bot auf mittel setzen
            Case "Hart"
            ' Bot auf hart setzen
            Case "Unmöglich"
                ' Bot auf hardcore setzen
        End Select

        ' Ballgeschwindigkeitszunahme setzen
        Select Case SettingsManager.BallSpeedIncreaseRate
            Case 1
            ' Ballgeschwindigkeitszunahme auf 1 setzen
            Case 2
            ' Ballgeschwindigkeitszunahme auf 2 setzen
            Case 3
            ' Ballgeschwindigkeitszunahme auf 3 setzen
            Case 4
            ' Ballgeschwindigkeitszunahme auf 4 setzen
            Case 5
                ' Ballgeschwindigkeitszunahme auf 5 setzen
        End Select

        Select Case SettingsManager.GameMode
            Case "Singleplayer"
            ' Singleplayer-Modus aktivieren
            Case "Multiplayer"
                ' Multiplayer-Modus aktivieren
        End Select
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

    ' Methode zum Entfernen des Fokus
    Private Sub RemoveFocus(sender As Object, e As EventArgs)
        Me.ActiveControl = Nothing
        focusTimer.Stop()
    End Sub

    ' **Spiel pausieren & fortsetzen**
    Private Sub PauseGame()
        isPaused = True
        gameTimer.Stop()
        pauseMenu.Visible = True
        Debug.WriteLine("PauseGame aufgerufen: isPaused = " & isPaused)
    End Sub

    Private Sub ResumeGame(sender As Object, e As EventArgs)
        isPaused = False ' <-- Setzt isPaused explizit auf False, damit ESC wieder funktioniert
        pauseMenu.Visible = False
        gameTimer.Start()
        Debug.WriteLine("ResumeGame aufgerufen: isPaused = " & isPaused)
    End Sub

    'Falls ein Spieler die Maximale Punktzahl erreicht hat
    Private Sub ResetGame()
        ' Setze Positionen zurück
        player1Bat.Location = New Point(30, (Me.ClientSize.Height / 2) - (player1Bat.Height / 2))
        player2Bat.Location = New Point(Me.ClientSize.Width - 40, (Me.ClientSize.Height / 2) - (player2Bat.Height / 2))
        ballControl.Location = New Point((Me.ClientSize.Width / 2) - (ballControl.Width / 2), (Me.ClientSize.Height / 2) - (ballControl.Height / 2))

        ' Setze Punkte zurück
        player1Score.Text = "0"
        player2Score.Text = "0"
    End Sub


    ' **Spiel neustarten**
    Private Sub RestartGame()
        Me.Close()
        Dim newGame As New frmPong()
        newGame.Show()
    End Sub

    ' **Einstellungen öffnen**
    Private Sub OpenSettings(sender As Object, e As EventArgs)
        Dim settingsForm As New frmSettings()
        settingsForm.ShowDialog()
    End Sub

    ' **Zurück zum Hauptmenü**
    Private Sub ReturnToMainMenu(sender As Object, e As EventArgs)
        Me.Close()
        Dim mainMenu As New Form1()
        mainMenu.Show()
    End Sub

    ' **Spiel beenden**
    Private Sub ExitGame(sender As Object, e As EventArgs)
        Application.Exit()
    End Sub



    Private Sub ResetBall()
        ballControl.Location = New Point((Me.ClientSize.Width / 2) - (ballControl.Width / 2), (Me.ClientSize.Height / 2) - (ballControl.Height / 2))

        Select Case SettingsManager.WhoStarts
            Case "Torschießer"
                If scorePlayer1 > scorePlayer2 Then
                    ballSpeedX = 5
                ElseIf scorePlayer2 > scorePlayer1 Then
                    ballSpeedX = -5
                Else
                    ballSpeedX = 5 * If(New Random().Next(0, 2) = 0, -1, 1) ' Zufällige Richtung
                End If
            Case "Wechselt ab"
                Static lastServer As Integer = 1
                If lastServer = 1 Then
                    ballSpeedX = -5
                    lastServer = 2
                Else
                    ballSpeedX = 5
                    lastServer = 1
                End If
            Case "Zufall"
                ballSpeedX = 5 * If(New Random().Next(0, 2) = 0, -1, 1) ' Zufällige Richtung
        End Select

        ballSpeedY = 3 * If(New Random().Next(0, 2) = 0, -1, 1)
    End Sub


    ' Punktestand aktualisieren
    Private Sub UpdateScore()
        player1Score.Text = scorePlayer1.ToString()
        player2Score.Text = scorePlayer2.ToString()

        If gameOver = True Then Exit Sub

        If scorePlayer1 >= SettingsManager.ScoreToWin Then
            gameOver = True
            isPaused = True
            MessageBox.Show("Spieler 1 gewinnt!", "Spiel beendet", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Close()
            Dim mainMenu As New Form1()
            mainMenu.Show()
        ElseIf scorePlayer2 >= SettingsManager.ScoreToWin Then
            gameOver = True
            isPaused = True
            MessageBox.Show("Spieler 2 gewinnt!", "Spiel beendet", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Close()
            Dim mainMenu As New Form1()
            mainMenu.Show()
        End If
    End Sub


    Private Sub frmPong_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        Debug.WriteLine("KeyDown-Ereignis ausgelöst: " & e.KeyCode.ToString())

        If SettingsManager.GameMode = "Singleplayer" Then
            ' Singleplayer-Steuerung
            If SettingsManager.SingleplayerControl = "Tastatur" Then
                Select Case e.KeyCode
                    Case Keys.Up
                        moveUpP2 = True
                    Case Keys.Down
                        moveDownP2 = True
                End Select
            End If
        Else
            ' Multiplayer-Steuerung
            Select Case e.KeyCode
                Case Keys.W
                    moveUpP1 = True
                Case Keys.S
                    moveDownP1 = True
                Case Keys.Up
                    moveUpP2 = True
                Case Keys.Down
                    moveDownP2 = True
            End Select
        End If
    End Sub





    Private Sub frmPong_MouseMove(sender As Object, e As MouseEventArgs) Handles MyBase.MouseMove
        If SettingsManager.SingleplayerControl = "Maus" Then
            player2Bat.Top = Math.Max(0, Math.Min(Me.ClientSize.Height - player2Bat.Height, e.Y - (player2Bat.Height / 2)))
        End If
    End Sub



    Private Sub frmPong_KeyUp(sender As Object, e As KeyEventArgs) Handles MyBase.KeyUp
        If SettingsManager.GameMode = "Singleplayer" Then
            ' Singleplayer-Steuerung
            If SettingsManager.SingleplayerControl = "Tastatur" Then
                Select Case e.KeyCode
                    Case Keys.Up
                        moveUpP2 = False
                    Case Keys.Down
                        moveDownP2 = False
                End Select
            End If
        Else
            ' Multiplayer-Steuerung
            Select Case e.KeyCode
                Case Keys.W
                    moveUpP1 = False
                Case Keys.S
                    moveDownP1 = False
                Case Keys.Up
                    moveUpP2 = False
                Case Keys.Down
                    moveDownP2 = False
            End Select
        End If
    End Sub



    ' PreviewKeyDown-Ereignis hinzufügen
    Private Sub frmPong_PreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs) Handles MyBase.PreviewKeyDown
        e.IsInputKey = True
    End Sub

    Private Sub GameLoop(sender As Object, e As EventArgs)
        If isPaused Then Exit Sub

        ' Anzahl der Schritte für die Sub-Stepping-Simulation
        Dim maxSteps As Integer = 10 ' Fester Wert für die Anzahl der Schritte
        Dim steps As Integer = Math.Min(maxSteps, Math.Max(Math.Abs(ballSpeedX), Math.Abs(ballSpeedY)))
        Dim stepX As Single = ballSpeedX / steps
        Dim stepY As Single = ballSpeedY / steps

        For i As Integer = 1 To steps
            ' Ballbewegung in kleinen Schritten
            ballControl.Left += stepX
            ballControl.Top += stepY

            ' Kollision mit oberer und unterer Wand
            If ballControl.Top <= 0 Or ballControl.Bottom >= Me.ClientSize.Height Then
                ballSpeedY = -ballSpeedY
                stepY = -stepY
            End If

            ' Kollision mit Schlägern
            If ballControl.Bounds.IntersectsWith(player1Bat.Bounds) Then
                ballSpeedX = Math.Abs(ballSpeedX) ' Ball nach rechts bewegen
                ballSpeedX += Math.Sign(ballSpeedX) + SettingsManager.BallSpeedIncreaseRate
                ballSpeedY += Math.Sign(ballSpeedY) + SettingsManager.BallSpeedIncreaseRate
                stepX = Math.Abs(stepX)
            End If

            If ballControl.Bounds.IntersectsWith(player2Bat.Bounds) Then
                ballSpeedX = -Math.Abs(ballSpeedX) ' Ball nach links bewegen
                stepX = -Math.Abs(stepX)
            End If

            ' Punktestand prüfen
            If ballControl.Left <= 0 Then
                scorePlayer2 += 1
                UpdateScore()
                ResetBall()
                Exit For
            ElseIf ballControl.Right >= Me.ClientSize.Width Then
                scorePlayer1 += 1
                UpdateScore()
                ResetBall()
                Exit For
            End If
        Next

        ' Bot-Bewegung
        If SettingsManager.GameMode = "Singleplayer" Then
            MoveBot()
        End If

        ' Spieler 1 Bewegung
        If moveUpP1 AndAlso player1Bat.Top > 0 Then
            player1Bat.Top -= playerSpeed
        End If
        If moveDownP1 AndAlso player1Bat.Bottom < Me.ClientSize.Height Then
            player1Bat.Top += playerSpeed
        End If

        ' Spieler 2 Bewegung
        If moveUpP2 AndAlso player2Bat.Top > 0 Then
            player2Bat.Top -= playerSpeed
        End If
        If moveDownP2 AndAlso player2Bat.Bottom < Me.ClientSize.Height Then
            player2Bat.Top += playerSpeed
        End If

        ' Begrenzung der Schlägerbewegung
        player1Bat.Top = Math.Max(0, Math.Min(Me.ClientSize.Height - player1Bat.Height, player1Bat.Top))
        player2Bat.Top = Math.Max(0, Math.Min(Me.ClientSize.Height - player2Bat.Height, player2Bat.Top))
    End Sub



    ' Dokumentierte If Sätze für Bot-Bewegung. Alle Kommentare die If-Sätze sind, dienen zum Experiment wo der Bot beide Schläger bewegen soll.

    Private Sub MoveBot()
        Select Case SettingsManager.BotDifficulty
            Case "Einfach"
                ' Bot bewegt sich langsam und trifft den Ball nicht immer
                If ballControl.Top < player1Bat.Top Then
                    player1Bat.Top -= playerSpeed / 2
                ElseIf ballControl.Bottom > player1Bat.Bottom Then
                    player1Bat.Top += playerSpeed / 2
                End If
                ' Uncomment to let the bot control player2Bat as well
            'If ballControl.Top < player2Bat.Top Then
            '    player2Bat.Top -= playerSpeed / 2
            'ElseIf ballControl.Bottom > player2Bat.Bottom Then
            '    player2Bat.Top += playerSpeed / 2
            'End If
            Case "Mittel"
                ' Bot bewegt sich mit mittlerer Geschwindigkeit und trifft den Ball häufiger
                If ballControl.Top < player1Bat.Top Then
                    player1Bat.Top -= playerSpeed
                ElseIf ballControl.Bottom > player1Bat.Bottom Then
                    player1Bat.Top += playerSpeed
                End If
                ' Uncomment to let the bot control player2Bat as well
                'If ballControl.Top < player2Bat.Top Then
                '    player2Bat.Top -= playerSpeed
                'ElseIf ballControl.Bottom > player2Bat.Bottom Then
                '    player2Bat.Top += playerSpeed
            'End If
            Case "Hart"
                ' Bot bewegt sich schnell und trifft den Ball fast immer
                If ballControl.Top < player1Bat.Top Then
                    player1Bat.Top -= playerSpeed * 1.5
                ElseIf ballControl.Bottom > player1Bat.Bottom Then
                    player1Bat.Top += playerSpeed * 1.5
                End If
                ' Uncomment to let the bot control player2Bat as well
            'If ballControl.Top < player2Bat.Top Then
            '    player2Bat.Top -= playerSpeed * 1.5
            'ElseIf ballControl.Bottom > player2Bat.Bottom Then
            '    player2Bat.Top += playerSpeed * 1.5
            'End If
            Case "Unmöglich"
                ' Bot trifft den Ball (fast) immer
                player1Bat.Top = ballControl.Top - (player1Bat.Height / 2) + (ballControl.Height / 2)
                ' Uncomment to let the bot control player2Bat as well
                'player2Bat.Top = ballControl.Top - (player2Bat.Height / 2) + (ballControl.Height / 2)
        End Select
    End Sub





    ' Pause umschalten
    Private Sub TogglePause()
        isPaused = Not isPaused
        If isPaused Then
            gameTimer.Stop()
        ElseIf isPaused = False Then
            gameTimer.Start()
            isPaused = False
        End If
    End Sub

    ' Dieses Ereignis wird ausgelöst, wenn das Formular den Fokus erhält.
    Private isInactive As Boolean = False

    ' Prüft, wenn das Formular den Fokus verliert.
    Private Sub frmPong_LostFocus(sender As Object, e As EventArgs) Handles Me.LostFocus
        isInactive = True
        Debug.WriteLine("Das Formular hat den Fokus verloren.")
    End Sub

    ' Prüft, wenn das Formular wieder den Fokus erhält.
    Private Sub frmPong_GotFocus(sender As Object, e As EventArgs) Handles Me.GotFocus
        isInactive = False
        Debug.WriteLine("Das Formular hat den Fokus zurückerhalten.")
    End Sub

    ' Prüft, wenn das Formular minimiert wird.
    Private Sub frmPong_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        If Me.WindowState = FormWindowState.Minimized Then
            isPaused = True
            Debug.WriteLine("Das Formular wurde minimiert.")
        Else
            isPaused = False
            Debug.WriteLine("Das Formular ist nicht minimiert.")
        End If
    End Sub

End Class
