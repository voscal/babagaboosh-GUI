using System;
using System.Collections.Generic;
using Godot;


public partial class CharacterManager : Manager
{
	private Manager manager;
	public List<Character> ActiveCharacters = new();
	public int focusedCharacter;

	[Export]
	public PackedScene BaseCharacterScene;

	public override void _Ready()
	{
		manager = GetNode<Manager>("/root/Managers");
	}

	public void AddCharacter(Character character)
	{
		var characerScene = BaseCharacterScene.Instantiate();
		//create and update a new viewport
		characerScene.GetNode<Node2D>("Dummy/Head").Position = character.headPosition;
		characerScene.GetNode<TextureRect>("Dummy/Head/Sprite").Size = character.headSize;
		characerScene.GetNode<TextureRect>("Dummy/Head/Sprite").Texture = character.image2;
		characerScene.GetNode<Node2D>("Dummy/Body").Position = character.bodyPosition;
		characerScene.GetNode<TextureRect>("Dummy/Body/Sprite").Size = character.bodySize;
		characerScene.GetNode<TextureRect>("Dummy/Body/Sprite").Texture = character.image1;

		ActiveCharacters.Add(character);
		characerScene.Name = ActiveCharacters.IndexOf(character) + " " + character.name;
		character.path = $"/root/Managers/View/{characerScene.Name}";
		manager.view.AddChild(characerScene);

	}
	public void RemoveCharacter(int index)
	{
		var character = manager.character.ActiveCharacters[index];
		GetNode<SubViewport>(character.path).QueueFree();
		ActiveCharacters.RemoveAt(index);
	}

	public void UpdateCharacter(int oldIndex, Character newCharacter)
	{
		var oldCharacter = manager.character.ActiveCharacters[oldIndex];
		UpdateCharacter(oldCharacter, newCharacter);
	}
	public void UpdateCharacter(Character oldCharacter, Character newCharacter)
	{
		var characerScene = GetNode<SubViewport>(oldCharacter.path);

		characerScene.GetNode<Node2D>("Dummy/Head").Position = newCharacter.headPosition;
		characerScene.GetNode<TextureRect>("Dummy/Head/Sprite").Size = newCharacter.headSize;
		characerScene.GetNode<TextureRect>("Dummy/Head/Sprite").Texture = newCharacter.image2;
		characerScene.GetNode<Node2D>("Dummy/Body").Position = newCharacter.bodyPosition;
		characerScene.GetNode<TextureRect>("Dummy/Body/Sprite").Size = newCharacter.bodySize;
		characerScene.GetNode<TextureRect>("Dummy/Body/Sprite").Texture = newCharacter.image1;

		ActiveCharacters[ActiveCharacters.IndexOf(oldCharacter)] = newCharacter;
		characerScene.Name = ActiveCharacters.IndexOf(newCharacter) + " " + newCharacter.name;
		newCharacter.path = $"/root/Managers/View/{characerScene.Name}";

	}

	public void SetFocus(int index)
	{
		GetNode<TextureRect>("/root/Main Window/CharacterView").Texture = GetNode<SubViewport>(manager.character.ActiveCharacters[index].path).GetTexture();
	}
	public void SetFocus(Character character)
	{
		GetNode<TextureRect>("/root/Main Window/CharacterView").Texture = GetNode<SubViewport>(character.path).GetTexture();
	}





	Vector2 StringToVector2(string VectString)
	{
		// Remove parentheses and split by comma
		string[] parts = VectString.Trim('(', ')').Split(',');

		// Parse each part into float
		float x = float.Parse(parts[0]);
		float y = float.Parse(parts[1]);

		return new Vector2(x, y);
	}

	#region List Functions
	private void MoveUp(int index)
	{
		if (index > 0 && index < ActiveCharacters.Count)
		{
			(ActiveCharacters[index], ActiveCharacters[index - 1]) = (ActiveCharacters[index - 1], ActiveCharacters[index]);
		}
		else
		{
			GD.Print("MoveUp: Index out of range");
		}
	}

	private void MoveDown(int index)
	{
		if (index >= 0 && index < ActiveCharacters.Count - 1)
		{
			(ActiveCharacters[index], ActiveCharacters[index + 1]) = (ActiveCharacters[index + 1], ActiveCharacters[index]);
		}
		else
		{
			GD.Print("MoveDown: Index out of range");
		}
	}

	private void MoveTo(int currentIndex, int newIndex)
	{
		if (currentIndex < 0 || currentIndex >= ActiveCharacters.Count)
		{
			GD.Print("MoveTo: Current index out of range");
			return;
		}

		// Clamp the new index within bounds
		newIndex = Mathf.Clamp(newIndex, 0, ActiveCharacters.Count - 1);

		var item = ActiveCharacters[currentIndex];
		ActiveCharacters.RemoveAt(currentIndex);
		ActiveCharacters.Insert(newIndex, item);
	}
	#endregion
}
