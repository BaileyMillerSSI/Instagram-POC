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
            using (var db = new SettingsContext())
            {
                return db.Settings.Where(x => x.Key == Key && x.ApplicationName == ApplicationName && x.Version == Version).Count() != 0;
            }
        }

        public async Task<bool> HasSettingAsync(String Key)
        {
            using (var db = new SettingsContext())
            {
                return (await db.Settings.Where(x => x.Key == Key && x.ApplicationName == ApplicationName && x.Version == Version).CountAsync()) != 0;
            }
        }

        public void AddOrUpdateSetting(String Key, String Value)
        {
            if (HasSetting(Key))
            {
                using (var db = new SettingsContext())
                {
                    //Get Old Setting
                    var oldSetting = GetSetting(Key);

                    //Update all the changed values
                    oldSetting.LastUpdated = DateTime.Now;
                    oldSetting.Value = Value;

                    db.SaveChanges();
                }
            }
            else
            {
                //Add the setting as a new setting
                using (var db = new SettingsContext())
                {
                    db.Settings.Add(new Setting()
                    {
                        Key = Key,
                        ApplicationName = ApplicationName,
                        Version = Version,
                        Value = Value,
                        LastUpdated = DateTime.Now
                    });

                    db.SaveChanges();
                }
            }
        }

        public async Task AddOrUpdateSettingAsync(String Key, String Value)
        {
            if (await HasSettingAsync(Key))
            {
                using (var db = new SettingsContext())
                {
                    //Get Old Setting
                    var oldSetting = await GetSettingAsync(Key);

                    //Update all the changed values
                    oldSetting.LastUpdated = DateTime.Now;
                    oldSetting.Value = Value;

                    await db.SaveChangesAsync();
                }
            }
            else
            {
                //Add the setting as a new setting
                using (var db = new SettingsContext())
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

        public void DeleteSetting(String Key)
        {
            using (var db = new SettingsContext())
            {
                var setting = GetSetting(Key);
                if (setting != null)
                {
                    db.Settings.Remove(setting);
                    db.SaveChanges();
                }
            }
        }

        public async Task DeleteSettingAsync(String Key)
        {
            if (await HasSettingAsync(Key))
            {
                var setting = GetSettingAsync(Key);
            }
        }

        public Setting GetSetting(String Key)
        {
            if (HasSetting(Key))
            {
                using (var db = new SettingsContext())
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
                using (var db = new SettingsContext())
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

    }
}
