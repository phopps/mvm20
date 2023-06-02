using System.Diagnostics;
using Godot;

public partial class Main : Node
{
    private RigidBody2D player;
    private Line2D launchLine; // lineStart - lineEnd
    private Vector2 lineStart; // player.Position
    private Vector2 lineEnd; // player.Position - inputVector
    private Vector2 inputStart;
    private Vector2 inputEnd;
    private Vector2 inputVector; // inputStart - inputEnd
    private Vector2 impulse; // inputVector * multiplier
    private Timer launchTimer; // 1 second
    private string state = "none"; // none, idle, prep, launch
    [Export] public float multiplier; // 9.0f

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

                inputStart = GetViewport().GetMousePosition();
                inputEnd = inputStart;
                // inputVector = inputStart - inputEnd;
                // GD.Print("[prep] Input vector = ", inputVector.ToString());

                lineStart = player.Position;
                lineEnd = player.Position;
                launchLine.SetPointPosition(0, lineStart);
                launchLine.SetPointPosition(1, lineEnd);
                launchLine.Visible = true;
            }
        }
        else if (state == "prep")
        {
            if (@event.IsActionReleased("Launch"))
            {
                GD.Print("[prep -> launch] Launch button released. Launch timer started.");
                state = "launch";
                launchTimer.Start();

                inputEnd = GetViewport().GetMousePosition();
                inputVector = inputStart - inputEnd;
                GD.Print("[launch] Input vector = ", inputVector.ToString());

                if (multiplier == 0)
                {
                    GD.Print("[launch] ERROR! Multiplier can not be set to zero.");
                }

                impulse = inputVector * multiplier;
                GD.Print("[launch] Impulse vector = ", impulse.ToString());

                launchLine.Visible = false;

                player.ApplyCentralImpulse(impulse);
            }
            else if (@event is InputEventMouseMotion)
            {
                inputEnd = GetViewport().GetMousePosition();
                inputVector = inputStart - inputEnd;
                GD.Print("[launch] Input vector = ", inputVector.ToString());

                if (multiplier == 0)
                {
                    GD.Print("[launch] ERROR! Multiplier can not be set to zero.");
                }

                impulse = inputVector * multiplier;
                GD.Print("[launch] Impulse vector = ", impulse.ToString());

                // lineStart = player.Position;
                // lineEnd = lineStart - inputVector;
                // launchLine.SetPointPosition(0, lineStart);
                // launchLine.SetPointPosition(1, lineEnd);
                // GD.Print("[prep] Mouse moved.");
            }
            else if (@event is InputEventJoypadMotion)
            {
                // GD.Print("[prep] Joystick moved.");
            }
            else if (@event is InputEventScreenDrag)
            {
                // GD.Print("[prep] Touchscreen dragged.");
            }
        }
        else if (state == "launch")
        {
            // GD.Print("[launch] ", @event.GetClass(), " detected.");
        }
        else
        {
            GD.Print("[none] Error: no player state detected.");
        }
    }

    public override void _Process(double delta)
    {
        if (launchLine.Visible)
        {
            lineStart = player.Position;
            lineEnd = lineStart - inputVector;
            launchLine.SetPointPosition(0, lineStart);
            launchLine.SetPointPosition(1, lineEnd);
        }
    }

    private void _OnLaunchTimerTimeout()
    {
        GD.Print("[launch -> idle] Launch timer stopped.");
        state = "idle";
    }
}

// Increase lineStart from center of player to radius of player
// Emit signal whenever state is changed
// Animate line to move endpoint toward start point over time duration
