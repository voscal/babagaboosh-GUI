using ElevenLabs.Voices;
using Godot;


public partial class EditorUI : Control
{
	string openedUI;
	public bool menuOpen = false;

	#region Voice Variables
	VoiceData voiceData;

	public Slider stablitliytySlider;

	#endregion



	public override void _Ready()
	{
		voiceData = GetNode<VoiceData>("/root/Data/VoiceData");

	}

	//play any ui's opening/closing animation
	public void EditButtonPressed(string nodePath)
	{
		GD.Print("CLICK");
		TextureButton button = GetNode<TextureButton>(nodePath);

		if (openedUI != null)
		{
			GetNode<AnimationPlayer>($"{openedUI}/AnimationPlayer").Play("CloseUI");
		}
		if (openedUI == nodePath)
		{
			GetNode<AnimationPlayer>($"{openedUI}/AnimationPlayer").Play("CloseUI");
			openedUI = null;
			return;
		}

		button.GetNode<AnimationPlayer>("AnimationPlayer").Play("OpenUI");

		openedUI = nodePath;





	}

	public void UpdateVoiceList()
	{

		VBoxContainer vBox = GetNode<VBoxContainer>("Voices/Background/ScrollContainer/VBoxContainer");
		foreach (voiceShelf voiceShelf in vBox.GetChildren())
		{
			voiceShelf.QueueFree();
		}
		var scene = GD.Load<PackedScene>("res://assets/Scenes/voiceShelf.tscn");
		foreach (Voice voice in voiceData.voices)
		{
			var sceneInstance = scene.Instantiate<voiceShelf>();
			sceneInstance.Name = voice.Name;
			sceneInstance.voice = voice;
			vBox.AddChild(sceneInstance);
		}


	}

	public VoiceSettings GetVoiceSettings()
	{
		VoiceSettings voiceSettingsNew = new()
		{
			Stability = (float)GetNode<Slider>("Voice Tweeks/Background/Panel/Stability").Value,
			SimilarityBoost = (float)GetNode<Slider>("Voice Tweeks/Background/Panel/Clarity").Value,
			Style = (float)GetNode<Slider>("Voice Tweeks/Background/Panel/Exaggeration").Value,

		};
		return voiceSettingsNew;
	}


}
