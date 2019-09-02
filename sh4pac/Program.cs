using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace sh4pac
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("sh4pac\nA tool for unpacking and repacking Silent Hill 4 .pac sound archives, generally located in the SILENT HILL 4/sound/ directory in the game's installation directory. This will *NOT* unpack the .pac files found in the /movie/ directory, as those use a different format.\n\nUsage:\nsh4pac analyze <file.pac> - Prints information about the specified .pac file\nsh4pac unpack <file.pac> <output directory> - Unpacks a .pac file into the specified folder\nsh4pac pack <output directory> <file.pac> - Repacks a folder into a .pac file\n\nNote:\nIt seems that the order of sounds in .pac files is hardcoded by the game somewhere. If you are looking to modify an existing sound, do *NOT* change the order of the files in the .pac, or the game will play the incorrect sound! Furthermore, adding more sounds seems to be pointless, as the game has no way to play those extra sounds. Same goes with removing sounds, if you do this the game will likely crash when it attempts to play sounds that no longer exist.\nUse caution when using this tool!");
                return;
            }

            if(args[0] == "analyze")
            {

                // Open a filestream with the user selected file
                FileStream file = new FileStream(args[1], FileMode.Open);

                // Create a binary reader that will be used to read the file
                BinaryReader reader = new BinaryReader(file);

                // Grab the type, currently the only supported type is 0xD
                String magic = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(4));

                // Return if magic does not match PAC magic
                if (magic != "SDPA")
                {
                    Console.WriteLine("The input file does not appear to be a valid .pac file.");
                    return;
                }

                int numFiles = reader.ReadInt32();

                Console.WriteLine("Number of files in .pac: " + numFiles);

                for(int i = 0; i < numFiles; i++)
                {
                    Console.WriteLine("File offset: " + reader.ReadInt32());
                    Console.WriteLine("File size: " + reader.ReadInt32());
                    Console.WriteLine("File sample rate: " + reader.ReadInt32());
                }
            }

            if (args[0] == "unpack")
            {
                // Open a filestream with the user selected file
                FileStream file = new FileStream(args[1], FileMode.Open);

                // Create a binary reader that will be used to read the file
                BinaryReader reader = new BinaryReader(file);

                // Grab the type, currently the only supported type is 0xD
                String magic = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(4));

                // Return if magic does not match PAC magic
                if (magic != "SDPA")
                {
                    Console.WriteLine("The input file does not appear to be a valid .pac file.");
                    return;
                }

                int numFiles = reader.ReadInt32();

                Console.WriteLine("Number of files in .pac: " + numFiles);

                for (int i = 0; i < numFiles; i++)
                {
                    int offset = reader.ReadInt32();
                    int size = reader.ReadInt32();
                    int sampleRate = reader.ReadInt32();
                    long originalPos = reader.BaseStream.Position;

                    reader.BaseStream.Position = offset;
                    byte[] fileBytes = reader.ReadBytes(size);

                    reader.BaseStream.Position = originalPos;

                    File.WriteAllBytes(i + ".wav", fileBytes);

                    Console.WriteLine("Successfully extracted file " + i + ".wav to disk");
                }
            }

            if (args[0] == "pack")
            {
                Console.WriteLine("This has not been implemented yet");
            }


        }
    }
}
