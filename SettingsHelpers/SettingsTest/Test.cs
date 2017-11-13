using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SettingsHelpers;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace SettingsTest
{
    [TestClass]
    public class Test
    {

        SettingsApi api;
        SqliteConnection connection;

        
        [TestMethod]
        public void UpdateSettingSaves()
        {

            EnsureDatabaseExists();
            api = new SettingsApi("com.company.name", "UnitTests") { options = GetOptions() };

            String settingBeforeSave = null;

            api.AddOrUpdateSetting("Number", 1.ToString());

            var settingAfterSave1 = api.GetSetting("Number");

            api.AddOrUpdateSetting("Number", 2.ToString());

            var settingAfterSave2 = api.GetSetting("Number");

            Assert.AreNotEqual(settingBeforeSave, settingAfterSave1);

            Assert.AreNotEqual(settingAfterSave1.Value, settingAfterSave2.Value);

            connection.Close();
        }

        private void EnsureDatabaseExists()
        {
            // In-memory database only exists while the connection is open
            connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            using (var db = new SettingsContext(GetOptions()))
            {
                db.Database.EnsureCreated();
            }
        }

        private DbContextOptions<SettingsContext> GetOptions()
        {
            

            return new DbContextOptionsBuilder<SettingsContext>()
                .UseSqlite(connection).Options;
        }
    }
}
