using Godot;
using System;

public partial class Prop : Node2D
{
	public enum Transition
	{
		None,
		Slide,
		Fade,
		Pop
		// Custom (i wish lol, maybe ill do something with this later)
	}

	public Image image;
	public Vector2 scale;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
