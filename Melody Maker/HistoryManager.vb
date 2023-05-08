Public Module History
    Public Class HistoryManager(Of T)
        Public Sub New(HistoryObject As T)
            Dim MissingMethods = False
            If HistoryObject.GetType().GetMethod("SetTo") Is Nothing Then MissingMethods = True
            If MissingMethods Then _
                Throw New ArgumentException("HistoryObject must have a SetTo() method.")

            Undo_ = New List(Of History(Of T))
            Redo_ = New List(Of History(Of T))
            HistoryObject_ = HistoryObject
        End Sub

        ' these are private to restrict access to the functions only
        Private Property Undo_ As List(Of History(Of T))
        Private Property Redo_ As List(Of History(Of T))
        Private Property HistoryObject_ As T

        Public Sub Add(HistoryObject As T, Description As String)
            Undo_.Insert(0, New History(Of T)(HistoryObject, Description))
            Redo_.Clear()
        End Sub
        Public Sub Clear()
            Undo_.Clear()
            Redo_.Clear()
        End Sub
        Public Sub Undo(CurrentHistoryObject As T)
            If Undo_.Count = 0 Then Exit Sub
            Redo_.Insert(0, New History(Of T)(CurrentHistoryObject, Undo_(0).HistoryDescription))
            CObj(HistoryObject_).SetTo(Undo_(0).HistoryObject)
            Undo_.RemoveAt(0)
        End Sub
        Public Sub Redo(CurrentHistoryObject As T)
            If Redo_.Count = 0 Then Exit Sub
            Undo_.Insert(0, New History(Of T)(CurrentHistoryObject, Redo_(0).HistoryDescription))
            CObj(HistoryObject_).SetTo(Redo_(0).HistoryObject)
            Redo_.RemoveAt(0)
        End Sub

        Public Function GetUndoDescriptions() As String()
            Return Undo_.Select(Function(a) a.HistoryDescription).ToArray()
        End Function
        Public Function GetRedoDescriptions() As String()
            Return Redo_.Select(Function(a) a.HistoryDescription).ToArray()
        End Function

        Public Function GetCurrent() As T
            Return HistoryObject_
        End Function

    End Class

    Public Class History(Of T)
        Public Sub New(HObj As T, HDesc As String)
            HistoryObject = HObj
            HistoryDescription = HDesc
        End Sub

        Property HistoryObject As T
        Property HistoryDescription As String
    End Class

End Module