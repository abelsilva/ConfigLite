using System;
using System.IO;
using ConfigLite.File;
using Xunit;

namespace ConfigLite.Tests.File
{
    [Collection("ConfigLite")]
    public class ConfigFileReaderTests
    {
        [Fact]
        public void ConfigFileReaderTest()
        {
            ConfigFileReader reader = ConfigFileReader.CreateFromFile(null, "config.ini");

            Assert.Equal(13, reader.Configurations.Count);

            Assert.Equal("VALUE_01", reader.GetValue(null, "CONFIG_WITH_NO_SECTION"));
            Assert.Equal(null, reader.GetValue(null, "CONFIG_WITH_NO_VALUE"));
            Assert.Equal(null, reader.GetValue(null, "CONFIG_WITH_NO_VALUE_WITH_INLINE_COMMENT"));
            Assert.Equal("value_02", reader.GetValue(null, "CONFIG_WITH_lowercase"));
            Assert.Equal("VALUE_03", reader.GetValue("SECTION1", "CONFIG_INSIDE_SECTION1"));
            Assert.Equal("VALUE_04", reader.GetValue("SECTION1", "CONFIG_WITH_INLINE_COMMENT"));
            Assert.Equal("VALUE_05", reader.GetValue("SECTION2.SECTION3", "CONFIG_INSIDE_TREE_SECTION"));
            Assert.Equal(";", reader.GetValue("SECTION2.SECTION3", "CONFIG_WITH_COMMENT_CHAR_BETWEEN_DOUBLE_QUOTES"));
            Assert.Equal("#", reader.GetValue("SECTION2.SECTION3", "CONFIG_WITH_COMMENT_CHAR_BETWEEN_SINGLE_QUOTES"));
            Assert.Equal("true", reader.GetValue("SECTION2.SECTION3", "CONFIG_BOOL_TRUE"));
            Assert.Equal("false", reader.GetValue("SECTION2.SECTION3", "CONFIG_BOOL_FALSE"));
            Assert.Equal("123.456", reader.GetValue("SECTION2.SECTION3", "CONFIG_DECIMAL"));
            Assert.Equal("789", reader.GetValue("SECTION2.SECTION3", "CONFIG_INT"));
        }

        [Fact]
        public void ConfigFileReaderTestMissing()
        {
            ConfigFileReader reader = ConfigFileReader.CreateFromFile(null, "config.missing.ini");

            Assert.Equal(0, reader.Configurations.Count);
        }

        [Fact]
        public void ConfigFileReaderTestError()
        {
            ConfigFileReader reader;

            using (System.IO.File.Open("config.ini", FileMode.Open, FileAccess.Read, FileShare.None))
            {
                reader = ConfigFileReader.CreateFromFile(null, "config.ini");

                Assert.Equal(0, reader.Configurations.Count);
            }

            reader = ConfigFileReader.CreateFromFile(null, null);

            Assert.Equal(0, reader.Configurations.Count);
        }
        
        [Fact]
        public void ConfigFileReaderTestFromEnvVar()
        {
            Environment.SetEnvironmentVariable("CONFIGLITE_" + ConfigFileReader.CONFIG_FILE_ENV_VAR, "config.ini");

            ConfigFileReader reader = ConfigFileReader.CreateFromFile("CONFIGLITE", "config.error.ini");

            Assert.Equal(13, reader.Configurations.Count);

            reader = ConfigFileReader.CreateFromEnvVar("CONFIGLITE");

            Assert.Equal(13, reader.Configurations.Count);

            Environment.SetEnvironmentVariable("CONFIGLITE_" + ConfigFileReader.CONFIG_FILE_ENV_VAR, null);
        }
    }
}