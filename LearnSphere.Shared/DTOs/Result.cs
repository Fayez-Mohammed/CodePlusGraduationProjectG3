namespace LearnSphere.Shared.DTOs
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

    }
    public class Result<T>:Result
    {
        public bool IsSuccess { get; }
        public T? Value { get; }
        public string? Error { get; }
        public int StatusCode { get; }

        private Result(bool isSuccess, T? value, string? error, int statusCode)
            :base(isSuccess, error, statusCode)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
            StatusCode = statusCode;
        }

        public static Result<T> Success(T value, int statusCode = 200)
            => new(true, value, null, statusCode);

        public static Result<T> Failure(string error, int statusCode = 400)
            => new(false, default, error, statusCode);
    }
    public class Result
    {
        public bool IsSuccess { get; }
        public string? Error { get; }
        public int StatusCode { get; }

        protected Result(bool isSuccess, string? error, int statusCode)
        {
            IsSuccess = isSuccess;
            Error = error;
            StatusCode = statusCode;
        }

        public static Result Success(int statusCode = 200)
            => new(true, null, statusCode);

        public static Result Failure(string error, int statusCode = 400)
            => new(false, error, statusCode);
    }
}