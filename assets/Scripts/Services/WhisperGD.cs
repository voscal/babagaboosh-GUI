using System.IO;
using System.Threading.Tasks;
using Godot;


public partial class WhisperGD : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public async Task<string> GetText(string input)
	{

		GD.Print((string)GetNode<Node>("Model")._Get("text"));



		byte[] wavData = File.ReadAllBytes(ProjectSettings.GlobalizePath(input));


		AudioStreamWav audioStreamSample = new AudioStreamWav()
		{
			Format = AudioStreamWav.FormatEnum.Format16Bits,
			MixRate = 44100, // Adjust this based on your WAV file's sample rate
			Stereo = false, // Adjust this based on your WAV file's channel count
			Data = wavData
		};

		// Set the stream to the audio player and play
		GetNode<Node>("Model")._Set("audio_stream", audioStreamSample);

		//GetNode<Node>("Model").Call("get_text");

		return "test";

	}
}
