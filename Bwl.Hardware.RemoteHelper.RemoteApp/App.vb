Imports Bwl.Framework

Module TestApp
    Private _app As New AppBase
    Private _board As New RemoteHelperBoard

    Private _ascii As System.Text.Encoding = System.Text.Encoding.ASCII
    Private WithEvents _connectButton As New AutoButton(_app.AutoUI, "Connect")
    Private WithEvents _simplSerialToolButton As New AutoButton(_app.AutoUI, "SimplSerialTool")
    Private WithEvents _upButton As New AutoButton(_app.AutoUI, "Up")
    Private WithEvents _downButton As New AutoButton(_app.AutoUI, "Down")
    Private WithEvents _leftButton As New AutoButton(_app.AutoUI, "Left")
    Private WithEvents _rightButton As New AutoButton(_app.AutoUI, "Right")


    Public Sub Main()
        Application.EnableVisualStyles()
        Application.Run(AutoUIForm.Create(_app))
    End Sub

    Private Sub _leftButton_Click(source As AutoButton) Handles _leftButton.Click
        Dim hspeed = -40
        If My.Computer.Keyboard.ShiftKeyDown Then hspeed = -200
        _board.PointerMove(hspeed, 0, 0, 0)
    End Sub

    Private Sub _connectButton_Click(source As AutoButton) Handles _connectButton.Click
        _board.Connect("COM24")
    End Sub

    Private Sub _rightButton_Click(source As AutoButton) Handles _rightButton.Click
        Dim hspeed = 40
        If My.Computer.Keyboard.ShiftKeyDown Then hspeed = 200
        _board.PointerMove(hspeed, 0, 0, 0)
    End Sub

    Private Sub _downButton_Click(source As AutoButton) Handles _downButton.Click
        Dim vspeed = 10
        If My.Computer.Keyboard.ShiftKeyDown Then vspeed = 100
        _board.PointerMove(0, 0, vspeed, 1)
    End Sub

    Private Sub _upButton_Click(source As AutoButton) Handles _upButton.Click
        Dim vspeed = -10
        If My.Computer.Keyboard.ShiftKeyDown Then vspeed = -100
        _board.PointerMove(0, 0, vspeed, 1)
    End Sub

    Private Sub _simplSerialToolButton_Click(source As AutoButton) Handles _simplSerialToolButton.Click
        Dim sst = New SimplSerial.SimplSerialTool(_board.SS)
        sst.Show()
    End Sub
End Module
