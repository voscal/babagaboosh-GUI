using System;
using Godot;


public partial class MasterScript : Node
{
	[Export]
	public Character character;
	UI ui;

	Remotelibraries Libraries;

	AudioManager audioManager;
	public override void _Ready()
	{
		Libraries = GetNode<Remotelibraries>("Remote Libraries");
		audioManager = GetNode<AudioManager>("Audio Manager");
		ui = GetNode<UI>("UI");
		ui.coreUI.record.Pressed += askAI;
		SetCharacter();
	}


	public void SetCharacter()
	{
		Libraries.chatGPT.SetContext(character.context);
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
