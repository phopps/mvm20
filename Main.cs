using Godot;

public partial class Main : Node
{
    private Line2D launchLine;
    private Vector2 lineStart;
    private Vector2 lineEnd;
    private Vector2 impulse;
    private RigidBody2D player;
    private Vector2 playerStart;
    private Vector2 playerEnd;

    private int startPointIndex = 0;
    private int endPointIndex = 1;
    private bool isPreparing = false;

    public override void _Ready()
    {
        player = GetNode<RigidBody2D>("Player");
        launchLine = GetNode<Line2D>("LaunchLine");
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton eventMouseButton)
        {
            if (eventMouseButton.Pressed && eventMouseButton.ButtonIndex == MouseButton.Left)
            {
                // StartLaunchMode(eventMouseButton.Position);
                isPreparing = true;
                launchLine.Visible = true;
                // lineStart = startPosition;
                lineStart = eventMouseButton.Position;
                playerStart = player.Position;
                launchLine.SetPointPosition(startPointIndex, playerStart);
                GD.Print("\nLine start = ", lineStart);
            }
            else if (eventMouseButton.ButtonIndex == MouseButton.Left)
            {
                // EndLaunchMode(eventMouseButton.Position);
                // lineEnd = endPosition;
                lineEnd = eventMouseButton.Position;
                impulse = lineStart - lineEnd;
                playerEnd = playerStart + impulse;
                GD.Print("Line end = ", lineEnd);
                GD.Print("Impulse = ", impulse);
                GD.Print("Player end = ", playerEnd);
                player.ApplyImpulse(impulse);
                isPreparing = false;
                launchLine.Visible = false;
            }
        }
        else if (@event is InputEventMouseMotion eventMouseMotion && isPreparing)
        {
            launchLine.SetPointPosition(endPointIndex, eventMouseMotion.Position);
        }
    }

    // private void StartLaunchMode(Vector2 startPosition)
    // {
    //     isPreparing = true;
    //     launchLine.Visible = true;
    //     start = startPosition;
    //     launchLine.SetPointPosition(startPointIndex, start);
    //     GD.Print("\nStart position = ", startPosition);
    // }

    // private void EndLaunchMode(Vector2 endPosition)
    // {
    //     end = endPosition;
    //     GD.Print("End position = ", endPosition);
    //     impulse = start - end;
    //     GD.Print("Launch impulse = ", impulse);
    //     player.ApplyImpulse(impulse);
    //     isPreparing = false;
    //     launchLine.Visible = false;
    //     // could animate line to move endpoint toward start point over time duration
    // }
}

/*
prep to launch
  make line visible
  set start point to start position
  end point set to current mouse position
  any mouse movement
*/
