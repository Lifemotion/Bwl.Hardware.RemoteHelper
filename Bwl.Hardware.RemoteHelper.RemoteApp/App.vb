Imports Bwl.Framework

Module TestApp
    Private _app As New AppBase
    Private _board As New RemoteHelperBoard
    Private _logger As Logger = _app.RootLogger

    Private _ascii As System.Text.Encoding = System.Text.Encoding.ASCII
    Private WithEvents _connectButton As New AutoButton(_app.AutoUI, "Connect")
    Private WithEvents _simplSerialToolButton As New AutoButton(_app.AutoUI, "SimplSerialTool")
    Private WithEvents _upButton As New AutoButton(_app.AutoUI, "Up")
    Private WithEvents _downButton As New AutoButton(_app.AutoUI, "Down")
    Private WithEvents _leftButton As New AutoButton(_app.AutoUI, "Left")
    Private WithEvents _rightButton As New AutoButton(_app.AutoUI, "Right")
    Private WithEvents _adc0Button As New AutoButton(_app.AutoUI, "Adc0")
    Private WithEvents _adcAll As New AutoButton(_app.AutoUI, "AdcAll")

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
        _board.Connect(IO.Ports.SerialPort.GetPortNames(0))
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

    Private Sub _adc0Button_Click(source As AutoButton) Handles _adc0Button.Click
        Try
            _logger.AddMessage("Adc0: " + _board.GetAdcSingleAvg(0).ToString("0.00"))
            Dim time = Now
            Dim values As New List(Of Int16)
            Do While (Now - time).TotalMilliseconds < 1000
                values.Add(_board.GetAdcSingleContinue())
            Loop
            _logger.AddMessage("Continous Rate: " + values.Count.ToString + " Hz")
        Catch ex As Exception
            _logger.AddWarning(ex.Message)
        End Try
    End Sub

    Private Sub _adcAll_Click(source As AutoButton) Handles _adcAll.Click
        Try
            Dim results = _board.GetAdcAllAvg()
            For i = 0 To results.Length - 1
                _logger.AddMessage("Adc" + i.ToString + ": " + results(i).ToString("0.00"))
            Next
            Dim time = Now
            Dim values As New List(Of Single())
            Do While (Now - time).TotalMilliseconds < 1000
                values.Add(_board.GetAdcAllAvg())
            Loop
            _logger.AddMessage("Continous Rate: " + values.Count.ToString + " Hz")
        Catch ex As Exception
            _logger.AddWarning(ex.Message)
        End Try
    End Sub
End Module
