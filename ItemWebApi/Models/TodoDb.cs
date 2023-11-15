namespace ItemWebApi.Models;

using Microsoft.EntityFrameworkCore;

class TodoDbContext : DbContext
{
    public TodoDbContext(DbContextOptions<TodoDb> options)
        : base(options) { }

    public DbSet<Todo> Todos => Set<Todo>();
}
