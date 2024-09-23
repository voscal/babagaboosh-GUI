using Godot;
using System;
using System.Threading.Tasks;

public partial class STT : Node
{
	public enum STTProviders
	{
		Whisper,
		Vosk,
		Azuir,
	}

	[Export]
	public STTProviders provider;

	[Export]
	private string Input = "user://Audio/record.wav";

	Services services;


	public override void _Ready()
	{
		services = GetNode<Services>("/root/Services");
	}

	public async Task<string> GetText()
	{
		switch (provider)
		{
			case STTProviders.Whisper:
				return services.whisper.GetText(Input);
			case STTProviders.Vosk:
				return services.vosk.GetText(Input);
			case STTProviders.Azuir:
				return await services.azuir.GetText(Input);
		}
		return null;
	}


}
