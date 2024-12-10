using Godot;
using System;

public partial class AudioReactive : Node
{
	public enum Style
	{
		Move,
		Switch
	};

	//Move Style 
	public Vector2 MoveTo;
	public float tween;

	//switch
	public Image image;

	//audio
	public AudioEffectSpectrumAnalyzer audio;
	
	


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
