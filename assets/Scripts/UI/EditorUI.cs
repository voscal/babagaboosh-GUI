using Godot;
using System;

public partial class EditorUI : Control
{

	public bool menuOpen = false;
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void EditButtonPressed(string nodePath)
	{
		TextureButton button = GetNode<TextureButton>(nodePath);

		if (menuOpen)
		{
			button.GetNode<AnimationPlayer>("AnimationPlayer").Play("OpenUI");
			menuOpen = false;
		}

		else
		{
			button.GetNode<AnimationPlayer>("AnimationPlayer").Play("CloseUI");
			menuOpen = true;
		}


	}
}
