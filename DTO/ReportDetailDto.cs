namespace SavelevCrossPlatform.Dtos;

public record ReportDetailDto(
    int Id, string Teacher, string Work, string Classroom, decimal DeviationPercent,
    bool Passed, string Conclusion, DateTimeOffset CheckedAt);
