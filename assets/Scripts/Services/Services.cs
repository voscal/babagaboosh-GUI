using Godot;


public partial class Services : Node
{
	public ChatGD chatGPT;
	public AzuirGD azuir;
	public ElevinLabsGD elevinLabs;
	public VoskGD vosk;
	public WhisperGD whisper;
	public override void _Ready()
	{
		chatGPT = GetNode<ChatGD>("ChatGPT");
		azuir = GetNode<AzuirGD>("Azuir");
		elevinLabs = GetNode<ElevinLabsGD>("ElevinLabs");
		vosk = GetNode<VoskGD>("Vosk");
		whisper = GetNode<WhisperGD>("Whisper");

	}



}
