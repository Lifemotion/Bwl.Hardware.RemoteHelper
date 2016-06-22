Imports Bwl.Network.ClientServer
Imports Bwl.Framework

Public Class RemoteHelperControlForm
    Inherits FormBase
    Private _app As AppBase
    Private _logger As Logger
    Private WithEvents _transport As IMessageTransport
    Private _userID As String
    Private _targetID As String

    Public Sub New(app As AppBase, transport As NetClient, userId As String, targetId As String)
        InitializeComponent()
        _app = app
        _transport = transport
        _logger = app.RootLogger
        _storageForm = _app.RootStorage
        _loggerServer = _app.RootLogger
        _userID = userId
        _targetID = targetId
    End Sub

    Private Sub _transport_ReceivedMessage(message As NetMessage) Handles _transport.ReceivedMessage
        Select Case message.Part(0).ToLower
            Case "checkboard-result"
                _logger.AddMessage(message.ToString)
            Case "board-error"
                _logger.AddWarning("Board error: " + message.Part(1))
            Case "board-message"
                _logger.AddMessage("Board message: " + message.Part(1))
        End Select
    End Sub

    Private Sub bCheckBoard_Click(sender As Object, e As EventArgs) Handles bCheckBoard.Click
        Dim msg As New NetMessage("S", "checkboard")
        msg.FromID = _userID
        msg.ToID = _targetID
        _transport.SendMessage(msg)
    End Sub

    Private Sub RemoteHelperControlForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Dim x As Integer, y As Integer
        If e.KeyCode = Keys.NumPad4 Then x = -300
        If e.KeyCode = Keys.NumPad6 Then x = 300
        If e.KeyCode = Keys.NumPad8 Then y = -70
        If e.KeyCode = Keys.NumPad2 Then y = 70
        ' If e.Shift Then 
        If My.Computer.Keyboard.AltKeyDown Then x = x / 5 : y = y / 5
        If x <> 0 Or y <> 0 Then
            Dim msg As New NetMessage("S", "pointermove", x.ToString, "0", y.ToString, "0")
            msg.FromID = _userID
            msg.ToID = _targetID
            _transport.SendMessage(msg)
            Threading.Thread.Sleep(50)
        End If
    End Sub

    Private Sub RemoteHelperControlForm_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress

    End Sub
End Class