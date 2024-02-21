using Godot;


public partial class CoreUI : Control
{

	public Button record;
	public Button Replay;

	public override void _Ready()
	{
		record = GetNode<Button>("Record");
		Replay = GetNode<Button>("Replay");

	}
}
