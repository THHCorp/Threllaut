using Microsoft.EntityFrameworkCore;

namespace Threllaut.Data.Contexts;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IDbContext
{
    public static string? Schema => "app";

    public DbSet<Board> Boards { get; set; } = default!;

    public DbSet<BoardTask> Tasks { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>(b =>
        {
            b.HasMany<Board>().WithMany(b => b.Members);
            b.HasMany<BoardTask>().WithMany(t => t.Assignees);
        });
    }

    public static void SeedData(ApplicationDbContext context)
    {
        var user = new ApplicationUser
        {
            UserName = "Test user",
            Email = "test@user.com"
        };

        var board = new Board
        {
            Id = new(-98798765, -7865, -28786, 99, 178, 78, 156, 98, 237, 196, 24),
            Name = "Task board",
            Description = "Example board with example data.",
            Columns = [],
            Members = [],
            Owner = user
        };

        board.Columns.Add(new() { Name = "To Do", Description = "Tasks not yet started", Board = board, Tasks = [] });
        board.Columns.Add(new() { Name = "In Progress", Description = "Tasks currently being worked on", Board = board, Tasks = [] });
        board.Columns.Add(new() { Name = "Done", Description = "Tasks that have been completed", Board = board, Tasks = [] });

        board.Columns[0].Tasks.Add(new() { Name = "Example task 1", Description = "Example task 1 description", Assignees = [], Column = board.Columns[0], Events = [] });
        board.Columns[0].Tasks.Add(new() { Name = "Example task 2", Description = "Example task 2 description", Assignees = [], Column = board.Columns[0], Events = [] });
        board.Columns[1].Tasks.Add(new() { Name = "Example task 3", Description = "Example task 3 description", Assignees = [], Column = board.Columns[1], Events = [] });
        board.Columns[2].Tasks.Add(new() { Name = "Example task 4", Description = "Example task 4 description", Assignees = [], Column = board.Columns[2], Events = [] });

        context.Boards.Add(board);
    }
}
