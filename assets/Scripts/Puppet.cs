using Godot;
//using System;

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

	bool canZoom = true;
	public override void _Ready()
	{
		audioManager = GetParent().GetParent().GetNode<AudioManager>("/root/Managers/Audio");
		headSprite = GetNode<TextureButton>("Head/Sprite");
		GetNode<Panel>("/root/Main Scene/UI/EditorUI/BG/Voices/Background").MouseEntered += UIMouseEntered;
		GetNode<Panel>("/root/Main Scene/UI/EditorUI/BG/Voices/Background").MouseExited += UIMouseExit;
	}

	public override void _Process(double delta)
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
		if (Input.IsActionJustPressed("ZoomIn") && canZoom)
		{
			GetNode<Camera2D>("Camera").Zoom -= new Vector2(0.1f, 0.1f);
		}
		else if (Input.IsActionJustPressed("ZoomOut") && canZoom)
		{
			GetNode<Camera2D>("Camera").Zoom += new Vector2(0.1f, 0.1f);
		}

		if (Input.IsActionPressed("MoveCam") && canZoom)
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

	private void UIMouseEntered()
	{
		canZoom = false;

	}
	private void UIMouseExit()
	{
		canZoom = true;
	}




}


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




