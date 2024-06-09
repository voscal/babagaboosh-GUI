
using System.IO;
using Godot;


public partial class AudioManager : Node
{
	string[] inputDevices;
	string[] outputDevices;
	public AudioEffectRecord recordEffect;

	public AudioEffectSpectrumAnalyzerInstance spectrum;

	public AudioStreamWav recording;

	UI ui;


	bool currentlyRecording = false;

	public override void _Ready()
	{


		ui = GetNode<UI>("/root/Main Scene/UI");
		int idx = AudioServer.GetBusIndex("Recording");
		recordEffect = (AudioEffectRecord)AudioServer.GetBusEffect(idx, 0);
		spectrum = (AudioEffectSpectrumAnalyzerInstance)AudioServer.GetBusEffectInstance(5, 0);

		#region Signals
		ui.GetNode<OptionButton>("SettingsUI/SettingsSelect/ScrollContainer/HBoxContainer/Audio/MicList").ItemSelected += MicSelected;
		ui.GetNode<OptionButton>("SettingsUI/SettingsSelect/ScrollContainer/HBoxContainer/Audio/OutputList").ItemSelected += OutputSelected;
		ui.GetNode<TextureButton>("CoreUI/Replay").Pressed += PlayAudio;
		#endregion
		GetMicList();
		GetOutputList();


	}



	public bool RecordBttnPressed()
	{
		if (!currentlyRecording)
		{
			RecordInput();
			return true;
		}

		StopRecording();
		return false;

	}

	/// <summary>
	/// records the input of the currently selected microphone
	/// </summary>
	public void RecordInput()
	{

		recordEffect.SetRecordingActive(true);
		GD.Print("Recording");
		currentlyRecording = true;

	}


	/// <summary>
	/// stops the recording and saves it to a wav file
	/// </summary>
	public void StopRecording()
	{
		recording = recordEffect.GetRecording();
		recordEffect.SetRecordingActive(false);
		GD.Print("Stopped");
		recording.SaveToWav("Audio/record.wav");
		currentlyRecording = false;



	}

	public void PlayAudio()
	{
		GD.Print("Play");
		AudioStreamPlayer audioStreamPlayer = GetNode<AudioStreamPlayer>("AIVoice");

		byte[] wavData = File.ReadAllBytes(ProjectSettings.GlobalizePath("res://Audio/AIresponse.wav"));


		AudioStreamWav audioStreamSample = new AudioStreamWav()
		{
			Format = AudioStreamWav.FormatEnum.Format16Bits,
			MixRate = 44100, // Adjust this based on your WAV file's sample rate
			Stereo = false, // Adjust this based on your WAV file's channel count
			Data = wavData
		};


		// Set the stream to the audio player and play
		audioStreamPlayer.Stream = audioStreamSample;
		audioStreamPlayer.Play();
	}




	/// <summary>
	/// will grab a list of avaliable audio inputs and updates the micraphone dropdown
	/// </summary>
	public void GetMicList()
	{
		inputDevices = AudioServer.GetInputDeviceList();

		ui.GetNode<OptionButton>("SettingsUI/SettingsSelect/ScrollContainer/HBoxContainer/Audio/MicList").Clear();
		foreach (string device in inputDevices)
		{
			ui.GetNode<OptionButton>("SettingsUI/SettingsSelect/ScrollContainer/HBoxContainer/Audio/MicList").AddItem(device);
		}

	}



	/// <summary>
	/// make selected microphone defult
	/// </summary>
	/// <param name="index">Input index</param>
	public void MicSelected(long index)
	{
		GD.Print($"Input set : {inputDevices[index]}");
		AudioServer.InputDevice = inputDevices[index];
	}



	/// <summary>
	/// will grab a list of avaliable audio Outputs and updates the output dropdown
	/// </summary>
	public void GetOutputList()
	{
		outputDevices = AudioServer.GetOutputDeviceList();

		ui.GetNode<OptionButton>("SettingsUI/SettingsSelect/ScrollContainer/HBoxContainer/Audio/OutputList").Clear();
		foreach (string device in outputDevices)
		{
			ui.GetNode<OptionButton>("SettingsUI/SettingsSelect/ScrollContainer/HBoxContainer/Audio/OutputList").AddItem(device);
		}

	}



	/// <summary>
	/// make selected Output defult
	/// </summary>
	/// <param name="index">Input index</param>
	public void OutputSelected(long index)
	{
		AudioServer.OutputDevice = outputDevices[index];
	}








}
