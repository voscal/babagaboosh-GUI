using ElevenLabs.Voices;
using Godot;
using System.Threading.Tasks;

public partial class VoiceData : Node
{
	Remotelibraries Libraries;
	public Voice[] voices;

	// Voice settings
	public float Stability;
	public float SimilarityBoost;
	public float Style;

	public override void _Ready()
	{
		Libraries = GetNodeOrNull<Remotelibraries>("/root/Main Scene/Remote Libraries");

	}

	public async Task GetVoice()
	{
		if (Libraries != null)
		{
			voices = await Libraries.elevinLabs.GetVoices();
			return;
		}
	}

	public VoiceSettings GetVoiceSettings()
	{


		VoiceSettings voiceSettingsNew = new()
		{
			Stability = 0f,
			SimilarityBoost = 0.5f,
			Style = 1f

		};


		return voiceSettingsNew;
	}
}
