namespace Threllaut.Data;

public class Board
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public required IList<Column> Columns { get; set; }
}
