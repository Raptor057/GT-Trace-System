using GT.Trace.EZ2000.Packaging.App.Dtos;

namespace GT.Trace.EZ2000.Packaging.App.UseCases.PackUnit.Responses
{
    public record UnitPackedResponse(string LineCode, long UnitID, bool QCContainerApprovalInWarning, bool QCContainerApprovalRequired, string PartNo, string WorkOrderCode)
        : SuccessPackUnitResponse(LineCode, UnitID);

    public sealed record ContainerCompleteResponse(string LineCode, long UnitID, bool QCContainerApprovalInWarning, bool QCContainerApprovalRequired, ContainerDto Container, string WorkOrderCode)
        : UnitPackedResponse(LineCode, UnitID, QCContainerApprovalInWarning, QCContainerApprovalRequired, Container.PartNo, WorkOrderCode);

    public sealed record PalletCompleteResponse(string LineCode, long UnitID, bool QCContainerApprovalInWarning, bool QCContainerApprovalRequired, PalletDto Pallet, string WorkOrderCode)
        : UnitPackedResponse(LineCode, UnitID, QCContainerApprovalInWarning, QCContainerApprovalRequired, Pallet.PartNo, WorkOrderCode);
}