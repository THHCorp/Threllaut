using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Threllaut.Data;
using Threllaut.Data.Contexts;

var builder = WebApplication.CreateBuilder(args);

builder.AddSqlServerDbContext<IdentityDbContext>("database");
builder.AddSqlServerDbContext<ApplicationDbContext>("database", null,
    builder => builder.UseSeeding((context, _) =>
    {
        ApplicationDbContext.SeedData((ApplicationDbContext)context);
        context.SaveChanges();
    }).UseAsyncSeeding((context, _, cancellationToken) =>
    {
        ApplicationDbContext.SeedData((ApplicationDbContext)context);
        return context.SaveChangesAsync(cancellationToken);
    }));

builder.Services.AddIdentityApiEndpoints<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<IdentityDbContext>();

builder.Services.AddAuthorization();

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services
    .AddProblemDetails()
    .AddCors()
    .AddOpenApi();

var app = builder.Build();

if (!EF.IsDesignTime)
    using (var scope = app.Services.CreateScope())
    {
        await scope.ServiceProvider.GetRequiredService<IdentityDbContext>()
            .Database.MigrateAsync();
        await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>()
            .Database.MigrateAsync();
    }

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapOpenApi();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
        options.RoutePrefix = "swagger";
    });
}

app.MapGet("/boards", ([FromServices] ApplicationDbContext context) =>
    context.Boards.Select(b => new
    {
        b.Id,
        b.Name,
        Owner = b.Owner.Id,
        Columns = b.Columns.Select(c => c.Id)
    }).AsAsyncEnumerable());

app.MapGet("/board/{id}", async ([FromRoute] Guid id, [FromServices] ApplicationDbContext context) =>
    await context.Boards.Select(b => new
    {
        b.Id,
        b.Name, b.Description,
        Owner = b.Owner.Id,
        Columns = b.Columns.Select(c => c.Id),
        Members = b.Members.Select(c => c.Id)
    }).SingleAsync(b => b.Id == id));

app.MapGet("/column/{id}", async ([FromRoute] Guid id, [FromServices] ApplicationDbContext context) =>
    await context.Set<Column>().Select(c => new
    {
        c.Id,
        c.Name, c.Description,
        Board = c.Board.Id,
        Tasks = c.Tasks.Select(t => t.Id)
    }).SingleAsync(c => c.Id == id));

app.MapGet("/task/{id}", async ([FromRoute] Guid id, [FromServices] ApplicationDbContext context) =>
    await context.Tasks.Select(t => new
    {
        t.Id,
        t.Name, t.Description,
        Column = t.Column.Id,
        Assignees = t.Assignees.Select(u => u.Id)
    }).SingleAsync(t => t.Id == id));

app.MapGet("/task/{id}/events", async ([FromRoute] Guid id, [FromServices] ApplicationDbContext context) =>
    context.Tasks.Where(t => t.Id == id).SelectMany(t => t.Events).Select(e => new
    {
        e.Id,
        e.Date,
        User = e.User.Id,
        Column = e.Column.Id,
        PreviousColumn = (Guid?)(e.PreviousColumn != null ? e.PreviousColumn.Id : null),
    }).AsAsyncEnumerable());

app.MapDefaultEndpoints()
    .MapIdentityApi<ApplicationUser>();

await app.RunAsync();
