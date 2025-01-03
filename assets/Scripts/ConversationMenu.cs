using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
public partial class ConversationMenu : Control
{
	bool CharactersOpen = false;
	SaveData saveData;
	Manager manager;

	AnimationPlayer animationPlayer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		saveData = GetNode<SaveData>("/root/Data/SaveData");
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		RefreshConversationList(saveData.LoadAllConversations());
		manager = GetNode<Manager>("/root/Managers");


	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	void OpenMenu()
	{
		if (CharactersOpen)
		{
			animationPlayer.Play("Close");
			CharactersOpen = !CharactersOpen;
		}
		else
		{
			animationPlayer.Play("Open");
			CharactersOpen = !CharactersOpen;
		}

	}
	void NewConversationButton()
	{
		GetNode<AnimationPlayer>("Conversation Maker/AnimationPlayer").Play("OpenMenu");
	}

	public void EditConversation(string name, string start, string end)
	{
		GetNode<AnimationPlayer>("Conversation Maker/AnimationPlayer").Play("OpenMenu");
		GetNode<TextEdit>("Conversation Maker/Pannle/Panel/Name").Text = name;
		GetNode<TextEdit>("Conversation Maker/Pannle/Panel/Starting Prompt").Text = start;
		GetNode<TextEdit>("Conversation Maker/Pannle/Panel/Ending Propmt").Text = end;
	}

	void CreateConversation()
	{
		saveData.SaveConversation();
		RefreshConversationList(saveData.LoadAllConversations());
		CloseConvoMaker();
	}
	void CloseConvoMaker()
	{
		GetNode<AnimationPlayer>("Conversation Maker/AnimationPlayer").Play("CloseMenu");
	}


	public void RefreshConversationList(Godot.Collections.Dictionary<string, string>[] conversations)
	{
		// Get the VBoxContainer node
		VBoxContainer vBox = GetNode<VBoxContainer>("BackGround/ScrollContainer/VBoxContainer");

		// Remove all existing children (ConversationBox instances)
		foreach (ConversationBox conversationBox in vBox.GetChildren())
		{
			conversationBox.QueueFree();
		}

		// Load the ConversationBox scene
		var scene = GD.Load<PackedScene>("res://assets/Scenes/ConversationBox.tscn");

		// Add new ConversationBox instances for each conversation
		foreach (var conversation in conversations)
		{
			var sceneInstance = scene.Instantiate<ConversationBox>();

			// Set data for the ConversationBox instance
			if (conversation.ContainsKey("name"))
			{
				sceneInstance.Name = conversation["name"];
				sceneInstance.name = conversation["name"];
			}
			if (conversation.ContainsKey("start_prompt"))
			{
				sceneInstance.startPrompt = conversation["start_prompt"]; // Assuming ConversationBox has startPrompt property
			}
			if (conversation.ContainsKey("end_prompt"))
			{
				sceneInstance.endPrompt = conversation["end_prompt"]; // Assuming ConversationBox has endPrompt property
			}

			// Update the label text
			sceneInstance.GetNode<Label>("Label").Text = sceneInstance.name;

			// Add the ConversationBox instance to the VBoxContainer
			vBox.AddChild(sceneInstance);
		}
	}


	void ConversationToggle(bool state)
	{
		if (GetNode<CheckButton>("BackGround/Settings/CheckButton").ButtonPressed == true)
		{
			if (manager.character.ActiveCharacters.Count < 2)
			{
				GetNode<CheckButton>("BackGround/Settings/CheckButton").ButtonPressed = false;
				GetNode<NotificationsManager>("/root/Managers/Notification").NewNotification("error", "[center]Conversation", "[center]you must have 2 characters to start a conversation", 5);
				return;
			}
			manager.conversation.ReadyConversation();

		}
		else
		{
			manager.conversation.StopConversation();
		}

		GetParent().GetNode<Button>("CoreUI/StartConvo").Visible = state;
		manager.conversation.isGroupConversation = state;
	}



}
