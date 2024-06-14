using Godot;
using Godot.Collections;
using System.Linq;
using System.Text;

public partial class SaveManager : Node
{
	string path = "user://data.json";
	string KeysPath = "user://APIKEYS.json";
	string CharactersPath = "user://Characters/";

	UI ui;

	public override void _Ready()
	{
		ui = GetNode<UI>("/root/Main Scene/UI");
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
			{ "11labs", "" }
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
			GetNode<NotificationsManager>("/root/Managers/Notifications").NewNotification("Error", "[center]Error", "[center]Please give your character a name", 6);
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
		zipPacker.WriteFile(GetNode<TextureButton>("/root/Main Scene/Puppet/Character/Head/Sprite").TextureNormal.GetImage().SavePngToBuffer());
		zipPacker.CloseFile();

		zipPacker.StartFile("BodySpr.png");
		zipPacker.WriteFile(GetNode<TextureButton>("/root/Main Scene/Puppet/Character/Body/Sprite").TextureNormal.GetImage().SavePngToBuffer());
		zipPacker.CloseFile();

		zipPacker.StartFile("Data.json");
		zipPacker.WriteFile(Encoding.UTF8.GetBytes(CreateCharacterJSON()));
		zipPacker.CloseFile();

		zipPacker.Close();
	}

	private bool DirExists(string path)
	{
		var dir = DirAccess.Open(path);
		return dir != null;
	}

	private void DirCreate(string path)
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
			{ "HeadPosition", GetNode<Node2D>("/root/Main Scene/Puppet/Character/Head").Position },
			{ "HeadSize", GetNode<TextureButton>("/root/Main Scene/Puppet/Character/Head/Sprite").Size },
			{ "BodyPosition", GetNode<Node2D>("/root/Main Scene/Puppet/Character/Body").Position },
			{ "BodySize", GetNode<TextureButton>("/root/Main Scene/Puppet/Character/Body/Sprite").Size },
			{ "VoiceID", GetNode<MasterScript>("/root/Main Scene").Libraries.elevinLabs.currentVoice },
			{ "Stability", ui.editorUI.GetVoiceSettings().Stability },
			{ "Clarity", ui.editorUI.GetVoiceSettings().SimilarityBoost },
			{ "Exaggeration", ui.editorUI.GetVoiceSettings().Style },
			{ "temperature", "" }
		};

		return Json.Stringify(data);
	}

	public void LoadCharacter(string path)
	{
		ZipReader zipReader = new();
		Error error = zipReader.Open(path);

		if (error != Error.Ok)
		{
			GD.PrintErr("Failed to open ZIP archive: " + path);
			return;
		}

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
				GetNode<TextureButton>("/root/Main Scene/Puppet/Character/Body/Sprite").TextureNormal = ImageTexture.CreateFromImage(image);
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
				GetNode<TextureButton>("/root/Main Scene/Puppet/Character/Head/Sprite").TextureNormal = ImageTexture.CreateFromImage(image);
			}
		}

		// Load Data
		data = zipReader.ReadFile("Data.json");
		if (data == null)
		{
			GD.PrintErr("Failed to read Data.json from ZIP archive.");
			return;
		}

		string jsonData = Encoding.UTF8.GetString(data);
		Json json = new();
		error = json.Parse(jsonData);
		if (error != Error.Ok)
		{
			GD.PrintErr("Failed to parse JSON data.");
			return;
		}

		Dictionary jsonDataDict = (Dictionary)json.Data;
		GetNode<CharacterData>("/root/Data/CharacterData").AutoPopulateData(jsonDataDict);
	}

	#endregion
}
