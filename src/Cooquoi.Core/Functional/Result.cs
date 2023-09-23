using System.Data.SqlTypes;

namespace Cooquoi.Core.Functional;

public class Result<T> 
{ 
    private readonly List<Failure> _failures;
    
    protected Result(T? data, IEnumerable<Failure> failures)
    {
        Data = data;
        _failures = failures.ToList();
    }

    public T? Data { get; }
    public bool IsSuccess => _failures.All(f => f.Severity != Severity.Error);
    public IReadOnlyCollection<Failure> Failures => _failures.AsReadOnly();
    public static Result<T> Success(T data) =>  new(data, Enumerable.Empty<Failure>());
    public static Result<T> Fail(IEnumerable<Failure> failures) => new(default, failures);
}

public class Result : Result<INullable>
{
    private Result(IEnumerable<Failure> failures) : base(null,failures)
    {
    }
    
    public static Result Success() =>  new(Enumerable.Empty<Failure>());
    public new static Result Fail(IEnumerable<Failure> failures) => new(failures);
}