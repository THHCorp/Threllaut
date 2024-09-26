namespace Threllaut.ApiService.Database.Contexts;

public interface IDbContext
{
    static abstract string? Schema { get; }
}
