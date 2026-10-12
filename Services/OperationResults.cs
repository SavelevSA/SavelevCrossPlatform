namespace SavelevCrossPlatform.Services;

public enum DeleteResult
{
    Deleted,
    NotFound,
    InUse
}

public enum ReportUpdateResult
{
    Updated,
    NotFound,
    InvalidReferences
}
