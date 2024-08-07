using Carlton.Core.Flux.Debug.Extensions;
namespace Carlton.Core.Flux.Debug.State;

internal sealed record FluxDebugState
{
	private readonly IFluxStateWrapper _wrapper;

	//Logs
	public IReadOnlyList<FluxDebugLogMessage> LogMessages { get; init; } = new List<FluxDebugLogMessage>();
	public IReadOnlyList<TraceLogMessageGroup> TraceLogMessageGroups => LogMessages.MapLogMessagesToTraceLogMessage().ToList();

	//Selected Log Messages
	public int? SelectedLogMessageIndex { get; init; }
	public int? SelectedTraceLogMessageIndex { get; init; }

	public FluxDebugLogMessage? SelectedLogMessage => LogMessages.ElementAtOrDefault(SelectedLogMessageIndex ?? -1);
	public FluxDebugLogMessage? SelectedTraceLogMessage => LogMessages.ElementAtOrDefault(SelectedTraceLogMessageIndex ?? -1);

	//Main Application State
	public IEnumerable<Type> ViewModelTypes { get; private init; } = new List<Type>();
	public IEnumerable<Type> MutationCommandTypes { get; private init; } = new List<Type>();
	public IEnumerable<RecordedMutation> RecordedMutations => _wrapper.RecordedMutations;
	public int SelectedMutationIndex { get; init; }
	public object SelectedCommandMutation => RecordedMutations.FirstOrDefault(x => x.MutationIndex == SelectedMutationIndex)?.MutationCommand;
	public object SelectedState => _wrapper.Replay(SelectedMutationIndex + 1);

	public FluxDebugState(FluxTypes fluxTypes, IFluxStateWrapper wrapper)
	{
		ViewModelTypes = fluxTypes.ViewModelTypes;
		MutationCommandTypes = fluxTypes.CommandTypes;
		_wrapper = wrapper;
		SelectedMutationIndex = RecordedMutations.Count() - 1;
	}
}

