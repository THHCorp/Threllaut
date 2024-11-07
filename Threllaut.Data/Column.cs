namespace Threllaut.Data;

public class Column
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public required Board Board { get; set; }

    public required IList<BoardTask> Tasks { get; set; }
}
