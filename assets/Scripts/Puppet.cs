using Godot;
using System;

public partial class Puppet : Node2D
{
	[Export]
	public float maxHeadHeight;



	bool mouseHoverd;

	string CurrentPart;

	bool Selected = false;

	Vector2 MouseOffset = new(0, 0);

	private AudioManager audioManager;
	private TextureButton headSprite;

	public override void _Ready()
	{
		audioManager = GetParent().GetParent().GetNode<AudioManager>("/root/Managers/Audio");
		headSprite = GetNode<TextureButton>("Head/Sprite");
	}

	public override void _Process(Double delta)
	{
		if (audioManager.GetNode<AudioStreamPlayer>("AIVoice").Playing == true)
		{
			float magnitude = audioManager.spectrum.GetMagnitudeForFrequencyRange(0, 1000).Length();

			// Calculate the scale factor for head movement based on volume
			magnitude = Mathf.Clamp(magnitude, 0f, 1f);
			headSprite.Position = new Vector2(0, Mathf.Lerp(headSprite.Position.Y, magnitude * -300, 0.15f));
		}




		if (GetNode<UI>("/root/Main Scene/UI").editorUI.editorOpen == true)
		{
			CameraControlles();

		}


	}

	public void MouseOverDummy(string Part)
	{
		if (GetNode<UI>("/root/Main Scene/UI").editorUI.editorOpen == true)
		{
			GetNode<AnimationPlayer>($"{Part}/AnimationPlayer").Play("Hover");
			mouseHoverd = true;
			CurrentPart = Part;
		}

	}

	public void MouseExitDummy(string Part)
	{
		if (GetNode<UI>("/root/Main Scene/UI").editorUI.editorOpen == true)
		{
			GetNode<AnimationPlayer>($"{Part}/AnimationPlayer").Play("UnHover");
			mouseHoverd = false;
			CurrentPart = "";
		}

	}

	public void CameraControlles()
	{
		if (Input.IsActionJustPressed("ZoomIn"))
		{
			GetNode<Camera2D>("Camera").Zoom -= new Vector2(0.1f, 0.1f);
		}
		else if (Input.IsActionJustPressed("ZoomOut"))
		{
			GetNode<Camera2D>("Camera").Zoom += new Vector2(0.1f, 0.1f);
		}

		if (Input.IsActionPressed("MoveCam"))
		{

			InputEventMouseMotion mousemotion = new();
			GetNode<Camera2D>("Camera").Position -= mousemotion.Relative;
		}

	}



	public override void _Input(InputEvent @event)
	{
		// Mouse in viewport coordinates.
		if (Input.IsActionPressed("MoveCam") && GetNode<UI>("/root/Main Scene/UI").editorUI.editorOpen == true)
			if (@event is InputEventMouseMotion eventMouseMotion)
				GetNode<Camera2D>("Camera").Position -= eventMouseMotion.Relative / GetNode<Camera2D>("Camera").Zoom;
	}



}







