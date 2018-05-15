using System;
using UnblockHackNET.BL.DB;
using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace UnblockHackNET.Helpers
{
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string SettingsKey = "settings_key";
        private static readonly string SettingsDefault = string.Empty;
        private static readonly User DefaultUser = new User();
        private static string DefaultSerUser = JsonConvert.SerializeObject(DefaultUser);
        private static readonly string UserKey = "3c6f1bd0-eca7-4c33-9f42-3eea38a59d1b";

        #endregion

        public static User GlobalUser
        {
            get
            {
                var tmp = AppSettings.GetValueOrDefault(UserKey, DefaultSerUser);
                var result = new User();
                if (!string.IsNullOrEmpty(tmp))
                {
                    result = JsonConvert.DeserializeObject<User>(tmp);
                }
                return result;
            }
            set
            {
                AppSettings.AddOrUpdateValue(UserKey, JsonConvert.SerializeObject(value));
            }
        }

        public static string GeneralSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(SettingsKey, value);
            }
        }

    }
}
