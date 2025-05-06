namespace Teatronik.Core.Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string? Error { get; }

        protected Result(bool isSuccess, string? error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        // Succeed result
        public static Result Ok() => new(true, null);

        // Error result
        public static Result Fail(string error) => new(false, error);
    }
}
