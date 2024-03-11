
using Godot;
using Godot.Collections;

public partial class SaveManager : Node
{

	string path = "user://data.json";
	string KeysPath = "user://APIKEYS.json";


	public override void _Ready()
	{


	}
	/*

	#region genral data
	void WriteSave(Dictionary content)
	{
		var file = FileAccess.Open(path, FileAccess.ModeFlags.Write);
		file.StoreString(Json.Stringify(content));
		file.Close();
		file = null;
	}

	Dictionary ReadSave()
	{
		var file = FileAccess.Open(path, FileAccess.ModeFlags.Read);
		var content = Json.ParseString(file.GetAsText());
		return (Dictionary)content;
	}

	void CreateNewSaveFile()
	{
		var file = FileAccess.Open(path, FileAccess.ModeFlags.Read);
		var content = Json.ParseString(file.GetAsText());
		data = (Dictionary)content;
		WriteSave((Dictionary)content);
	}


	#endregion
*/
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


}
