using Godot;

public partial class Trajectory : Node2D
{
    [Export] public Vector2[] points;
    [Export] public int pointsMax;
    private RigidBody2D player;
    private Main.STATE state;

    public override void _Ready()
    {
        player = GetNode<RigidBody2D>("/root/Main/Player");
        state = GetNode<Main>("/root/Main").state;
        GD.Print("[", state, "] Trajectory ready.");
    }

    public override void _Draw()
    {
        state = GetNode<Main>("/root/Main").state;
        GD.Print("[", state, "] Drawing trajectory starting at ", GlobalPosition);
        // GetEuler();
        // DrawPolyline(points, new Color(10, 20, 30, 40), GetViewport().GetMousePosition().Y, false);
        // DrawPolyline(points2, new Color(100, 200, 30, 40), GetViewport().GetMousePosition().X / 10, false);
    }

    public override void _Process(double delta)
    {
        if (Visible)
        {
            GlobalPosition = player.GlobalPosition;
            QueueRedraw();
        }
    }

    // private void GetEuler()
    // {
    //     // GD.Print("[PREP] Calculating Euler's method for ", pointsMax, " points.");
    // }
}
