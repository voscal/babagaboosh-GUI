using System.IO;
using ElevenLabs.Voices;
using Godot;


public partial class EditorUI : Control
{
	string openedUI;
	public bool menuOpen = false;



	bool editorOpen = false;

	#region Voice Variables
	VoiceData voiceData;
	CharacterData characterData;


	[Export]
	AnimationPlayer coreAnimationPlayer;


	#endregion



	public override void _Ready()
	{
		voiceData = GetNode<VoiceData>("/root/Data/VoiceData");

	}

	//play any ui's opening/closing animation
	public void EditButtonPressed(string nodePath)
	{
		GD.Print("CLICK");
		TextureButton button = GetNode<TextureButton>($"BG/{nodePath}");

		if (openedUI != null)
		{
			GetNode<AnimationPlayer>($"BG/{openedUI}/AnimationPlayer").Play("CloseUI");
		}
		if (openedUI == nodePath)
		{
			GetNode<AnimationPlayer>($"BG/{openedUI}/AnimationPlayer").Play("CloseUI");
			openedUI = null;
			return;
		}

		button.GetNode<AnimationPlayer>("AnimationPlayer").Play("OpenUI");

		openedUI = nodePath;





	}

	#region Voice Menus
	public void UpdateVoiceList()
	{

		VBoxContainer vBox = GetNode<VBoxContainer>("BG/Voices/Background/ScrollContainer/VBoxContainer");
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
			Stability = (float)GetNode<Slider>("BG/Voice Tweeks/Background/Panel/Stability").Value,
			SimilarityBoost = (float)GetNode<Slider>("BG/Voice Tweeks/Background/Panel/Clarity").Value,
			Style = (float)GetNode<Slider>("BG/Voice Tweeks/Background/Panel/Exaggeration").Value,

		};
		return voiceSettingsNew;
	}
	#endregion



	#region About Menus
	public void OpenContextMenu()
	{
		GetNode<AnimationPlayer>("AIContext/AnimationPlayer").Play("OpenMenu");
	}

	public void CloseContextMenu()
	{
		GetNode<AnimationPlayer>("AIContext/AnimationPlayer").Play("CloseMenu");
	}

	public void OpenEditorPressed()
	{

		GD.Print("OPEN EDITOR");
		if (openedUI != null)
		{
			GetNode<AnimationPlayer>($"BG/{openedUI}/AnimationPlayer").Play("CloseUI");
			openedUI = null;
		}

		if (editorOpen == false)
			coreAnimationPlayer.Play("OpenEditor");
		else
			coreAnimationPlayer.Play("CloseEditor");

		editorOpen = !editorOpen;

	}



	public void StartResize()
	{

		ProjectSettings.SetSetting("display/window/size/resizable", true);
		coreAnimationPlayer.Play("ResizeWindow");
	}
	public void FinnishResize()
	{
		coreAnimationPlayer.Play("ResizeWindowFinnished");
		DisplayServer.WindowSetSize(new Vector2I(1152, 648), 0);
	}
	public void FileSelectedSave(string path)
	{
		GetNode<SaveManager>("/root/Data/SaveData").SaveCharacter(path);
	}

	public void FileSelectedLoad(string path)
	{
		GetNode<SaveManager>("/root/Data/SaveData").LoadCharacter(path);
		coreAnimationPlayer.Play("ImportCharacter");
	}


	public void LoadData()
	{
		//name
		//description
		//ai context
	}



	#endregion
}
