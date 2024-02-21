using Godot;
using System;

public partial class Puppet : Node2D
{
	[Export]
	public float maxHeadHeight;

	private AudioManager audioManager;
	private Sprite2D headSprite;

	public override void _Ready()
	{
		audioManager = GetParent().GetParent().GetNode<AudioManager>("Audio Manager");
		headSprite = GetNode<Sprite2D>("Head");
	}

	public override void _Process(Double delta)
	{
		float magnitude = audioManager.spectrum.GetMagnitudeForFrequencyRange(0, 1000).Length();

		// Calculate the scale factor for head movement based on volume
		magnitude = Mathf.Clamp(magnitude, 0f, 1f);



		headSprite.Position = new Vector2(0, Mathf.Lerp(headSprite.Position.Y, magnitude * -300, 0.15f));
	}

}
