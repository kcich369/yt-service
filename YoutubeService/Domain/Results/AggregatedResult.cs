namespace Domain.Results;

public class AggregatedResult
{
    public bool IsError { get; private set; }
    public string ErrorMessage { get; private set; }
    public IEnumerable<object> Data { get; private set; }

    private AggregatedResult( IEnumerable<object> data)
    {
        Data = data;
    }

    private AggregatedResult(string errorMessage)
    {
        ErrorMessage = errorMessage;
        IsError = true;
    }

    public static AggregatedResult Success(IEnumerable<object> data) => new AggregatedResult(data);


    public static AggregatedResult Error(string errorMessage) => new AggregatedResult(errorMessage);
}