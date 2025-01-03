using Godot;
using System;

public partial class Popup : Control
{
	public string pathToProp;
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

	void ImageSelected(string imgPath)
	{
		GetNode<Prop>(pathToProp).changeImage(imgPath);
	}

	void ResetSizing()
	{
		GetNode<Prop>(pathToProp).ResetSizing();
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
