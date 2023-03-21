namespace GT.Trace.EtiMovements.App.UseCases.GetEtiInfo
{
    public sealed record EtiMovementDto(string PointOfUseCode, string EtiNo, string ComponentNo, DateTime? StartTime, DateTime? UsageTime, DateTime? EndTime, bool IsDepleted);
}