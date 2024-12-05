using Microsoft.EntityFrameworkCore;

namespace Threllaut.Data;

public class Board
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    [DeleteBehavior(DeleteBehavior.Restrict)]
    public required ApplicationUser Owner { get; set; }

    [DeleteBehavior(DeleteBehavior.Restrict)]
    public required IList<ApplicationUser> Members { get; set; }

    public required IList<Column> Columns { get; set; }
}
