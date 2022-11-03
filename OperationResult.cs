namespace TestStocks
{
    public class OperationResult
    {
        public string Error { get; set; }

        public bool Success => string.IsNullOrWhiteSpace(Error);

        public OperationResult()
        {
        }

        public OperationResult(string error)
        {
            Error = error;
        }

        public static readonly OperationResult Ok = new OperationResult();
        public static OperationResult FromError(string error) => new OperationResult(error);
    }

    public class OperationResult<T> : OperationResult
    {
        public OperationResult()
        { }

        public OperationResult(string error) : base(error)
        { }

        public OperationResult(T result)
        {
            Result = result;
        }

        public T Result { get; set; }

        public new static OperationResult<T> Ok(T result) => new OperationResult<T>(result);
        public new static OperationResult<T> FromError(string error) => new OperationResult<T>(error);
    }
}
