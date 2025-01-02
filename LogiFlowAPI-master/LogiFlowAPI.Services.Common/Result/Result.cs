namespace LogiFlowAPI.Services.Common.Result
{
    // Casting from Result to Result<T> is an issue
    public class Result<T> : Result
    {
        private readonly T? value;
        private readonly string? errorMessage;

        public T? Value
        {
            get => this.IsSuccess
                ? this.value
                : throw new InvalidOperationException("Cannot access the Value of an unsuccessful Result.");
        }

        private Result(bool isSuccess, int statusCode, T? value, string? errorMessage) : base(isSuccess, statusCode, errorMessage)
        {
            this.value = value;
        }

        public static Result<T> Ok(T? value = default) => Ok(200, value);

        public static new Result<T> Error(string errorMessage) => Error(400, errorMessage);

        public static Result<T> Ok(int statusCode, T? value = default)
        {
            return new Result<T>(true, statusCode, value, null);
        }

        public static new Result<T> Error(int statusCode, string errorMessage)
        {
            return new Result<T>(false, statusCode, default, errorMessage);
        }

        public static Result<T> ToGenericResult(Result result)
        {
            if (result.IsSuccess)
                return new Result<T>(result.IsSuccess, result.StatusCode, default, null);
            else
                return new Result<T>(result.IsSuccess, result.StatusCode, default, result.ErrorMessage);
        }

        public override string ToString()
        {
            if (this.IsSuccess)
                return $"Success: {this.Value}";
            else
            {
                return $"Error: {this.ErrorMessage}";
            }
        }
    }

    public class Result
    {
        private readonly string? errorMessage;

        public bool IsSuccess { get; }

        public bool IsFailure => !this.IsSuccess;

        public int StatusCode { get; }

        public string? ErrorMessage
        {
            get => !this.IsSuccess
                ? this.errorMessage
                : throw new InvalidOperationException("Cannot access the ErrorMessage of a successful Result.");
        }

        protected Result(bool isSuccess, int statusCode, string? errorMessage)
        {
            this.IsSuccess = isSuccess;
            this.StatusCode = statusCode;
            this.errorMessage = errorMessage;
        }


        public static Result Ok() => Ok(200);

        public static Result Error(string errorMessage) => Error(400, errorMessage);

        public static Result Ok(int statusCode)
        {
            return new Result(true, statusCode, null);
        }

        public static Result Error(int statusCode, string errorMessage)
        {
            return new Result(false, statusCode, errorMessage);
        }

        public override string ToString()
        {
            if (this.IsSuccess)
                return $"Success.";
            else
            {
                return $"Error: {this.ErrorMessage}";
            }
        }
    }
}
