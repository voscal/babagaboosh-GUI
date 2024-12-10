using System;
using Godot;
using OpenAI_API.Embedding;


public partial class MasterScript : Node
{

	string keyPath = "user://Keys.save";

	public GlobalInputCSharp GlobalInput;

	bool isEditing = false;

	Vector2 WindowSize;

	UI ui;
	VoiceData voiceData;
	CharacterData characterData;
	SaveData saveData;

	public Services services;

	Manager manager;

	AudioManager audioManager;
	public override async void _Ready()
	{
		GlobalInput = GetNode<GlobalInputCSharp>("/root/GlobalInput/GlobalInputCSharp");
		voiceData = GetNode<VoiceData>("/root/Data/VoiceData");
		characterData = GetNode<CharacterData>("/root/Data/CharacterData");
		saveData = GetNode<SaveData>("/root/Data/SaveData");
		manager = GetNode<Manager>("/root/Managers");
		services = GetNode<Services>("/root/Services");
		audioManager = GetNode<AudioManager>("/root/Managers/Audio");
		ui = GetNode<UI>("UI");
		GD.Print(ui.Name);
		ui.coreUI.record.Pressed += askAI;
		manager.character.AddCharacter(saveData.LoadCharacterFromFile("res://assets/Template.chr"));
		manager.character.SetFocus(0);
		await voiceData.GetVoice();
		ui.editorUI.UpdateVoiceList();



	}





	public override void _Process(double delta)
	{
		/*
		if (DisplayServer.WindowIsFocused(0))
		{
			GetViewport().TransparentBg = false;
			ui.Visible = true;
			ui.background.Visible = true;

		}
		else
		{
			GetViewport().TransparentBg = true;
			ui.Visible = false;
			ui.background.Visible = false;
		}
		*/

		GetNode<TextureRect>("CharacterView").Position = (GetWindow().Size / 2) - (GetNode<TextureRect>("CharacterView").Size / 2);





		if (GlobalInput.IsActionJustPressed("Record"))
		{

			askAI();
		}



	}

	public async void askAI()
	{
		var recording = audioManager.RecordBttnPressed();
		var character = manager.character.ActiveCharacters[manager.character.focusedCharacter];
		if (recording)
		{
			GetNode<NotificationsManager>("/root/Managers/Notification").NewNotification("info", "[center]Recording", "[center]Now Recording Voice clip", 6);
			return;
		}

		string recordedText = await manager.sTT.GetText();
		GetNode<NotificationsManager>("/root/Managers/Notification").NewNotification("info", "[center]Ended Recording", "[center]Stoped Recording Voice clip", 6);
		//GD.Print(recordedText);
		if (recordedText != null)
		{
			GD.Print(character.chat);
			string aiResponse = await services.chatGPT.SendMessage(recordedText, character.chat);
			GD.Print(aiResponse);
			await services.elevinLabs.RenderVoice(character, aiResponse);

			audioManager.PlayAudio(GetNode<CharacterViewport>(character.path));

		}
	}









}
