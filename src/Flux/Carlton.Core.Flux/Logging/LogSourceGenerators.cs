﻿namespace Carlton.Core.Flux.Logging;

public static partial class LogSourceGenerators
{
    [LoggerMessage(
       Level = LogLevel.Error,
       Message = "An error occured processing query for ViewModel of type {Type}")]
    public static partial void ViewModelQueryErrored(this ILogger logger, string type, Exception ex);

    [LoggerMessage(
      Level = LogLevel.Error,
      Message = "An error occured processing mutation command of type {Type}")]
    public static partial void MutationCommandErrored(this ILogger logger, string type, Exception ex);

    [LoggerMessage(
       EventId = LogEvents.ViewModel_Completed,
       Level = LogLevel.Information,
       Message = "Completed processing query for ViewModel of type {Type}")]
    public static partial void ViewModelQueryCompleted(this ILogger logger, string type);

    [LoggerMessage(
        EventId = LogEvents.ViewModel_JsInterop_Completed,
        Level = LogLevel.Information,
        Message = "Completed JSInterop query for ViewModel of type {Type}")]
    public static partial void ViewModelJsInteropQueryCompleted(this ILogger logger, string type);

    [LoggerMessage(
        EventId = LogEvents.ViewModel_JsInterop_Error,
        Level = LogLevel.Error,
        Message = "An error occurred during JSInterop query for ViewModel of type {Type}")]
    public static partial void ViewModelJsInteropQueryErrored
        (this ILogger logger, Exception ex, string type);

    [LoggerMessage(
         EventId = LogEvents.Mutation_Completed,
         Level = LogLevel.Information,
         Message = "Completed processing mutation of type {Type}")]
    public static partial void MutationCommandCompleted(this ILogger logger, string type);
}