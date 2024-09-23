using Godot;
using System;

public partial class Manager : Node
{
	public STT sTT;

	public override void _Ready()
	{
		sTT = GetNode<STT>("STT");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

