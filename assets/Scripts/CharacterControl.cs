using Godot;
using System;

public partial class CharacterControl : Panel
{
	Manager manager;
	public int id = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		manager = GetNode<Manager>("/root/Managers");
		GetNode<Label>("Id").Text = id.ToString();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void focusCharacter()
	{

		manager.character.SetFocus(id);
	}

	public void RemoveCharacter()
	{
		GetNode<Manager>("/root/Managers").character.RemoveCharacter(id);
	}


}
