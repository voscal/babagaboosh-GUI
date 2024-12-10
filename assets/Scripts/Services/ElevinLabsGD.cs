using Godot;
using ElevenLabs;
using ElevenLabs.Voices;
using System.IO;
using System.Threading.Tasks;
using NAudio.Wave;
using System.Net.Http;
using System;
using System.Linq;

public partial class ElevinLabsGD : Node
{
	public string currentVoice;
	ElevenLabsClient api;
	SaveData saveData;
	VoiceData voiceData;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		saveData = GetNode<SaveData>("/root/Data/SaveData");
		voiceData = GetNode<VoiceData>("/root/Data/VoiceData");
		api = new ElevenLabsClient(saveData.GetAPIKey("11labs"));
	}

	public async Task RenderVoice(Character character, string text)
	{
		if (string.IsNullOrEmpty(character.voiceID))
		{
			GD.PrintErr("Please select a voice!");
			GetNode<NotificationsManager>("/root/Managers/Notification").NewNotification("Error", "[center]Voice Error!", "[center]Please select a voice!", 5);
			return;
		}

		try
		{
			Voice voice = await api.VoicesEndpoint.GetVoiceAsync(character.voiceID, withSettings: true);

			await api.VoicesEndpoint.EditVoiceSettingsAsync(voice, character.voiceSettings);

			GetNode<NotificationsManager>("/root/Managers/Notification").NewNotification("info", "[center]Generating Voice!", "[center]11 labs is now generating the voice!", 3);
			VoiceClip voiceClip = await api.TextToSpeechEndpoint.TextToSpeechAsync(text, voice, voiceSettings: character.voiceSettings);
			await File.WriteAllBytesAsync(ProjectSettings.GlobalizePath("user://Audio/AIresponse.mp3"), voiceClip.ClipData.ToArray());
			ConvertMp3ToWav(ProjectSettings.GlobalizePath("user://Audio/AIresponse.mp3"), ProjectSettings.GlobalizePath("user://Audio/AIresponse.wav"));
			GD.Print("Finished result");
		}
		catch (HttpRequestException ex)
		{
			GD.PrintErr($"Network error: {ex.Message}");
			GetNode<NotificationsManager>("/root/Managers/Notification").NewNotification("Error", "[center]Network Error!", "[center]Please check your connection.", 10);
		}
		catch (Exception ex)
		{
			if (ex.Message.Contains("invalid_api_key"))
			{
				GD.PrintErr("Invalid API key. Please check your API key and try again.");
				GetNode<NotificationsManager>("/root/Managers/Notification").NewNotification("Error", "[center]Invalid API Key!", "[center]Please check your API key and try again.", 10);
			}
			else if (ex.Message.Contains("rate_limit_exceeded"))
			{
				GD.PrintErr("Rate limit exceeded. Please wait and try again later.");
				GetNode<NotificationsManager>("/root/Managers/Notification").NewNotification("Error", "[center]Rate Limit Exceeded!", "[center]Please wait and try again later.", 10);
			}
			else
			{
				GD.PrintErr($"General error: {ex.Message}");
				GetNode<NotificationsManager>("/root/Managers/Notification").NewNotification("Error", "[center]An Error Occurred!", "[center]Please try again.", 10);
			}
		}
	}

	private static void ConvertMp3ToWav(string inPath, string outPath)
	{
		using (Mp3FileReader mp3 = new Mp3FileReader(inPath))
		{
			using (WaveStream pcm = WaveFormatConversionStream.CreatePcmStream(mp3))
			{
				WaveFileWriter.CreateWaveFile(outPath, pcm);
			}
		}
	}

	public async Task<Voice[]> GetVoices()
	{
		try
		{
			var allVoices = await api.VoicesEndpoint.GetAllVoicesAsync();

			if (allVoices == null || allVoices.Count == 0)
			{
				GetNode<NotificationsManager>("/root/Managers/Notification").NewNotification("Error", "[center]Voices Failed", "[center]Voices failed to load. This may be due to an invalid API key.", 10);
				return null;
			}

			foreach (var voice in allVoices)
			{
				GD.Print($"{voice.Id} | {voice.Name} | similarity boost: {voice.Settings?.SimilarityBoost} | stability: {voice.Settings?.Stability} | custom: {voice.PreviewUrl}");
			}
			GetNode<NotificationsManager>("/root/Managers/Notification").NewNotification("Info", "[center]Voices Loaded", "[center]All voicebanks have successfully loaded", 10);
			return allVoices.ToArray();
		}
		catch (Exception ex)
		{
			if (ex.Message.Contains("invalid_api_key"))
			{
				GD.PrintErr("Invalid API key. Please check your API key and try again.");
				GetNode<NotificationsManager>("/root/Managers/Notification").NewNotification("Error", "[center]11labs Error!", "[center]Invalid API Key! Please check your API key and try again.", 10);
			}
			else if (ex.Message.Contains("rate_limit_exceeded"))
			{
				GD.PrintErr("Rate limit exceeded. Please wait and try again later.");
				GetNode<NotificationsManager>("/root/Managers/Notification").NewNotification("Error", "[center]11labs Error!", "[center]Rate Limit Exceeded!, Please wait and try again later.", 10);
			}
			else
			{
				GD.PrintErr($"General error: {ex.Message}");
				GetNode<NotificationsManager>("/root/Managers/Notification").NewNotification("Error", "[center]11labs Error!", "[center]An Error Occurred! Please try again.", 10);
			}
			return null;
		}
	}
}
