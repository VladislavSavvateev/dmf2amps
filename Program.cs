using System;
using System.IO;
using dmf2amps.Models;
using zlib;

namespace dmf2amps {
    internal class Program {
        public static void Main(string[] args) {
            Console.WriteLine("dmf2amps v1.0 by VladislavSavvateev");
            if (args.Length == 0) {
                PrintError("There is no input file.");
            } else {
                var file = new FileInfo(args[0]);
                if (!file.Exists) {
                    PrintError("File not found!");
                    return;
                }
                
                var dmf = new DMF(DecompressDmf(args[0]));

                using (var sw = new StreamWriter("/home/savok/AMPS/AMPS/music/SmoothCriminal.s2a")) sw.Write(dmf.ConvertToAmps());
            }
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