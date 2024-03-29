﻿using Microsoft.EntityFrameworkCore;
using SwapMe.Domain.Users;

namespace SwapMe.Infrastructure.Sql.Contexts;

public class UsersContext : DbContext
{
    public UsersContext(DbContextOptions<UsersContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
}