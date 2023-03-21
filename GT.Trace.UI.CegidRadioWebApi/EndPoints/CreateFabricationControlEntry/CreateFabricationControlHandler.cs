namespace GT.Trace.UI.CegidRadioWebApi.EndPoints.ReportFabricationControl
{
    public class CreateFabricationControlHandler
    {
        private static string GetFileName(string partNo, string revision, long etiID) =>
            $"{partNo}[{revision}]{etiID}.txt";

        private static string GetEntry(string workOrderCode, string locationCode, int quantity, bool orderIsClosed) =>
            $"I;{workOrderCode};{locationCode}-;{quantity};{(orderIsClosed ? 'S' : 'A')}";

        private readonly IConfigurationRoot _configuration;

        public CreateFabricationControlHandler(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }

        public bool CanExecute(CreateFabricationControlEntryRequest request, out List<string> errors)
        {
            errors = new();
            if (string.IsNullOrEmpty(request.DepoCode))
            {
                errors.Add("El código de locación no puede estar en blanco.");
            }
            if (string.IsNullOrEmpty(request.PartNo))
            {
                errors.Add("El número de parte no puede estar en blanco.");
            }
            if (!request.Quantity.HasValue)
            {
                errors.Add("La cantidad no puede estar en blanco.");
            }
            if ((request.Quantity ?? 1) < 1)
            {
                errors.Add($"La cantidad tiene que ser mayor que cero [ quantity = {request.Quantity} ].");
            }
            if (!request.EtiID.HasValue)
            {
                errors.Add("El identificador de ETI no puede estar en blanco.");
            }
            if ((request.EtiID ?? 1) < 1)
            {
                errors.Add($"El identificador de ETI tiene que ser mayor que cero [ ID = {request.EtiID} ].");
            }
            return errors.Count == 0;
        }

        public async Task ExecuteAsync(CreateFabricationControlEntryRequest request)
        {
            if (!CanExecute(request, out var errors)) throw new InvalidOperationException(errors.Select(e => $"- {e}").Aggregate((a, b) => $"{a}\n{b}"));

            var fileName = GetFileName(request.PartNo!, request.Revision!, request.EtiID!.Value);
            var path = Path.Combine(_configuration.GetSection("Directories:FabricationControl").Value, fileName);
            var entry = GetEntry(request.WorkOrderCode!, request.LocationCode!, request.Quantity!.Value, request.OrderIsClosed);
            await File.WriteAllTextAsync(path, entry).ConfigureAwait(false);
        }
    }
}