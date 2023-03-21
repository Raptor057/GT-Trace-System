using GT.Trace.Common.CleanArch;

namespace GT.Trace.Packaging.App.UseCases.SetLineHeadcount
{
    public sealed record SetLineHeadcountRequest(string LineCode, string WorkOrderCode, int Headcount) : IRequest<SetLineHeadcountResponse>;
}