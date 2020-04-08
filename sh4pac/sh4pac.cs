using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace sh4pac
{
    class sh4pac
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("sh4pac\nA tool for unpacking and repacking Silent Hill 4 .pac sound archives, generally located in the SILENT HILL 4/sound/ directory in the game's installation directory. This will *NOT* unpack the .pac files found in the /movie/ directory, as those are simply MPEG-1/2 encoded video files with a .pac extension.\n\nUsage:\nsh4pac analyze <file.pac> - Prints information about the specified .pac file\nsh4pac unpack <file.pac> <output directory> - Unpacks a .pac file into the specified folder\nsh4pac pack <input directory> <file.pac> - Repacks a folder into a .pac file\n\nNote:\nIt seems that the order of sounds in .pac files is hardcoded by the game somewhere. If you are looking to modify an existing sound, do *NOT* change the order of the files in the .pac, or the game will play the incorrect sound! Furthermore, adding more sounds seems to be pointless, as the game has no way to play those extra sounds. Same goes with removing sounds, if you do this the game will likely crash when it attempts to play sounds that no longer exist.\nUse caution when using this tool!");
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

                // Grab the .pac magic
                String magic = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(4));

                // Return if magic does not match PAC magic
                if (magic != "SDPA")
                {
                    Console.WriteLine("The input file does not appear to be a valid .pac file.");
                    return;
                }

                int numFiles = reader.ReadInt32();

                Console.WriteLine("Number of files in .pac: " + numFiles);

                // Loop through every sound in the .pac and write it to disk, including the "unknown" value in the name of the sound for repacking purposes
                for (int i = 0; i < numFiles; i++)
                {
                    int offset = reader.ReadInt32();
                    int size = reader.ReadInt32();

                    // Thought this was sample rate but was mistaken, not sure what it is, could be some sort of id linking sounds to when they should be played in-game possibly, or maybe the volume the sound should be played at?
                    // The formats used by this engine are filled with unknown values that don't make sense :'-(
                    int unknownValue = reader.ReadInt32();

                    long originalPos = reader.BaseStream.Position;

                    reader.BaseStream.Position = offset;
                    byte[] fileBytes = reader.ReadBytes(size);

                    reader.BaseStream.Position = originalPos;

                    Directory.CreateDirectory(args[2]);
                    File.WriteAllBytes(args[2]+"/"+i + "_" + unknownValue + "_" + ".wav", fileBytes);

                    Console.WriteLine("Successfully extracted file " + i + "_" + unknownValue + "_" + ".wav to disk");
                }
            }

            if (args[0] == "pack")
            {
                // Create a new file
                FileStream file = new FileStream(args[2], FileMode.Create);

                // Create a binary writer that will write the new bin file
                BinaryWriter writer = new BinaryWriter(file);

                // Get the files inside the input directory
                string[] files = Directory.GetFiles(args[1]);

                var sortedFiles = files.CustomSort().ToArray();

                // Grab the number of files inside the user's output directory
                int fileCount = sortedFiles.Length;

                Console.WriteLine("Number of files in new .pac: " + fileCount);

                List<byte> pacBody = new List<byte>();

                // PAC magic
                writer.Write(1095779411);

                writer.Write(fileCount);


                int padding = 8 + ((fileCount * 3) * 4);
                int previousLength = 0;

                foreach (string inputFile in sortedFiles)
                {
                    long length = new System.IO.FileInfo(inputFile).Length;

                    writer.Write(padding + previousLength);
                    writer.Write(Convert.ToInt32(length));

                    previousLength = previousLength + Convert.ToInt32(length);

                    string extractedPart = inputFile.Split('_')[1];
                    if (Int32.TryParse(extractedPart, out int unknownValue))
                    {
                        writer.Write(unknownValue);
                    }
                    else
                    {
                        // Failed to get the part from the header, it may be broken in-game!
                        writer.Write(0x2c);
                        Console.WriteLine("Could not get the unknown value from the filename, the resulting .pac may make the game unstable or cause unintended behavior!");
                    }

                    pacBody.AddRange(File.ReadAllBytes(inputFile));
                }

                writer.Write(pacBody.ToArray());

                writer.Close();
                file.Close();

                Console.WriteLine(args[2] + " successfully created!");
            }


        }
    }
}
