using Godot;
using System;

public partial class Manager : Node
{
	public STT sTT;
	public CharacterManager character;
	public ViewManager view;

	public override void _Ready()
	{
		sTT = GetNode<STT>("STT");
		character = GetNode<CharacterManager>("Character");
		view = GetNode<ViewManager>("/root/Managers/View");

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

