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

	public void StartConversation(bool value)
	{
		if (value)
		{
			GetNode<Manager>("/root/Managers").conversation.StartConversation();
			GetNode<Button>("StartConvo").Text = "Stop Conversation";
		}
		else
		{
			GetNode<Manager>("/root/Managers").conversation.StopConversation();
			GetNode<Button>("StartConvo").Text = "Start Conversation";
		}
	}
}
