using System.ComponentModel.Design;
using Godot;
using Godot.Collections;


public partial class SettingsUI : Control
{
	public OptionButton micSelect;
	public OptionButton OutputSelect;

	bool settingsOpen = false;

	//api keys
	TextEdit ChatGPTtext;
	TextEdit Azuretext;
	TextEdit Labtext;
	SaveManager saveManager;


	[Export]
	AnimationPlayer animationPlayer;
	public override void _Ready()
	{
		saveManager = GetNode<SaveManager>("/root/Data/SaveData");

		micSelect = GetNode<OptionButton>("SettingsSelect/ScrollContainer/HBoxContainer/Audio/MicList");
		OutputSelect = GetNode<OptionButton>("SettingsSelect/ScrollContainer/HBoxContainer/Audio/OutputList");

		ChatGPTtext = GetNode<TextEdit>("SettingsSelect/ScrollContainer/HBoxContainer/API keys/ChatAPI");
		Azuretext = GetNode<TextEdit>("SettingsSelect/ScrollContainer/HBoxContainer/API keys/AzureAPI");
		Labtext = GetNode<TextEdit>("SettingsSelect/ScrollContainer/HBoxContainer/API keys/11labAPI");


		// api keys
		ChatGPTtext.Text = saveManager.GetAPIKey("ChatGPT");
		Azuretext.Text = saveManager.GetAPIKey("Azuir");
		Labtext.Text = saveManager.GetAPIKey("11labs");



	}

	public void SettingPressed()
	{
		if (settingsOpen)
		{
			animationPlayer.Play("CloseSettings");
			settingsOpen = !settingsOpen;
		}
		else
		{
			animationPlayer.Play("OpenSettings");
			settingsOpen = !settingsOpen;
		}

	}


	public void SaveKeys()
	{
		Dictionary data = new()
		{
			{ "ChatGPT", ChatGPTtext.Text },
			{ "Azuir", Azuretext.Text },
			{ "11labs", Labtext.Text }
		};

		saveManager.SaveAPIKeys(data);
	}

	void ScrollNextSettings()
	{
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(GetNode<ScrollContainer>("SettingsSelect/ScrollContainer"), "scroll_horizontal", GetNode<ScrollContainer>("SettingsSelect/ScrollContainer").ScrollHorizontal + 357, 0.1f).SetTrans(Tween.TransitionType.Cubic);
		//GetNode<ScrollContainer>("SettingsSelect/ScrollContainer").ScrollHorizontal += 357;
	}
	void ScrollPreviousSettings()
	{
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(GetNode<ScrollContainer>("SettingsSelect/ScrollContainer"), "scroll_horizontal", GetNode<ScrollContainer>("SettingsSelect/ScrollContainer").ScrollHorizontal - 357, 0.1f).SetTrans(Tween.TransitionType.Cubic);
	}






}
