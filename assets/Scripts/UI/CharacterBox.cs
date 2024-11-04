using Godot;
using System;

public partial class CharacterBox : Panel
{
	public string chrName;

	public void LoadBttnClicked()
	{
		GetNode<SaveData>("/root/Data/SaveData").LoadCharacterFromUserFolder(chrName);
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
