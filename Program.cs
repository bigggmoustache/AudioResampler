using static System.Console;
using System.IO;
using NWaves;
using NWaves.Signals;
using NWaves.Audio;
using NWaves.Audio.Interfaces;

WriteLine("AudioResampler makes iterative sample processing easier.");



string sample= $"C:\\Users\\samue\\source\\repos\\AudioResampler\\Samples\\SampleForResample.wav";


WaveFile waveFileImport;

//creates new stream for importing audio
using (var stream = new FileStream(sample, FileMode.Open))
{
    //puts audio into waveFile
    waveFileImport = new WaveFile(stream);
}

//assigns signals to channels of stream
var signalLeft = waveFileImport[Channels.Left];
var signalRight = waveFileImport[Channels.Right];

//creates stream for exporting audio, assigns signals to stream
var waveFileExport = new WaveFile(new[] { signalLeft, signalRight });

//naming temp file
var filename = string.Format("{0}.wav", Guid.NewGuid());

//
using (var stream = new FileStream(filename, FileMode.Create))
{
    waveFileExport.SaveTo(stream);
}

var player = new MemoryStreamPlayer();
await player.PlayAsync(filename);
ReadKey();
// cleanup temporary file
File.Delete(filename);