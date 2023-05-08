Partial Public Module MelodyClasses
    Public Class MelodyTrack
        Public Sub New(Optional IsME1Track As Boolean = False)
            If IsME1Track Then ParseNewME1() Else ParseNewME2()
        End Sub
        Property Version As String
        Property IsME1 As Boolean
        Property Samples As Integer
        Property Duration As Decimal
        Property Tempo As Integer
        Property Is34TimeSignature As Boolean
        Property Intensities As List(Of MelodyIntensity)
        Property Obstacles As List(Of MelodyObstacle)
        Public Overrides Function ToString() As String
            Dim i = String.Join(";", Intensities)
            Dim o = String.Join(";", Obstacles)

            Return String.Format("{1}{0}{2};{3};{4};{5}{0}{6}{0}{7}",
                vbNewLine,
                Version,
                Samples,
                Duration,
                Tempo,
                If(Is34TimeSignature, "1", "0"),
                i,
                o
            )
        End Function
        Public Sub SortLists()
            Intensities.Sort(Function(x, y) x.Time.CompareTo(y.Time))
            Obstacles.Sort(Function(x, y) x.Time.CompareTo(y.Time))
        End Sub
        Public Sub ParseNewME1()
            Version = "1.02"
            IsME1 = True
            ParseNew()
        End Sub
        Public Sub ParseNewME2()
            Version = "1.13"
            IsME1 = False
            ParseNew()
        End Sub
        Private Sub ParseNew()
            Samples = 0
            Duration = 0
            Tempo = 0
            Is34TimeSignature = False
            Intensities = New List(Of MelodyIntensity)
            Obstacles = New List(Of MelodyObstacle)
        End Sub
        Public Sub SetTo(obj As MelodyTrack)
            Version = obj.Version
            Samples = obj.Samples
            Duration = obj.Duration
            Tempo = obj.Tempo
            Is34TimeSignature = obj.Is34TimeSignature
            Intensities = New List(Of MelodyIntensity)(obj.Intensities.Select(Function(a) a.Clone()))
            Obstacles = New List(Of MelodyObstacle)(obj.Obstacles.Select(Function(a) a.Clone()))
        End Sub
        Public Sub Parse(data As String)
            Dim lines = data.Split(vbNewLine)
            Dim info = lines(1).Split(";")

            Version = lines(0).Trim()
            Samples = info(0).Trim()
            Duration = info(1).Trim()
            Tempo = info(2).Trim()
            Is34TimeSignature = info(3).Trim() = "1"

            IsME1 = Decimal.Parse(Version) <= 1.02

            Intensities.Clear()
            Obstacles.Clear()

            ' intensities
            For Each intensity As String In lines(2).Split(";")
                If String.IsNullOrWhiteSpace(intensity) Then Continue For
                Dim i As New MelodyIntensity With {.IsME1 = IsME1}
                If IsME1 Then
                    Dim idata = intensity.Trim().Split("-")
                    Dim istart = idata(0).Split(":")
                    Dim iend = idata(1).Split(":")
                    i.Type = iend(0)
                    i.Time = istart(1)
                    i.TransDuration = CInt(iend(1)) - CInt(istart(1))
                    i.ME1_StartType = istart(0)
                Else
                    Dim idata = intensity.Trim().Split(":")
                    Dim timedur = idata(1).Split("-")
                    i.Type = idata(0)
                    i.Time = timedur(0)
                    i.TransDuration = timedur(1)
                    If timedur.Length > 2 AndAlso timedur(2) = "A" Then
                        i.IsAngelJump = True
                    End If
                End If
                Intensities.Add(i)
            Next
            Intensities.Sort(Function(x, y) x.Time.CompareTo(y.Time))

            ' obstacles
            For Each obstacle As String In lines(3).Split(";")
                If String.IsNullOrWhiteSpace(obstacle) Then Continue For
                Dim odata = obstacle.Trim().Split(":")
                Dim splits = odata(1).Split("-")
                Dim o As New MelodyObstacle With {
                    .Time = odata(0),
                    .Type = splits(0).FirstOrDefault(), 'get the first char of the type, game only uses this.
                    .IsHold = splits.Length > 1
                }
                If o.IsHold Then o.HoldDuration = splits(1)
                Obstacles.Add(o)
            Next
            Obstacles.Sort(Function(x, y) x.Time.CompareTo(y.Time))
        End Sub
        Public Function Clone() As MelodyTrack
            Dim i As New List(Of MelodyIntensity)(Intensities.Select(Function(a) a.Clone()))
            Dim o As New List(Of MelodyObstacle)(Obstacles.Select(Function(a) a.Clone()))
            Return New MelodyTrack With {
                .Version = Version,
                .Samples = Samples,
                .Duration = Duration,
                .Tempo = Tempo,
                .Is34TimeSignature = Is34TimeSignature,
                .Intensities = i,
                .Obstacles = o
            }
        End Function
        Public Function GetIntensityGroup(o As MelodyObstacle) As MelodyIntensity
            If Intensities.Count = 0 Then Return Nothing
            Dim ints = Intensities.Where(Function(x) x.Time < o.Time)
            Return ints.OrderByDescending(Function(x) x.Time).FirstOrDefault()
        End Function
    End Class
    Public Class MelodyIntensity
        Property Type As Char
        Property Time As Integer
        Property TransDuration As Integer
        Property IsAngelJump As Boolean 'Angel Jumps are transitions in ME2, but they were obstacles in ME1
        Property IsME1 As Boolean
        Property ME1_StartType As String
        Public Function ToNiceString() As String
            If IsME1 Then Return String.Format("{0}: {1} -> {2} [T: {3}]",
                Time,
                ValidateNiceType(ME1_StartType),
                ValidateNiceType(Type),
                TransDuration)

            Return String.Format("{0}: {1} [T: {2}]{3}",
                Time,
                ValidateNiceType(Type),
                TransDuration,
                If(IsAngelJump, " [AngelJump]", ""))
        End Function
        Public Overrides Function ToString() As String
            If IsME1 Then Return String.Format("{0}:{1}-{2}:{3}",
                    ME1_StartType,
                    Time,
                    Type,
                    Time + TransDuration)

            Return String.Format("{0}:{1}-{2}{3}",
                                 Type,
                                 Time,
                                 TransDuration,
                                 If(IsAngelJump, "-A", ""))
        End Function
        Public Function GetColour() As Color
            'these are based on in-game colours
            Select Case Type
                Case "L"
                    Return Color.FromArgb(0, 162, 207)
                Case "N"
                    Return Color.FromArgb(4, 175, 119)
                Case "H"
                    Return Color.FromArgb(226, 149, 9)
                Case "E"
                    Return Color.FromArgb(188, 52, 113)
                Case Else
                    Return Color.White
            End Select
        End Function
        Public Function Clone() As MelodyIntensity
            Return New MelodyIntensity With {
                .Type = Type,
                .Time = Time,
                .TransDuration = TransDuration,
                .IsAngelJump = IsAngelJump,
                .IsME1 = IsME1,
                .ME1_StartType = ME1_StartType
            }
        End Function

        Public Shared Function ValidateNiceType(s As String)
            If IntensityType.Keys.Contains(s) Then Return IntensityType.Item(s)
            Return s
        End Function
        Public Shared Function ValidateType(s As String)
            Dim i = IntensityType.Values.ToList().IndexOf(s)
            If i <> -1 Then Return IntensityType.Keys(i)
            Return s
        End Function
    End Class

    Public Class MelodyObstacle
        Property Type As Char
        Property Time As Integer
        Property IsHold As Boolean
        Property HoldDuration As Integer

        Public Function ToNiceString() As String
            Return String.Format("{0}: {1}{2}", Time, ValidateNiceType(Type),
                                                  If(IsHold, String.Format(" [T: {0}]", HoldDuration), ""))
        End Function
        Public Overrides Function ToString() As String
            'If IsME1 Then Return String.Format("{0}:{1}{2}")
            Return String.Format("{0}:{1}{2}", Time, Type, If(IsHold, String.Format("-{0}", HoldDuration), ""))
        End Function
        Public Function Clone() As MelodyObstacle
            Return New MelodyObstacle With {
                .Type = Type,
                .Time = Time,
                .IsHold = IsHold,
                .HoldDuration = HoldDuration
            }
        End Function

        Public Shared Function ValidateNiceType(s As String)
            If ObstacleType.Keys.Contains(s) Then Return ObstacleType.Item(s)
            Return s
        End Function
        Public Shared Function ValidateType(s As String)
            Dim i = ObstacleType.Values.ToList().IndexOf(s)
            If i <> -1 Then Return ObstacleType.Keys(i)
            Return s
        End Function
    End Class
End Module