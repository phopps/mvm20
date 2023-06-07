using Godot;

public partial class Pause : CanvasLayer
{
  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
    GD.Print("Pause ready.");
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta)
  {
  }
}
