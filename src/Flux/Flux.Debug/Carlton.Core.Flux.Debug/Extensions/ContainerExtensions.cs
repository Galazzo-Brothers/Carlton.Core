﻿using Carlton.Core.Flux.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
namespace Carlton.Core.Flux.Debug.Extensions;

/// <summary>
/// Provides extension methods for configuring Carlton Flux with debug options and logging services.
/// </summary>
public static class ContainerExtensions
{
	/// <summary>
	/// Adds Carlton Flux with debug options to the specified <see cref="IServiceCollection"/>.
	/// </summary>
	/// <typeparam name="TState">The type of the state used by Carlton Flux.</typeparam>
	/// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
	/// <param name="state">The initial state for Carlton Flux.</param>
	public static void AddCarltonFluxDebug<TState>(this IServiceCollection services, params Type[] ignoreTypes)
	{
		services.AddSingleton(FluxTypes.Create<TState>(ignoreTypes));
		services.AddSingleton<IFluxStateWrapper, FluxStateWrapper<TState>>();
		services.AddSingleton<FluxDebugState>();

		RegisterLogging(services);

		services.AddCarltonFlux<FluxDebugState>(opts =>
		{
			opts.AddHttpInterception = false;
			opts.AddLocalStorage = false;
		});

		services.Decorate<IViewModelQueryDispatcher<FluxDebugState>, FluxDebugViewModelQueryLoggingScopesMiddleware<FluxDebugState>>();
		services.Decorate<IMutationCommandDispatcher<FluxDebugState>, FluxDebugMutationCommandLoggingScopesMiddleware<FluxDebugState>>();
	}

	private static void RegisterLogging(IServiceCollection services)
	{
		var provider = new MemoryLoggerProvider();
		services.AddLogging(b =>
		{
			b.AddProvider(provider);
		});
		services.AddSingleton(_ => (MemoryLogger)provider.CreateLogger(null));
	}
}


