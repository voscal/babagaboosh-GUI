using Godot;
using System;

public partial class Windowtest : Window
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Position = new(-9999, -9999);
		GetViewport().TransparentBg = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
