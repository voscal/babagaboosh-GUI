using Godot;
using System;

public partial class Popup : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<AnimationPlayer>("AnimationPlayer").Play("Open");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void remove()
	{
		QueueFree();
	}




	public override void _Input(InputEvent input)
	{
		if (input is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
		{
			// Get the global mouse position
			Vector2 mousePos = GetGlobalMousePosition();

			// Check if the mouse is inside the bounds of the current Control node
			Rect2 controlBounds = new Rect2(GlobalPosition, Size);

			if (!controlBounds.HasPoint(mousePos))
			{

				GetNode<AnimationPlayer>("AnimationPlayer").Play("Close");

			}
		}
	}
}
