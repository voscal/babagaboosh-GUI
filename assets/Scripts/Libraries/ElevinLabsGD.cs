using Godot;
using ElevenLabs;
using ElevenLabs.Voices;
using System.IO;
using System.Threading.Tasks;

using NAudio.Wave;

using System.Linq;
public partial class ElevinLabsGD : Node
{
	public Voice currentVoice;
	ElevenLabsClient api;
	SaveManager saveManager;
	VoiceData voiceData;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		saveManager = GetNode<SaveManager>("/root/Data/SaveData");
		voiceData = GetNode<VoiceData>("/root/Data/VoiceData");
		api = new ElevenLabsClient(saveManager.GetAPIKey("11labs"));


	}



	public async Task RenderVoice(string text)
	{
		if (currentVoice == null)
		{
			GD.PrintErr("Please select a voice!");
			return;
		}



		Voice voice = await api.VoicesEndpoint.GetVoiceAsync(currentVoice.Id, withSettings: true);
		VoiceSettings voiceSettingsNew = GetNode<UI>("/root/Main Scene/UI").editorUI.GetVoiceSettings();

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

	public async Task<Voice[]> GetVoices()
	{

		var allVoices = await api.VoicesEndpoint.GetAllVoicesAsync();


		foreach (var voice in allVoices)
		{


			GD.Print($"{voice.Id} | {voice.Name} | similarity boost: {voice.Settings?.SimilarityBoost} | stability: {voice.Settings?.Stability}");
		}

		return allVoices.ToArray(); //voiceList;

	}
}
