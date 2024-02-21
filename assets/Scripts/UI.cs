using Godot;
using System;

public partial class UI : CanvasLayer
{
	public CoreUI coreUI;
	public override void _Ready()
	{
		coreUI = GetNode<CoreUI>("CoreUI");
	}

}
