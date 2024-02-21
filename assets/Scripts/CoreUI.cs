using Godot;


public partial class CoreUI : Control
{
	public OptionButton micList;
	public Button record;
	public Button Replay;

	public override void _Ready()
	{
		micList = GetNode<OptionButton>("settings menu/MicList");
		record = GetNode<Button>("Record");
		Replay = GetNode<Button>("Replay");

	}
}
