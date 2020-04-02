using System;
using System.IO;
using System.Linq;
using dmf2amps.Models;
using zlib;

namespace dmf2amps {
    internal class Program {
        private static string InputFile { get; set; }
        private static string OutputFile { get; set; }
        
        public static bool ReverseDacChanges { get; set; }
    
        public static void Main(string[] args) {
            Console.WriteLine("dmf2amps v1.0 by VladislavSavvateev");

            if (!ParseArgs(args)) return;
            
            if (InputFile == null) {
                PrintError("there is no input file");
            } else if (OutputFile == null) {
                PrintError("there is no output file");
            } else {
                var file = new FileInfo(InputFile);
                if (!file.Exists) {
                    PrintError("file not found!");
                    return;
                }
                
                var dmf = new DMF(DecompressDmf(InputFile));

                using var sw = new StreamWriter(OutputFile);
                dmf.ConvertToAmps(sw);
            }
        }

        public static bool ParseArgs(string[] args) {
            for (var i = 0; i < args.Length; i++) {
                switch (args[i]) {
                    case "-i":
                    case "--input":
                        InputFile = args[++i];
                        break;
                    case "-o":
                    case "--output":
                        OutputFile = args[++i];
                        break;
                    case "-h":
                    case "--help":
                        PrintHelp();
                        return false;
                    case "--reverse-dac":
                    case "-r":
                        ReverseDacChanges = true;
                        break;
                    default:
                        PrintError($"unrecognized parameter: {args[i]}");
                        return false;
                }
            }

            return true;
        }

        private static void PrintHelp() {
            Console.WriteLine("Usage: dmf2amps.exe [-h] -i INPUT_FILE -o OUTPUT_FILE\n\n"
                              
                              + "Parameters:\n"
                              + "\t-i, --input        Input file (really?)\n"
                              + "\t-o, --output       Output file (omg)\n"
                              + "\t-h, --help         Shows this nice text\n"
                              + "\t-r, --reverse-dac  Reverse start FM6/DAC behaviour."
                              + "                     By default, every song starts "
                              + "                     with notes (not samples) on track"
                              + "                     FM6.");
        }

        public static void PrintError(string message)
            => Console.WriteLine("ERR: {0}", message);


        private static FileStream DecompressDmf(string path) {
            using (var fs = new FileStream("temp.dmf", FileMode.Create)) {
                using (var zis = new ZInputStream(File.OpenRead(path))) {
                    int read; var buf = new byte[16384];
                    while ((read = zis.read(buf, 0, 16384)) > 0) fs.Write(buf, 0, read);
                }
            }

            return File.OpenRead("temp.dmf");
        }
    }
}