namespace GT.Trace.Packaging.App.UseCases.UnpackUnit
{
    public interface IUnpackUnitGateway
    {
        /// <summary>
        /// Delete UnitID from GT-System in Database:
        /// GTT - ProcessHistory
        /// TRAZAB - Temp_pack_WB
        /// Update current_qty -1 in Database: 
        /// APPS - pro_production
        /// </summary>
        /// <param name="lineName"> Example "WB LO"</param>
        /// <param name="unitID"> This is obtained through a method that parses a regular expression and returns the scanInput data
        /// Example from scan Input: " [)>06SWB10725151PAUC15714ZGT1TGT871402TB23T12824 " expected value of the method for this variable 10725151 </param>
        /// <param name="workOrderCode">Example "W08411211"  </param>
        /// <param name="lineID"> Example: 5 </param>
        /// <param name="lineCode">Example: LO </param>
        /// <returns></returns>
        Task UnpackedUnitAsync(string lineName, long unitID, string workOrderCode, string lineCode);
    }
}
