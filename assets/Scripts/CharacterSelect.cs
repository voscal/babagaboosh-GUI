using Godot;
using System;

public partial class CharacterSelect : Control
{
	[Export]
	AnimationPlayer animationPlayer;

	SaveManager saveManager;
	bool CharactersOpen = false;

	public override void _Ready()
	{
		saveManager = GetNode<SaveManager>("/root/Data/SaveData");
		RefreshCharactersList(saveManager.LoadAllCharacters());

	}
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

	public void RefreshCharactersList(string[] characters)
	{
		GridContainer vBox = GetNode<GridContainer>("BackGround/ScrollContainer/BoxContainer");
		foreach (CharacterBox characterBox in vBox.GetChildren())
		{
			characterBox.QueueFree();
		}
		var scene = GD.Load<PackedScene>("res://assets/Scenes/characterBox.tscn");
		foreach (string character in characters)
		{
			var sceneInstance = scene.Instantiate<CharacterBox>();
			sceneInstance.Name = character;
			sceneInstance.chrName = character;
			sceneInstance.GetNode<Label>("Panel/Label").Text = character.Remove(character.Length - 4);
			vBox.AddChild(sceneInstance);
		}
	}


}
