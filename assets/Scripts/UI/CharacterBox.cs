using Godot;
using System;

public partial class CharacterBox : Panel
{
	public string chrName;

	public void LoadBttnClicked()
	{

		var character = GetNode<SaveData>("/root/Data/SaveData").LoadCharacterFromUserFolder(chrName);
		GetNode<Manager>("/root/Managers").character.UpdateCharacter(GetNode<Manager>("/root/Managers").character.focusedCharacter, character);
		//GetNode<Manager>("/root/Managers").character.AddCharacter(character);
		//GetNode<Manager>("/root/Managers").character.SetFocus(character);

	}
	public void moreBttnclicked()
	{
		GetParent().GetParent().GetParent().GetParent<CharacterSelect>().MoreButtonPressed(Name);
	}

	public void ExportPressed(string path)
	{
		GetNode<SaveData>("/root/Data/SaveData").ExportCharacter(path, chrName);
	}


	public void deletepressed()
	{
		GetNode<SaveData>("/root/Data/SaveData").DeleteCharacter(chrName);
	}
}
