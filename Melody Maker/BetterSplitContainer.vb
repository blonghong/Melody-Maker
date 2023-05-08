Imports System.ComponentModel

<DefaultEvent("SplitterMoved")>
Public Class BetterSplitContainer
    Inherits SplitContainer

    Public Property Panel1Collapsible() As Boolean = True
    Public Property Panel2Collapsible() As Boolean = False
    Event OnPanel1Collapsing(ByVal sender As Object, ByVal e As EventArgs)
    Event OnPanel2Collapsing(ByVal sender As Object, ByVal e As EventArgs)
    Event OnPanel1Collapsed(ByVal sender As Object, ByVal e As EventArgs)
    Event OnPanel2Collapsed(ByVal sender As Object, ByVal e As EventArgs)
    Event OnPanel1Uncollapsing(ByVal sender As Object, ByVal e As EventArgs)
    Event OnPanel2Uncollapsing(ByVal sender As Object, ByVal e As EventArgs)
    Event OnPanel1Uncollapsed(ByVal sender As Object, ByVal e As EventArgs)
    Event OnPanel2Uncollapsed(ByVal sender As Object, ByVal e As EventArgs)

    Private Sub BetterSplitContainer_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
        IsSplitterFixed = True
    End Sub

    Private Sub BetterSplitContainer_MouseUp(sender As Object, e As MouseEventArgs) Handles Me.MouseUp
        IsSplitterFixed = False
    End Sub

    Private Sub BetterSplitContainer_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
        If Not IsSplitterFixed Then Exit Sub
        If Not e.Button.Equals(MouseButtons.Left) Then Exit Sub

        Dim z As Integer
        Dim max As Integer
        If Orientation.Equals(Orientation.Vertical) Then
            z = e.X
            max = Width
        Else
            z = e.Y
            max = Height
        End If

        If Panel1Collapsible Then
            If z < Panel1MinSize / 2 Then
                RaiseEvent OnPanel1Collapsing(Me, EventArgs.Empty)
                Panel1Collapsed = True
                RaiseEvent OnPanel1Collapsed(Me, EventArgs.Empty)
            Else
                RaiseEvent OnPanel1Uncollapsing(Me, EventArgs.Empty)
                Panel1Collapsed = False
                RaiseEvent OnPanel1Uncollapsed(Me, EventArgs.Empty)
            End If
        End If
        If Panel2Collapsible Then
            If z > (max - Panel2MinSize) + Panel2MinSize / 2 Then
                max = (max - Panel2MinSize) + Panel2MinSize / 2
                RaiseEvent OnPanel2Collapsing(Me, EventArgs.Empty)
                Panel2Collapsed = True
                RaiseEvent OnPanel2Collapsed(Me, EventArgs.Empty)
            Else
                RaiseEvent OnPanel2Uncollapsing(Me, EventArgs.Empty)
                Panel2Collapsed = False
                RaiseEvent OnPanel2Uncollapsed(Me, EventArgs.Empty)
            End If
        End If

        If z > 0 AndAlso z < max Then
            SplitterDistance = z
            Invalidate()
        End If
    End Sub
End Class
