using Godot;


public partial class CharacterViewport : SubViewportContainer
{
	Manager manager;
	public bool focused = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		manager = GetNode<Manager>("/root/Managers");
		SetProcessUnhandledInput(false);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}


	public override void _Input(InputEvent @event)
	{
		foreach (SubViewport viewport in GetChildren())
		{
			if (@event is InputEventMouse mouseEvent)
			{
				// Transform the mouse position into the SubViewport's local space
				var transformedMouseEvent = (InputEventMouse)mouseEvent.Duplicate();
				transformedMouseEvent.Position = GetGlobalTransformWithCanvas().AffineInverse() * mouseEvent.Position;

				//GD.Print($"Transformed Position: {transformedMouseEvent.Position}");

				// Forward to the SubViewport, letting it process the input
				viewport._UnhandledInput(transformedMouseEvent);
			}
			else
			{
				// Forward non-mouse events
				viewport._UnhandledInput(@event);
			}
		}
	}

	public void set_name(string name)
	{
		Name = name;
	}

}
