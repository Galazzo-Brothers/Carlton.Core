﻿using Carlton.Core.Utilities.JsonConverters;
using System.Text.Json.Serialization;

namespace Carlton.Core.Flux.Internals.Dispatchers.Mutations;

internal class MutationCommandContext<TCommand>(TCommand command) : BaseRequestContext
{
	public override FluxOperationKind FluxOperationKind => FluxOperationKind.MutationCommand;
	[JsonConverter(typeof(JsonTypeConverter))]
	public override Type FluxOperationType => MutationCommand.GetType();

	public TCommand MutationCommand { get; private set; } = command;
	public TCommand InitialCommand { get; init; } = command;
	public bool CommandReplacedByResponse { get; private set; }
	public MutationCommandResult MutationCommandResult { get; private set; }
	public bool StateCommittedToLocalStorage { get; private set; }

	internal void ReplaceCommandWithResponseBody(TCommand command)
	{
		CommandReplacedByResponse = true;
		MutationCommand = command;
	}

	internal void MarkAsSucceeded(MutationCommandResult commandResult)
	{
		MutationCommandResult = commandResult;
		MarkAsSucceeded();
	}

	internal void MarkAsCommittedToLocalStorage()
		=> StateCommittedToLocalStorage = true;

	//Most uses of this context involve passing a
	//weakly typed object as a command at runtime
	//this override is primarily
	//so the actual value of the command can be logged
	public override string ToString()
		=> $"MutationCommandContext[{FluxOperationKind}]";
}

