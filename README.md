# sh4pac
This is a C# command-line tool for unpacking and (eventually) repacking Silent Hill 4 .PAC sound archives. There doesn't seem to be a tool anywhere on the internet for doing this, and the format is relatively simple, so I thought I'd create one as an open-source C# tool.

## Usage
sh4pac has three modes which allow you to analyze a .pac file, extract it's contents, or build a completely new one.

* sh4pac analyze <file.pac> - Prints some information about a .pac archive
* sh4pac unpack <file.pac> <output directory> - Unpacks a .pac archive into an output folder
* sh4pac pack <output directory> <file.pac> - Creates a .pac archive from an output folder

## To-do
* Implement repacking
* Verify an unknown int32 in the header of the .pac file
* Potentially find a filename table somewhere so the output files aren't named by their index in the .pac file
* Refactoring

## Credits
* Hunter Stanton (@HunterStanton) - Research, creating the tool
