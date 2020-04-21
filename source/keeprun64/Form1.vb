Imports System
Imports System.IO
Imports System.Diagnostics
Imports System.Threading
Imports System.Timers

Public Class Form1
    Public Const DefaultIniName As String = "keeprun64.ini"
    Public Const DefaultInterval As Integer = 2000
    Public Const DefaultReboot As String = "NO"
    Private _PathToSettings As String
    Private _Detect As String
    Private _Interval As Integer
    Private _Launch As String
    Private _Arguments As String
    Private _Reboot As String
    Private Shared aTimer As System.Timers.Timer

    'Used when parsing text files
    Private Const CommentDelimiter = ";"
    Private _SettingKeyValyeDelimiter() As Char = {"="c}
    Private _listDelimiter() As Char = {","c}
    Private _SimpleDnsDelimiter() As Char = {ControlChars.Tab, " "}

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Hide()
        ReadSettingsIni()

        If _Launch <> String.Empty Then
            aTimer = New System.Timers.Timer(10000)
            AddHandler aTimer.Elapsed, AddressOf CheckIfRunning
            aTimer.Interval = _Interval
            aTimer.Enabled = True
        End If

    End Sub

    Private Sub CheckIfRunning()
        Dim p() As Process
        p = Process.GetProcessesByName(_Detect)
        If p.Count > 0 Then
            ' Process is running
        Else
            ' Process is not running
            If _Reboot = "NO" Then
                Dim Start As New ProcessStartInfo
                Start.UseShellExecute = True
                Start.ErrorDialog = False
                Start.FileName = _Launch
                If _Arguments <> String.Empty Then
                    Start.Arguments = _Arguments
                End If
                Try
                    Process.Start(Start)
                Catch ex As Exception
                    'do nothing
                End Try
            ElseIf _Reboot = "YES" Then
                System.Diagnostics.Process.Start("shutdown.exe", "-r -t 0")
            End If
        End If
    End Sub

    Protected Sub ReadSettingsIni()

        If Not File.Exists(_PathToSettings & DefaultIniName) Then
            MsgBox("Error: keeprun64.ini not found", MsgBoxStyle.OkOnly, "Keep Running x64")
            Me.Close()
        End If

        Using SettingsStream As StreamReader = File.OpenText(_PathToSettings & DefaultIniName)

            While Not SettingsStream.EndOfStream 'If the character is -1 then we are at the end of the file
                Dim SettingsLine As String = SettingsStream.ReadLine()
                'Skip comment lines
                If String.IsNullOrEmpty(SettingsLine) OrElse SettingsLine.StartsWith(CommentDelimiter, StringComparison.OrdinalIgnoreCase) Then Continue While

                Dim Setting() As String = SettingsLine.Split(_SettingKeyValyeDelimiter)

                If Setting.Length = 2 Then 'if we have 2 items we have a key and value, else we don't know what we have

                    'Init the known settings
                    Select Case Setting(0).ToUpperInvariant 'Comparing uppercase strings is faster

                        Case "DETECT"
                            If _Detect Is Nothing AndAlso String.IsNullOrEmpty(Setting(1)) Then
                                _Detect = String.Empty
                                MsgBox("Error: Detect= not defined in INI", MsgBoxStyle.OkOnly, "Keep Running x64")
                                Me.Close()
                            ElseIf _Detect <> Setting(1) Then
                                _Detect = Setting(1)
                            End If

                        Case "INTERVAL"
                            If Not String.IsNullOrEmpty(Setting(1)) Then
                                Dim NewInterval As Integer
                                If Not Integer.TryParse(Setting(1), NewInterval) Then
                                    MsgBox("Error: Interval= not valid in INI, using default of 2 seconds", MsgBoxStyle.OkOnly, "Keep Running x64")
                                    _Interval = DefaultInterval
                                Else
                                    NewInterval *= 1000 'convert the seconds in the file to milliseconds
                                    If NewInterval <> _Interval Then
                                        _Interval = NewInterval
                                    End If
                                End If
                            End If

                        Case "LAUNCH"
                            If _Launch Is Nothing AndAlso String.IsNullOrEmpty(Setting(1)) Then
                                _Launch = String.Empty
                                MsgBox("Error: Launch= not defined in INI", MsgBoxStyle.OkOnly, "Keep Running x64")
                                Me.Close()
                            ElseIf _Launch <> Setting(1) Then
                                _Launch = Setting(1)
                            End If

                        Case "ARGUMENTS"
                                If _Arguments Is Nothing AndAlso String.IsNullOrEmpty(Setting(1)) Then
                                    _Arguments = String.Empty
                                ElseIf _Arguments <> Setting(1) Then
                                    _Arguments = Setting(1)
                                End If

                        Case "REBOOT"
                            If _Reboot Is Nothing AndAlso String.IsNullOrEmpty(Setting(1)) Then
                                _Reboot = String.Empty
                                MsgBox("Error: Reboot= not defined in INI, using default of No", MsgBoxStyle.OkOnly, "Keep Running x64")
                                _Reboot = DefaultReboot
                            ElseIf _Reboot <> Setting(1) Then
                                _Reboot = Setting(1).ToUpperInvariant
                            End If

                    End Select
                End If
            End While
        End Using 'Closes the stream

    End Sub

End Class