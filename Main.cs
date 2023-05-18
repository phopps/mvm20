using Godot;

public partial class Main : Node
{
  private Vector2 start;
  private Vector2 end;
  private Vector2 impulse;
  private RigidBody2D player;
  // private bool isPreparing;

  public override void _Ready()
  {
    player = GetNode<RigidBody2D>("Player");
  }

  public override void _Input(InputEvent @event)
  {
    if (@event is InputEventMouseButton eventMouseButton)
    {
      if (eventMouseButton.Pressed && eventMouseButton.ButtonIndex == MouseButton.Left)
      {
        start = eventMouseButton.Position;
        GD.Print("\nStart position = ", eventMouseButton.Position);
      }
      else if (eventMouseButton.ButtonIndex == MouseButton.Left)
      {
        end = eventMouseButton.Position;
        GD.Print("End position = ", eventMouseButton.Position);
        impulse = start - end;
        GD.Print("Launch impulse = ", impulse);
        player.ApplyImpulse(impulse);
      }
    }
  }
}
