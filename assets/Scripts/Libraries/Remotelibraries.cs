using Godot;


public partial class Remotelibraries : Node
{
	public ChatGD chatGPT;
	public AzuirGD azuir;
	public ElevinLabsGD elevinLabs;
	public override void _Ready()
	{
		chatGPT = GetNode<ChatGD>("ChatGPT");
		azuir = GetNode<AzuirGD>("Azuir");
		elevinLabs = GetNode<ElevinLabsGD>("ElevinLabs");

	}



}
