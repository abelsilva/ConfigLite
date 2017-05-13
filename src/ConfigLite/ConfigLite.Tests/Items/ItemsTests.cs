using System;
using ConfigLite.File;
using ConfigLite.Items;
using Xunit;

namespace ConfigLite.Tests.Items
{
    [Collection("ConfigLite")]
    public class ItemsTests : IDisposable
    {
        public ItemsTests()
        {
            ConfigFileReader reader = ConfigFileReader.CreateFromFile(null, "config.ini");

            Assert.Equal(13, reader.Configurations.Count);

            Item.ConfigFileReader = reader;
            Item.EnvVarEnvVarPrefix = "CONFIGLITE";
        }

        public void Dispose()
        {
            //nothing
        }

        [Fact]
        public void ItemsTestGetValue()
        {
            Assert.Equal("VALUE_01", Item.GetValue<string>(null, "CONFIG_WITH_NO_SECTION"));

            Assert.Equal("VALUE_DEFAULT", Item.GetValue<string>(null, "CONFIG_WITH_NO_VALUE", "VALUE_DEFAULT"));

            Assert.Equal(null, Item.GetValue<string>(null, "CONFIG_WITH_NO_VALUE_WITH_INLINE_COMMENT"));

            Assert.Equal("value_02", Item.GetValue<string>(null, "CONFIG_WITH_lowercase"));

            Assert.Equal("VALUE_03", Item.GetValue<string>("SECTION1", "CONFIG_INSIDE_SECTION1"));
            Assert.Equal("VALUE_04", Item.GetValue<string>("SECTION1", "CONFIG_WITH_INLINE_COMMENT"));

            Assert.Equal(';', Item.GetValue<char>("SECTION2.SECTION3", "CONFIG_WITH_COMMENT_CHAR_BETWEEN_DOUBLE_QUOTES"));
            Assert.Equal('#', Item.GetValue<char>("SECTION2.SECTION3", "CONFIG_WITH_COMMENT_CHAR_BETWEEN_SINGLE_QUOTES"));

            Assert.Equal(true, Item.GetValue<bool>("SECTION2.SECTION3", "CONFIG_BOOL_TRUE"));
            Assert.Equal(false, Item.GetValue<bool>("SECTION2.SECTION3", "CONFIG_BOOL_FALSE"));

            Assert.Equal(123.456m, Item.GetValue<decimal>("SECTION2.SECTION3", "CONFIG_DECIMAL"));

            Assert.Equal(789, Item.GetValue<int>("SECTION2.SECTION3", "CONFIG_INT"));
        }

        [Fact]
        public void ItemsTestGetValueOverridenByEnvVar()
        {
            Assert.Equal("VALUE_01", Item.GetValue<string>(null, "CONFIG_WITH_NO_SECTION"));
            Assert.Equal("VALUE_03", Item.GetValue<string>("SECTION1", "CONFIG_INSIDE_SECTION1"));
            Assert.Equal(789, Item.GetValue<int>("SECTION2.SECTION3", "CONFIG_INT"));

            try
            {
                Environment.SetEnvironmentVariable(Item.EnvVarEnvVarPrefix + "_CONFIG_WITH_NO_SECTION", "ENV_VALUE_01");
                Environment.SetEnvironmentVariable(Item.EnvVarEnvVarPrefix + "_SECTION1_" + "CONFIG_INSIDE_SECTION1", "ENV_VALUE_03");
                Environment.SetEnvironmentVariable(Item.EnvVarEnvVarPrefix + "_SECTION2.SECTION3_" + "CONFIG_INT", "799");

                Assert.Equal("ENV_VALUE_01", Item.GetValue<string>(null, "CONFIG_WITH_NO_SECTION"));
                Assert.Equal("ENV_VALUE_03", Item.GetValue<string>("SECTION1", "CONFIG_INSIDE_SECTION1"));
                Assert.Equal(799, Item.GetValue<int>("SECTION2.SECTION3", "CONFIG_INT"));
            }
            finally
            {
                Environment.SetEnvironmentVariable(Item.EnvVarEnvVarPrefix + "_CONFIG_WITH_NO_SECTION", null);
                Environment.SetEnvironmentVariable(Item.EnvVarEnvVarPrefix + "_SECTION1_" + "CONFIG_INSIDE_SECTION1", null);
                Environment.SetEnvironmentVariable(Item.EnvVarEnvVarPrefix + "_SECTION2.SECTION3_" + "CONFIG_INT", null);
            }
        }

        [Fact]
        public void ItemsTestGetValueInvalidKey()
        {
            Assert.Equal(null, Item.GetValue<string>(null, "BAD_KEY"));
            Assert.Equal(default(decimal), Item.GetValue<decimal>(null, "BAD_KEY"));

            Assert.Equal(null, Item.GetValue<string>("SECTION2.SECTION3", "BAD_KEY"));
            Assert.Equal(null, Item.GetValue<string>("BAD_SECTION", "CONFIG_INSIDE_SECTION1"));
            Assert.Equal(null, Item.GetValue<string>("BAD_SECTION", "BAD_KEY"));
        }

        [Fact]
        public void ItemsTestExists()
        {
            Assert.Equal(true, Item.Exists(null, "CONFIG_WITH_NO_SECTION"));
            Assert.Equal(true, Item.Exists("CONFIG_WITH_NO_SECTION"));
            Assert.Equal(true, Item.Exists("SECTION1", "CONFIG_INSIDE_SECTION1"));

            Assert.Equal(false, Item.Exists(null, "BAD_KEY"));
            Assert.Equal(false, Item.Exists("BAD_KEY"));

            Assert.Equal(false, Item.Exists("SECTION2.SECTION3", "BAD_KEY"));
            Assert.Equal(false, Item.Exists("BAD_SECTION", "CONFIG_INSIDE_SECTION1"));
            Assert.Equal(false, Item.Exists("BAD_SECTION", "BAD_KEY"));
        }
    }
}