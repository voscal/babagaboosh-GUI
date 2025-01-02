using Godot;
using System;

public partial class Prop : Control
{
	public enum Transition
	{
		None,
		Slide,
		Fade,
		Pop
		// Custom (i wish lol, maybe ill do something with this later)
	}
	UI ui;
	bool dragged = false;
	public PackedScene popup;
	public Image image;
	public Vector2 scale;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ui = GetNode<UI>("/root/Main Window/UI");



		// Create a tween node


		// Start the tween with the method call to change shader parameter

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// If the node is being dragged, update its position live
		if (_isDragging)
		{
			// Get the current mouse position and adjust the node's position based on the offset
			Vector2 mousePosition = GetViewport().GetMousePosition();
			Position = mousePosition - _dragOffset; // Keep the node centered on the mouse
		}
	}

	private void MouseIn()
	{
		GD.Print("IN " + GetParent().GetParent().GetParent().Name);
	}

	private void MouseOut()
	{

	}


	void SetShaderOutline(float amount)
	{


	}



	private Vector2 _dragOffset;
	private bool _isDragging = false;

	public override void _GuiInput(InputEvent input)
	{
		if (input is InputEventMouseButton)
		{
			InputEventMouseButton mouseInput = (InputEventMouseButton)input;

			// Context menu
			if (mouseInput.ButtonIndex == MouseButton.Right && mouseInput.Pressed)
			{
				GD.Print($"Right-clicked on {Name}");

				// Close existing popups
				foreach (Control popupInstance in GetTree().GetNodesInGroup("Popup"))
				{
					popupInstance.GetNode<AnimationPlayer>("AnimationPlayer").Play("Close");
				}

				// Get the mouse position relative to this Control

				// Create and position the popup
				Vector2 mousePos = GetWindow().GetViewport().GetMousePosition();

				var instance = (Control)ResourceLoader.Load<PackedScene>("res://assets/Scenes/popup.tscn").Instantiate();
				instance.Position = new Vector2(mousePos.X + 35, mousePos.Y - 25);
				instance.GetNode<Label>("Background/NameLabal").Text = Name;

				GetNode<UI>("/root/Main Window/UI").AddChild(instance);
			}


			// Start dragging
			if (mouseInput.ButtonIndex == MouseButton.Left && mouseInput.Pressed && !_isDragging)
			{
				_dragOffset = mouseInput.GlobalPosition - Position; // Calculate the initial offset
				_isDragging = true;
			}

			// Stop dragging
			if (mouseInput.ButtonIndex == MouseButton.Left && !mouseInput.Pressed && _isDragging)
			{
				_isDragging = false;
			}


		}
	}

}
