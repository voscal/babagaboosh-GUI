using Godot;


public partial class NotificationsManager : Node
{
	public void NewNotification(string type, string Heading, string message, float Timer)
	{
		GD.Print($"NEW NOTIFICATIONS Type{type}");
		if (type.ToLower() == "info")
		{
			PackedScene notification = GD.Load<PackedScene>("res://assets/Scenes/notifications/Info.tscn");
			var instance = notification.Instantiate();
			instance.GetNode<RichTextLabel>("Panel/Heading").Text = Heading;
			instance.GetNode<RichTextLabel>("Panel/Content").Text = message;
			instance.Set("TimeOut", Timer);
			GetNode<VBoxContainer>("/root/Main Window/UI/Notifications/ScrollContainer/VBoxContainer").AddChild(instance);
		}
		if (type.ToLower() == "error")
		{
			PackedScene notification = GD.Load<PackedScene>("res://assets/Scenes/notifications/Error.tscn");
			var instance = notification.Instantiate();
			instance.GetNode<RichTextLabel>("Panel/Heading").Text = Heading;
			instance.GetNode<RichTextLabel>("Panel/Content").Text = message;
			instance.Set("TimeOut", Timer);
			GetNode<VBoxContainer>("/root/Main Window/UI/Notifications/ScrollContainer/VBoxContainer").AddChild(instance);
		}
	}

}

// Manager.notifications.NewNotification(type, message)

// Manager.notifications.NewNotification(, message)
