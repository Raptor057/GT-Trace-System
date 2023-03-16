namespace GT.Trace.EZ2000.Packaging.Domain.Entities
{
    /// <summary>
    /// Record que reccibe informacion como nombre de linea y la fecha de empaque para el packaginInfo
    /// </summary>
    /// <param name="LineName"></param>
    /// <param name="TimeStamp"></param>
    public record PackagingInfo(string LineName, DateTime TimeStamp);
}
