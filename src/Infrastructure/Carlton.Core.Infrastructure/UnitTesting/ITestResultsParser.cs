﻿namespace Carlton.Core.Infrastructure.UnitTesting;

public interface ITestResultsParser
{
    public TestResultsReportModel ParseTestResults(string content);
    public IDictionary<string, TestResultsReportModel> ParseTestResultsByGroup(string content, string groupKey);
    public IDictionary<string, TestResultsReportModel> ParseTestResultsByGroup(string content);
}
