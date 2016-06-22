Imports Bwl.Network.ClientServer
Imports Bwl.Framework

Module TestApp
    Private _app As New AppBase
    Private _board As New RemoteHelperBoard
    Private _ui As New RemoteHelperAutoUI(_app, _board)

    Private _addressSetting As New StringSetting(_app.RootStorage, "ServerAddress", "dev.cleverflow.ru")
    Private _portSetting As New IntegerSetting(_app.RootStorage, "ServerPort", 3180)
    Private _userSetting As New StringSetting(_app.RootStorage, "ServerUser", "RemoteHelper-Local1")
    Private _passwordSetting As New StringSetting(_app.RootStorage, "ServerPassword", "")
    Private WithEvents _transport As New NetClient

    Public Sub Main()
        Dim connectThread As New Threading.Thread(Sub()
                                                      Threading.Thread.Sleep(1000)
                                                      Do
                                                          Try
                                                              If _transport.IsConnected = False Then
                                                                  _transport.Connect(_addressSetting.Value, _portSetting.Value)
                                                                  _transport.RegisterMe(_userSetting.Value, _passwordSetting.Value, "")
                                                                  _app.RootLogger.AddMessage("Connected to server " + _addressSetting.Value + ":" + _portSetting.Value.ToString + " as " + _userSetting.Value)
                                                              End If
                                                          Catch ex As Exception
                                                              _app.RootLogger.AddWarning(ex.Message)
                                                          End Try
                                                      Loop
                                                  End Sub)
        connectThread.IsBackground = True
        connectThread.Start()

        _ui.RunApp()
    End Sub

    Private Sub _transport_ReceivedMessage(message As NetMessage) Handles _transport.ReceivedMessage
        Select Case message.Part(0).ToLower
            Case "checkboard"
                Dim msg As New NetMessage("S", "checkboard-result")
                msg.ToID = message.FromID
                msg.FromID = message.ToID
                _transport.SendMessage(msg)
            Case "pointermove"
                Try
                    _board.PointerMove(message.PartDouble(1), message.PartDouble(2), message.PartDouble(3), message.PartDouble(4))
                    _app.RootLogger.AddDebug(message.ToString)
                Catch ex As Exception
                    Dim msg As New NetMessage("S", "board-error", ex.Message)
                    msg.ToID = message.FromID
                    msg.FromID = message.ToID
                    _transport.SendMessage(msg)
                End Try
        End Select
    End Sub
End Module
