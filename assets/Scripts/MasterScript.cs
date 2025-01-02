using System;
using Godot;
using OpenAI.Embeddings;


public partial class MasterScript : Node
{

	string keyPath = "user://Keys.save";

	public GlobalInputCSharp GlobalInput;

	bool isEditing = false;

	Vector2 WindowSize;

	UI ui;
	VoiceData voiceData;

	SaveData saveData;

	public Services services;

	Manager manager;

	AudioManager audioManager;
	public override async void _Ready()
	{
		GlobalInput = GetNode<GlobalInputCSharp>("/root/GlobalInput/GlobalInputCSharp");
		voiceData = GetNode<VoiceData>("/root/Data/VoiceData");
		saveData = GetNode<SaveData>("/root/Data/SaveData");
		manager = GetNode<Manager>("/root/Managers");
		services = GetNode<Services>("/root/Services");
		audioManager = GetNode<AudioManager>("/root/Managers/Audio");
		ui = GetNode<UI>("UI");

		ui.coreUI.record.Pressed += askAI;
		manager.character.AddCharacter(saveData.LoadCharacterFromFile("res://assets/Template.chr"));
		manager.character.SetFocus(0);
		await voiceData.GetVoice();
		ui.editorUI.UpdateVoiceList();



	}





	public override void _Process(double delta)
	{


		GetNode<Control>("CharacterView").Position = (GetWindow().Size / 2) - (GetNode<Control>("CharacterView").Size / 2);





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
			//manager.conversation.StartConversation();
			return;
		}
		//manager.conversation.StopConversation();

		string recordedText = await manager.sTT.GetText();
		GetNode<NotificationsManager>("/root/Managers/Notification").NewNotification("info", "[center]Ended Recording", "[center]Stoped Recording Voice clip", 6);

		if (recordedText != null)
		{
			character.GenerateResponse(recordedText, services, manager);
		}

	}









}
