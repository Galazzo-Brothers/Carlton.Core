﻿namespace Carlton.Core.Components.Flux;
public interface IStateProcessor
{
    public Task ProcessCommand<TCommand>(object sender, TCommand command);
}

public interface IStateProcessor<TState> : IStateProcessor
{
    public void RestoreState(Memento<TState> memento);
    public Memento<TState> SaveState();
}

public class Memento<TState>
{
    public TState State { get; init; }

    public Memento(TState state)
    {
        State = state;
    }
}


