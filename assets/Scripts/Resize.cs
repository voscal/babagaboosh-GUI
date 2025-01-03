using Godot;
using System;

public partial class Resize : TextureButton
{

	private enum ResizeDirection
	{
		None, Left, Right, Top, Bottom, TopLeft, TopRight, BottomLeft, BottomRight, Move
	}

	private ResizeDirection resizeDirection = ResizeDirection.None;
	private Vector2 originalPosition;
	private Vector2 originalSize;
	private Vector2 dragStartPos;

	// Constants for the draggable edges and corners
	private const int EdgeMargin = 10;

	private Camera2D camera;

	public override void _Ready()
	{
		SetProcessInput(true);
		camera = GetParent().GetParent().GetParent().GetNode<Camera2D>("Camera");
	}

	public override void _GuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent && GetNode<UI>("/root/Main Window/UI").editorUI.editorOpen == true)
		{
			Vector2 globalMousePos = camera.GetGlobalMousePosition();

			if (mouseEvent.ButtonIndex == MouseButton.Left)
			{
				if (mouseEvent.Pressed)
				{
					// Check if the click event occurs within the button's bounds
					if (IsMouseInside(globalMousePos))
					{
						dragStartPos = globalMousePos;
						originalPosition = GetParent<Node2D>().Position;
						originalSize = Size;

						// Determine which edge or corner is being pressed
						resizeDirection = GetResizeDirection(globalMousePos);
						if (resizeDirection == ResizeDirection.None)
						{
							resizeDirection = ResizeDirection.Move;
						}
						// Accept the event to prevent it from being handled by other objects
						GetViewport().SetInputAsHandled();

					}
				}
				else
				{
					resizeDirection = ResizeDirection.None;
				}
			}
		}
		else if (@event is InputEventMouseMotion mouseMotionEvent)
		{
			Vector2 globalMousePos = camera.GetGlobalMousePosition();

			if (resizeDirection != ResizeDirection.None)
			{
				Vector2 offset = globalMousePos - dragStartPos;
				ResizeOrMoveTextureButton(offset);
			}
			else
			{
				UpdateCursor(globalMousePos);
			}
		}
	}

	private bool IsMouseInside(Vector2 mousePos)
	{
		Vector2 globalPos = GetParent<Node2D>().GlobalPosition;
		return mousePos.X >= globalPos.X && mousePos.X <= globalPos.X + Size.X &&
			   mousePos.Y >= globalPos.Y && mousePos.Y <= globalPos.Y + Size.Y;
	}

	private ResizeDirection GetResizeDirection(Vector2 mousePos)
	{
		Vector2 globalPos = GetParent<Node2D>().GlobalPosition;

		if (mousePos.X >= globalPos.X - EdgeMargin && mousePos.X <= globalPos.X + EdgeMargin)
		{
			if (mousePos.Y >= globalPos.Y - EdgeMargin && mousePos.Y <= globalPos.Y + EdgeMargin)
				return ResizeDirection.TopLeft;
			if (mousePos.Y >= globalPos.Y + Size.Y - EdgeMargin && mousePos.Y <= globalPos.Y + Size.Y + EdgeMargin)
				return ResizeDirection.BottomLeft;
			return ResizeDirection.Left;
		}
		if (mousePos.X >= globalPos.X + Size.X - EdgeMargin && mousePos.X <= globalPos.X + Size.X + EdgeMargin)
		{
			if (mousePos.Y >= globalPos.Y - EdgeMargin && mousePos.Y <= globalPos.Y + EdgeMargin)
				return ResizeDirection.TopRight;
			if (mousePos.Y >= globalPos.Y + Size.Y - EdgeMargin && mousePos.Y <= globalPos.Y + Size.Y + EdgeMargin)
				return ResizeDirection.BottomRight;
			return ResizeDirection.Right;
		}
		if (mousePos.Y >= globalPos.Y - EdgeMargin && mousePos.Y <= globalPos.Y + EdgeMargin)
			return ResizeDirection.Top;
		if (mousePos.Y >= globalPos.Y + Size.Y - EdgeMargin && mousePos.Y <= globalPos.Y + Size.Y + EdgeMargin)
			return ResizeDirection.Bottom;

		return ResizeDirection.None;
	}

	private void ResizeOrMoveTextureButton(Vector2 offset)
	{
		if (resizeDirection == ResizeDirection.Move)
		{
			GetParent<Node2D>().Position += offset;
			dragStartPos += offset; // Adjust the start position to continue dragging smoothly
			return;
		}

		Vector2 newPosition = originalPosition;
		Vector2 newSize = originalSize;

		switch (resizeDirection)
		{
			case ResizeDirection.Left:
				newPosition.X += offset.X;
				newSize.X -= offset.X;
				break;
			case ResizeDirection.Right:
				newSize.X += offset.X;
				break;
			case ResizeDirection.Top:
				newPosition.Y += offset.Y;
				newSize.Y -= offset.Y;
				break;
			case ResizeDirection.Bottom:
				newSize.Y += offset.Y;
				break;
			case ResizeDirection.TopLeft:
				newPosition += offset;
				newSize -= offset;
				break;
			case ResizeDirection.TopRight:
				newPosition.Y += offset.Y;
				newSize.Y -= offset.Y;
				newSize.X += offset.X;
				break;
			case ResizeDirection.BottomLeft:
				newPosition.X += offset.X;
				newSize.X -= offset.X;
				newSize.Y += offset.Y;
				break;
			case ResizeDirection.BottomRight:
				newSize += offset;
				break;
		}

		Size = newSize;
		GetParent<Node2D>().Position = newPosition;
	}

	private void UpdateCursor(Vector2 mousePos)
	{
		ResizeDirection direction = GetResizeDirection(mousePos);
		if (GetNode<UI>("/root/Main Window/UI").editorUI.editorOpen == false)
		{
			MouseDefaultCursorShape = CursorShape.Arrow;
			return;
		}

		switch (direction)
		{
			case ResizeDirection.Top:
			case ResizeDirection.Bottom:
				MouseDefaultCursorShape = CursorShape.Vsize;
				break;
			case ResizeDirection.Left:
			case ResizeDirection.Right:
				MouseDefaultCursorShape = CursorShape.Hsize;
				break;
			case ResizeDirection.TopLeft:
			case ResizeDirection.BottomRight:
				MouseDefaultCursorShape = CursorShape.Fdiagsize;
				break;
			case ResizeDirection.TopRight:
			case ResizeDirection.BottomLeft:
				MouseDefaultCursorShape = CursorShape.Bdiagsize;
				break;
			default:
				MouseDefaultCursorShape = CursorShape.Drag;
				break;
		}
	}
}
