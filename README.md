# sh4pac
This is a C# command-line tool for unpacking and repacking Silent Hill 4 .PAC sound archives. There doesn't seem to be a tool anywhere on the internet for doing this, and the format is relatively simple, so I thought I'd create one as an open-source C# tool.

## Usage
sh4pac has three modes which allow you to analyze a .pac file, extract it's contents, or build a completely new one.

* sh4pac analyze <file.pac> - Prints some information about a .pac archive
* sh4pac unpack < file.pac > < output directory > - Unpacks a .pac archive into an output folder
* sh4pac pack < output directory > < file.pac > - Creates a .pac archive from an output folder

### Repacking Notes
When you have unpacked a .pac, you must keep the order and filenames of each .wav file the same. The filenames are currently used to store unknown information about each .wav that is crucial to repacking.

I believe you should also keep the sample rate/format/length of the .wavs the same as their original if you intend on modifying them. This engine hardcodes many things about how it expects things to be and is very particular about that, so modifying the parameters of the .wav files outside of the original Konami parameters can cause unintended behavior such as audio not playing, skipping, or the game outright crashing when a sound is played. There is currently no workaround for these issues, and I doubt there ever will be without significant modification to the game binary.

Other than that, you should be able to create sound mods using this tool. Have fun, and @ me on Twitter (@hun10sta) if you come up with something fun :-)

## Proof-of-Concept
Here is a simple sound mod created with the use of this tool.

[![Silent Hill 4 Sound Modding](https://img.youtube.com/vi/pFrb3Zjq9Hg/0.jpg)](https://www.youtube.com/watch?v=pFrb3Zjq9Hg)

## To-do
* Verify an unknown int32 in the header of the .pac file
* Refactoring

## Credits
* Hunter Stanton (@HunterStanton) - Research, creating the tool
