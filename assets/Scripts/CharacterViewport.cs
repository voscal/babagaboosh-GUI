using Godot;
using System;

public partial class CharacterViewport : SubViewport
{
	public bool focused = false;
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (focused)
			GetNode<Node2D>("IGNOR/Mouse").Position = GetWindow().GetViewport().GetMousePosition() - GetNode<TextureRect>("/root/Main Window/CharacterView").Position - (GetNode<TextureRect>("/root/Main Window/CharacterView").Size / 2);

	}
}
