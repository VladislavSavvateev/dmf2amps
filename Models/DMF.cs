using System;
using System.Collections.Generic;
using System.IO;

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
                var wavetable = new uint[stream.ReadByte()];
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
    }
}