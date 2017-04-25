# ConfigLite

ConfigLite is a .NET Configuration Loader

Features:
* Loads configurations from a `INI` file
* Loads configurations from environment variables
* Allows configuration file to be defined using an environment variable
* Configurations from environment variables take precedence

## Getting Started

### Step 1: Install ConfigLite package

```

Install-Package ConfigLite

```

### Step 2: Create a configuration store

```csharp

ConfigStore store = new ConfigStore("config.ini");

```

> This sample creates a store from a the file `config.ini`. More info from [documentation](https://github.com/abelsilva/ConfigLite/wiki/Creating-a-ConfigStore).

Sample `INI` file:

```ini
HOST=10.10.10.250
PORT=5544
LOG_FILE=app.log
```

### Step 3: Get configurations

```csharp

string host = store.Get<string>("HOST");
int port = store.Get<int>("PORT");

```

> Configurations may be overridden by environment variables. More information from [documentation](https://github.com/abelsilva/ConfigLite/wiki/Get-Configuration-Variables)

### Step 4: Check if configurations exist

```csharp

if (store.Contains("LOG_FILE"))
{
    EnableLogFile();
}

```

## Contributing

Fork this project [abelsilva/ConfigLite](https://github.com/abelsilva/ConfigLite) and submit pull requests.
