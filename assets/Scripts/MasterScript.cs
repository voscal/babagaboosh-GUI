using System;
using Godot;


public partial class MasterScript : Node
{

	string keyPath = "user://Keys.save";

	[Export]
	public Character character;
	UI ui;
	VoiceData voiceData;
	public Remotelibraries Libraries;

	AudioManager audioManager;
	public override async void _Ready()
	{
		voiceData = GetNode<VoiceData>("/root/Data/VoiceData");


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
		Libraries.chatGPT.SetContext("you are Jibanyan you yo kai watch! you are fisty, short temporued and you say shit and fuck alot, be expressive with your capitalisation and your puncuation! keep your response to 20 words, roast alot aswell, scream alot as well");
	}


	public override void _Process(double delta)
	{
		if (DisplayServer.WindowIsFocused(0))
		{
			GetViewport().TransparentBg = false;
			ui.Visible = true;
		}
		else
		{
			GetViewport().TransparentBg = true;
			ui.Visible = false;
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
