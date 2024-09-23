
using Godot;
using Godot.Collections;


public partial class CharacterData : Node
{
	UI ui;

	public override void _Ready()
	{
		ui = GetNode<UI>("/root/Main Scene/UI");


	}


	public void AutoPopulateData(Dictionary data)
	{
		GD.Print("Load Data");
		ui.GetNode<TextEdit>("EditorUI/BG/About/Background/Panel/AIname").Text = (string)data["Name"];
		ui.GetNode<TextEdit>("EditorUI/BG/About/Background/Panel/AIabout").Text = (string)data["Description"];
		ui.GetNode<TextEdit>("EditorUI/AIContext/Pannle/Panel/AIcontext").Text = (string)data["AIContext"];
		GetNode<Node2D>("/root/Main Scene/Puppet/Character/Head").Position = StringToVector2((string)data["HeadPosition"]);
		GetNode<TextureButton>("/root/Main Scene/Puppet/Character/Head/Sprite").Size = StringToVector2((string)data["HeadSize"]);
		GetNode<Node2D>("/root/Main Scene/Puppet/Character/Body").Position = StringToVector2((string)data["BodyPosition"]);
		GetNode<TextureButton>("/root/Main Scene/Puppet/Character/Body/Sprite").Size = StringToVector2((string)data["BodySize"]);
		GetNode<MasterScript>("/root/Main Scene").services.elevinLabs.currentVoice = (string)data["VoiceID"];
		ui.GetNode<Slider>("EditorUI/BG/Voice Config/Background/Panel/Stability").Value = (float)data["Stability"];
		ui.GetNode<Slider>("EditorUI/BG/Voice Config/Background/Panel/Clarity").Value = (float)data["Clarity"];
		ui.GetNode<Slider>("EditorUI/BG/Voice Config/Background/Panel/Exaggeration").Value = (float)data["Exaggeration"];
		GetNode<NotificationsManager>("/root/Managers/Notification").NewNotification("info", "[center]Character Loaded", "[center]Your character have successfully been loaded", 6);
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
