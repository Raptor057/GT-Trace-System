//using GT.Trace.Common;
//using GT.Trace.Common.CleanArch;

//namespace GT.Trace.EtiMovements.App.UseCases.UpdateEtiTraza
//{
//    public sealed class UpdateEtiTrazaRequest : IResultRequest<UpdateEtiTrazaResponse>
//    {
//        public static bool CanUpdate(string etiInput, out ErrorList errors)
//        {
//            errors = new();
//            if (string.IsNullOrWhiteSpace(etiInput))
//            {
//                errors.Add("El parámetro de ETI es requerido y se encuentra en blanco.");
//            }
//            return errors.IsEmpty;
//        }
//        public static UpdateEtiTrazaRequest Update(string etiInput)
//        {
//            if (!CanUpdate(etiInput, out var errors)) throw errors.AsException();
//            return new(etiInput!);
//        }

//        public UpdateEtiTrazaRequest(string etiInput)
//        {
//            EtiInput = etiInput;
//        }

//        public string EtiInput { get; }
//    }
//}