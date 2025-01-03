using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using ElevenLabs.Models;
using ElevenLabs.Voices;
using Godot;


public partial class EditorUI : Control
{
	CanvasLayer uiRoot;
	string openedUI;
	public bool menuOpen = false;

	public Services services;

	public bool editorOpen = false;

	#region Voice Variables
	VoiceData voiceData;

	Manager manager;

	[Export]
	AnimationPlayer AnimationPlayer;

	[Export]
	PackedScene popup;


	#endregion



	public override void _Ready()
	{
		voiceData = GetNode<VoiceData>("/root/Data/VoiceData");
		services = GetNodeOrNull<Services>("/root/Services");
		manager = GetNode<Manager>("/root/Managers");
	}


	//play any ui's opening/closing animation
	public void EditButtonPressed(string nodePath)
	{

		GD.Print("CLICK");
		TextureButton button = GetNode<TextureButton>($"Toolbar/{nodePath}");

		if (openedUI != null)
		{
			GetNode<AnimationPlayer>($"Toolbar/{openedUI}/AnimationPlayer").Play("CloseUI");
		}
		if (openedUI == nodePath)
		{
			GetNode<AnimationPlayer>($"Toolbar/{openedUI}/AnimationPlayer").Play("CloseUI");
			openedUI = null;
			return;
		}

		button.GetNode<AnimationPlayer>("AnimationPlayer").Play("OpenUI");

		openedUI = nodePath;

	}


	public void UpdateEditor(Character character)
	{

		// voice config
		GetNode<Slider>("Toolbar/Voice Config/Background/Panel/Stability").Value = character.voiceSettings.Stability;
		GetNode<Slider>("Toolbar/Voice Config/Background/Panel/Clarity").Value = character.voiceSettings.SimilarityBoost;
		GetNode<Slider>("Toolbar/Voice Config/Background/Panel/Exaggeration").Value = character.voiceSettings.Style;
		GetNode<TextEdit>("Toolbar/About/Background/Panel/AIname").Text = character.name;
		GetNode<TextEdit>("Toolbar/About/Background/Panel/AIabout").Text = character.description;
		GetNode<TextEdit>("AIContext/Pannle/Panel/AIcontext").Text = character.context;

	}

	#region Voice Menus
	public void UpdateVoiceList()
	{

		VBoxContainer customVBox = GetNode<VBoxContainer>("Toolbar/Voices/Background/ScrollContainer/VBoxContainer/CustomVoices");
		VBoxContainer premadeVBox = GetNode<VBoxContainer>("Toolbar/Voices/Background/ScrollContainer/VBoxContainer/PreMade");

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


	public VoiceSettings GetVoiceSettings()
	{
		VoiceSettings voiceSettingsNew = new()
		{
			Stability = (float)GetNode<Slider>("Toolbar/Voice Config/Background/Panel/Stability").Value,
			SimilarityBoost = (float)GetNode<Slider>("Toolbar/Voice Config/Background/Panel/Clarity").Value,
			Style = (float)GetNode<Slider>("Toolbar/Voice Config/Background/Panel/Exaggeration").Value,

		};
		return voiceSettingsNew;
	}

	void ExaggerationChanged(float value)
	{
		manager.character.GetFocusedCharacter().voiceSettings.Style = value;
	}
	void ClarityChanged(float value)
	{
		manager.character.GetFocusedCharacter().voiceSettings.SimilarityBoost = value;
	}
	void StabilityChanged(float value)
	{
		manager.character.GetFocusedCharacter().voiceSettings.Stability = value;
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
			GetNode<AnimationPlayer>($"Toolbar/{openedUI}/AnimationPlayer").Play("CloseUI");
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
		GetNode<SaveData>("/root/Data/SaveData").SaveCharacter(manager.character.GetFocusedCharacter());
	}
	public void ImportHead(string path)
	{
		Image image = Image.LoadFromFile(path);
		ImageTexture texture = new();
		texture.SetImage(image);
		GetNode<TextureButton>("/root/Main Window/Puppet/Character/Head/Sprite").TextureNormal = texture;
	}
	public void ImportBodyImage(string path)
	{
		Image image = Image.LoadFromFile(path);
		ImageTexture texture = new();
		texture.SetImage(image);
		GetNode<TextureButton>("/root/Main Window/Puppet/Character/Body/Sprite").TextureNormal = texture;
	}
	public void FileSelectedLoad(string path)
	{
		manager.character.AddCharacter(GetNode<SaveData>("/root/Data/SaveData").LoadCharacterFromFile(path));
	}
	public void UpdateContext()
	{
		manager.character.GetFocusedCharacter().context = GetNode<TextEdit>("AIContext/Pannle/Panel/AIcontext").Text;
		manager.character.GetFocusedCharacter().chat = services.chatGPT.CreateConversation(manager.character.GetFocusedCharacter());
		GetNode<AnimationPlayer>("AIContext/AnimationPlayer").Play("CloseMenu");
	}






	#endregion
}
