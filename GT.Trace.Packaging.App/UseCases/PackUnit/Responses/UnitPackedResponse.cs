using GT.Trace.Packaging.App.Dtos;

namespace GT.Trace.Packaging.App.UseCases.PackUnit.Responses
{
    public record UnitPackedResponse(string LineCode, long UnitID, bool QCContainerApprovalInWarning, bool QCContainerApprovalRequired, string PartNo, string WorkOrderCode)
        : SuccessPackUnitResponse(LineCode, UnitID);
#pragma warning disable CS8604
    public sealed record ContainerCompleteResponse(string LineCode, long UnitID, bool QCContainerApprovalInWarning, bool QCContainerApprovalRequired, ContainerDto Container, string WorkOrderCode)
        : UnitPackedResponse(LineCode, UnitID, QCContainerApprovalInWarning, QCContainerApprovalRequired, Container.PartNo, WorkOrderCode);
#pragma warning disable CS8604
    public sealed record PalletCompleteResponse(string LineCode, long UnitID, bool QCContainerApprovalInWarning, bool QCContainerApprovalRequired, PalletDto Pallet, string WorkOrderCode)
        : UnitPackedResponse(LineCode, UnitID, QCContainerApprovalInWarning, QCContainerApprovalRequired, Pallet.PartNo, WorkOrderCode);
}