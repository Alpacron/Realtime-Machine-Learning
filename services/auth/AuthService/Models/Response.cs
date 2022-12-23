namespace AuthService.Models;

public abstract class Response<T>
{
    public bool Success { get; set; } = true;
    public string Details { get; set; } = default!;
    public T Result { get; set; } = default!;
}
