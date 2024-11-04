using System;
using System.Collections.Generic;
using Godot;


public partial class CharacterManager : Manager
{
	private Manager manager;
	public List<Character> ActiveCharacters;
	public Character focusedCharacter;

	[Export]
	public PackedScene BaseCharacterScene;

	public override void _Ready()
	{
		manager = GetNode<Manager>("/root/Managers");

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void AddCharacter(Character character)
	{
		var characerScene = BaseCharacterScene.Instantiate();
		GetNode<Node2D>("Dummy/Head").Position = character.headPosition;
		GetNode<TextureButton>("Dummy/Head/Sprite").Size = character.headSize;
		GetNode<Sprite2D>("Dummy/Head/Sprite").Texture = character.image2;
		GetNode<Node2D>("Dummy/Body").Position = character.bodyPosition;
		GetNode<TextureButton>("Dummy/Body/Sprite").Size = character.bodySize;
		GetNode<Sprite2D>("Dummy/Body/Sprite").Texture = character.image1;

		manager.view.AddChild(characerScene);
		ActiveCharacters.Add(character);

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
}
