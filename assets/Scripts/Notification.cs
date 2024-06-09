using Godot;
using System;

public partial class Notification : Control
{

	public float TimeOut = 5f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Timer>("Timer").WaitTime = TimeOut;
		GetNode<Timer>("Timer").Start();
		GetNode<TextureProgressBar>("Panel/Timer Progress").MaxValue = TimeOut;
		GetNode<AnimationPlayer>("AnimationPlayer").Play("In");
	}

	public override void _Process(double delta)
	{
		GetNode<TextureProgressBar>("Panel/Timer Progress").Value = GetNode<Timer>("Timer").TimeLeft;


	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public void CloseNotification()
	{
		QueueFree();
	}


	void Timeout()
	{

		QueueFree();
	}
}
