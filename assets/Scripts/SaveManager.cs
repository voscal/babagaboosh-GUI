
using Godot;
using Godot.Collections;

public partial class SaveManager : Node
{

	string path = "user://data.json";
	string KeysPath = "user://APIKEYS.json";



	public override void _Ready()
	{
		GD.Print(GetNode<TextureButton>("/root/Main Scene/Puppet/Character/Head/Sprite").Name);
		SaveCharacter("");

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
		Error error = zipPacker.Open(path + ".chr");
		if (error != Error.Ok)
		{
			GD.PushError($"Couldn't open path for saving ZIP archive." + error.ToString());
			return;
		}


		zipPacker.StartFile("headSpr.png");
		zipPacker.WriteFile(GetNode<TextureButton>("/root/Main Scene/Puppet/Character/Head/Sprite").TextureNormal.GetImage().SavePngToBuffer());
		zipPacker.CloseFile();
		zipPacker.StartFile("BodySpr.png");
		zipPacker.WriteFile(GetNode<TextureButton>("/root/Main Scene/Puppet/Character/Body/Sprite").TextureNormal.GetImage().SavePngToBuffer());
		zipPacker.CloseFile();
		zipPacker.StartFile("data.xml");
		zipPacker.CloseFile();
	}






	#endregion

}
