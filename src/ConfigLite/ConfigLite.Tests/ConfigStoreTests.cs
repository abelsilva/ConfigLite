using System;
using ConfigLite.File;
using Xunit;

namespace ConfigLite.Tests
{
    [Collection("ConfigLite")]
    public class ConfigStoreTests
    {
        private const string ENV_PREFIX = "CONFIGLITE";
        private const string CONF_FILE_VAR = ENV_PREFIX + "_" + ConfigFileReader.CONFIG_FILE_ENV_VAR;

        [Fact]
        public void ConfigStoreTestCreateFromEnvVar()
        {
            Environment.SetEnvironmentVariable(CONF_FILE_VAR, "config.ini");

            ConfigStore store = new ConfigStore(ENV_PREFIX);

            Assert.Equal("VALUE_01", store.Get<string>("CONFIG_WITH_NO_SECTION"));

            Environment.SetEnvironmentVariable(CONF_FILE_VAR, null);
        }

        [Fact]
        public void ConfigStoreTestCreateFromEnvVarMissing()
        {
            ConfigStore store = new ConfigStore(ENV_PREFIX);

            Assert.Null(store.Get<string>("CONFIG_WITH_NO_SECTION"));
        }

        [Fact]
        public void ConfigStoreTestCreateFromConfigFile()
        {
            ConfigStore store = new ConfigStore(ENV_PREFIX, "config.ini");

            Assert.Equal("VALUE_01", store.Get<string>("CONFIG_WITH_NO_SECTION"));
        }

        [Fact]
        public void ConfigStoreTestCreateFromConfigFileMissing()
        {
            ConfigStore store = new ConfigStore(ENV_PREFIX, "config.missing.ini");

            Assert.Null(store.Get<string>("CONFIG_WITH_NO_SECTION"));
        }

        [Fact]
        public void ConfigStoreTestCreateFromConfigFileOverridedByEnvVar()
        {
            Environment.SetEnvironmentVariable(CONF_FILE_VAR, "config.ini");

            ConfigStore store = new ConfigStore(ENV_PREFIX, "config.missing.ini");

            Assert.Equal("VALUE_01", store.Get<string>("CONFIG_WITH_NO_SECTION"));

            Environment.SetEnvironmentVariable(CONF_FILE_VAR, null);
        }

        [Fact]
        public void ConfigStoreTestGet()
        {
            ConfigStore store = new ConfigStore(ENV_PREFIX, "config.ini");

            Assert.Equal("VALUE_01", store.Get<string>("CONFIG_WITH_NO_SECTION"));
            Assert.Equal("VALUE_03", store.Get<string>("SECTION1", "CONFIG_INSIDE_SECTION1"));
            Assert.Equal("VALUE_01", store.GetWithDefaultValue("CONFIG_WITH_NO_SECTION", "WRONG_VALUE"));
            Assert.Equal("VALUE_03", store.GetWithDefaultValue("SECTION1", "CONFIG_INSIDE_SECTION1", "WRONG_VALUE"));
            Assert.Equal("DEFAULT_VALUE", store.GetWithDefaultValue("WRONG_KEY", "DEFAULT_VALUE"));
            Assert.Equal("DEFAULT_VALUE", store.GetWithDefaultValue("WRONG_SECTION", "WRONG_KEY", "DEFAULT_VALUE"));
        }

        [Fact]
        public void ConfigStoreTestContains()
        {
            ConfigStore store = new ConfigStore(ENV_PREFIX, "config.ini");

            Assert.Equal(true, store.Contains("CONFIG_WITH_NO_SECTION"));
            Assert.Equal(true, store.Contains("SECTION1", "CONFIG_INSIDE_SECTION1"));
            Assert.Equal(false, store.Contains("WRONG_KEY"));
            Assert.Equal(false, store.Contains("WRONG_SECTION", "WRONG_KEY"));
        }
    }
}