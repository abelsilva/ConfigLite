using ConfigLite.File;
using ConfigLite.Items;

namespace ConfigLite
{
    /// <summary>
    ///     Configuration Store
    /// </summary>
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

        /// <summary>
        ///     Create a Configuration Store
        /// </summary>
        /// <param name="envVarPrefix">environment variables configurations prefix</param>
        public static ConfigStore Create(string envVarPrefix = null)
        {
            return new ConfigStore(envVarPrefix);
        }

        /// <summary>
        ///     Create a Configuration Store referencing a configuration file
        /// </summary>
        /// <param name="configFile">configuration file path</param>
        /// <param name="envVarPrefix">environment variables configurations prefix</param>
        /// <returns></returns>
        public static ConfigStore CreateFromFile(string configFile, string envVarPrefix = null)
        {
            return new ConfigStore(configFile, envVarPrefix);
        }

        /// <summary>
        ///     Get a configuration
        /// </summary>
        /// <typeparam name="T">return type</typeparam>
        /// <param name="section">INI section</param>
        /// <param name="key">INI key</param>
        /// <returns></returns>
        public T Get<T>(string section, string key)
        {
            return Item.GetValue<T>(section, key);
        }

        /// <summary>
        ///     Get a configuration
        /// </summary>
        /// <typeparam name="T">return type</typeparam>
        /// <param name="key">INI key</param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            return Item.GetValue<T>(null, key);
        }

        /// <summary>
        ///     Get a configuration with a default value in case the configuration is not found
        /// </summary>
        /// <typeparam name="T">return type</typeparam>
        /// <param name="section">INI section</param>
        /// <param name="key">INI key</param>
        /// <param name="defaultValue">Default value to return if configuration is not found</param>
        /// <returns></returns>
        public T GetWithDefaultValue<T>(string section, string key, T defaultValue)
        {
            return Item.GetValue(section, key, defaultValue);
        }

        /// <summary>
        ///     Get a configuration with a default value in case the configuration is not found
        /// </summary>
        /// <typeparam name="T">return type</typeparam>
        /// <param name="key">INI key</param>
        /// <param name="defaultValue">Default value to return if configuration is not found</param>
        /// <returns></returns>
        public T GetWithDefaultValue<T>(string key, T defaultValue)
        {
            return Item.GetValue(null, key, defaultValue);
        }

        /// <summary>
        ///     Check if a configuration exists
        /// </summary>
        /// <param name="section">INI section</param>
        /// <param name="key">INI key</param>
        /// <returns></returns>
        public bool Contains(string section, string key)
        {
            return Item.Exists(section, key);
        }

        /// <summary>
        ///     Check if a configuration exists
        /// </summary>
        /// <param name="key">INI key</param>
        /// <returns></returns>
        public bool Contains(string key)
        {
            return Item.Exists(key);
        }
    }
}