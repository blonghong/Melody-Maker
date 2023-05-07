﻿Public Class MelodyListBox
    Inherits ListBox

    Enum MelodyListBoxMode
        None
        Intensity
        Obstacle
    End Enum

    ' properties
    Private track As MelodyTrack
    Property MelodyTrack As MelodyTrack
        Get
            Return track
        End Get
        Set(value As MelodyTrack)
            track = value
            PopulateData()
            Invalidate()
        End Set
    End Property

    Private mode As MelodyListBoxMode = MelodyListBoxMode.None
    Property ListBoxMode As MelodyListBoxMode
        Get
            Return mode
        End Get
        Set(value As MelodyListBoxMode)
            mode = value
            PopulateData()
            Invalidate()
        End Set
    End Property

    ' functions
    Public Sub PopulateData()
        If ListBoxMode = MelodyListBoxMode.None Then Exit Sub
        Items.Clear()
        If track Is Nothing Then Exit Sub
        Select Case ListBoxMode
            Case MelodyListBoxMode.Intensity
                If track.intensities.Count = 0 Then Exit Sub
                For Each i In track.intensities
                    Items.Add(i.ToNiceString())
                Next

            Case MelodyListBoxMode.Obstacle
                If track.obstacles.Count = 0 Then Exit Sub
                For Each o In track.obstacles
                    Items.Add(o.ToNiceString())
                Next

        End Select
    End Sub
    Private Function Luminance(c As Color) As Integer
        Return Int(0.2126 * c.R + 0.7152 * c.G + 0.0722 * c.B)
    End Function

    ' overrides
    Public Sub New()
        SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw, True)
        DrawMode = DrawMode.OwnerDrawFixed
    End Sub
    Protected Overrides Sub OnDrawItem(e As DrawItemEventArgs)
        If e.Index >= 0 AndAlso e.Index < Items.Count Then
            Dim selected = (e.State And DrawItemState.Selected) = DrawItemState.Selected
            Dim g = e.Graphics
            Dim txt = Items(e.Index).ToString()
            Dim il = GetItemRectangle(e.Index).Location

            ' Draw Background
            Dim bc = BackColor
            ' get track color
            If track IsNot Nothing Then
                Select Case ListBoxMode
                    Case MelodyListBoxMode.Intensity
                        Dim i = track.intensities(e.Index)
                        bc = i.GetColour()

                    Case MelodyListBoxMode.Obstacle
                        Dim o = track.obstacles(e.Index)
                        Dim i = track.GetIntensityGroup(o)
                        If i IsNot Nothing Then bc = i.GetColour()
                End Select
            End If
            Using b As New SolidBrush(bc)
                g.FillRectangle(b, e.Bounds)
            End Using

            ' Draw Text
            Dim fc = Color.Black
            Dim y = Luminance(bc)
            If y < 255 / 2 Then fc = Color.White
            Using b As New SolidBrush(fc)
                Dim loc As New Point(il.X, il.Y)
                Dim fnt As New Font(e.Font, e.Font.Style)
                If selected Then
                    loc.X += e.Font.Size
                    fnt = New Font(fnt, FontStyle.Bold)
                End If
                g.DrawString(txt, fnt, b, loc)
            End Using

            ' Draw Arrow
            If selected Then
                Dim ymid As Single = Math.Round(e.Bounds.Height / 2) + il.Y
                g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
                Dim arrow_size = e.Font.Size

                Using p As New Pen(fc, arrow_size / 2) With {.EndCap = Drawing2D.LineCap.ArrowAnchor}
                    g.DrawLine(p, e.Bounds.Left + 2, ymid, e.Bounds.Left + arrow_size, ymid)
                End Using
            End If
        End If

        MyBase.OnDrawItem(e)
    End Sub
End Class
