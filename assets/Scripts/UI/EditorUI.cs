using System.IO;
using ElevenLabs.Voices;
using Godot;


public partial class EditorUI : Control
{
	string openedUI;
	public bool menuOpen = false;

	public Remotelibraries remotelibraries;

	public bool editorOpen = false;

	#region Voice Variables
	VoiceData voiceData;
	CharacterData characterData;


	[Export]
	AnimationPlayer AnimationPlayer;


	#endregion



	public override void _Ready()
	{
		voiceData = GetNode<VoiceData>("/root/Data/VoiceData");
		remotelibraries = GetNode<Remotelibraries>("/root/Main Scene/Remote Libraries");
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
			Stability = (float)GetNode<Slider>("BG/Voice Config/Background/Panel/Stability").Value,
			SimilarityBoost = (float)GetNode<Slider>("BG/Voice Config/Background/Panel/Clarity").Value,
			Style = (float)GetNode<Slider>("BG/Voice Config/Background/Panel/Exaggeration").Value,

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
			AnimationPlayer.Play("OpenEditor");
		else
			AnimationPlayer.Play("CloseEditor");

		editorOpen = !editorOpen;

	}



	public void FileSelectedSave(string path)
	{
		GetNode<SaveManager>("/root/Data/SaveData").SaveCharacter(path);

	}

	public void FileSelectedLoad(string path)
	{
		GetNode<SaveManager>("/root/Data/SaveData").LoadCharacter(path);
		GetParent().GetNode<AnimationPlayer>("Funnyshit/AnimatedSprite2D/AnimationPlayer").Play("Explosion");
	}

	public void ImportHead(string path)
	{
		Image image = Image.LoadFromFile(path);
		ImageTexture texture = new();
		texture.SetImage(image);
		GetNode<TextureButton>("/root/Main Scene/Puppet/Character/Head/Sprite").TextureNormal = texture;
	}
	public void ImportBodyImage(string path)
	{
		Image image = Image.LoadFromFile(path);
		ImageTexture texture = new();
		texture.SetImage(image);
		GetNode<TextureButton>("/root/Main Scene/Puppet/Character/Body/Sprite").TextureNormal = texture;
	}


	public void UpdateContext()
	{
		remotelibraries.chatGPT.SetContext(GetNode<TextEdit>("AIContext/Pannle/Panel/AIcontext").Text);
	}

	#endregion
}
