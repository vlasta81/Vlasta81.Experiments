# Pathman


Pathman is a CLI tool managing the windows "Path" environment variable.


#### USING
### Commands

    Usage:
      Pathman [command] [options]

    Options:
      --version       Show version information
      -?, -h, --help  Show help and usage information

    Commands:
      add     Add path to environment variable.
      remove  Remove path from environment variable.

### Command "add"

    Description:
      Add path to environment variable.

    Usage:
      Pathman add [options]

    Options:
      -d, --directory <path> (REQUIRED)  The directory path that is added to the "Path" environment variable.
      -s, --system                       Select if you want to add/remove the directory path to/from the system instead of
                                         user environment "Path" variable. (Required administrator permissions!) [default:
                                         False]
      -f, --first                        Select if you want to add the directory path to the beginning of the "Path"
                                         environment variable. [default: False]
      -?, -h, --help                     Show help and usage information

### Command "remove"

    Description:
      Remove path from environment variable.

    Usage:
      Pathman remove [options]

    Options:
      -d, --directory <path> (REQUIRED)  The directory path that is added to the "Path" environment variable.
      -s, --system                       Select if you want to add/remove the directory path to/from the system instead of
                                         user environment "Path" variable. (Required administrator permissions!) [default:
                                         False]
      -?, -h, --help                     Show help and usage information


#### EXAMPLES

    Pathman add "C:\Path\To Directory" --system
    Pathman add "C:\Path\To Directory" --system --first
    Pathman add C:\Path\To\Directory
    Pathman add "C:\Path\To Directory" -f
    Pathman add C:\Path\To\Directory --first
    Pathman remove "C:\Path\To Directory" -s
    Pathman remove C:\Path\To\Directory
    Pathman -?
    Pathman -h
    Pathman -help