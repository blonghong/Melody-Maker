Public Module MelodyClasses

    Public Class MelodyTrack
        Public Sub New()
            ParseNew()
        End Sub
        Property version As String
        Property samples As Integer
        Property duration As Decimal
        Property tempo As Integer
        Property is_3_4 As Boolean
        Property intensities As List(Of MelodyIntensity)
        Property obstacles As List(Of MelodyObstacle)

        Public Overrides Function ToString() As String
            Dim i = String.Join("", intensities)
            Dim o = String.Join("", obstacles)

            Return String.Format("{1}{0}{2};{3};{4};{5}{0}{6}{0}{7}",
                vbNewLine,
                version,
                samples,
                duration,
                tempo,
                If(is_3_4, "1", "0"),
                i,
                o
            )
        End Function

        Public Sub SortLists()
            intensities.Sort(Function(x, y) x.time.CompareTo(y.time))
            obstacles.Sort(Function(x, y) x.time.CompareTo(y.time))
        End Sub
        Public Sub ParseNew()
            version = "1.13"
            samples = 0
            duration = 0
            tempo = 0
            is_3_4 = False
            intensities = New List(Of MelodyIntensity)
            obstacles = New List(Of MelodyObstacle)
        End Sub
        Public Sub SetTo(obj As MelodyTrack)
            version = obj.version
            samples = obj.samples
            duration = obj.duration
            tempo = obj.tempo
            is_3_4 = obj.is_3_4
            intensities = New List(Of MelodyIntensity)(obj.intensities.Select(Function(a) a.Clone()))
            obstacles = New List(Of MelodyObstacle)(obj.obstacles.Select(Function(a) a.Clone()))
        End Sub
        Public Sub Parse(data As String)
            Dim lines = data.Split(vbNewLine)
            Dim info = lines(1).Split(";")

            version = lines(0).Trim()
            samples = info(0).Trim()
            duration = info(1).Trim()
            tempo = info(2).Trim()
            is_3_4 = info(3).Trim() = "1"

            intensities.Clear()
            obstacles.Clear()

            ' intensities
            For Each intensity As String In lines(2).Split(";")
                If String.IsNullOrWhiteSpace(intensity) Then Continue For
                Dim idata = intensity.Trim().Split(":")
                Dim timedur = idata(1).Split("-")
                Dim i As New MelodyIntensity With {
                    .type = idata(0)(0), 'get the first char of the type, game only uses this.
                    .time = timedur(0),
                    .trans_duration = timedur(1),
                    .has_a = If(timedur.Length > 2, timedur(2), "")
                }
                intensities.Add(i)
            Next
            intensities.Sort(Function(x, y) x.time.CompareTo(y.time))

            ' obstacles
            For Each obstacle As String In lines(3).Split(";")
                If String.IsNullOrWhiteSpace(obstacle) Then Continue For
                Dim odata = obstacle.Trim().Split(":")
                Dim splits = odata(1).Split("-")
                Dim o As New MelodyObstacle With {
                    .time = odata(0),
                    .type = splits(0)(0), 'get the first char of the type, game only uses this.
                    .duration = If(splits.Length > 1, splits(1), -1)
                }
                obstacles.Add(o)
            Next
            obstacles.Sort(Function(x, y) x.time.CompareTo(y.time))
        End Sub

        Public Function Clone() As MelodyTrack
            Dim i As New List(Of MelodyIntensity)(intensities.Select(Function(a) a.Clone()))
            Dim o As New List(Of MelodyObstacle)(obstacles.Select(Function(a) a.Clone()))
            Return New MelodyTrack With {
                .version = version,
                .samples = samples,
                .duration = duration,
                .tempo = tempo,
                .is_3_4 = is_3_4,
                .intensities = i,
                .obstacles = o
            }
        End Function

        Public Function GetIntensityGroup(o As MelodyObstacle) As MelodyIntensity
            If intensities.Count = 0 Then Return Nothing
            Dim ints = intensities.Where(Function(x) x.time < o.time)
            Return ints.OrderByDescending(Function(x) x.time).FirstOrDefault()
        End Function

    End Class
    Public Class MelodyIntensity
        Property type As String
        Property time As Integer
        Property trans_duration As Integer
        Property has_a As String ' idk what the -A means after durations? could be slow down mode or something

        Public Function ToNiceString() As String
            Return String.Format("{0}: {1} [T: {2}]{3}", time, ValidateNiceType(type), trans_duration,
                If(Not String.IsNullOrWhiteSpace(has_a), String.Format(" -{0}", has_a), ""))
        End Function
        Public Overrides Function ToString() As String
            Return String.Format("{0}:{1}-{2}{3};", type, time, trans_duration, If(Not String.IsNullOrWhiteSpace(has_a), String.Format("-{0}", has_a), ""))
        End Function

        Public Function GetColour() As Color
            'these are based on in-game colours
            Select Case type
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
                .type = type,
                .time = time,
                .trans_duration = trans_duration,
                .has_a = has_a
            }
        End Function

        Public Shared TypeEnum As New Dictionary(Of String, String) From {
            {"L", "Walk"},
            {"N", "Jog"},
            {"H", "Run"},
            {"E", "Fly"}
        }
        Public Shared Function ValidateNiceType(s As String)
            If TypeEnum.Keys.Contains(s) Then
                Return TypeEnum.Item(s)
            End If
            Return s
        End Function
        Public Shared Function ValidateType(s As String)
            Dim i = TypeEnum.Values.ToList().IndexOf(s)
            If i <> -1 Then
                Return TypeEnum.Keys(i)
            End If
            Return s
        End Function

    End Class
    Public Class MelodyObstacle

        Public Shared ObstacleType As New Dictionary(Of String, String) From {
            {"2", "Color Down"},
            {"4", "Color Left"},
            {"6", "Color Right"},
            {"8", "Color Up"},
                              _
            {"D", "Direction Down"},
            {"L", "Direction Left"},
            {"R", "Direction Right"},
            {"U", "Direction Up"}
        }
        Public Shared Function ObstacleType_GetKey(value As String) As String
            Dim i = ObstacleType.Values.ToList().IndexOf(value)
            If i <> -1 Then Return ObstacleType.Keys(i)
            Return Nothing
        End Function


        Property type As String
        Property time As Integer
        Property duration As Integer '-1 = no duration
        Public Shared Function ValidateNiceType(s As String)
            If ObstacleType.Keys.Contains(s) Then Return ObstacleType.Item(s)
            Return s
        End Function
        Public Shared Function ValidateType(s As String)
            Dim type = ObstacleType_GetKey(s)
            If String.IsNullOrWhiteSpace(type) Then Return s
            Return type
        End Function
        Public Function ToNiceString() As String
            Return String.Format("{0}: {1}{2}", time, ValidateNiceType(type),
                                                  If(duration <> -1, String.Format(" [T: {0}]", duration), ""))
        End Function

        Public Overrides Function ToString() As String
            Return String.Format("{0}:{1}{2};", time, type, If(duration <> -1, String.Format("-{0}", duration), ""))
        End Function

        Public Function Clone() As MelodyObstacle
            Return New MelodyObstacle With {
                .type = type,
                .time = time,
                .duration = duration
            }
        End Function
    End Class

    Public Class IntensityTypeEnum
        Public Const Walk = "L",
            Jog = "N",
            Run = "H",
            Fly = "E"
    End Class

End Module