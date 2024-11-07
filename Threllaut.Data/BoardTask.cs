namespace Threllaut.Data;

public class BoardTask
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public required IList<ApplicationUser> Assignees { get; set; }

    public required Column Column { get; set; }
}
