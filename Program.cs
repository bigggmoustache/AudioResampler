using static System.Console;
using System.IO;
using NWaves;
using NWaves.Signals;
using NWaves.Audio;
using NWaves.Audio.Interfaces;
using NWaves.Filters.Bessel;

WriteLine("AudioResampler makes iterative sample processing easier.");
WriteLine("Playing original sample now...");
string sample= $"C:\\Users\\samue\\source\\repos\\AudioResampler\\Samples\\SampleForResample.wav";
var player = new MemoryStreamPlayer();
await player.PlayAsync(sample);
ReadKey();
player.Stop();


//creates new stream for importing audio
WaveFile waveFileImport;
//puts audio into waveFile container
using (var stream = new FileStream(sample, FileMode.Open))
{
    waveFileImport = new WaveFile(stream);
}

//assigns signals to channels of stream
var signalLeft = waveFileImport[Channels.Left];
var signalRight = waveFileImport[Channels.Right];

//creates lowpass filter
int samplingRate = signalLeft.SamplingRate;
double freq = 500;
double f = freq / samplingRate; // normalize frequency onto [0, 0.5] range
int order = 5;
var filter = new LowPassFilter(f, order);

//applying filter to signals
var filteredLeft = filter.ApplyTo(signalLeft);
//must reset for new data
filter.Reset();
var filteredRight = filter.ApplyTo(signalRight);
filter.Reset();


//creates stream for exporting audio, assigns signals to stream
var waveFileExport = new WaveFile(new[] { filteredLeft, filteredRight });

//naming temp file
var filename = string.Format("{0}.wav", Guid.NewGuid());

//
using (var stream = new FileStream(filename, FileMode.Create))
{
    waveFileExport.SaveTo(stream);
}
WriteLine("Playing modified sample now...");

await player.PlayAsync(filename);
ReadKey();
player.Stop();
// cleanup temporary file
File.Delete(filename);