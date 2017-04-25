using ConfigLite.File;
using ConfigLite.Items;

namespace ConfigLite
{
    public class ConfigStore
    {
        private ConfigStore(string envVarPrefix)
        {
            Item.EnvVarEnvVarPrefix = envVarPrefix;
            Item.ConfigFileReader = ConfigFileReader.CreateFromEnvVar(envVarPrefix);
        }

        private ConfigStore(string configFile, string envVarPrefix)
        {
            Item.EnvVarEnvVarPrefix = envVarPrefix;
            Item.ConfigFileReader = ConfigFileReader.CreateFromFile(envVarPrefix, configFile);
        }

        public static ConfigStore CreateFromFile(string configFile, string envVarPrefix = null)
        {
            return new ConfigStore(configFile, envVarPrefix);
        }

        public static ConfigStore CreateFromEnvVar(string envVarPrefix = null)
        {
            return new ConfigStore(envVarPrefix);
        }

        public T Get<T>(string section, string key)
        {
            return Item.GetValue<T>(section, key);
        }

        public T Get<T>(string key)
        {
            return Item.GetValue<T>(null, key);
        }

        public T GetWithDefaultValue<T>(string section, string key, T defaultValue)
        {
            return Item.GetValue(section, key, defaultValue);
        }

        public T GetWithDefaultValue<T>(string key, T defaultValue)
        {
            return Item.GetValue(null, key, defaultValue);
        }

        public bool Contains(string section, string key)
        {
            return Item.Exists(section, key);
        }

        public bool Contains(string key)
        {
            return Item.Exists(key);
        }
    }
}