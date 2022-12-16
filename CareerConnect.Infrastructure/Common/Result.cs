namespace CareerConnect.Infrastructure.Common
{
    public class Result<TResultData> : Result
    {
        public Result()
        {
        }
        public Result(string message)
        {
            Message = message;
        }

        public TResultData? Data { get; set; }
    }

    public class Result
    {
        public Result()
        {
        }

        public Result(string message)
        {
            Message = message;
        }
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}
