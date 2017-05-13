using System;
using ConfigLite.File;

namespace ConfigLite.Items
{
    internal class Item
    {
        public static string EnvVarEnvVarPrefix { get; set; }
        public static ConfigFileReader ConfigFileReader { get; set; }

        public static T GetValue<T>(string section, string key, T defaultValue = default(T))
        {
            try
            {
                string value = GetValueFromEnvVar(section, key);

                if (string.IsNullOrWhiteSpace(value))
                    if (ConfigFileReader != null)
                        value = ConfigFileReader.GetValue(section, key);

                return value != null ? GetValueFromString<T>(value) : defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        public static string GetValueFromEnvVar(string section, string key)
        {
            string variable = "";

            if (string.IsNullOrWhiteSpace(EnvVarEnvVarPrefix) == false)
                variable = EnvVarEnvVarPrefix + "_";

            if (string.IsNullOrWhiteSpace(section) == false)
                variable += section + "_";

            variable += key;

            return Environment.GetEnvironmentVariable(variable);
        }

        public static T GetValueFromString<T>(string value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public static bool Exists(string key)
        {
            return Exists(null, key);
        }

        public static bool Exists(string section, string key)
        {
            try
            {
                string value = GetValueFromEnvVar(section, key);

                if (string.IsNullOrWhiteSpace(value))
                    if (ConfigFileReader != null)
                        value = ConfigFileReader.GetValue(section, key);

                return value != null;
            }
            catch
            {
                return false;
            }
        }
    }
}