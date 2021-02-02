using Microsoft.EntityFrameworkCore;

namespace BingoMaster_Entities
{
	public class BingoMasterDbContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Player> Players { get; set; }
		public DbSet<GamePlayer> GamePlayers { get; set;}
		public DbSet<Game> Games { get; set; }

		public BingoMasterDbContext(DbContextOptions<BingoMasterDbContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>()
				.HasOne(user => user.Player)
				.WithOne(player => player.User)
				.HasForeignKey<Player>(player => player.UserId);

			modelBuilder.Entity<User>(user =>
			{
				user.HasIndex(e => e.EmailAddress).IsUnique();
			});

			modelBuilder.Entity<Player>(player =>
			{
				player.HasIndex(e => e.Name).IsUnique();
			});

			modelBuilder.Entity<GamePlayer>()
				.HasIndex(gamePlayer => new { gamePlayer.GameId, gamePlayer.PlayerId });

			modelBuilder.Entity<GamePlayer>()
				.HasOne(gamePlayer => gamePlayer.Game)
				.WithMany(game => game.GamePlayers)
				.HasForeignKey(gamePlayer => gamePlayer.GameId);

			modelBuilder.Entity<GamePlayer>()
				.HasOne(gamePlayer => gamePlayer.Player)
				.WithMany(player => player.GamePlayers)
				.HasForeignKey(gamePlayer => gamePlayer.PlayerId);
		}
	}
}
