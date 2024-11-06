using Godot;
using ElevenLabs.Voices;
using System.Threading.Tasks;
using Godot.Collections;

public partial class Character : Node
{
	public enum Style
	{
		Puppet,
		Switch,
		Static
	};
	public enum Transition
	{
		None,
		Slide,
		Fade,
		Pop
		// Custom (i wish lol, maybe ill do something with this later)
	}

	public string path;

	public string name;
	public string description;
	public string context;
	public Vector2 resolution = new(512, 512);
	public Style style;
	public Transition transition;
	public Texture2D image1;
	public Texture2D image2;
	bool isTalking;
	public VoiceSettings voiceSettings;
	public string voiceID;
	public Vector2 headPosition;
	public Vector2 headSize;
	public Vector2 bodyPosition;
	public Vector2 bodySize;



	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	public void Talk()
	{

	}


}
