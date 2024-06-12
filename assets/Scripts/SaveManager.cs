
using System;
using Godot;
using Godot.Collections;


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
		var file = FileAccess.Open(KeysPath, FileAccess.ModeFlags.Write);
		file.StoreString(dataStr);
		file.Close();


	}

	public string GetAPIKey(string value)
	{
		Json json = new();
		var file = FileAccess.Open(KeysPath, FileAccess.ModeFlags.Read);

		Error error = json.Parse(file.GetAsText());

		if (error != Error.Ok)
		{
			GD.PrintErr("Fail to load json");
			return null;
		}
		Dictionary data = (Dictionary)json.Data;
		return (string)data[value];
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
		var file = FileAccess.Open(KeysPath, FileAccess.ModeFlags.Write);
		file.StoreString(dataStr);
		file.Close();
	}

	#endregion




	#region  Character Loading



	public void SaveCharacter(string path)
	{
		ZipPacker zipPacker = new();
		Error error = zipPacker.Open(CharactersPath + ui.GetNode<TextEdit>("EditorUI/BG/About/Background/Panel/AIname").Text + ".chr");
		if (error != Error.Ok)
		{
			GD.PushError($"Couldn't open path for saving ZIP archive." + error.ToString());
			return;
		}


		zipPacker.StartFile("HeadSpr.png");
		zipPacker.WriteFile(GetNode<TextureButton>("/root/Main Scene/Puppet/Character/Head/Sprite").TextureNormal.GetImage().SavePngToBuffer());
		zipPacker.CloseFile();
		zipPacker.StartFile("BodySpr.png");
		zipPacker.WriteFile(GetNode<TextureButton>("/root/Main Scene/Puppet/Character/Body/Sprite").TextureNormal.GetImage().SavePngToBuffer());
		zipPacker.CloseFile();
		zipPacker.StartFile("Data.json");
		zipPacker.WriteFile(CreateCharacterJSON().ToUtf8Buffer());
		zipPacker.CloseFile();
		zipPacker.Close();
	}

	public string CreateCharacterJSON()
	{
		Dictionary data = new()
		{
			{ "Name", ui.GetNode<TextEdit>("EditorUI/BG/About/Background/Panel/AIname").Text},
			{ "Discription", ui.GetNode<TextEdit>("EditorUI/BG/About/Background/Panel/AIabout").Text},
			{ "AIContext", ui.GetNode<TextEdit>("EditorUI/AIContext/Pannle/Panel/AIcontext").Text},
			
			//head
			{ "HeadPosition", GetNode<Node2D>("/root/Main Scene/Puppet/Character/Head").Position},
			{ "HeadSize", GetNode<TextureButton>("/root/Main Scene/Puppet/Character/Head/Sprite").Size},

			//body
			{ "BodyPosition", GetNode<Node2D>("/root/Main Scene/Puppet/Character/Body").Position },
			{ "BodySize", GetNode<TextureButton>("/root/Main Scene/Puppet/Character/Body/Sprite").Size},
			
			//voice
			{ "VoiceID", GetNode<MasterScript>("/root/Main Scene").Libraries.elevinLabs.currentVoice},
			{ "Stability", ui.editorUI.GetVoiceSettings().Stability},
			{ "Clarity", ui.editorUI.GetVoiceSettings().SimilarityBoost},
			{ "Exaggeration", ui.editorUI.GetVoiceSettings().Style},
			{ "temperature", "" },
		};

		var dataStr = Json.Stringify(data);
		return dataStr;

	}


	public void LoadCharacter(string path)
	{
		ZipReader zipReader = new();
		zipReader.Open(path);


		// LOAD BODY SPRITE
		byte[] data = zipReader.ReadFile("BodySpr.png");
		Image image = new();
		Error error = image.LoadPngFromBuffer(data);
		if (error != Error.Ok)
			GD.Print("Unable to load Body");
		else
			GetNode<TextureButton>("/root/Main Scene/Puppet/Character/Body/Sprite").TextureNormal = ImageTexture.CreateFromImage(image);

		//LOAD HEAD SPRITE
		data = zipReader.ReadFile("HeadSpr.png");
		image = new();
		error = image.LoadPngFromBuffer(data);
		if (error != Error.Ok)
			GD.Print("Unable to load Body");
		else
			GetNode<TextureButton>("/root/Main Scene/Puppet/Character/Head/Sprite").TextureNormal = ImageTexture.CreateFromImage(image);

		//load Data

		data = zipReader.ReadFile("Data.json");

		// Convert bytes to variant
		string jsonData = System.Text.Encoding.UTF8.GetString(data);

		// Parse JSON data
		Json json = new();
		error = json.Parse(jsonData.ToString());
		// Check for parsing errors
		if (error != Error.Ok)
		{
			GD.PrintErr("Failed to load json Data");
			return;
		}


		// Extract parsed data as dictionary
		Dictionary Jdata = (Dictionary)json.Data;
		GetNode<CharacterData>("/root/Data/CharacterData").AutoPopulateData(Jdata);





	}






	#endregion

}
