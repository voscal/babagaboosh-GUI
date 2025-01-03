using Godot;
using System;
using System.Threading;

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
	public PackedScene popup;
	public Image image;
	public Vector2 scale;



	// resizing / dragging variables
	Vector2 start;
	Vector2 initialPosition;
	bool isMoving = false;
	bool isResizing = false;
	bool resizeX = false;
	bool resizeY = false;
	Vector2 initialSize;
	[Export]
	float GrabThreshold = 20;
	[Export]
	float ResizeThreshold = 5;

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
		if (ui.editorUI.editorOpen == true)
		{
			var tw = CreateTween();
			tw.TweenProperty(GetNode<ColorRect>("Selection").GetMaterial(), "shader_parameter/line_thickness", 0.03, 0.2);
		}
	}

	private void MouseOut()
	{
		if (ui.editorUI.editorOpen == true)
		{
			var tw = CreateTween();
			tw.TweenProperty(GetNode<ColorRect>("Selection").GetMaterial(), "shader_parameter/line_thickness", 0, 0.2);
		}
	}

	public void changeImage(string imagePath)
	{
		Image image = Image.LoadFromFile(imagePath);
		ImageTexture texture = new();
		texture.SetImage(image);
		GetNode<TextureRect>("Sprite").Texture = texture;
		if (Name == "Head")
		{
			GetNode<Manager>("/root/Managers").character.ActiveCharacters[GetNode<Manager>("/root/Managers").character.ActiveCharacters.IndexOf(GetNode<Manager>("/root/Managers").character.GetFocusedCharacter())].image2 = texture;
		}
		else if (Name == "Body")
		{
			GetNode<Manager>("/root/Managers").character.ActiveCharacters[GetNode<Manager>("/root/Managers").character.ActiveCharacters.IndexOf(GetNode<Manager>("/root/Managers").character.GetFocusedCharacter())].image1 = texture;
		}
	}

	public void ResetSizing()
	{
		Size = new Vector2(50, 50);
	}



	private Vector2 _dragOffset;
	private bool _isDragging = false;

	public override void _GuiInput(InputEvent input)
	{

		if (input is InputEventMouse)
		{
			InputEventMouse mouseInput = (InputEventMouse)input;

			if (ui.editorUI.editorOpen == true)
			{
				// Context menu
				if (Input.IsActionJustPressed("RightMouseDown"))
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

					var instance = (Popup)ResourceLoader.Load<PackedScene>("res://assets/Scenes/popup.tscn").Instantiate();
					instance.Position = new Vector2(mousePos.X + 35, mousePos.Y - 25);
					instance.pathToProp = GetPath().ToString();
					instance.GetNode<Label>("Background/NameLabal").Text = Name;

					GetNode<UI>("/root/Main Window/UI").AddChild(instance);
				}


				//Decide if we want to drag or resize prop
				if (Input.IsActionJustPressed("LeftMouseDown"))
				{
					var rect = GetGlobalRect();
					var localMousePos = GetLocalMousePosition(); // Adjusted to account for viewport transformations




					if (Mathf.Abs(localMousePos.X - rect.Size.X) < ResizeThreshold)
					{

						start.X = localMousePos.X;
						initialSize.X = GetSize().X;
						resizeX = true;
						isResizing = true;
					}

					if (Mathf.Abs(localMousePos.Y - rect.Size.Y) < ResizeThreshold)
					{

						start.Y = localMousePos.Y;
						initialSize.Y = GetSize().Y;
						resizeY = true;
						isResizing = true;
					}

					if (localMousePos.X < ResizeThreshold && localMousePos.X > -ResizeThreshold)
					{

						start.X = localMousePos.X;
						initialPosition.X = GlobalPosition.X;
						initialSize.X = GetSize().X;
						isResizing = true;
						resizeX = true;
					}

					if (localMousePos.Y < ResizeThreshold && localMousePos.Y > -ResizeThreshold)
					{

						start.Y = localMousePos.Y;
						initialPosition.Y = GlobalPosition.Y;
						initialSize.Y = GetSize().Y;
						isResizing = true;
						resizeY = true;
					}
					if (!isResizing)
					{

						start = localMousePos;
						initialPosition = GlobalPosition;
						isMoving = true;
					}
				}

			}

			// Move or resize
			if (Input.IsActionPressed("LeftMouseDown"))
			{
				var localMousePos = GetLocalMousePosition(); // Ensure local mouse position for resizing/moving

				if (isMoving)
				{
					SetPosition(GetGlobalMousePosition());
				}

				if (isResizing)
				{

					var newWidth = GetSize().X;
					var newHeight = GetSize().Y;

					if (resizeX)
					{
						newWidth = initialSize.X + (localMousePos.X - start.X);
					}

					if (resizeY)
					{
						newHeight = initialSize.Y + (localMousePos.Y - start.Y);
					}

					if (initialPosition.X != 0)
					{
						newWidth = initialSize.X + (start.X - localMousePos.X);
						SetPosition(new Vector2(initialPosition.X - (newWidth - initialSize.X), GetPosition().Y));
					}

					if (initialPosition.Y != 0)
					{
						newHeight = initialSize.Y + (start.Y - localMousePos.Y);
						SetPosition(new Vector2(GetPosition().X, initialPosition.Y - (newHeight - initialSize.Y)));
					}

					SetSize(new Vector2(newWidth, newHeight));
				}
			}

			if (Input.IsActionJustReleased("LeftMouseDown"))
			{
				isMoving = false;
				initialPosition = Vector2.Zero;
				resizeX = false;
				resizeY = false;
				isResizing = false;
			}

		}
	}

}

