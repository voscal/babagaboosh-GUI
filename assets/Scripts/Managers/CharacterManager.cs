using System.Collections.Generic;
using Godot;
using Godot.Collections;

public partial class CharacterManager : Manager
{
	private List<Character> ActiveCharacters;
	[Export]
	public PackedScene BaseCharacterScene;

	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void AddCharacter(Character character)
	{
		var characerScene = BaseCharacterScene.Instantiate();


		ActiveCharacters.Add(character);


	}
}
