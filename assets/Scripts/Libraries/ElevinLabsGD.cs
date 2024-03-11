using Godot;
using ElevenLabs;
using ElevenLabs.Voices;
using System.IO;
using System.Threading.Tasks;

using NAudio.Wave;
public partial class ElevinLabsGD : Node
{

	ElevenLabsClient api;

	SaveManager saveManager;
	// Called when the node enters the scene tree for the first time.
	public async override void _Ready()
	{
		saveManager = GetNode<SaveManager>("/root/saveManager");
		api = new ElevenLabsClient(saveManager.GetAPIKey("11labs"));
		var allVoices = await api.VoicesEndpoint.GetAllVoicesAsync();

		foreach (var voice in allVoices)
		{
			GD.Print($"{voice.Id} | {voice.Name} | similarity boost: {voice.Settings?.SimilarityBoost} | stability: {voice.Settings?.Stability}");
		}

	}



	public async Task RenderVoice(string text)
	{
		Voice voice = await api.VoicesEndpoint.GetVoiceAsync("d8denygOxqQud1nMqAw5", withSettings: true);
		VoiceSettings voiceSettingsNew = new()
		{
			Stability = 0.5f,
			SimilarityBoost = 0.75f,
			Style = 0.5f

		};
		await api.VoicesEndpoint.EditVoiceSettingsAsync(voice, voiceSettingsNew);
		GD.Print(await api.VoicesEndpoint.GetVoiceSettingsAsync(voice));


		VoiceClip voiceClip = await api.TextToSpeechEndpoint.TextToSpeechAsync(text, voice, voiceSettings: voiceSettingsNew);
		await File.WriteAllBytesAsync(ProjectSettings.GlobalizePath("res://Audio/AIresponse.mp3"), voiceClip.ClipData.ToArray());
		ConvertMp3ToWav(ProjectSettings.GlobalizePath("res://Audio/AIresponse.mp3"), ProjectSettings.GlobalizePath("res://Audio/AIresponse.wav"));
		GD.Print("Finnished result");




	}



	private static void ConvertMp3ToWav(string _inPath_, string _outPath_)
	{
		using (Mp3FileReader mp3 = new Mp3FileReader(_inPath_))
		{
			using (WaveStream pcm = WaveFormatConversionStream.CreatePcmStream(mp3))
			{
				WaveFileWriter.CreateWaveFile(_outPath_, pcm);
			}
		}
	}


}
