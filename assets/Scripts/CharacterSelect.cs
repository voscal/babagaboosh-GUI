using Godot;
using System;

public partial class CharacterSelect : Control
{
	[Export]
	AnimationPlayer animationPlayer;
	bool CharactersOpen = false;
	public void CharactersPressed()
	{
		if (CharactersOpen)
		{
			animationPlayer.Play("CloseSideMenu");
			CharactersOpen = !CharactersOpen;
		}
		else
		{
			animationPlayer.Play("OpenSideMenu");
			CharactersOpen = !CharactersOpen;
		}

	}
}
