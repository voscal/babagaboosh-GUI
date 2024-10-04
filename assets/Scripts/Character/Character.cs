using Godot;
using System;

public partial class Character : Node
{
	public enum Style
	{
		Puppet,
		Switch,
		Static
	};
	public enum Transition
	{
		None,
		Slide,
		Fade,
		Pop
		// Custom (i wish lol, maybe ill do something with this later)
	}
	private string name;
	private string description;
	private string context;
	private Vector2 resolution;
	private Style style;
	private Transition transition;
	private Image image1;
	private Image image2;
	bool isTalking;

	int CharacterIndex; //used to identify chracters with the same name 
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	public void Talk()
	{

	}

}
