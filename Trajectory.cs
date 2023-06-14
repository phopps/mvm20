using Godot;

public partial class Trajectory : Node2D
{
    [Export] public Vector2[] points;
    [Export] public int pointsMax;
    private RigidBody2D player;
    private Game.GAME_STATE state;

    public override void _Ready()
    {
        player = GetNode<RigidBody2D>("/root/Main/Player");
        GD.Print("[", state, "] Trajectory ready.");
    }

    public override void _Draw()
    {
        GD.Print("[", state, "] Drawing trajectory starting at ", GlobalPosition);
    }

    public override void _Process(double delta)
    {
        if (Visible)
        {
            GlobalPosition = player.GlobalPosition;
            QueueRedraw();
        }
    }
}
