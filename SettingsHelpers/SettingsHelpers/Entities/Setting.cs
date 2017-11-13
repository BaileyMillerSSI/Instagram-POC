using System;
using System.Collections.Generic;
using System.Text;

namespace SettingsHelpers.Entities
{
    public class Setting
    {
        public int Id { get; set; }

        public String Key { get; set; }

        public String Value { get; set; }

        public DateTime LastUpdated { get; set; }

        public String ApplicationName { get; set; }

        public String Version { get; set; }
    }
}
