using Godot;
using System;

public partial class CharacterSelect : Control
{
	[Export]
	AnimationPlayer animationPlayer;

	SaveData saveData;
	bool CharactersOpen = false;
	string openedUI;
	public override void _Ready()
	{
		saveData = GetNode<SaveData>("/root/Data/SaveData");
		RefreshCharactersList(saveData.LoadAllCharacters());

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
		GridContainer vBox = GetNode<GridContainer>("BackGround/ScrollContainer/VBoxContainer/BoxContainer");

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



	public void MoreButtonPressed(string nodePath)
	{

		GD.Print("CLICK");


		if (openedUI != null)
		{
			try
			{
				GetNode<AnimationPlayer>($"BackGround/ScrollContainer/VBoxContainer/BoxContainer/{openedUI}/AnimationPlayer").Play("close");
			}
			catch
			{
				openedUI = null;
			}

		}
		if (openedUI == nodePath)
		{
			GetNode<AnimationPlayer>($"BackGround/ScrollContainer/VBoxContainer/BoxContainer/{openedUI}/AnimationPlayer").Play("close");
			openedUI = null;
			return;
		}

		GetNode<AnimationPlayer>($"BackGround/ScrollContainer/VBoxContainer/BoxContainer/{nodePath}/AnimationPlayer").Play("open");

		openedUI = nodePath;





	}


}
