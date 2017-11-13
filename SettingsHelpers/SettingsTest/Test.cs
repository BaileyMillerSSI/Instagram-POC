using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SettingsHelpers;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using System.Threading.Tasks;

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

            StartTest();
            ConfigureApi();

            String settingBeforeSave = null;

            api.AddOrUpdateSetting("Number", 1.ToString());

            var settingAfterSave1 = api.GetSetting("Number");

            api.AddOrUpdateSetting("Number", 2.ToString());

            var settingAfterSave2 = api.GetSetting("Number");

            Assert.AreNotEqual(settingBeforeSave, settingAfterSave1);

            Assert.AreNotEqual(settingAfterSave1.Value, settingAfterSave2.Value);

            EndTest();
            
        }

        [TestMethod]
        public async Task UpdateSettingSavesAsync()
        {
            StartTest();
            ConfigureApi();

            String settingBeforeSave = null;

            await api.AddOrUpdateSettingAsync("Number", 1.ToString());

            var settingAfterSave1 = await api.GetSettingAsync("Number");

            await api.AddOrUpdateSettingAsync("Number", 2.ToString());

            var settingAfterSave2 = await api.GetSettingAsync("Number");

            Assert.AreNotEqual(settingBeforeSave, settingAfterSave1);

            Assert.AreNotEqual(settingAfterSave1.Value, settingAfterSave2.Value);

            EndTest();
        }

        [TestMethod]
        public void MultipleVersionIsSafe()
        {
            StartTest();

            ConfigureApi();

            var quickyApi = new SettingsApi("com.company.name", "UnitTests2") { options = GetOptions() };

            api.AddOrUpdateSetting("FirstName", "Bailey1");

            quickyApi.AddOrUpdateSetting("FirstName", "Bailey2");
            quickyApi.AddOrUpdateSetting("LastName", "Miller");

            Assert.AreEqual(api.GetValue("FirstName"), "Bailey1");

            Assert.AreEqual(quickyApi.GetValue("FirstName"), "Bailey2");
            Assert.AreEqual(quickyApi.GetValue("LastName"), "Miller");

            Assert.IsTrue(api.GetAllCompanySettings().Count == 1);

            Assert.IsTrue(quickyApi.GetAllCompanySettings().Count == 2);

            EndTest();
            

        }

        [TestMethod]
        public async Task MultipleVersionIsSafeAsync()
        {
            StartTest();

            ConfigureApi();

            var quickyApi = new SettingsApi("com.company.name", "UnitTests2") { options = GetOptions() };

            await api.AddOrUpdateSettingAsync("FirstName", "Bailey1");

            await quickyApi.AddOrUpdateSettingAsync("FirstName", "Bailey2");
            await quickyApi.AddOrUpdateSettingAsync("LastName", "Miller");

            Assert.AreEqual(await api.GetValueAsync("FirstName"), "Bailey1");

            Assert.AreEqual(await quickyApi.GetValueAsync("FirstName"), "Bailey2");
            Assert.AreEqual(await quickyApi.GetValueAsync("LastName"), "Miller");

            Assert.IsTrue((await api.GetAllCompanySettingsAsync()).Count == 1);

            Assert.IsTrue((await quickyApi.GetAllCompanySettingsAsync()).Count == 2);

            EndTest();
        }

        [TestMethod]
        public void MultipleCompanyIsSafe()
        {
            StartTest();

            ConfigureApi();

            var quickyApi = new SettingsApi("com.company.name1", "UnitTests") { options = GetOptions() };

            api.AddOrUpdateSetting("FirstName", "Bailey1");

            quickyApi.AddOrUpdateSetting("FirstName", "Bailey2");
            quickyApi.AddOrUpdateSetting("LastName", "Miller");

            Assert.AreEqual(api.GetValue("FirstName"), "Bailey1");

            Assert.AreEqual(quickyApi.GetValue("FirstName"), "Bailey2");
            Assert.AreEqual(quickyApi.GetValue("LastName"), "Miller");

            Assert.IsTrue(api.GetAllCompanySettings().Count == 1);

            Assert.IsTrue(quickyApi.GetAllCompanySettings().Count == 2);

            EndTest();
        }

        [TestMethod]
        public async Task MultipleCompanyIsSafeAsync()
        {
            StartTest();

            ConfigureApi();

            var quickyApi = new SettingsApi("com.company.name1", "UnitTests") { options = GetOptions() };

            await api.AddOrUpdateSettingAsync("FirstName", "Bailey1");

            await quickyApi.AddOrUpdateSettingAsync("FirstName", "Bailey2");
            await quickyApi.AddOrUpdateSettingAsync("LastName", "Miller");

            Assert.AreEqual(await api.GetValueAsync("FirstName"), "Bailey1");

            Assert.AreEqual(await quickyApi.GetValueAsync("FirstName"), "Bailey2");
            Assert.AreEqual(await quickyApi.GetValueAsync("LastName"), "Miller");

            Assert.IsTrue((await api.GetAllCompanySettingsAsync()).Count == 1);

            Assert.IsTrue((await quickyApi.GetAllCompanySettingsAsync()).Count == 2);

            EndTest();
        }

        [TestMethod]
        public void DeleteSettingWorks()
        {
            StartTest();
            ConfigureApi();

            api.AddOrUpdateSetting("FirstName", "Bailey");

            var settingBeforeDelete = api.GetSetting("FirstName");

            api.DeleteSetting("FirstName");

            var settingAfterDelete = api.GetSetting("FirstName");

            Assert.IsNotNull(settingBeforeDelete);

            Assert.IsNull(settingAfterDelete);

        }

        [TestMethod]
        public async Task DeleteSettingWorksAsync()
        {
            StartTest();
            ConfigureApi();

            await api.AddOrUpdateSettingAsync("FirstName", "Bailey");

            var settingBeforeDelete = await api.GetSettingAsync("FirstName");

            await api.DeleteSettingAsync("FirstName");

            var settingAfterDelete = await api.GetSettingAsync("FirstName");

            Assert.IsNotNull(settingBeforeDelete);

            Assert.IsNull(settingAfterDelete);

        }

        private void ConfigureApi()
        {
            api = new SettingsApi("com.company.name", "UnitTests") { options = GetOptions() };
        }

        private void EndTest()
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }


        private void StartTest()
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
