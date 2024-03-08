﻿namespace Carlton.Core.Lab.State.Mutations;

public class RecordEventMutation : IFluxStateMutation<LabState, RecordEventCommand>
{
    public string StateEvent => LabStateEvents.EventRecorded.ToString();

    public LabState Mutate(LabState currentState, RecordEventCommand command)
    {
        var newEvents = currentState.ComponentEvents.Append(
            new ComponentRecordedEvent
            {
                Name = command.RecordedEventName,
                EventObj = command.EventArgs
            });
        return currentState with { ComponentEvents = newEvents };
    }
}
