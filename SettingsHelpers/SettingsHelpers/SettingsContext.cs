using Microsoft.EntityFrameworkCore;
using SettingsHelpers.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SettingsHelpers
{
    /// <summary>
    /// The class used to query against the stored settings in the database
    /// </summary>
    public class SettingsContext: DbContext
    {
        /// <summary>
        /// The connection string used to store information
        /// </summary>
        public static String ConnectionString { get; set; }
        
        /// <summary>
        /// The collection of settings stored in the database
        /// </summary>
        public DbSet<Setting> Settings { get; set; }

        public SettingsContext() : base()
        {

        }

        public SettingsContext(DbContextOptions<SettingsContext> contextOptions) : base(contextOptions)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(SettingsContext.ConnectionString);
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }

    }
}
