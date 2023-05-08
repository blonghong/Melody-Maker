Partial Public Module MelodyClasses
    Public ObstacleType As New Dictionary(Of String, String) From {
        {"2", "Color Down"},
        {"4", "Color Left"},
        {"6", "Color Right"},
        {"8", "Color Up"},
                          _
        {"D", "Direction Down"},
        {"L", "Direction Left"},
        {"R", "Direction Right"},
        {"U", "Direction Up"},
                              _
        {"A", "ME1 Angel Jump"}
    }

    Public IntensityType As New Dictionary(Of String, String) From {
        {"L", "Walk"},
        {"N", "Jog"},
        {"H", "Run"},
        {"E", "Fly"}
    }
End Module