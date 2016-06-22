Imports Bwl.Hardware.SimplSerial

Class RemoteHelperBoard
    ReadOnly Property SS As New SimplSerialBus()

    Public Sub New()

    End Sub

    Public Sub Connect(port As String)
        _ss.Disconnect()
        _ss.SerialDevice.DeviceAddress = port
        _SS.SerialDevice.DeviceSpeed = 250000
        _SS.Connect()
    End Sub

    Public Sub PointerMove(horizontal As Integer, horizontalStop As Integer, vertical As Integer, verticalStop As Integer)
        Dim data(5) As Byte
        data(0) = horizontal / 10 + 127
        data(1) = horizontalStop
        data(2) = vertical / 10 + 127
        data(3) = verticalStop
        _ss.Send(New SSRequest(0, 5, data))
    End Sub

    Public Property AdcRegularMultiplier As Single = (68 + 7.5) / 7.5 / 1024 * 2.56

    Public Function GetAdcSingleAvg(channel As Byte, Optional avgCount As Byte = 4) As Single
        Dim command = 10
        Dim result = _SS.Request(New SSRequest(0, command, {channel, avgCount}))
        If result.ResponseState <> ResponseState.ok Or result.Result <> 128 + command Then Throw New Exception("Wrong SS response: " + result.ToString)
        Dim val As Integer = result.Data(0) * 256 + result.Data(1)
        Return val * AdcRegularMultiplier
    End Function

    Public Function GetAdcSingleContinue() As Single
        Dim command = 11
        Dim result = _SS.Request(New SSRequest(0, command, {}))
        If result.ResponseState <> ResponseState.ok Or result.Result <> 128 + command Then Throw New Exception("Wrong SS response: " + result.ToString)
        Dim val As Integer = result.Data(0) * 256 + result.Data(1)
        Return val * AdcRegularMultiplier
    End Function

    Public Function GetAdcAllAvg(Optional avgCount As Byte = 4) As Single()
        Dim command = 12
        Dim result = _SS.Request(New SSRequest(0, command, {0, avgCount}))
        If result.ResponseState <> ResponseState.ok Or result.Result <> 128 + command Then Throw New Exception("Wrong SS response: " + result.ToString)
        Dim results As New List(Of Single)
        For i = 0 To 7
            Dim val As Integer = result.Data(i * 2 + 0) * 256 + result.Data(i * 2 + 1)
            results.Add(val * AdcRegularMultiplier)
        Next
        Return results.ToArray
    End Function

End Class
