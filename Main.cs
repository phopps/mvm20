using Godot;

public partial class Main : Node
{
    public string statePrevious = "NONE";
    public string state = "LOAD"; // NONE, LOAD, IDLE, PREP, SHOT
    [Export] public float multiplier; // 9.0f
    private RigidBody2D player;
    private Vector2 inputStart; // Set at start of PREP
    private Vector2 inputEnd; // Set at end of PREP
    private Vector2 inputVector; // inputStart - inputEnd
    private Line2D impulse; // impulseStart - impulseEnd
    private Vector2 impulseStart; // player.Position
    private Vector2 impulseEnd; // player.Position - inputVector
    private Vector2 impulseVector; // inputVector * multiplier
    private Trajectory trajectory; // polyline
    private Timer timer; // 1 second
    private CanvasLayer home; // Main menu
    private Sprite2D background;
    private Node2D level;

    public override void _Ready()
    {
        home = GetNode<CanvasLayer>("Home");
        player = GetNode<RigidBody2D>("Player");
        impulse = GetNode<Line2D>("Impulse");
        timer = GetNode<Timer>("Timer");
        trajectory = GetNode<Trajectory>("Trajectory");
        level = GetNode<Node2D>("Level");
        background = GetNode<Sprite2D>("Background");
        statePrevious = state;
        state = "IDLE";
        GD.Print("[", state, " <- ", statePrevious, "] Main ready.");
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (state == "IDLE")
        {
            if (@event.IsActionPressed("Shot"))
            {
                // Checks all devices, including mouse left click, controller bottom button, and touchscreen tap
                statePrevious = state;
                state = "PREP";
                GD.Print("[", state, " <- ", statePrevious, "] Shot button pressed.");
                inputStart = GetViewport().GetMousePosition();
                inputEnd = inputStart;
                impulseStart = player.Position;
                impulseEnd = player.Position;
                impulse.SetPointPosition(0, impulseStart);
                impulse.SetPointPosition(1, impulseEnd);
                impulse.Visible = true;
                trajectory.Visible = true;
            }
        }
        else if (state == "PREP")
        {
            if (@event.IsActionReleased("Shot"))
            {
                statePrevious = state;
                state = "SHOT";
                GD.Print("[", state, " <- ", statePrevious, "] Shot button released. Shot timer started.");
                timer.Start();
                inputEnd = GetViewport().GetMousePosition();
                inputVector = inputStart - inputEnd;
                GD.Print("[", state, "] Input vector = ", inputVector.ToString());
                if (multiplier == 0) { GD.Print("[SHOT] ERROR! Multiplier can not be set to zero."); }
                impulseVector = inputVector * multiplier;
                GD.Print("[", state, "] Impulse vector = ", impulseVector.ToString());
                impulse.Visible = false;
                trajectory.Visible = false;
                player.ApplyCentralImpulse(impulseVector);
            }
            else if (@event is InputEventMouseMotion)
            {
                inputEnd = GetViewport().GetMousePosition();
                inputVector = inputStart - inputEnd;
                GD.Print("[", state, "] Input vector = ", inputVector.ToString());
                if (multiplier == 0) { GD.Print("[", state, "] ERROR! Multiplier can not be set to zero."); }
                impulseVector = inputVector * multiplier;
                GD.Print("[", state, "] Impulse vector = ", impulseVector.ToString());
            }
            else if (@event is InputEventJoypadMotion)
            {
                // GD.Print("[PREP] Joystick moved.");
            }
            else if (@event is InputEventScreenDrag)
            {
                // GD.Print("[PREP] Touchscreen dragged.");
            }
        }
        else if (state == "SHOT")
        {
            // GD.Print("[SHOT] ", @event.GetClass(), " detected.");
        }
        else
        {
            GD.Print("[", state, "] Error: no player state detected.");
        }
    }

    public override void _Process(double delta)
    {
        if (impulse.Visible)
        {
            impulseStart = player.Position;
            impulseEnd = impulseStart - inputVector;
            impulse.SetPointPosition(0, impulseStart);
            impulse.SetPointPosition(1, impulseEnd);
        }
    }

    private void OnTimerTimeout()
    {
        statePrevious = state;
        state = "IDLE";
        GD.Print("[", state, " <- ", statePrevious, "] Shot timer stopped.");
    }

    private void OnHomeOnPlayGame()
    {
        GD.Print("[", state, "] Playing game. Signal received from Home.");
        home.Hide();
        player.Show();
        background.Show();
        level.Show();
    }

    private void OnHomeOnQuitGame()
    {
        GD.Print("[", state, "] Quitting game. Signal received from Home.");
        GetTree().Quit();
    }
}
