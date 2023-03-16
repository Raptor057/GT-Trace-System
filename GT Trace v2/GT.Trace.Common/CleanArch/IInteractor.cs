namespace GT.Trace.Common.CleanArch
{
    public interface IInteractor<TRequest, TResponse> : MediatR.IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IResponse
    { }
}