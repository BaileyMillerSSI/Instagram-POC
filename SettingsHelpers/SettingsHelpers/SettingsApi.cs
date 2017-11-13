using Microsoft.EntityFrameworkCore;
using SettingsHelpers.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SettingsHelpers
{
    public class SettingsApi
    {

        public DbContextOptions<SettingsContext> options;

        public String ApplicationName { get; set; }

        public String Version
        {
            get
            {
                if (String.IsNullOrEmpty(_Version))
                {
                    return Device.RuntimePlatform;
                }
                else
                {
                    return _Version;
                }
            }set
            {
                _Version = value;
            }
        }

        private String _Version;

        public SettingsApi(String AppName, String Version = null)
        {
            this.ApplicationName = AppName;
            this._Version = Version;
        }



        public bool HasSetting(String Key)
        {
            using (var db = CreateNewContent())
            {
                return db.Settings.Where(x => x.Key == Key && x.ApplicationName == ApplicationName && x.Version == Version).Count() != 0;
            }
        }

        public async Task<bool> HasSettingAsync(String Key)
        {
            using (var db = await CreateNewContextAsync())
            {
                return (await db.Settings.Where(x => x.Key == Key && x.ApplicationName == ApplicationName && x.Version == Version).CountAsync()) != 0;
            }
        }

        public void AddOrUpdateSetting(String Key, String Value)
        {
            if (HasSetting(Key))
            {
                using (var db = CreateNewContent())
                {
                    //Get Old Setting
                    var oldSetting = db.Attach(GetSetting(Key));

                    //Update all the changed values
                    oldSetting.CurrentValues.SetValues(new Dictionary<String, object>() { { "Value", Value}, { "LastUpdated", DateTime.Now } });

                    var saved = db.SaveChanges();
                }
            }
            else
            {
                //Add the setting as a new setting
                using (var db = CreateNewContent())
                {
                    db.Settings.Add(new Setting()
                    {
                        Key = Key,
                        ApplicationName = ApplicationName,
                        Version = Version,
                        Value = Value,
                        LastUpdated = DateTime.Now
                    });

                    var saved = db.SaveChanges();
                }
            }
        }

        public async Task AddOrUpdateSettingAsync(String Key, String Value)
        {
            try
            {
                if (await HasSettingAsync(Key))
                {
                    using (var db = await CreateNewContextAsync())
                    {
                        //Get Old Setting
                        var oldSetting = db.Attach(await GetSettingAsync(Key));

                        //Update all the changed values
                        oldSetting.CurrentValues.SetValues(new Dictionary<String, object>() { { "Value", Value }, { "LastUpdated", DateTime.Now } });

                        await db.SaveChangesAsync();
                    }
                }
                else
                {
                    //Add the setting as a new setting
                    using (var db = await CreateNewContextAsync())
                    {
                        await db.Settings.AddAsync(new Setting()
                        {
                            Key = Key,
                            ApplicationName = ApplicationName,
                            Version = Version,
                            Value = Value,
                            LastUpdated = DateTime.Now
                        });

                        await db.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        public void DeleteSetting(String Key)
        {

            var setting = GetSetting(Key);

            if (setting != null)
            {
                using (var db = CreateNewContent())
                {
                    db.Settings.Remove(setting);
                    db.SaveChanges();

                }
            }
        }

        public async Task DeleteSettingAsync(String Key)
        {
            var setting = await GetSettingAsync(Key);

            if (setting != null)
            {
                using (var db = await CreateNewContextAsync())
                {
                    db.Settings.Remove(setting);
                    await db.SaveChangesAsync();
                }
            }
        }

        public Setting GetSetting(String Key)
        {
            if (HasSetting(Key))
            {
                using (var db = CreateNewContent())
                {
                    return db.Settings.Single(x => x.Key == Key && x.ApplicationName == ApplicationName && x.Version == Version);
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<Setting> GetSettingAsync(String Key)
        {
            if (await HasSettingAsync(Key))
            {
                using (var db = await CreateNewContextAsync())
                {
                    return await db.Settings.SingleAsync(x => x.Key == Key && x.ApplicationName == ApplicationName && x.Version == Version);
                }
            }
            else
            {
                return null;
            }
        }

        public String GetValue(String Key)
        {
            var data = GetSetting(Key);

            if (data != null)
            {
                return data.Value;
            }
            else
            {
                return null;
            }
        }

        public async Task<String> GetValueAsync(String Key)
        {
            var data = await GetSettingAsync(Key);

            if (data != null)
            {
                return data.Value;
            }
            else
            {
                return null;
            }
        }

        public List<Setting> GetAllCompanySettings()
        {
            using (var db = CreateNewContent())
            {
                return db.Settings.Where(x => x.ApplicationName == ApplicationName && x.Version == Version).ToList();
            }
        }

        public async Task<List<Setting>> GetAllCompanySettingsAsync()
        {
            using (var db = await CreateNewContextAsync())
            {
                return await db.Settings.Where(x => x.ApplicationName == ApplicationName && x.Version == Version).ToListAsync();
            }
        }

        private async Task<SettingsContext> CreateNewContextAsync()
        {
            return await Task.Run(()=> 
            {
                if (options != null)
                {
                    return new SettingsContext(options);
                }
                else
                {
                    return new SettingsContext();
                }
            });
        }

        private SettingsContext CreateNewContent()
        {
            return CreateNewContextAsync().Result;
        }

    }
}
