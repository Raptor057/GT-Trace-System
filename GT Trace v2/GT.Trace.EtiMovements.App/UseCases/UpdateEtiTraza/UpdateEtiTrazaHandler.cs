//using GT.Trace.App.UseCases.MaterialLoading.FetchPointOfUseEtis;
//using GT.Trace.Common;
//using GT.Trace.Common.CleanArch;
//using GT.Trace.EtiMovements.Domain.Repositories;
//using Microsoft.Extensions.Logging;

//namespace GT.Trace.EtiMovements.App.UseCases.UpdateEtiTraza
//{
//    internal class UpdateEtiTrazaHandler 
//        : ResultInteractorBase<UpdateEtiTrazaRequest, UpdateEtiTrazaResponse>
//    {
//        private readonly IUpdateEtiTrazaRepository _update;

//        public UpdateEtiTrazaHandler(IUpdateEtiTrazaRepository update)
//        {
//            _update=update;
//        }

//        public override async Task<Result<UpdateEtiTrazaResponse>> Handle(UpdateEtiTrazaRequest request, CancellationToken cancellationToken)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}