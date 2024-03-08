﻿namespace Carlton.Core.Flux.Debug.State.Mutations;

internal class TraceLogMessageTableRowsPerPageMutation : IFluxStateMutation<FluxDebugState, ChangeTraceLogMessageTableRowsPerPageOptsCommand>
{
    public string StateEvent => FluxDebugStateEvents.TraceLogTableRowsPerPageChanged.ToString();

    public FluxDebugState Mutate(FluxDebugState state, ChangeTraceLogMessageTableRowsPerPageOptsCommand command)
    {
        var updatedTableState = state.TraceLogTableState with
        {
            CurrentPage = 1,
            SelectedRowsPerPageOptsIndex = command.SelectedRowsPerPageIndex
        };
        return state with { TraceLogTableState = updatedTableState };
    }
}
