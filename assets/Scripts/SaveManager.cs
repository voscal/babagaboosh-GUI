
using Godot;
using Godot.Collections;

public partial class SaveManager : Node
{

	string path = "user://data.json";
	string KeysPath = "user://APIKEYS.json";


	public override void _Ready()
	{


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









	#endregion

}
