using Godot;
using System;
using System.IO;

public partial class WhisperGD : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public string GetText(string input)
	{
		GDScript MyGDScript = GD.Load<GDScript>("res://addons/godot_whisper/audio_stream_to_text.gd");
		GodotObject myGDScriptNode = (GodotObject)MyGDScript.New(); // This is a GodotObject.

		//var stream = GetChild<GDScript>(0);
		//myGDScriptNode.Set("audio_stream", ProjectSettings.GlobalizePath("user://Audio/AIresponse.wav"));
		return (string)myGDScriptNode.Call("get_text");

	}
}
