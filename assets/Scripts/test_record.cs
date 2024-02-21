/*
using Godot;

public partial class test_record : Control
{



	

	public override void _Ready()
	{

		
	}

	public async void OnRecordPressed()
	{
		toggled = !toggled;

		if (toggled)
		{
			recordEffect.SetRecordingActive(true);
			GD.Print("Recording");
		}
		else
		{
			await ToSignal(GetTree().CreateTimer(0.1f), "timeout");  // Adjust the delay as needed
			recording = recordEffect.GetRecording();
			recordEffect.SetRecordingActive(false);
			GD.Print("Stopped");
		}
	}


	public void PlayPressed()
	{
		GD.Print("PLAY");
		var audioStreamPlayer = GetNode<AudioStreamPlayer>("Audio/AudioStreamPlayer");
		audioStreamPlayer.Stream = recording;
		audioStreamPlayer.Play();
		SendToAI();
	}


	public void GetMicList()
	{
		devices = AudioServer.GetInputDeviceList();
		OptionButton dropdown = GetNode<OptionButton>("MicList");
		dropdown.Clear();
		foreach (string device in devices)
		{
			dropdown.AddItem(device);
		}
	}


	public void MicSelected(int index)
	{
		AudioServer.InputDevice = devices[index];
	}


	public async void SendToAI()
	{
		recording.SaveToWav("res/record.wav");
		AzuirGD azuir = GetNode<AzuirGD>("Remote Libraries/Azuir");
		ChatGD chat = GetNode<ChatGD>("Remote Libraries/ChatGPT");
		string recordedText = await azuir.GetTextFromWav(ProjectSettings.GlobalizePath("res://res/record.wav"));
		if (recordedText != null)
		{
			//chat.SetContext();
		}

	}
}
*/