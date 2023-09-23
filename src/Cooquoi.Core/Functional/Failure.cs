namespace Cooquoi.Core.Functional;

public record Failure
{
    public Severity Severity { get; init; } = Severity.Error;
    public string Code { get; init; } = "Unknown";
    public string SubCode { get; init; } = "N/A";
    public string Message { get; init; } = string.Empty;
    public dynamic? Data { get; init; } = null;
}