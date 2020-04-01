using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace dmf2amps.Models {
    public class DMF {
        public string SongName { get; } 
        public string SongAuthor { get; }
        
        public Tempo Tempo { get; }
        
        public byte[,] PatternMatrixValues { get; }
        
        public List<Instrument> Instruments { get; }
        
        public List<uint[]> Wavetables { get; }
        
        public uint TotalRowsPerPattern { get; }
        public byte TotalRowsInPatternMatrix { get; }
        
        public List<Channel> Channels { get; }
        
        public List<Sample> Samples { get; }
        
        public DMF(Stream stream) {
            if (StaticBinaryReader.ReadString(stream, 16) != ".DelekDefleMask.")
                throw new ArgumentException("Invalid magic word.");

            if (stream.ReadByte() != 0x18)
                throw new ArgumentException(
                    "File has old version. Please re-save the file with newer version of DefleMask.");
            
            if (stream.ReadByte() != 0x02)
                throw new ArgumentException("Unsupported system.");
            
            var br = new BinaryReader(stream);

            SongName = StaticBinaryReader.ReadString(stream, stream.ReadByte());
            SongAuthor = StaticBinaryReader.ReadString(stream, stream.ReadByte());

            stream.Seek(2, SeekOrigin.Current);
            
            Tempo = new Tempo(stream);
            stream.Seek(5, SeekOrigin.Current);

            TotalRowsPerPattern = StaticBinaryReader.ReadUInt32(stream);
            TotalRowsInPatternMatrix = (byte)stream.ReadByte();
            
            PatternMatrixValues = new byte[10, TotalRowsInPatternMatrix];
            for (var ch = 0; ch < 10; ch++) 
                for (var row = 0; row < TotalRowsInPatternMatrix; row++) 
                    PatternMatrixValues[ch, row] = (byte)stream.ReadByte();

            var totalInstruments = stream.ReadByte();
            Instruments = new List<Instrument>();
            for (var i = 0; i < totalInstruments; i++) 
                Instruments.Add(new Instrument(br));

            var totalWavetables = stream.ReadByte();
            Wavetables = new List<uint[]>();
            for (var i = 0; i < totalWavetables; i++) {
                var wavetable = new uint[StaticBinaryReader.ReadUInt32(stream)];
                for (var k = 0; k < wavetable.Length; k++) wavetable[k] = StaticBinaryReader.ReadUInt32(stream);
                Wavetables.Add(wavetable);
            }
            
            Channels = new List<Channel>();
            for (var i = 0; i < 10; i++) Channels.Add(new Channel(this, br));
            
            Samples = new List<Sample>();
            var totalSamples = stream.ReadByte();
            for (var i = 0; i < totalSamples; i++)
                Samples.Add(new Sample(br));
            
            Console.WriteLine();
        }

        public string ConvertToAmps() {
            var labelName = GenerateLabelName();
            if (string.IsNullOrWhiteSpace(labelName)) labelName = GenerateRandomSuffix();
            if (labelName.Length > 8) labelName = labelName.Substring(0, 8);

            var result = $"; {new string('=', 72)}\n" +
                         $"; {SongName} by {SongAuthor}\n" +
                         "; Converted with dmf2amps v1.0 by VladislavSavvateev\n" +
                         $"; {new string('=', 72)}\n\n" +

                         $"{labelName}_Header:\n" +
                         "\tsHeaderInit\n" +
                         "\tsHeaderTempo\t$01, $00\n" +
                         "\tsHeaderCh\t$05, $03\n" +
                         $"\tsHeaderDAC\t{labelName}_DAC1\n" +
                         $"\tsHeaderDAC\t{labelName}_DAC2\n" +
                         $"\tsHeaderFM\t{labelName}_FM1, $00, $00\n" +
                         $"\tsHeaderFM\t{labelName}_FM2, $00, $00\n" +
                         $"\tsHeaderFM\t{labelName}_FM3, $00, $00\n" +
                         $"\tsHeaderFM\t{labelName}_FM4, $00, $00\n" +
                         $"\tsHeaderFM\t{labelName}_FM5, $00, $00\n" +
                         $"\tsHeaderPSG\t{labelName}_PSG1, $00, $00, $00, $00\n" +
                         $"\tsHeaderPSG\t{labelName}_PSG2, $00, $00, $00, $00\n" +
                         $"\tsHeaderPSG\t{labelName}_PSG3, $00, $00, $00, $00\n\n";

            result += "; Instruments\n\n";

            // add some instruments
            byte fmCounter = 0;
            foreach (var ins in Instruments.Where(ins => ins.Type == Instrument.InstrumentType.FM)) {
                result += $"; ${fmCounter:X2}: {ins.Name}\n";
	            result += ins.FM;
	            ins.FM.Offset = fmCounter++;
            }

            // proceed fms
            for (var i = 0; i < 5; i++) {
                var channel = Channels[i];

                var channelData = $"\n{labelName}_FM{i + 1}:\n";
                
                for (var p_i = 0; p_i < TotalRowsInPatternMatrix; p_i++) {
                    var pattern = channel.Patterns[p_i];

                    if (pattern.AmpsName == null)
                        pattern =
                            channel.Patterns.FirstOrDefault(p => !ReferenceEquals(p, pattern) && p.Equals(pattern)) ??
                            pattern;

                    if (pattern.AmpsName == null) {
	                    pattern.AmpsName = $"{labelName}_FM{i + 1}_{p_i}";
	                    result += $"{pattern.AmpsName}:\n";

                        for (var r_i = 0; r_i < pattern.Rows.Length; r_i++) {
                            var row = pattern.Rows[r_i];

                            if (row.Volume != -1) 
                                result += $"\tssVol\t${0x7F - row.Volume:X2}\n";

                            if (row.Instrument != -1)
                                result += $"\tsVoice\t${Instruments[row.Instrument].FM.Offset:X2}\n";

                            result = row.Effects.Select(e => e.ToAmpsCoord())
                                .Where(str => !string.IsNullOrWhiteSpace(str))
                                .Aggregate(result, (current, str) => current + str);

                            if (row.Note == 100)
                                result += $"\tdc.b\tnRst, ${(r_i % 2 == 0 ? Tempo.EvenTime : Tempo.OddTime):X2}\n";
                            else if (row.Note == 0 && row.Octave == 0)
                                result += $"\tdc.b\tsHold, ${(r_i % 2 == 0 ? Tempo.EvenTime : Tempo.OddTime):X2}\n";
                            else result += $"\tdc.b\t{Row.Notes[row.Note]}{row.Octave}, ${(r_i % 2 == 0 ? Tempo.EvenTime : Tempo.OddTime):X2}\n";
                        }
                        
                        result += "\tsRet\n\n";
                    }

                    channelData += $"\tsCall\t{pattern.AmpsName}\n";
                }

                result += $"{channelData}\tsStop\n\n";
            }
            
            // proceed dac
            var dac = Channels[5];
            var dacChannelData = $"\n{labelName}_DAC1:\n";
            for (var p_i = 0; p_i < TotalRowsInPatternMatrix; p_i++) {
                var pattern = dac.Patterns[PatternMatrixValues[5, p_i]];

                if (pattern.AmpsName == null)
                    pattern =
                        dac.Patterns.FirstOrDefault(p => !ReferenceEquals(p, pattern) && p.Equals(pattern)) ??
                        pattern;

                if (pattern.AmpsName == null) {
                    pattern.AmpsName = $"{labelName}_DAC1_{p_i}";

                    result += $"{pattern.AmpsName}:\n\tsRet\n\n";
                }

                dacChannelData += $"\tsCall\t{pattern.AmpsName}\n";
            }

            result += $"{dacChannelData}\tsStop\n\n{labelName}_DAC2:\n\tsStop\n\n";

            // proceed psgs
            for (var i = 7; i < 10; i++) {
                var channel = Channels[i];

                var channelData = $"\n{labelName}_PSG{i - 6}:\n";
                for (var p_i = 0; p_i < TotalRowsInPatternMatrix; p_i++) {
                    var pattern = channel.Patterns[PatternMatrixValues[i, p_i]];

                    if (pattern.AmpsName == null)
                        pattern =
                            channel.Patterns.FirstOrDefault(p => !ReferenceEquals(p, pattern) && p.Equals(pattern)) ??
                            pattern;

                    if (pattern.AmpsName == null) {
                        pattern.AmpsName = $"{labelName}_PSG{i - 6}_{p_i}";

                        result += $"{pattern.AmpsName}:\n\tsRet\n\n";
                    }

                    channelData += $"\tsCall\t{pattern.AmpsName}\n";
                }

                result += $"{channelData}\tsStop\n\n";
            }

            return result;
        }

        private string GenerateLabelName() => new Regex("[^A-Za-z]").Replace(SongName, "");

        private readonly Random r = new Random();
        private string GenerateRandomSuffix(int count = 6, string alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ") {
            var result = "";
            for (var i = 0; i < count; i++) result += alphabet[r.Next(alphabet.Length)];

            return result;
        }
    }
}