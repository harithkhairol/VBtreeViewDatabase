Imports System.Data.OleDb

Public Class Form1

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        'TODO: This line of code loads data into the 'Testdb4DataSet1.workingdays' table. You can move, or remove it, as needed.
        Me.WorkingdaysTableAdapter.Fill(Me.Testdb4DataSet1.workingdays)
        'TODO: This line of code loads data into the 'Testdb4DataSet1.employees' table. You can move, or remove it, as needed.
        Me.EmployeesTableAdapter.Fill(Me.Testdb4DataSet1.employees)



        EmployeesTableAdapter.Connection.Open()
        '   TreeView1.Nodes.Clear()
        FillTree("1", "Level", Nothing)
        EmployeesTableAdapter.Connection.Close()


    End Sub

    Public Sub FillTree(ByVal Key As String, ByVal Txt As String, ByVal N As TreeNode)

        Dim cn As OleDbConnection
        Dim cmd As OleDbCommand
        Dim NN As TreeNode

        'if N has no parent than N is parent

        If N Is Nothing Then
            NN = TreeView1.Nodes.Add(Key, Txt)

            'if N has parent than it become child node
        Else
            NN = N.Nodes.Add(Key, Txt)
        End If

        cn = EmployeesTableAdapter.Connection()
        cmd = New OleDbCommand("select * from employees where node_parent='" & Key & "'", cn)

        Dim dr = cmd.ExecuteReader
        Do While dr.Read()
            FillTree(dr("node"), dr("node_name"), NN)
        Loop

        dr.Close()
        cmd.Dispose()

    End Sub

    Private Sub EmployeesBindingNavigatorSaveItem_Click(sender As System.Object, e As System.EventArgs)
        Me.Validate()
        Me.EmployeesBindingSource.EndEdit()
        Me.TableAdapterManager.UpdateAll(Me.Testdb4DataSet1)

    End Sub

    Private Sub TreeView1_AfterSelect(sender As System.Object, e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView1.AfterSelect

        If TreeView1.SelectedNode Is Nothing Then

            DGV.DataSource = Nothing

            Exit Sub

        End If

        If TreeView1.SelectedNode.Name = "root" Then

            DGV.DataSource = Nothing
            Exit Sub

        End If

        Dim ID As String
        ID = TreeView1.SelectedNode.Name




        DGV.DataSource = Testdb4DataSet1.workingdays.Select("node='" & ID & "'")





        DGV.Visible = True






    End Sub

    Private Sub WorkingdaysDataGridView_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DGV.CellContentClick

    End Sub
End Class
