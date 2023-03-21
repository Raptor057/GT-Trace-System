namespace GT.Trace.Packaging.Domain.Entities
{
    //public class ErrorList
    //{
    //    private readonly Queue<string> _errors = new();

    //    public bool IsEmpty => _errors.Count == 0;

    //    public void Add(string error) => _errors.Enqueue(error);

    //    public void Add(ErrorList errors)
    //    {
    //        foreach (var e in errors._errors) _errors.Enqueue(e);
    //    }

    //    public override string ToString() => _errors.Select(e => $"- {e}").Aggregate((x, y) => $"{x}\n{y}");

    //    public InvalidOperationException AsException() => new InvalidOperationException(ToString());
    //}
}