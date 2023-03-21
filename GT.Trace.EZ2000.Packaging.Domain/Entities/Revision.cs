namespace GT.Trace.EZ2000.Packaging.Domain.Entities
{
    public class Revision
    {
        /// <summary>
        /// Funcion que establece una nueva revision.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static Revision New(string number) => new Revision(number ?? "");

        /// <summary>
        /// Propiedad que sirve para validacion de si una pieza es prototipo o no.
        /// Nota: Se agrego el 1 para que detectara la rev 19 de EZ2000 y funciono 02/28/2023
        /// Nota: Se agrego el 2 para que detectara la rev 20 de EZ2000 y funciono 03/08/2023
        /// </summary>
        private static readonly string _prototypeRevisionInitialChars = "XY012";

        /// <summary>
        /// Metodo que recibe un valor y lo asigna a la variable original value.
        /// si el originalvalue no es null ni empty entonces evalua si es prorotipo o no
        /// </summary>
        /// <param name="value"></param>
        public Revision(string value)
        {
            OriginalValue = value.Trim().ToUpper();
            if (!string.IsNullOrEmpty(OriginalValue))
            {
                IsPrototype = _prototypeRevisionInitialChars.Contains(OriginalValue[0]);
                Number = IsPrototype ? OriginalValue : OriginalValue[0].ToString();
            }
        }

        /// <summary>
        /// Propiedades de la clase
        /// </summary>
        public string Number { get; } = "";

        public string OriginalValue { get; }

        public bool IsPrototype { get; } = false;
        /// <summary>
        /// Pendientes de ver.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Number;

        /// <summary>
        /// Pendiente de ver como funciona este metodo
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            return obj is Revision rev && rev.Number == Number;
        }
        /// <summary>
        /// Pendiente de ver ocmo funciona este metodo
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => HashCode.Combine(Number);
    }
}
