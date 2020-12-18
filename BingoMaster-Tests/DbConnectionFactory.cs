using BingoMaster_Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;

namespace BingoMaster_Tests
{
	public class DbConnectionFactory : IDisposable
	{
		private bool disposed = false;

		public BingoMasterDbContext CreateInMemoryContext()
		{
			var options = new DbContextOptionsBuilder<BingoMasterDbContext>().UseInMemoryDatabase(databaseName: "BingoMaster_Test_Database").Options;
			var context = new BingoMasterDbContext(options);

			if (context != null)
			{
				context.Database.EnsureDeleted();
				context.Database.EnsureCreated();
			}

			return context;
		}

		public BingoMasterDbContext CreateSQLiteContext()
		{
			var connection = new SqliteConnection("DataSource=:memory:");
			connection.Open();

			var options = new DbContextOptionsBuilder<BingoMasterDbContext>().UseSqlite(connection).Options;
			var context = new BingoMasterDbContext(options);

			if (context != null)
			{
				context.Database.EnsureDeleted();
				context.Database.EnsureCreated();
			}

			return context;
		}

		public void Dispose()
		{
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
			{
				return;
			}

			if (disposing)
			{
			}

			disposed = true;
		}
	}
}
