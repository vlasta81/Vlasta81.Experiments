# Pathman


Manage Windows environment variable "Path".


#### USING

    Pathman -s [path]
        Add path to system variable "Path". 
        ! Requires administrator rights.
        [path] can be only absolute path to directory

    Pathman -u [path]
        Add path to user variable "Path".
        [path] can be only absolute path to directory

    -b
        Add path to beginning variable "Path"

    Pathman -h [or] -help
        Show this help.

#### EXAMPLES

    Pathman -s "C:\Path\To Directory"
    Pathman -u C:\Path\To\Directory
    Pathman -s "C:\Path\To Directory" -b
    Pathman -u C:\Path\To\Directory -b
    Pathman -h
    Pathman -help