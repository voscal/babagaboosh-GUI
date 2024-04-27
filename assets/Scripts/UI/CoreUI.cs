using Godot;


public partial class CoreUI : Control
{

	public TextureButton record;
	public TextureButton Replay;

	public override void _Ready()
	{
		record = GetNode<TextureButton>("Record");
		Replay = GetNode<TextureButton>("Replay");

	}
}
