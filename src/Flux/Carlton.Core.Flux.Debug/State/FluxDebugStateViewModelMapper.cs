﻿#pragma warning disable RMG020 // Source member is not mapped to any target member
using Riok.Mapperly.Abstractions;
namespace Carlton.Core.Lab.State;

[Mapper]
public partial class FluxDebugStateViewModelMapper : IViewModelProjectionMapper<FluxDebugState>
{
	public partial TViewModel Map<TViewModel>(FluxDebugState state);

	[MapProperty(nameof(FluxDebugState.SelectedLogMessage), nameof(EventLogDetailsViewModel.SelectedLogMessage))]
	public partial EventLogDetailsViewModel ToEventLogDetailsViewModel(FluxDebugState state);

	[MapProperty(nameof(FluxDebugState.SelectedLogMessage), nameof(EventLogScopesViewModel.SelectedLogMessage))]
	public partial EventLogScopesViewModel ToEventLogScopesViewModel(FluxDebugState state);

	[MapProperty(nameof(FluxDebugState.LogMessages), nameof(EventLogViewerViewModel.LogMessages))]
	[MapProperty(nameof(FluxDebugState.EventLogViewerFilterState), nameof(EventLogViewerViewModel.EventLogViewerFilterState))]
	[MapProperty(nameof(FluxDebugState.EventLogTableState), nameof(EventLogViewerViewModel.EventLogTableState))]
	public partial EventLogViewerViewModel ToEventLogViewerViewModelViewModel(FluxDebugState state);

	[MapProperty(nameof(FluxDebugState.TraceLogMessageGroups), nameof(TraceLogViewerViewModel.TraceLogMessages))]
	[MapProperty(nameof(FluxDebugState.SelectedTraceLogMessageIndex), nameof(TraceLogViewerViewModel.SelectedTraceLogMessageIndex))]
	[MapProperty(nameof(FluxDebugState.ExpandedTraceLogMessageIndexes), nameof(TraceLogViewerViewModel.ExpandedRowIndexes))]
	[MapProperty(nameof(FluxDebugState.TraceLogTableState), nameof(TraceLogViewerViewModel.TraceLogTableState))]
	public partial TraceLogViewerViewModel ToTraceLogViewerViewModel(FluxDebugState state);

	internal static TraceLogRequestContextDetailsViewModel ToTraceLogRequestContextDetailsViewModel(FluxDebugState state)
	{
		if (state.SelectedTraceLogMessage == null)
			return new TraceLogRequestContextDetailsViewModel { SelectedRequestContext = null };

		return new TraceLogRequestContextDetailsViewModel { SelectedRequestContext = state.SelectedTraceLogMessage.RequestContext };
	}

	public static TraceLogRequestObjectDetailsViewModel ToTraceLogRequestObjectDetailsViewModel(FluxDebugState state)
	{
		var defaultViewModel = new TraceLogRequestObjectDetailsViewModel { SelectedRequestObject = null };

		if (state.SelectedTraceLogMessage == null)
			return defaultViewModel;
		var selectedContext = state.SelectedTraceLogMessage.RequestContext;
		return state.SelectedTraceLogMessage.FluxAction switch
		{
			FluxActions.ViewModelQuery => defaultViewModel with { SelectedRequestObject = ((dynamic)selectedContext).ResultViewModel },
			FluxActions.MutationCommand => defaultViewModel with { SelectedRequestObject = ((dynamic)selectedContext).MutationCommand },
			_ => defaultViewModel
		};
	}

	public static HeaderActionsViewModel ToHeaderActionsViewModel(FluxDebugState state)
	{
		return new HeaderActionsViewModel
		{
			UserName = "Stephen",
			AvatarUrl = "_content/Carlton.Core.Components/images/avatar.jpg"
		};
	}
}



