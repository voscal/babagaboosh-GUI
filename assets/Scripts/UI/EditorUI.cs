using System.Collections.Generic;
using System.IO;
using System.Linq;
using ElevenLabs.Models;
using ElevenLabs.Voices;
using Godot;


public partial class EditorUI : Control
{
	string openedUI;
	public bool menuOpen = false;

	public Services services;

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
		services = GetNodeOrNull<Services>("/root/Services");
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

		VBoxContainer customVBox = GetNode<VBoxContainer>("BG/Voices/Background/ScrollContainer/VBoxContainer/CustomVoices");
		VBoxContainer premadeVBox = GetNode<VBoxContainer>("BG/Voices/Background/ScrollContainer/VBoxContainer/PreMade");

		foreach (voiceShelf voiceShelf in customVBox.GetChildren().OfType<voiceShelf>())
			voiceShelf.QueueFree();
		foreach (voiceShelf voiceShelf in premadeVBox.GetChildren().OfType<voiceShelf>())
			voiceShelf.QueueFree();

		var scene = GD.Load<PackedScene>("res://assets/Scenes/voiceShelf.tscn");
		foreach (Voice voice in voiceData.voices)
		{
			if (voice.PreviewUrl.Contains("premade"))
			{
				var premadeSceneInstance = scene.Instantiate<voiceShelf>();
				premadeSceneInstance.Name = voice.Name;
				premadeSceneInstance.voice = voice;
				premadeVBox.AddChild(premadeSceneInstance);
				continue;
			}
			var customSceneInstance = scene.Instantiate<voiceShelf>();
			customSceneInstance.Name = voice.Name;
			customSceneInstance.voice = voice;
			customVBox.AddChild(customSceneInstance);

		}


	}

	public async void UpdateModleList()
	{
		await services.chatGPT.GetModles();
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



	public void SaveButtonClicked()
	{
		GetNode<SaveManager>("/root/Data/SaveData").SaveCharacter();

	}
	public void SaveButtonClicked(string path)
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
	public void FileSelectedLoad(string path)
	{
		GetNode<SaveManager>("/root/Data/SaveData").ImportCharacter(path);
	}
	public void UpdateContext()
	{
		services.chatGPT.SetContext(GetNode<TextEdit>("AIContext/Pannle/Panel/AIcontext").Text);
	}

	#endregion
}
