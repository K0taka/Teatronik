namespace Teatronik.Core.Common
{
    public sealed class Result<T> : Result
    {
        public T? Value { get; }

        private Result(bool isSuccess, string? error, T? value) : base(isSuccess, error)
        {
            Value = value;
        }

        //Succeed result with value
        public static Result<T> Ok(T value) => new(true, null, value);

        //Error result
        public static new Result<T> Fail(string error) => new(false, error, default);
    }
}
