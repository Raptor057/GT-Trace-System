﻿namespace GT.Trace.EZ2000.Packaging.App.UseCases.PackUnit.Responses
{
    public abstract record SuccessPackUnitResponse(string LineCode, long UnitID) : PackUnitResponse;
}