namespace Threllaut.Data.Contexts;

public interface IDbContext
{
    static abstract string? Schema { get; }
}
