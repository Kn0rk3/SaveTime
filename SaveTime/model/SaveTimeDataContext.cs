using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace SaveTime.model
{
    class SaveTimeDataContext : DbContext
    {
        private string connectionString = String.Empty;

        public SaveTimeDataContext(string connectString) 
        {
            connectionString = connectString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(connectionString);
        }

    }
}
