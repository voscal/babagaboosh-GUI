using System.ComponentModel.Design;
using System.Net.Security;
using Azure.Core;
using Azure.Core.Pipeline;
using Godot;
using Godot.Collections;


public partial class SettingsUI : Control
{

	Services services;

	Manager manager;

	public OptionButton micSelect;
	public OptionButton OutputSelect;

	bool settingsOpen = false;

	//api keys
	TextEdit ChatGPTtext;
	TextEdit Azuretext;
	TextEdit Labtext;

	TextEdit regionText;
	SaveData saveData;

	[Export]
	AnimationPlayer animationPlayer;
	public override void _Ready()
	{

		saveData = GetNode<SaveData>("/root/Data/SaveData");
		services = GetNode<Services>("/root/Services");
		//micSelect = GetNode<OptionButton>("SettingsSelect/ScrollContainer/HBoxContainer/Audio/MicList");
		//OutputSelect = GetNode<OptionButton>("SettingsSelect/ScrollContainer/HBoxContainer/Audio/OutputList");

		manager = GetNode<Manager>("/root/Managers");



		ChatGPTtext = GetNode<TextEdit>("SettingsSelect/ScrollContainer/GridContainer/Secret/Panel/ChatAPI");
		Azuretext = GetNode<TextEdit>("SettingsSelect/ScrollContainer/GridContainer/Secret/Panel/AzureAPI");
		Labtext = GetNode<TextEdit>("SettingsSelect/ScrollContainer/GridContainer/Secret/Panel/11labAPI");
		regionText = GetNode<TextEdit>("SettingsSelect/ScrollContainer/GridContainer/Secret/Panel/Region Input");

		// api keys
		ChatGPTtext.Text = saveData.GetAPIKey("ChatGPT");
		Azuretext.Text = saveData.GetAPIKey("Azuir");
		Labtext.Text = saveData.GetAPIKey("11labs");
		regionText.Text = saveData.GetAPIKey("AZReigon");

	}
	string currentOption;
	void optionOpen(string name)
	{
		currentOption = name;
		GetNode<AnimationPlayer>($"SettingsSelect/ScrollContainer/GridContainer/{name}/AnimationPlayer").Play("Open");
		foreach (Button button in GetNode<GridContainer>("SettingsSelect/ScrollContainer/GridContainer").GetChildren())
		{
			if (button.Name != name)
				button.Disabled = true;
			button.MouseFilter = MouseFilterEnum.Ignore;
		}
	}
	void optionClose(string name)
	{
		currentOption = null;
		GetNode<AnimationPlayer>($"SettingsSelect/ScrollContainer/GridContainer/{name}/AnimationPlayer").Play("Close");
		foreach (Button button in GetNode<GridContainer>("SettingsSelect/ScrollContainer/GridContainer").GetChildren())
		{
			button.Disabled = false;
			button.MouseFilter = MouseFilterEnum.Stop;
		}
	}


	public void SettingPressed()
	{
		if (settingsOpen)
		{
			animationPlayer.Play("CloseSideMenu");
			settingsOpen = !settingsOpen;
			if (currentOption != null)
				optionClose(currentOption);
		}
		else
		{
			animationPlayer.Play("OpenSideMenu");
			settingsOpen = !settingsOpen;
		}

	}


	public void SaveKeys()
	{
		Dictionary data = new()
		{
			{ "ChatGPT", ChatGPTtext.Text },
			{ "Azuir", Azuretext.Text },
			{ "11labs", Labtext.Text },
			{ "AZReigon", regionText.Text}
		};

		saveData.SaveAPIKeys(data);
	}

	void GPTModelChange(int index)
	{

		if (index == 0)
			services.chatGPT.model = "gpt-4o-mini";
		else if (index == 1)
			services.chatGPT.model = "gpt-4o";
		else
			services.chatGPT.model = "gpt-3.5-turbo";
	}

	void TempChanged(float value)
	{
		GetNode<Label>("SettingsSelect/ScrollContainer/GridContainer/ChatGpt/Panel/VoicesLevel/Value").Text = value.ToString();
		services.chatGPT.temperature = value;
	}

	void MaxTokenChange(float value)
	{
		services.chatGPT.maxTokenCount = (int)value;
	}

	void SaveGPT()
	{
		services.chatGPT.CreateNewClient();
	}


	void STTChanged(int index)
	{
		if (index == 0)
		{
			manager.sTT.provider = STT.STTProviders.Whisper;
			GetNode<Control>("SettingsSelect/ScrollContainer/GridContainer/STT/Panel/Whisper").Visible = true;
			GetNode<Control>("SettingsSelect/ScrollContainer/GridContainer/STT/Panel/Azuir").Visible = false;
		}
		else if (index == 1)
		{
			manager.sTT.provider = STT.STTProviders.Azuir;
			GetNode<Control>("SettingsSelect/ScrollContainer/GridContainer/STT/Panel/Azuir").Visible = true;
			GetNode<Control>("SettingsSelect/ScrollContainer/GridContainer/STT/Panel/Whisper").Visible = false;
		}
	}



}
