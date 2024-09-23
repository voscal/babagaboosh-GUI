using System;
using Godot;


public partial class MasterScript : Node
{

	string keyPath = "user://Keys.save";

	public GlobalInputCSharp GlobalInput;


	Vector2 WindowSize;

	UI ui;
	VoiceData voiceData;
	CharacterData characterData;
	public Services services;

	Manager manager;

	AudioManager audioManager;
	public override async void _Ready()
	{
		GlobalInput = GetNode<GlobalInputCSharp>("/root/GlobalInput/GlobalInputCSharp");
		voiceData = GetNode<VoiceData>("/root/Data/VoiceData");
		characterData = GetNode<CharacterData>("/root/Data/CharacterData");
		manager = GetNode<Manager>("/root/Managers");

		services = GetNode<Services>("/root/Services");
		audioManager = GetNode<AudioManager>("/root/Managers/Audio");
		ui = GetNode<UI>("UI");
		GD.Print(ui.Name);
		ui.coreUI.record.Pressed += askAI;
		SetCharacter();
		await voiceData.GetVoice();
		ui.editorUI.UpdateVoiceList();

	}


	public void SetCharacter()
	{
		services.chatGPT.SetContext("");

	}


	public override void _Process(double delta)
	{

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





		if (GlobalInput.IsActionJustPressed("Record"))
		{

			askAI();
		}



	}

	public async void askAI()
	{
		var recording = audioManager.RecordBttnPressed();

		if (recording)
		{
			GetNode<NotificationsManager>("/root/Managers/Notification").NewNotification("info", "[center]Recording", "[center]Now Recording Voice clip", 6);
			return;
		}

		string recordedText = await manager.sTT.GetText();
		GetNode<NotificationsManager>("/root/Managers/Notification").NewNotification("info", "[center]Ended Recording", "[center]Stoped Recording Voice clip", 6);
		GD.Print(recordedText);
		if (recordedText != null)
		{
			string aiResponse = await services.chatGPT.SendMessage(recordedText);
			GD.Print(aiResponse);
			await services.elevinLabs.RenderVoice(aiResponse);

			audioManager.PlayAudio();

		}
	}









}
