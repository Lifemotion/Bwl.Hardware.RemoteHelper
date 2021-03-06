﻿Imports Bwl.Framework
Imports Bwl.Network.ClientServer

Module TestApp
    Private _app As New AppBase
    'TODO: перенести автоконнект во фреймворк
    Private _addressSetting As New StringSetting(_app.RootStorage, "ServerAddress", "dev.cleverflow.ru")
    Private _portSetting As New IntegerSetting(_app.RootStorage, "ServerPort", 3180)
    Private _userSetting As New StringSetting(_app.RootStorage, "ServerUser", "RemoteHelper-Remote")
    Private _passwordSetting As New StringSetting(_app.RootStorage, "ServerPassword", "")

    Private _targetSetting As New StringSetting(_app.RootStorage, "TargetID", "RemoteHelper-Local1")
    Private _transport As New NetClient
    Private _form As New RemoteHelperControlForm(_app, _transport, _userSetting.Value, _targetSetting.Value)

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
        Application.EnableVisualStyles()
        Application.Run(_form)
    End Sub

End Module
