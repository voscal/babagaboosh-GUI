using Godot;
using System;

public partial class SettingsUI : Control
{
	public OptionButton micSelect;
	public OptionButton OutputSelect;
	public override void _Ready()
	{
		micSelect = GetNode<OptionButton>("ScrollContainer/VBoxContainer/Audio/MicList");
		OutputSelect = GetNode<OptionButton>("ScrollContainer/VBoxContainer/Audio/OutputList");
	}

}
