
using System;
using System.IO;
using Godot;
using NAudio.Wasapi.CoreAudioApi;


public partial class AudioManager : Manager
{
	string[] inputDevices;
	string[] outputDevices;
	public AudioEffectRecord recordEffect;


	public AudioStreamWav recording;

	UI ui;


	bool currentlyRecording = false;

	public override void _Ready()
	{
		recordEffect = (AudioEffectRecord)AudioServer.GetBusEffect(2, 0);

		ui = GetNode<UI>("/root/Main Window/UI");

		if (GetNode<SaveData>("/root/Data/SaveData").DirExists("user://Audio") == false)
		{
			GetNode<SaveData>("/root/Data/SaveData").DirCreate("user://Audio");
		}

		#region Signals
		ui.GetNode<OptionButton>("SettingsUI/SettingsSelect/ScrollContainer/HBoxContainer/Audio/MicList").ItemSelected += MicSelected;
		ui.GetNode<OptionButton>("SettingsUI/SettingsSelect/ScrollContainer/HBoxContainer/Audio/OutputList").ItemSelected += OutputSelected;



		#endregion
		GetMicList();
		GetOutputList();


	}

	public override void _Process(double delta)
	{

		//settings menu
		int voiceChannle = AudioServer.GetBusIndex("Voices");
		AudioServer.SetBusVolumeDb(voiceChannle, (float)ui.settingsUI.GetNode<Slider>("SettingsSelect/ScrollContainer/HBoxContainer/Audio/VoicesLevel").Value);


		int SFXChannle = AudioServer.GetBusIndex("SFX");
		AudioServer.SetBusVolumeDb(SFXChannle, (float)ui.settingsUI.GetNode<Slider>("SettingsSelect/ScrollContainer/HBoxContainer/Audio/SFX Level").Value);

		int BGmusic = AudioServer.GetBusIndex("BGMusic");
		AudioServer.SetBusVolumeDb(BGmusic, (float)ui.settingsUI.GetNode<Slider>("SettingsSelect/ScrollContainer/HBoxContainer/Audio/MusicLevel").Value);

	}

	public bool RecordBttnPressed()
	{
		if (!currentlyRecording)
		{
			StartRecording();
			currentlyRecording = true;
			return true;
		}
		StopRecording();
		currentlyRecording = false;
		return false;

	}

	/// <summary>
	/// records the input of the currently selected microphone
	/// </summary>
	public void StartRecording()
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

		recording.SaveToWav("user://Audio/record.wav");
		GD.Print("Stopped");

		currentlyRecording = false;
	}

	public void PlayAudio(CharacterViewport character)
	{
		GD.Print("Play");
		AudioStreamPlayer audioStreamPlayer = character.GetNode<AudioStreamPlayer>("Dummy/AudioPlayer");

		byte[] wavData = File.ReadAllBytes(ProjectSettings.GlobalizePath("user://Audio/AIresponse.wav"));


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


	private byte[] MixStereoToMono(byte[] input)
	{
		// If the sample length can be divided by 4, it's a valid stero sound
		if (input.Length % 4 == 0)
		{
			byte[] output = new byte[input.Length / 2];                 // create a new byte array half the size of the stereo length
			int outputIndex = 0;
			for (int n = 0; n < input.Length; n += 4)                     // Loop through each stero sample
			{
				int leftChannel = BitConverter.ToInt16(input, n);        // Get the left channel
				int rightChannel = BitConverter.ToInt16(input, n + 2);     // Get the right channel
				int mixed = (leftChannel + rightChannel) / 2;           // Mix them together
				byte[] outSample = BitConverter.GetBytes((short)mixed); // Convert mix to bytes

				// copy in the first 16 bit sample
				output[outputIndex++] = outSample[0];
				output[outputIndex++] = outSample[1];
			}
			return output;
		}
		else
		{
			byte[] output = new byte[24];

			return output;
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

	public AudioEffectSpectrumAnalyzerInstance NewCharacterBus(string characterPath)
	{
		AudioServer.AddBus(AudioServer.BusCount);
		AudioServer.SetBusName(AudioServer.BusCount - 1, characterPath);
		var spectrumAnalyzer = new AudioEffectSpectrumAnalyzer();
		AudioServer.AddBusEffect(AudioServer.BusCount - 1, spectrumAnalyzer);
		AudioServer.SetBusSend(AudioServer.BusCount - 1, "Voices");

		return (AudioEffectSpectrumAnalyzerInstance)AudioServer.GetBusEffectInstance(AudioServer.BusCount - 1, 0);

	}

	public void UpdateCharacterBus(string oldCharacterPath, string newCharacterPath)
	{
		AudioServer.SetBusName(AudioServer.GetBusIndex(oldCharacterPath), newCharacterPath);
	}


	public void RemoveCharacterBus(string characterPath)
	{
		AudioServer.RemoveBus(AudioServer.GetBusIndex(characterPath));
	}






}
