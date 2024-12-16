using Godot;
using System;

public partial class ConversationManager : Node
{
	Manager manager;
	Services service;
	bool active = false;
	bool speakingLock;
	public bool conversationLock;

	public Timer audioTimer;

	string ExplnationIntro = "your in a debait about the best 3 video games";

	string ExplnationOutro = "you are conversing with 2 other people";

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
			manager.character.ActiveCharacters[randomCharacter].GenerateOpenResponse("Okay what is your response? Try to be as chaotic and bizarre and adult-humor oriented as possible. Again, 3 sentences maximum.", service, manager);
			conversationLock = true;
		}
	}

	public void StartConversation()
	{
		foreach (Character character in manager.character.ActiveCharacters)
		{
			character.openChat = service.chatGPT.CreateConversation($"{ExplnationIntro}\n{character.context}\n{ExplnationOutro}");
		}
		GD.Print("LET THE CONVO BEGIN");
		active = true;

	}

	public void StopConversation()
	{
		active = false;
	}

	public void ResetConversation()
	{

	}

	void ClipOver()
	{
		conversationLock = false;
	}


}
