using Godot;
using System;

public partial class CharacterBox : Panel
{
	public string chrName;

	public void LoadBttnClicked()
	{
		GetNode<SaveManager>("/root/Data/SaveData").LoadCharacter(chrName);
	}
	public void moreBttnclicked()
	{
		GetParent().GetParent().GetParent().GetParent<CharacterSelect>().MoreButtonPressed(Name);
	}

	public void deletepressed()
	{
		GetNode<SaveManager>("/root/Data/SaveData").DeleteCharacter(chrName);
	}
}
