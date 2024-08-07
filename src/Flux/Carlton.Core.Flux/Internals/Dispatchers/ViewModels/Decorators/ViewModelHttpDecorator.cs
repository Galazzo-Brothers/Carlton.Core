﻿using System.Net.Http.Json;
using Carlton.Core.Flux.Attributes;
using Carlton.Core.Flux.Handlers.Base;
namespace Carlton.Core.Flux.Internals.Dispatchers.ViewModels.Decorators;

internal sealed class ViewModelHttpDecorator<TState>(
	IViewModelQueryDispatcher<TState> _decorated,
	HttpClient _client,
	IMutableFluxState<TState> _state)
	: BaseHttpDecorator<TState>(_client), IViewModelQueryDispatcher<TState>
{
	public async Task<Result<TViewModel, FluxError>> Dispatch<TViewModel>(object sender, ViewModelQueryContext<TViewModel> context, CancellationToken cancellationToken)
	{
		//Get FluxServerCommunicationAttribute Attribute
		var attributes = sender.GetType().GetCustomAttributes();
		var fluxServerCommunicationAttribute = attributes.OfType<FluxViewModelServerUrlAttribute>().FirstOrDefault();
		var requiresRefresh = GetRefreshPolicy(fluxServerCommunicationAttribute?.ServerCommunicationPolicy);

		if (requiresRefresh)
		{
			//Update the context for logging and auditing
			context.MarkAsRequiresHttpRefresh();

			//Find FluxServerCommunicationParameterAttributes
			var parameterAttributes = GetParameterAttributes(sender);

			//Construct Http Refresh URL
			var serverUrlResult = GetServerUrl(fluxServerCommunicationAttribute, parameterAttributes, sender);

			//Get ViewModel from server
			var vmResult = await SendRequest(serverUrlResult, context, cancellationToken);

			//If http refresh failed return error
			if (!vmResult.IsSuccess)
				return vmResult.GetError();
		}

		//Continue with Dispatch
		return await _decorated.Dispatch(sender, context, cancellationToken);
	}

	private async Task<Result<bool, FluxError>> SendRequest<TViewModel>(Result<string, FluxError> serverUrlResult, ViewModelQueryContext<TViewModel> context, CancellationToken cancellationToken)
	{
		return await serverUrlResult.Match
		(
			async serverUrl =>
			{
				//Get ViewModel from server
				var vmResult = await GetHttpViewModel(serverUrl, context, cancellationToken);

				//Update the StateStore and pickup any mutation errors
				return await ApplyViewModelStateMutation(vmResult, context);
			},
			err => err.ToResultTask<bool, FluxError>()
		);
	}

	private async Task<Result<TViewModel, FluxError>> GetHttpViewModel<TViewModel>(string serverUrl, ViewModelQueryContext<TViewModel> context, CancellationToken cancellationToken)
	{
		try
		{
			//Get ViewModel from server
			var response = await Client.GetAsync(serverUrl, cancellationToken);

			//Update Context
			context.MarkAsHttpCallMade(serverUrl, response.StatusCode);

			//Return error value if http call unsuccessful
			if (!response.IsSuccessStatusCode)
				return HttpRequestFailedError(response);

			//Deserialize the content to the specified type
			var viewModel = await response.Content.ReadFromJsonAsync<TViewModel>(cancellationToken: cancellationToken);

			//Update Context
			context.MarkAsHttpCallSucceeded(viewModel);

			//return ViewModel
			return viewModel;
		}
		catch (HttpRequestException ex)
		{
			return HttpError(ex);
		}
		catch (JsonException ex)
		{
			return JsonError(ex);
		}
		catch (NotSupportedException ex) when (ex.Message.Contains("Serialization and deserialization"))
		{
			return JsonError(ex);
		}
	}

	private async Task<Result<bool, FluxError>> ApplyViewModelStateMutation<TViewModel>(Result<TViewModel, FluxError> vmResult, ViewModelQueryContext<TViewModel> context)
	{
		return await vmResult.Match
		(
			async vm =>
			{
				//Apply State Mutation
				var result = await _state.ApplyMutationCommand(vm);

				if (!result.IsSuccess)
					return result.GetError();

				//Update Context with Success and Return
				context.MarkAsStateModifiedByHttpRefresh();
				return true;
			},
			err => err.ToResultTask<bool, FluxError>()
		);
	}
}
