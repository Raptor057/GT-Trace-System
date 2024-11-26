namespace GT.Trace.Packaging.Domain.Entities
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="id">UnitID</param>
    /// <param name="line">Line Code</param>
    /// <param name="serial">GT PartNum</param>
    /// <param name="functional_test_count">Count Of Functional Test</param>
    /// <param name="functional_test_datetime">Datetime Of Functional Test</param>
    /// <param name="functional_test_final_result">Result Of Functional Test</param>
    public sealed record FunctionalTestStatus(long id, string line ,string serial ,int? functional_test_count, DateTime? functional_test_datetime, bool? functional_test_final_result); 
}
