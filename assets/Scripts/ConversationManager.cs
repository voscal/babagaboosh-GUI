using Godot;
using System;

public partial class ConversationManager : Node
{
	Manager manager;
	Services service;
	bool active = false;
	public bool isGroupConversation = false;
	bool speakingLock;
	public bool conversationLock;

	public Timer audioTimer;

	bool userBuffer;
	bool userRecording;

	public string ExplnationIntro = "your in a debait about the best 3 video games";

	public string ExplnationOutro = "you are conversing with 2 other people";

	int lastSpoke;


	public override void _Ready()
	{
		manager = GetNode<Manager>("/root/Managers");
		service = GetNode<Services>("/root/Services");
		audioTimer = GetNode<Timer>("AudioTimer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (active && !conversationLock)
		{
			int randomCharacter;
			while (true)
			{
				var rng = new RandomNumberGenerator();
				randomCharacter = rng.RandiRange(0, manager.character.ActiveCharacters.Count - 1);
				if (randomCharacter != lastSpoke)
				{
					break;
				}
			}
			lastSpoke = randomCharacter;
			conversationLock = true;
			manager.character.ActiveCharacters[randomCharacter].GenerateOpenResponse("Okay what is your response? remeber to keep it between 20 - 30 words", service, manager);

		}
	}

	public void ReadyConversation()
	{
		foreach (Character character in manager.character.ActiveCharacters)
		{
			character.openChat = service.chatGPT.CreateConversation($"{ExplnationIntro}\n{character.context}\nMessages that you receive from the other people in the conversation will always begin with their title, to help you distinguish who has said what. For example a message from someone named Victoria will begin with \"[VICTORIA]\", while a message from someone named Tony will begin with [TONY]. You should NOT begin your message with this, just answer normally.\n{ExplnationOutro}\nOkay, let the story begin!");
		}
		GD.Print("LET THE CONVO BEGIN");
		GetNode<NotificationsManager>("/root/Managers/Notification").NewNotification("info", "[center]Context", "[center]The Context has been set", 6);
	}

	public void StopConversation()
	{
		GetNode<NotificationsManager>("/root/Managers/Notification").NewNotification("info", "[center]Ending", "[center]Has Stopped", 6);
		active = false;
	}

	public void StartConversation()
	{
		GetNode<NotificationsManager>("/root/Managers/Notification").NewNotification("info", "[center]Starting", "[center]the conversation will now begin", 6);
		active = true;
	}


	void ClipOver()
	{
		conversationLock = false;
	}

	public async void AskAI()
	{



		if (isGroupConversation == true)
		{
			active = false;
			var recording = manager.audio.RecordBttnPressed();

			if (recording)
			{
				GetNode<NotificationsManager>("/root/Managers/Notification").NewNotification("info", "[center]Recording", "[center]Now Recording Voice clip", 6);
				return;
			}
			string recordedText = await manager.sTT.GetText();
			GetNode<NotificationsManager>("/root/Managers/Notification").NewNotification("info", "[center]Ended Recording", "[center]Stoped Recording Voice clip", 6);
			userRecording = false;
			if (recordedText != null)
			{
				conversationLock = true;

				var focusedCharacter = manager.character.ActiveCharacters[manager.character.focusedCharacter];
				service.chatGPT.UpdateChatHistory("HOST", recordedText);

				conversationLock = true;
				focusedCharacter.GenerateOpenResponse("Okay what is your response? remeber to keep it between 20 - 30 words", service, manager);

			}
			active = true;
		}
		else
		{

			if (conversationLock && !userRecording)
			{
				GetNode<NotificationsManager>("/root/Managers/Notification").NewNotification("error", "[center]Recording", "[center]an ai is still talking, please wait for it to finnish", 6);
				return;
			}

			conversationLock = true;
			userRecording = true;
			var recording = manager.audio.RecordBttnPressed();

			if (recording)
			{
				GetNode<NotificationsManager>("/root/Managers/Notification").NewNotification("info", "[center]Recording", "[center]Now Recording Voice clip", 6);
				return;
			}
			string recordedText = await manager.sTT.GetText();
			GetNode<NotificationsManager>("/root/Managers/Notification").NewNotification("info", "[center]Ended Recording", "[center]Stoped Recording Voice clip", 6);
			userRecording = false;
			if (recordedText != null)
			{
				var character = manager.character.ActiveCharacters[manager.character.focusedCharacter];
				await character.GenerateResponse(recordedText, service, manager);
				return;
			}
			conversationLock = false;

		}
	}


}
