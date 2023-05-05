Public Class ListBoxColour
    Inherits ListBox

    Public Colours As Dictionary(Of Integer, Color)

    Public Sub New()
        Colours = New Dictionary(Of Integer, Color)
        SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw, True)
        DrawMode = DrawMode.OwnerDrawVariable
        DoubleBuffered = True
    End Sub
    Private Sub ListBoxColour_DrawItem(sender As Object, e As DrawItemEventArgs) Handles Me.DrawItem
        e.DrawBackground()

        If e.Index >= 0 AndAlso e.Index < Items.Count Then
            Dim selected = (e.State And DrawItemState.Selected) = DrawItemState.Selected
            Dim g = e.Graphics
            Dim txt = Items(e.Index).ToString()
            Dim il = GetItemRectangle(e.Index).Location

            Dim c = BackColor
            If Colours.Keys.Contains(e.Index) Then c = Colours.Item(e.Index)
            Using b As New SolidBrush(c)
                g.FillRectangle(b, e.Bounds)
            End Using

            Dim y = 0.2126 * c.R + 0.7152 * c.G + 0.0722 * c.B
            Dim cfore = Color.Black
            If y < 255 / 2 Then
                cfore = Color.White
            End If

            If selected Then
                Dim r As New Rectangle(e.Bounds.Location, e.Bounds.Size)
                r.Size = New Size(10, r.Height)
                Using b As New SolidBrush(BackColor)
                    'g.FillRectangle(b, r)
                End Using
                g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
                Using p As New Pen(cfore, 4)
                    p.EndCap = Drawing2D.LineCap.ArrowAnchor
                    Dim ymid As Single = Math.Round(e.Bounds.Height / 2) + il.Y
                    g.DrawLine(p, e.Bounds.Left + 4, ymid, 8, ymid)
                End Using
            End If





            Using b As New SolidBrush(cfore)
                Dim loc As New Point(il.X, il.Y)
                Dim fnt As New Font(e.Font, e.Font.Style)
                If selected Then
                    loc.X += 10
                    fnt = New Font(fnt, FontStyle.Bold)
                End If
                g.DrawString(txt, fnt, b, loc)
            End Using
        End If

        e.DrawFocusRectangle()
    End Sub

    Private Sub ListBoxColour_Leave(sender As Object, e As EventArgs) Handles Me.Leave
        'Update()
    End Sub
End Class
