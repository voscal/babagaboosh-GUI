using System;
using System.Collections.Generic;
using System.Data;
using Godot;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;


public partial class CharacterManager : Manager
{
	private Manager manager;
	public List<Character> ActiveCharacters = new();
	public int focusedCharacter;

	public UI ui;

	[Export]
	public PackedScene BaseCharacterScene;

	[Export]
	public PackedScene BaseCharacterControlScene;
	public Services services;
	public override void _Ready()
	{
		ui = GetNode<UI>("/root/Main Window/UI");
		manager = GetNode<Manager>("/root/Managers");
		services = GetNode<Services>("/root/Services");
	}

	public void AddCharacter(Character character)
	{
		var characerScene = BaseCharacterScene.Instantiate();
		//create and update a new viewport
		characerScene.GetNode<Control>("Viewport/Dummy/Head").Position = character.headPosition;
		characerScene.GetNode<Control>("Viewport/Dummy/Head").Size = character.headSize;
		characerScene.GetNode<TextureRect>("Viewport/Dummy/Head/Sprite").Texture = character.image2;
		characerScene.GetNode<Control>("Viewport/Dummy/Body").Position = character.bodyPosition;
		characerScene.GetNode<Control>("Viewport/Dummy/Body").Size = character.bodySize;
		characerScene.GetNode<TextureRect>("Viewport/Dummy/Body/Sprite").Texture = character.image1;
		character.chat = services.chatGPT.CreateConversation(character);
		ActiveCharacters.Add(character);
		characerScene.Name = ActiveCharacters.IndexOf(character) + " " + character.name;
		character.path = $"/root/Main Window/CharacterView/{characerScene.Name}";
		character.audioSpectrum = manager.audio.NewCharacterBus(character.path);


		characerScene.GetNode<AudioStreamPlayer>("Viewport/Dummy/AudioPlayer").Bus = character.path;
		characerScene.GetNode<Dummy>("Viewport/Dummy").audioSpectrum = character.audioSpectrum;
		manager.view.AddChild(characerScene);
		GD.Print(ActiveCharacters[ActiveCharacters.IndexOf(character)].path);
		SetFocus(ActiveCharacters.Count - 1);

		var controlScene = BaseCharacterControlScene.Instantiate();
		controlScene.Set("id", ActiveCharacters.IndexOf(character));
		controlScene.Name = ActiveCharacters.IndexOf(character) + character.name;
		controlScene.GetNode<TextureRect>("Profile/TextureRect").Texture = GetNode<SubViewport>(character.path + "/Viewport").GetTexture();
		ui.GetNode<BoxContainer>("Character Select/BackGround/ScrollContainer/VBoxContainer/Volume/BoxContainer2").AddChild(controlScene);



	}
	public async void RemoveCharacter(int index)
	{
		GD.Print($"Removing character at index {index}. Active characters count: {ActiveCharacters.Count}");
		if (ActiveCharacters.Count == 1)
		{
			GetNode<NotificationsManager>("/root/Managers/Notification").NewNotification("error", "[center]Unable to Remove", "[center]You must have at least one character enabled", 5);
			return;
		}
		if (manager.conversation.isGroupConversation == true && ActiveCharacters.Count == 2)
		{
			GetNode<NotificationsManager>("/root/Managers/Notification").NewNotification("error", "[center]Unable to Remove", "[center]You must have at least two characters for a conversation", 5);
			return;
		}
		var character = ActiveCharacters[index];

		//we rename and them .QueueFree() due to godot not being able to .QueueFree() it in time for the rename :( skill issue
		GetNode<SubViewportContainer>(character.path).Name = "REMOVE";
		GetNode<SubViewportContainer>("/root/Main Window/CharacterView/REMOVE").QueueFree();
		ui.GetNode<Panel>($"Character Select/BackGround/ScrollContainer/VBoxContainer/Volume/BoxContainer2/{ActiveCharacters.IndexOf(character) + character.name}").Name = "REMOVE";
		ui.GetNode<Panel>($"Character Select/BackGround/ScrollContainer/VBoxContainer/Volume/BoxContainer2/REMOVE").QueueFree();
		ActiveCharacters.RemoveAt(index);
		manager.audio.RemoveCharacterBus(character.path);
		foreach (var activeCharacter in ActiveCharacters)
		{

			var oldCharacterPath = activeCharacter.path;
			var oldCharacterindex = (index <= ActiveCharacters.IndexOf(activeCharacter)) ? (ActiveCharacters.IndexOf(activeCharacter) + 1).ToString() : ActiveCharacters.IndexOf(activeCharacter).ToString();
			var newCharacterIndex = ActiveCharacters.IndexOf(activeCharacter);


			GetNode<CharacterViewport>(activeCharacter.path).CallDeferred("set_name", newCharacterIndex + " " + activeCharacter.name);
			activeCharacter.path = $"/root/Main Window/CharacterView/{newCharacterIndex} {activeCharacter.name}";
			manager.audio.UpdateCharacterBus(oldCharacterPath, activeCharacter.path);
			GD.Print(ActiveCharacters[newCharacterIndex].path);


			var controlScene = ui.GetNode<CharacterControl>($"Character Select/BackGround/ScrollContainer/VBoxContainer/Volume/BoxContainer2/{oldCharacterindex + activeCharacter.name}");
			controlScene.id = newCharacterIndex;
			controlScene.Name = newCharacterIndex + activeCharacter.name;
			controlScene.GetNode<Label>("Id").Text = newCharacterIndex.ToString();

		}



		SetFocus();

	}

	void CallDeferredName(Node node, String name)
	{
		GD.Print("UPDATENAME" + name);
		node.Name = name;

	}

	public void UpdateCharacter(int oldIndex, Character newCharacter)
	{
		var oldCharacter = manager.character.ActiveCharacters[oldIndex];
		UpdateCharacter(oldCharacter, newCharacter);
	}
	public void UpdateCharacter(Character oldCharacter, Character newCharacter)
	{
		var characerScene = GetNode<SubViewportContainer>(oldCharacter.path);

		characerScene.GetNode<Prop>("Viewport/Dummy/Head").Position = newCharacter.headPosition;
		characerScene.GetNode<Prop>("Viewport/Dummy/Head").Size = newCharacter.headSize;
		characerScene.GetNode<TextureRect>("Viewport/Dummy/Head/Sprite").Texture = newCharacter.image2;
		characerScene.GetNode<Prop>("Viewport/Dummy/Body").Position = newCharacter.bodyPosition;
		characerScene.GetNode<Prop>("Viewport/Dummy/Body").Size = newCharacter.bodySize;
		characerScene.GetNode<TextureRect>("Viewport/Dummy/Body/Sprite").Texture = newCharacter.image1;
		ActiveCharacters[ActiveCharacters.IndexOf(oldCharacter)] = newCharacter;
		characerScene.Name = ActiveCharacters.IndexOf(newCharacter) + " " + newCharacter.name;
		newCharacter.path = $"/root/Main Window/CharacterView/{characerScene.Name}";
		manager.audio.UpdateCharacterBus(oldCharacter.path, newCharacter.path);
		characerScene.GetNode<AudioStreamPlayer>("Viewport/Dummy/AudioPlayer").Bus = newCharacter.path;
		newCharacter.chat = services.chatGPT.CreateConversation(newCharacter);



		var controlScene = ui.GetNode<CharacterControl>($"Character Select/BackGround/ScrollContainer/VBoxContainer/Volume/BoxContainer2/{ActiveCharacters.IndexOf(oldCharacter) + 1 + oldCharacter.name}");
		controlScene.id = ActiveCharacters.IndexOf(newCharacter);
		controlScene.Name = ActiveCharacters.IndexOf(newCharacter) + newCharacter.name;
		controlScene.GetNode<Label>("Id").Text = ActiveCharacters.IndexOf(newCharacter).ToString();
		SetFocus(ActiveCharacters.IndexOf(newCharacter));
	}

	public void SetFocus(int index)
	{
		GD.Print("HOCUS FOCUS");
		if (focusedCharacter < ActiveCharacters.Count)
		{
			GetNode<CharacterViewport>(manager.character.ActiveCharacters[focusedCharacter].path).focused = false;
			GetNode<CharacterViewport>(manager.character.ActiveCharacters[focusedCharacter].path).ZIndex = 0;
			GetNode<CharacterViewport>(manager.character.ActiveCharacters[focusedCharacter].path).MouseFilter = Control.MouseFilterEnum.Ignore;
		}
		var character = manager.character.ActiveCharacters[index];
		focusedCharacter = index;
		GetNode<Control>("/root/Main Window/CharacterView").Size = GetNode<SubViewportContainer>(character.path).Size;
		GetNode<CharacterViewport>(character.path).focused = true;
		GetNode<CharacterViewport>(character.path).ZIndex = 1;
		GetNode<CharacterViewport>(character.path).MouseFilter = Control.MouseFilterEnum.Stop;
		ui.editorUI.UpdateEditor(character);

	}
	public void SetFocus()
	{
		SetFocus(manager.character.ActiveCharacters.Count - 1);
	}



	public void UpdateChild()
	{
		//updates the child nodes


	}

	public void EditorOpened()
	{

	}

	public Character GetFocusedCharacter()
	{
		return ActiveCharacters[focusedCharacter];
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
