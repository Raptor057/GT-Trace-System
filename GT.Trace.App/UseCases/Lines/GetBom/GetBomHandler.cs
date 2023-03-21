using GT.Trace.Common;
using GT.Trace.Common.CleanArch;

namespace GT.Trace.App.UseCases.Lines.GetBom
{
    internal sealed class GetBomHandler
        : ResultInteractorBase<GetBomRequest, GetBomResponse>
    {
        private readonly IGetBomGateway _gateway;

        public GetBomHandler(IGetBomGateway gateway)
        {
            _gateway = gateway;
        }

        public override async Task<Result<GetBomResponse>> Handle(GetBomRequest request, CancellationToken cancellationToken)
        {
            var getWorkOrderResult = await _gateway.GetBomAsync(request.PartNo, request.Revision).ConfigureAwait(false);
            if (getWorkOrderResult is ISuccess<IEnumerable<BomComponentDto>> success)
            {
                return OK(new GetBomResponse(success.Data));
            }
            return Fail((getWorkOrderResult as IFailure)!.Message);
        }
    }
}