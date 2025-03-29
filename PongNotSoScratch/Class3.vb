Public Class SettingsManager
    ' Spiel-Einstellungen
    Public Shared Property ScoreToWin As Integer = 10
    Public Shared Property BallSpeedOnServe As Boolean = True
    Public Shared Property ControlSensitivity As String = "Normal"
    Public Shared Property WhoStarts As String = "Wechselt ab"
    Public Shared Property SingleplayerControl As String = "Tastatur"
    Public Shared Property BotDifficulty As String = "Mittel"
    Public Shared Property BallSpeedIncreaseRate As Integer = 1
    Public Shared Property GameMode As String = "Singleplayer"


    ' Speichern der Einstellungen
    Public Shared Sub SaveSettings()
        My.Settings.ScoreToWin = ScoreToWin
        My.Settings.BallSpeedOnServe = BallSpeedOnServe
        My.Settings.ControlSensitivity = ControlSensitivity
        My.Settings.WhoStarts = WhoStarts
        My.Settings.SingleplayerControl = SingleplayerControl
        My.Settings.BotDifficulty = BotDifficulty
        My.Settings.BallSpeedIncreaseRate = BallSpeedIncreaseRate
        My.Settings.GameMode = GameMode
        My.Settings.Save()
        Debug.WriteLine("Einstellungen gespeichert: ScoreToWin = " & ScoreToWin)
        Debug.WriteLine("BallSpeedIncreaseRate: " & BallSpeedIncreaseRate)
    End Sub

    ' Laden der Einstellungen
    Public Shared Sub LoadSettings()
        ScoreToWin = My.Settings.ScoreToWin
        BallSpeedOnServe = My.Settings.BallSpeedOnServe
        ControlSensitivity = My.Settings.ControlSensitivity
        WhoStarts = My.Settings.WhoStarts
        SingleplayerControl = My.Settings.SingleplayerControl
        BotDifficulty = My.Settings.BotDifficulty
        BallSpeedIncreaseRate = My.Settings.BallSpeedIncreaseRate
        GameMode = My.Settings.GameMode
        Debug.WriteLine("ScoreToWin: " & ScoreToWin)
        Debug.WriteLine("BallSpeedIncreaseRate: " & BallSpeedIncreaseRate)
    End Sub
End Class
