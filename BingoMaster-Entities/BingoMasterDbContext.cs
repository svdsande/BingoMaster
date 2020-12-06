using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BingoMaster_Entities
{
	public class BingoMasterDbContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Player> Players { get; set; }
		public DbSet<Game> Games { get; set; }

		public BingoMasterDbContext(DbContextOptions<BingoMasterDbContext> options) :base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>()
				.HasOne(user => user.Player)
				.WithOne(player => player.User)
				.HasForeignKey<Player>(player => player.UserId);
		}
	}
}
