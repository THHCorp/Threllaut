using System.ComponentModel.DataAnnotations.Schema;

namespace Threllaut.Data;

[Table("Columns")]
public class Column
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public required Board Board { get; set; }

    public required IList<BoardTask> Tasks { get; set; }
}
