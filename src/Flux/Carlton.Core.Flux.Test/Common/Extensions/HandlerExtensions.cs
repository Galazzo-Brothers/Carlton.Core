﻿using Carlton.Core.Flux.Contracts;
using Carlton.Core.Flux.Models;
using Carlton.Core.Flux.Tests.Common;
using Carlton.Core.Utilities.Results;
namespace Carlton.Core.Flux.Test.Common.Extensions;

public static class HandlerExtensions
{
    public static void SetupHandler<T>(this IViewModelQueryHandler<TestState> handler, T response)
    {
        handler.Handle(Arg.Any<ViewModelQueryContext<T>>(), Arg.Any<CancellationToken>())
               .Returns(Task.FromResult((Result<T, ViewModelFluxError>)response));
    }

    public static void SetupHandler<T>(this IMutationCommandHandler<TestState> handler)
    {
        handler.Handle(Arg.Any<MutationCommandContext<T>>(), Arg.Any<CancellationToken>())
               .Returns(Task.CompletedTask);
    }

    public static void VerifyHandler<TViewModel>(this IViewModelQueryHandler<TestState> handler)
    {
        handler.Received(1).Handle(Arg.Any<ViewModelQueryContext<TViewModel>>(), Arg.Any<CancellationToken>());
    }

    public static void VerifyHandler<T>(this IMutationCommandHandler<TestState> handler, T command)
    {
        handler.Received(1).Handle(Arg.Is<MutationCommandContext<T>>(context => context.MutationCommand.Equals(command)), Arg.Any<CancellationToken>());
    }
}