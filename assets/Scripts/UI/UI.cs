using Godot;
using System;

public partial class UI : CanvasLayer
{
	public CoreUI coreUI;
	public SettingsUI settingsUI;
	public override void _Ready()
	{
		coreUI = GetNode<CoreUI>("CoreUI");
		settingsUI = GetNode<SettingsUI>("SettingsUI");
	}

}