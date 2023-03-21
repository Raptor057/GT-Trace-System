using GT.Trace.Common;

namespace GT.Trace.Domain.Entities
{
    public sealed class Part
    {
        public static bool CanCreate(string number, out ErrorList errors)
        {
            errors = new();
            if (string.IsNullOrWhiteSpace(number))
            {
                errors.Add("El número de parte se encuentra en blanco.");
            }
            return errors.IsEmpty;
        }

        public static Part Create(string number, Revision revision, string? description = null, string? productFamily = null)
        {
            if (!CanCreate(number, out var errors)) throw errors.AsException();
            return new(number, revision, description, productFamily);
        }

        private Part(string number, Revision revision, string? description, string? productFamily)
        {
            Number = number.Trim();
            Revision = revision;
            Description = description;
            ProductFamily = productFamily;
        }

        public string Number { get; }

        public Revision Revision { get; }

        public string? Description { get; }

        public string? ProductFamily { get; }

        public override int GetHashCode()
        {
            return HashCode.Combine(Number, Revision);
        }
    }
}