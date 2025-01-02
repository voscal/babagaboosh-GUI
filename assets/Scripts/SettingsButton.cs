using Godot;
using System;

public partial class SettingsButton : Button
{
	bool isOpen = false;

	public void BttnPressed()
	{
		GD.Print("Clicked");

		if (isOpen)
		{
			GetNode<AnimationPlayer>("AnimationPlayer").Play("Close");
			isOpen = false;
			GD.Print("Close");
		}
		else
		{
			GetNode<AnimationPlayer>("AnimationPlayer").Play("Open");
			isOpen = true;
			GD.Print("Open");
		}
	}
}
