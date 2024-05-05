namespace Argo.CA.Infrastructure.Logging;

public class LogOptions
{
    public string Level { get; set; } = string.Empty;
    public SeqOptions Seq { get; set; } = new();
}