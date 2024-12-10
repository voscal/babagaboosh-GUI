using Godot;
using System;

public partial class ConversationManager : Node
{
	bool active;
	bool speakingLock;
	bool conversationLocked;



	string ExplnationIntro;

	string ExplnationOutro;

	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
