using Godot;
using System;

public partial class MasterUI : Control
{
	public CoreUI coreUI;
	public override void _Ready()
	{
		coreUI = GetNode<CoreUI>("CoreUI");
	}

}
