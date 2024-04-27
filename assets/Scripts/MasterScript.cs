using System;
using Godot;


public partial class MasterScript : Node
{

	string keyPath = "user://Keys.save";

	public GlobalInputCSharp GlobalInput;


	Vector2 WindowSize;


	[Export]
	public Character character;
	UI ui;
	VoiceData voiceData;
	CharacterData characterData;
	public Remotelibraries Libraries;

	AudioManager audioManager;
	public override async void _Ready()
	{
		GlobalInput = GetNode<GlobalInputCSharp>("/root/GlobalInput/GlobalInputCSharp");
		voiceData = GetNode<VoiceData>("/root/Data/VoiceData");
		characterData = GetNode<CharacterData>("/root/Data/CharacterData");


		Libraries = GetNode<Remotelibraries>("Remote Libraries");
		audioManager = GetNode<AudioManager>("Audio Manager");
		ui = GetNode<UI>("UI");
		ui.coreUI.record.Pressed += askAI;
		SetCharacter();
		await voiceData.GetVoice();
		ui.editorUI.UpdateVoiceList();

	}


	public void SetCharacter()
	{
		Libraries.chatGPT.SetContext("you are now dianne peterson, you are old and fucking stupid, tell your students to get an A but say it like EYyyyyy, keep your response between 20 words, act like your are about to be sold bilbords but you dont want any, go along with every joke, say fuck alot, and negative");
	}


	public override void _Process(double delta)
	{
		GD.Print(DisplayServer.WindowGetSize(0));
		if (DisplayServer.WindowIsFocused(0))
		{
			GetViewport().TransparentBg = false;
			ui.Visible = true;
			DisplayServer.WindowSetSize(new Vector2I(1152, 648), 0);
		}
		else
		{
			GetViewport().TransparentBg = true;
			ui.Visible = false;
			DisplayServer.WindowSetSize(new Vector2I(242, 377), 0);
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
			return;

		audioManager.recordEffect.SetRecordingActive(false);
		string recordedText = await Libraries.azuir.GetTextFromWav(ProjectSettings.GlobalizePath("res://Audio/record.wav"));
		if (recordedText != null)
		{

			string aiResponse = await Libraries.chatGPT.SendMessage(recordedText);
			GD.Print(aiResponse);
			await Libraries.elevinLabs.RenderVoice(aiResponse);

			audioManager.PlayAudio();

		}
	}









}
