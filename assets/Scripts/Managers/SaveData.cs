using Godot;
using Godot.Collections;
using System.Text;
using ElevenLabs.Voices;
public partial class SaveData : Node
{
	string path = "user://data.json";
	string KeysPath = "user://APIKEYS.json";
	string CharactersPath = "user://Characters/";



	UI ui;

	public override void _Ready()
	{
		ui = GetNode<UI>("/root/Main Window/UI");
	}

	#region KEY data

	public void SaveAPIKeys(Dictionary data)
	{
		var dataStr = Json.Stringify(data);
		using (var file = FileAccess.Open(KeysPath, FileAccess.ModeFlags.Write))
		{
			if (file == null)
			{
				GD.PrintErr("Failed to open file for writing: " + KeysPath);
				return;
			}
			file.StoreString(dataStr);
		}
	}

	public string GetAPIKey(string value)
	{
		Json json = new();
		using (var file = FileAccess.Open(KeysPath, FileAccess.ModeFlags.Read))
		{
			if (file == null)
			{
				GD.PrintErr("Failed to open file for reading: " + KeysPath);
				return null;
			}
			Error error = json.Parse(file.GetAsText());

			if (error != Error.Ok)
			{
				GD.PrintErr("Failed to parse JSON");
				return null;
			}
			Dictionary data = (Dictionary)json.Data;
			return (string)data[value];
		}
	}

	void CreateNewAPIKeyFile()
	{
		Dictionary data = new()
		{
			{ "ChatGPT", "" },
			{ "Azuir", "" },
			{ "11labs", "" },
			{"AZReigon", ""}
		};
		var dataStr = Json.Stringify(data);
		using (var file = FileAccess.Open(KeysPath, FileAccess.ModeFlags.Write))
		{
			if (file == null)
			{
				GD.PrintErr("Failed to open file for writing: " + KeysPath);
				return;
			}
			file.StoreString(dataStr);
		}
	}

	#endregion

	#region Character Loading

	public void SaveCharacter()
	{
		if (ui.GetNode<TextEdit>("EditorUI/BG/About/Background/Panel/AIname").Text == string.Empty)
		{
			GetNode<NotificationsManager>("/root/Managers/Notification").NewNotification("Error", "[center]Error", "[center]Please give your character a name", 5);
			return;
		}


		string characterName = ui.GetNode<TextEdit>("EditorUI/BG/About/Background/Panel/AIname").Text;
		string characterPath = $"user://Characters/{characterName}.chr";

		// Ensure the directory exists
		if (!DirExists(CharactersPath))
		{
			GD.Print("Creating directory: " + CharactersPath);
			DirCreate(CharactersPath);
		}

		// Logging the character path
		GD.Print("Saving character to path: " + characterPath);

		ZipPacker zipPacker = new();
		Error error = zipPacker.Open(characterPath);
		if (error != Error.Ok)
		{
			GD.PushError($"Couldn't open path for saving ZIP archive: {error.ToString()}");
			return;
		}

		zipPacker.StartFile("HeadSpr.png");
		zipPacker.WriteFile(GetNode<TextureButton>("/root/Main Window/Puppet/Character/Head/Sprite").TextureNormal.GetImage().SavePngToBuffer());
		zipPacker.CloseFile();

		zipPacker.StartFile("BodySpr.png");
		zipPacker.WriteFile(GetNode<TextureButton>("/root/Main Window/Puppet/Character/Body/Sprite").TextureNormal.GetImage().SavePngToBuffer());
		zipPacker.CloseFile();

		zipPacker.StartFile("Data.json");
		zipPacker.WriteFile(Encoding.UTF8.GetBytes(CreateCharacterJSON()));
		zipPacker.CloseFile();

		zipPacker.Close();
		GetNode<NotificationsManager>("/root/Managers/Notification").NewNotification("info", "[center]Character Saved", "[center]" + ui.GetNode<TextEdit>("EditorUI/BG/About/Background/Panel/AIname").Text + " has successfully saved!", 6);
		ui.GetNode<CharacterSelect>("Character Select").RefreshCharactersList(LoadAllCharacters());

	}

	public void ExportCharacter(string exportPath, string charName)
	{
		// Ensure the exported file has a .chr extension
		if (!exportPath.EndsWith(".chr"))
		{
			exportPath += ".chr";
		}

		string characterPath = $"user://Characters/{charName}";

		if (!FileExists(characterPath))
		{
			GD.PrintErr("Character file does not exist: " + characterPath);
			return;
		}

		// Copy character file to export path
		using (var sourceFile = FileAccess.Open(characterPath, FileAccess.ModeFlags.Read))
		{
			if (sourceFile == null)
			{
				GD.PrintErr("Failed to open character file for reading: " + characterPath);
				return;
			}

			using (var destFile = FileAccess.Open(exportPath, FileAccess.ModeFlags.Write))
			{
				if (destFile == null)
				{
					GD.PrintErr("Failed to open export file for writing: " + exportPath);
					return;
				}

				destFile.StoreBuffer(sourceFile.GetBuffer((int)sourceFile.GetLength()));
			}
		}

		GD.Print("Character exported to: " + exportPath);
		GetNode<NotificationsManager>("/root/Managers/Notification").NewNotification("info", "[center]Character Exported", "[center]" + charName + " has been successfully exported!", 6);
	}



	public bool DirExists(string path)
	{
		var dir = DirAccess.Open(path);
		return dir != null;
	}

	public void DirCreate(string path)
	{
		var dir = DirAccess.Open("user://");
		if (dir != null)
		{
			dir.MakeDirRecursive(path);
		}
	}

	public string CreateCharacterJSON()
	{
		Dictionary data = new()
		{
			{ "Name", ui.GetNode<TextEdit>("EditorUI/BG/About/Background/Panel/AIname").Text },
			{ "Description", ui.GetNode<TextEdit>("EditorUI/BG/About/Background/Panel/AIabout").Text },
			{ "AIContext", ui.GetNode<TextEdit>("EditorUI/AIContext/Pannle/Panel/AIcontext").Text },
			{ "HeadPosition", GetNode<Node2D>("/root/Main Window/Puppet/Character/Head").Position },
			{ "HeadSize", GetNode<TextureButton>("/root/Main Window/Puppet/Character/Head/Sprite").Size },
			{ "BodyPosition", GetNode<Node2D>("/root/Main Window/Puppet/Character/Body").Position },
			{ "BodySize", GetNode<TextureButton>("/root/Main Window/Puppet/Character/Body/Sprite").Size },
			{ "VoiceID", GetNode<MasterScript>("/root/Main Window").services.elevinLabs.currentVoice },
			{ "Stability", ui.editorUI.GetVoiceSettings().Stability },
			{ "Clarity", ui.editorUI.GetVoiceSettings().SimilarityBoost },
			{ "Exaggeration", ui.editorUI.GetVoiceSettings().Style },
			{ "temperature", "" }
		};

		return Json.Stringify(data);
	}


	public Character LoadCharacterFromUserFolder(string chrName)
	{
		return LoadCharacterFromFile("user://Characters/" + chrName);
	}

	public Character LoadCharacterFromFile(string path)
	{
		ZipReader zipReader = new();
		Error error = zipReader.Open(path);

		if (error != Error.Ok)
		{
			GD.PrintErr("Failed to open ZIP archive: " + path);
			return null;
		}


		Character character = new Character();

		// Load Body Sprite
		byte[] data = zipReader.ReadFile("BodySpr.png");
		if (data == null)
		{
			GD.PrintErr("Failed to read BodySpr.png from ZIP archive.");
		}
		else
		{
			Image image = new();
			error = image.LoadPngFromBuffer(data);
			if (error != Error.Ok)
			{
				GD.PrintErr("Unable to load Body");
			}
			else
			{
				character.image1 = ImageTexture.CreateFromImage(image);
			}
		}

		// Load Head Sprite
		data = zipReader.ReadFile("HeadSpr.png");
		if (data == null)
		{
			GD.PrintErr("Failed to read HeadSpr.png from ZIP archive.");
		}
		else
		{
			Image image = new();
			error = image.LoadPngFromBuffer(data);
			if (error != Error.Ok)
			{
				GD.PrintErr("Unable to load Head");
			}
			else
			{
				character.image2 = ImageTexture.CreateFromImage(image);
			}
		}

		// Load Data
		data = zipReader.ReadFile("Data.json");
		if (data == null)
		{
			GD.PrintErr("Failed to read Data.json from ZIP archive.");
			return null;
		}

		string jsonData = Encoding.UTF8.GetString(data);
		Json json = new();
		error = json.Parse(jsonData);
		if (error != Error.Ok)
		{
			GD.PrintErr("Failed to parse JSON data.");
			return null;
		}

		Dictionary jsonDictionary = (Dictionary)json.Data;
		//GetNode<CharacterData>("/root/Data/CharacterData").AutoPopulateData(jsonDictionary);

		character.name = (string)jsonDictionary["Name"];
		character.description = (string)jsonDictionary["Description"];
		character.context = (string)jsonDictionary["AIContext"];
		character.headPosition = StringToVector2((string)jsonDictionary["HeadPosition"]);
		character.headSize = StringToVector2((string)jsonDictionary["HeadSize"]);
		character.bodyPosition = StringToVector2((string)jsonDictionary["BodyPosition"]);
		character.bodySize = StringToVector2((string)jsonDictionary["BodySize"]);
		character.voiceID = (string)jsonDictionary["VoiceID"];
		character.voiceSettings = new();
		character.voiceSettings.Stability = (float)jsonDictionary["Stability"];
		character.voiceSettings.SimilarityBoost = (float)jsonDictionary["Clarity"];
		character.voiceSettings.Style = (float)jsonDictionary["Exaggeration"];

		return character;
	}


	public string[] LoadAllCharacters()
	{
		// Ensure the directory exists
		if (!DirExists(CharactersPath))
		{
			GD.Print("Creating directory: " + CharactersPath);
			DirCreate(CharactersPath);
		}

		string[] characters = DirAccess.GetFilesAt(CharactersPath);
		foreach (string character in characters)
		{
			GD.Print(character);

		}
		return characters;
	}

	public void DeleteCharacter(string chrName)
	{
		string characterPath = $"user://Characters/{chrName}";

		if (!FileExists(characterPath))
		{
			GD.PrintErr("Character file does not exist: " + characterPath);
			return;
		}

		var dir = DirAccess.Open("user://Characters/");
		if (dir == null)
		{
			GD.PrintErr("Failed to open directory: user://Characters/");
			return;
		}

		Error error = dir.Remove(characterPath);
		if (error != Error.Ok)
		{
			GD.PrintErr("Failed to delete character file: " + characterPath + " Error: " + error.ToString());
			return;
		}

		GD.Print("Successfully deleted character: " + chrName);
		ui.GetNode<CharacterSelect>("Character Select").RefreshCharactersList(LoadAllCharacters());
	}
	#endregion
	public bool FileExists(string path)
	{
		var file = FileAccess.Open(path, FileAccess.ModeFlags.Read);
		if (file != null)
		{
			file.Close();
			return true;
		}
		return false;
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
