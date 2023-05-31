using Godot;

public partial class Main : Node
{
    private RigidBody2D player;
    private Line2D launchLine;
    private Vector2 lineStart;
    private Vector2 lineEnd;
    private Vector2 impulse;
    private Timer launchTimer;
    private string state = "none"; // none, idle, prep, launch

    // private Vector2 playerStart;
    // private Vector2 playerEnd;
    // private int startPointIndex = 0;
    // private int endPointIndex = 1;

    public override void _Ready()
    {
        player = GetNode<RigidBody2D>("Player");
        launchLine = GetNode<Line2D>("LaunchLine");
        launchTimer = GetNode<Timer>("LaunchTimer");
        state = "idle";
        GD.Print("[none -> idle] Ready.");
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (state == "idle")
        {
            if (@event.IsActionPressed("Launch"))
            {
                // All devices (mouse left click, controller bottom button, touchscreen tap)
                GD.Print("[idle -> prep] Launch button pressed.");
                state = "prep";
            }
        }
        else if (state == "prep")
        {
            if (@event.IsActionReleased("Launch"))
            {
                GD.Print("[prep -> launch] Launch button released. Launch timer started.");
                state = "launch";
                launchTimer.Start();
            }
            else if (@event is InputEventMouseMotion)
            {
                GD.Print("[prep] Mouse moved.");
            }
            else if (@event is InputEventJoypadMotion)
            {
                GD.Print("[prep] Joystick moved.");
            }
            else if (@event is InputEventScreenDrag)
            {
                GD.Print("[prep] Touchscreen dragged.");
            }
        }
        else if (state == "launch")
        {
            GD.Print("[launch] ", @event.GetClass(), " detected.");
        }
        else
        {
            GD.Print("[none] Error: no player state detected.");
        }
    }

    private void _OnLaunchTimerTimeout()
    {
        GD.Print("[launch -> idle] Launch timer stopped.");
        state = "idle";
    }
}

// TODO: emit signal whenever state is changed

// public override void _Input(InputEvent @event)
// {
//     if (@event is InputEventMouseButton eventMouseButton)
//     {
//         if (eventMouseButton.Pressed && eventMouseButton.ButtonIndex == MouseButton.Left)
//         {
//             // StartLaunchMode(eventMouseButton.Position);
//             isPreparing = true;
//             launchLine.Visible = true;
//             // lineStart = startPosition;
//             lineStart = eventMouseButton.Position;
//             playerStart = player.Position;
//             launchLine.SetPointPosition(startPointIndex, playerStart);
//             GD.Print("\nLine start = ", lineStart);
//         }
//         else if (eventMouseButton.ButtonIndex == MouseButton.Left)
//         {
//             // EndLaunchMode(eventMouseButton.Position);
//             // lineEnd = endPosition;
//             lineEnd = eventMouseButton.Position;
//             impulse = lineStart - lineEnd;
//             playerEnd = playerStart + impulse;
//             GD.Print("Line end = ", lineEnd);
//             GD.Print("Impulse = ", impulse);
//             GD.Print("Player end = ", playerEnd);
//             player.ApplyImpulse(impulse);
//             isPreparing = false;
//             launchLine.Visible = false;
//         }
//     }
//     else if (@event is InputEventMouseMotion eventMouseMotion && isPreparing)
//     {
//         launchLine.SetPointPosition(endPointIndex, eventMouseMotion.Position);
//     }
// }

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
