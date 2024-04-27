using Godot;
using System;

public partial class Puppet : Node2D
{
	[Export]
	public float maxHeadHeight;

	[Export]
	bool editorMode = false;

	bool mouseHoverd;

	string CurrentPart;

	bool IsDragging;

	private AudioManager audioManager;
	private TextureButton headSprite;

	public override void _Ready()
	{
		audioManager = GetParent().GetParent().GetNode<AudioManager>("Audio Manager");
		headSprite = GetNode<TextureButton>("Head/Sprite");
	}

	public override void _Process(Double delta)
	{
		float magnitude = audioManager.spectrum.GetMagnitudeForFrequencyRange(0, 1000).Length();

		// Calculate the scale factor for head movement based on volume
		magnitude = Mathf.Clamp(magnitude, 0f, 1f);



		headSprite.Position = new Vector2(0, Mathf.Lerp(headSprite.Position.Y, magnitude * -300, 0.15f));

		if (!editorMode)
		{
			EditorPupet();
		}

	}

	public void EditorPupet()
	{

	}

	public void MouseOverDummy(string Part)
	{
		GetNode<AnimationPlayer>($"{Part}/AnimationPlayer").Play("Hover");
		mouseHoverd = true;
		CurrentPart = Part;
	}

	public void MouseExitDummy(string Part)
	{
		GetNode<AnimationPlayer>($"{Part}/AnimationPlayer").Play("UnHover");
		mouseHoverd = false;
		CurrentPart = "";
	}

	public void CameraControlles()
	{
		if (Input.IsMouseButtonPressed(MouseButton.Middle))
		{

		}
	}

	/*

		[Export]
		float min_zoom = 0.5F;
		[Export]
		float max_zoom = 2.0F;
		[Export]
		float zoom_factor = 0.1f;
		[Export]
		float zoom_duration = 0.2f;

		float _zoom_level = 1.0f;


		Tween tween = new();


		void _set_zoom_level(float value)
		{

			_zoom_level = Mathf.Clamp(value, min_zoom, max_zoom);
			tween.SetTrans(Tween.TransitionType.Sine);
			tween.SetEase(Tween.EaseType.Out);
			tween.TweenProperty(GetNode<Camera2D>("Camera"), "zoom", new Vector2(_zoom_level, _zoom_level), zoom_duration);
			tween.Play();
		}

		*/

}
