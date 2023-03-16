namespace GT.Trace.Etis.Domain.Entities
{
    public sealed class ErrorList : List<string>
    {
        public InvalidOperationException AsException() =>
            new InvalidOperationException(ToString());

        public override string ToString() =>
            this.Select(item => $"- {item}").Aggregate((x, y) => $"{x}\n{y}");
    }
}