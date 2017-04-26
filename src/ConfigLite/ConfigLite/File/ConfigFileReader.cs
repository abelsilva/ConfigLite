using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConfigLite.File
{
    internal class ConfigFileReader
    {
        public const string CONFIG_FILE_ENV_VAR = "CONFIG_FILE";

        private ConfigFileReader(string filepath)
        {
            Configurations = new List<ConfigFileItem>();
            ParseConfigurationFile(filepath);
        }

        public static ConfigFileReader CreateFromEnvVar(string envVarPrefix)
        {
            return new ConfigFileReader(GetFilePathFromEnvVar(envVarPrefix));
        }

        public static ConfigFileReader CreateFromFile(string envVarPrefix, string configFile)
        {
            string filepath = GetFilePathFromEnvVar(envVarPrefix);
            if (string.IsNullOrWhiteSpace(filepath))
                filepath = configFile;

            return new ConfigFileReader(filepath);
        }

        private static string GetFilePathFromEnvVar(string envVarPrefix)
        {
            string variable = "";

            if (string.IsNullOrWhiteSpace(envVarPrefix) == false)
                variable += envVarPrefix + "_";

            variable += CONFIG_FILE_ENV_VAR;

            return Environment.GetEnvironmentVariable(variable);
        }

        internal readonly List<ConfigFileItem> Configurations;

        public string GetValue(string section, string key)
        {
            return string.IsNullOrWhiteSpace(section)
                ? Configurations.FirstOrDefault(c => c.Key == key)?.Value
                : Configurations.FirstOrDefault(c => c.Section == section && c.Key == key)?.Value;
        }

        private void ParseConfigurationFile(string filepath)
        {
            try
            {
                if (filepath == null)
                    return;

                if (System.IO.File.Exists(filepath) == false)
                    return;

                using (Stream fs = System.IO.File.OpenRead(filepath))
                using (StreamReader sr = new StreamReader(fs))
                {
                    string line;
                    string section = null;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (LineIsBlankOrComment(line))
                            continue;

                        if (LineIsSection(line))
                            section = GetSection(line);

                        if (LineIsVariable(line))
                            Configurations.Add(GetConfiguration(section, line));
                    }
                }
            }
            catch
            {
                // ignore, assume no configurations available
            }
        }

        private static bool LineIsBlankOrComment(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return true;

            string trimmedLine = line.TrimStart();
            return trimmedLine.StartsWith(";") || trimmedLine.StartsWith("#");
        }

        private static bool LineIsSection(string line)
        {
            if (LineContainsInlineComment(line))
                return LineIsSection(RemoveInlineComment(line));

            Regex reg = new Regex(@"\[([A-Za-z0-9-_,\\\.]+)\]");
            Match match = reg.Match(line);

            return match.Success;
        }

        private static string GetSection(string line)
        {
            if (LineContainsInlineComment(line))
                return GetSection(RemoveInlineComment(line));

            Regex reg = new Regex(@"\[([A-Za-z0-9-_,\\\.]+)\]");
            Match match = reg.Match(line);

            return match.Success ? match.Groups[1].Value : null;
        }

        private static bool LineIsVariable(string line)
        {
            if (LineIsBlankOrComment(line) || LineIsSection(line))
                return false;

            if (LineContainsInlineComment(line))
                return LineIsVariable(RemoveInlineComment(line));

            int index = line.IndexOf('=');
            return index > 0;
        }

        private static ConfigFileItem GetConfiguration(string section, string line)
        {
            if (LineContainsInlineComment(line))
                return GetConfiguration(section, RemoveInlineComment(line));

            int index = line.IndexOf('=');

            string key = line.Substring(0, index);

            index++;
            string value = null;
            if (index < line.Length)
            {
                value = line.Substring(index).Trim();

                if (value.StartsWith("\"") && value.EndsWith("\""))
                    value = value.Substring(1, value.Length - 2);
                else if (value.StartsWith("'") && value.EndsWith("'"))
                    value = value.Substring(1, value.Length - 2);
            }

            return new ConfigFileItem
            {
                Section = section,
                Key = key,
                Value = value
            };
        }

        private static bool LineContainsInlineComment(string line)
        {
            return IndexOfInlineComment(line) >= 0;
        }

        private static int IndexOfInlineComment(string line)
        {
            char insideQuoteChar = '\0';
            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if ((c == ';' || c == '#') && insideQuoteChar == '\0')
                    return i;

                if (c != '"' && c != '\'')
                    continue;

                if (insideQuoteChar == '\0')
                    insideQuoteChar = c;
                else if (insideQuoteChar == c)
                    insideQuoteChar = '\0';
            }

            return -1;
        }

        private static string RemoveInlineComment(string line)
        {
            return line.Substring(0, IndexOfInlineComment(line)).Trim();
        }
    }
}