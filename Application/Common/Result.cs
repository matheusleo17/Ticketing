using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ticketing.Application.Common
{
    public class Result<T>
    {
        public Result() { }
        public T? Value { get; }
        public bool IsSuccess { get; }
        public ErrorType? Error { get; }

        private Result(bool isSuccess, T? value, ErrorType? error)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
        }

        public static Result<T> Success(T value)
            => new(true, value, null);

        public static Result<T> Failure(ErrorType error)
            => new(false, default, error);
    }
}
