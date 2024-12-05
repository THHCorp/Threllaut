using Microsoft.EntityFrameworkCore;

namespace Threllaut.Data;

public class BoardTask
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    [DeleteBehavior(DeleteBehavior.Restrict)]
    public required IList<ApplicationUser> Assignees { get; set; }

    public required Column Column { get; set; }

    public required IList<TaskEvent> Events { get; set; }
}
