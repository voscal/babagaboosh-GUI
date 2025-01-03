using Godot;
//using System;

public partial class Dummy : Prop
{
	public enum Style
	{
		Puppet,
		Switch,
		Static
	};




	[Export]
	public float maxHeadHeight;


	string CurrentPart;

	bool Selected = false;

	Vector2 MouseOffset = new(0, 0);

	private AudioManager audioManager;
	private TextureRect headSprite;

	public AudioEffectSpectrumAnalyzerInstance audioSpectrum;

	bool canZoom = true;
	public override void _Ready()
	{

		audioManager = GetParent().GetParent().GetNode<AudioManager>("/root/Managers/Audio");
		headSprite = GetNode<TextureRect>("Head/Sprite");

	}



	public override void _Process(double delta)
	{
		if (GetNode<AudioStreamPlayer>("AudioPlayer").Playing == true)
		{
			float magnitude = audioSpectrum.GetMagnitudeForFrequencyRange(0, 1000).Length();

			// Calculate the scale factor for head movement based on volume
			magnitude = Mathf.Clamp(magnitude, 0f, 1f);
			//Rotation = Mathf.Lerp(Rotation, magnitude * -300, 0.35f);
			headSprite.Position = new Vector2(0, Mathf.Lerp(headSprite.Position.Y, magnitude * -300, 0.35f));
			return;
		}
		headSprite.Position = new Vector2(0, 0);
		Rotation = Mathf.Lerp(Rotation, 0, 0.35f);



	}
}









