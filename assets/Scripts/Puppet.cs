using Godot;
//using System;

public partial class Puppet : Prop
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

	bool canZoom = true;
	public override void _Ready()
	{

		audioManager = GetParent().GetParent().GetNode<AudioManager>("/root/Managers/Audio");
		headSprite = GetNode<TextureRect>("Head/Sprite");

	}



	public override void _Process(double delta)
	{
		//if (audioManager.GetNode<AudioStreamPlayer>("AudioPlayer").Playing == true)
		//{
		//float magnitude = AudioServer.GetMagnitudeForFrequencyRange(0, 1000).Length();

		// Calculate the scale factor for head movement based on volume
		//magnitude = Mathf.Clamp(magnitude, 0f, 1f);
		//headSprite.Position = new Vector2(0, Mathf.Lerp(headSprite.Position.Y, magnitude * -300, 0.15f));
		//}





	}
}









