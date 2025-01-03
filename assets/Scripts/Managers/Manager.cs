using Godot;
using System;

public partial class Manager : Node
{
	public STT sTT;
	public CharacterManager character;
	public ConversationManager conversation;
	public AudioManager audio;

	public Node view;
	public override void _Ready()
	{
		sTT = GetNode<STT>("STT");
		character = GetNode<CharacterManager>("Character");
		audio = GetNode<AudioManager>("Audio");
		conversation = GetNode<ConversationManager>("/root/Managers/Conversation");
		view = GetNode<Node>("/root/Main Window/CharacterView");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

