using Godot;
using System;

public partial class Manager : Node
{
	public STT sTT;
	public CharacterManager character;
	public ViewManager view;
	public ConversationManager conversation;
	public AudioManager audio;

	public override void _Ready()
	{
		sTT = GetNode<STT>("STT");
		character = GetNode<CharacterManager>("Character");
		view = GetNode<ViewManager>("/root/Managers/View");
		audio = GetNode<AudioManager>("Audio");
		conversation = GetNode<ConversationManager>("/root/Managers/Conversation");

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

