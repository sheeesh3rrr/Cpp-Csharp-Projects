namespace ApiGateway.Options
{
    public class DownstreamServiceOptions
    {
        public string? BaseUrl { get; set; }
    }

    public class DownstreamOptions
    {
        public DownstreamServiceOptions? FileStoring { get; set; }
        public DownstreamServiceOptions? FileAnalysis { get; set; }
        public int HttpTimeoutSeconds { get; set; } = 30;
    }
}
