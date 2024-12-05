using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace Threllaut.Data;

[Table("TaskEvents")]
public class TaskEvent
{
    public Guid Id { get; set; }

    public required DateTimeOffset Date { get; set; }

    [DeleteBehavior(DeleteBehavior.Restrict)]
    public required ApplicationUser User { get; set; }

    public required BoardTask Task { get; set; }

    [DeleteBehavior(DeleteBehavior.NoAction)]
    public required Column Column { get; set; }

    public Column? PreviousColumn { get; set; }
}
