using Microsoft.EntityFrameworkCore;

namespace AnonFilesUpload.Data.Entity
{
    public class DataContext: DbContext
    {

        public DbSet<Data> Data { get; set; }

        public string DbPath { get; private set; }

        //public DataContext()
        //{
        //    var folder = Environment.SpecialFolder.LocalApplicationData;
        //    var path = Environment.GetFolderPath(folder);
        //    DbPath = $"{path}{System.IO.Path.DirectorySeparatorChar}AnnonFile.db";
        //}

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            // optionsBuilder.UseSqlite($"Data Source={DbPath}");

            //var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "MyDb.db" };
            //var connectionString = connectionStringBuilder.ToString();
            //var connection = new SqliteConnection(connectionString);

            //optionsBuilder.UseSqlite(connection);



        }

    }
}
