Imports Bwl.Hardware.SimplSerial

Class RemoteHelperBoard
    ReadOnly Property SS As New SimplSerialBus()

    Public Sub New()

    End Sub

    Public Sub Connect(port As String)
        _ss.Disconnect()
        _ss.SerialDevice.DeviceAddress = port
        _ss.SerialDevice.DeviceSpeed = 9600
        _ss.Connect()
    End Sub

    Public Sub PointerMove(horizontal As Integer, horizontalStop As Integer, vertical As Integer, verticalStop As Integer)
        Dim data(5) As Byte
        data(0) = horizontal / 10 + 127
        data(1) = horizontalStop
        data(2) = vertical / 10 + 127
        data(3) = verticalStop
        _ss.Send(New SSRequest(0, 5, data))
    End Sub
End Class
