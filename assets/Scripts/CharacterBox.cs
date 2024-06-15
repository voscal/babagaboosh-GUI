using Godot;
using System;

public partial class CharacterBox : Panel
{
	public string chrName;

	public void LoadBttnClicked()
	{
		GetNode<SaveManager>("/root/Data/SaveData").LoadCharacter(chrName);
	}
}
