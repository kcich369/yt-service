namespace Domain.Results;

public class ModelResult<TFirst, TSecond>
{
    public static Func<TFirst, Task<IResult<TSecond>>> Next(Func<TFirst, Task<IResult<TSecond>>> next) => next;
}