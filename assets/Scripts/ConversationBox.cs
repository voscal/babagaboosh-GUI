using Godot;
using System;

public partial class ConversationBox : Button
{
	public string name;
	[Export]
	public string startPrompt;
	[Export]
	public string endPrompt;
	public override void _Ready()
	{
		GetNode<Label>("Label").Text = name;

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	void Edit()
	{
		GetParent().GetParent().GetParent().GetParent<ConversationMenu>().EditConversation(name, startPrompt, endPrompt);
	}
	void Remove()
	{
		GetNode<SaveData>("/root/Data/SaveData").RemoveConversation(name);
		GetParent().GetParent().GetParent().GetParent<ConversationMenu>().RefreshConversationList(GetNode<SaveData>("/root/Data/SaveData").LoadAllConversations());

	}
	void pressed()
	{
		var convManager = GetNode<ConversationManager>("/root/Managers/Conversation");
		convManager.ExplnationIntro = startPrompt;
		convManager.ExplnationOutro = endPrompt;
		convManager.ReadyConversation();
	}
}
