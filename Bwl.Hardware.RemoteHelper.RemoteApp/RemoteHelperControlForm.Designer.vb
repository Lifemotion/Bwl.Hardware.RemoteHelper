Imports Bwl.Network.ClientServer
Imports Bwl.Framework

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class RemoteHelperControlForm

    'Форма переопределяет dispose для очистки списка компонентов.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Является обязательной для конструктора форм Windows Forms
    Private components As System.ComponentModel.IContainer


    'Примечание: следующая процедура является обязательной для конструктора форм Windows Forms
    'Для ее изменения используйте конструктор форм Windows Form.  
    'Не изменяйте ее в редакторе исходного кода.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(RemoteHelperControlForm))
        Me.bCheckBoard = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'logWriter
        '
        Me.logWriter.ExtendedView = False
        Me.logWriter.Location = New System.Drawing.Point(0, 233)
        Me.logWriter.Size = New System.Drawing.Size(853, 310)
        '
        'bCheckBoard
        '
        Me.bCheckBoard.Location = New System.Drawing.Point(12, 27)
        Me.bCheckBoard.Name = "bCheckBoard"
        Me.bCheckBoard.Size = New System.Drawing.Size(75, 23)
        Me.bCheckBoard.TabIndex = 2
        Me.bCheckBoard.Text = "CheckBoard"
        Me.bCheckBoard.UseVisualStyleBackColor = True
        '
        'RemoteHelperControlForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(853, 543)
        Me.Controls.Add(Me.bCheckBoard)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "RemoteHelperControlForm"
        Me.Text = "RemoteHelper Control Form"
        Me.Controls.SetChildIndex(Me.logWriter, 0)
        Me.Controls.SetChildIndex(Me.bCheckBoard, 0)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents bCheckBoard As Button
End Class
