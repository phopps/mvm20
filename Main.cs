using Godot;

public partial class Main : Node
{
    // [Signal] public delegate void OnPauseGameEventHandler();
    // [Signal] public delegate void OnResumeGameEventHandler();
    public enum STATE { NONE, LOAD, IDLE, PREP, SHOT, PAUSED }
    public STATE statePrevious = STATE.NONE;
    public STATE state = STATE.LOAD;
    public STATE stateNext = STATE.NONE;
    [Export] public float multiplier; // 9.0f
    [Export] public Vector2 levelStart; // starting position for level
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
    private CanvasLayer pause;

    public override void _Ready()
    {
        GetTree().Paused = true;
        home = GetNode<CanvasLayer>("Home");
        pause = GetNode<CanvasLayer>("Pause");
        player = GetNode<RigidBody2D>("Player");
        impulse = GetNode<Line2D>("Impulse");
        timer = GetNode<Timer>("Timer");
        trajectory = GetNode<Trajectory>("Trajectory");
        level = GetNode<Node2D>("Level");
        background = GetNode<Sprite2D>("Background");
        ResetPlayer();
        statePrevious = state;
        state = STATE.LOAD;
        GD.Print("[", state, "] Main ready.");
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("Pause"))
        {
            if (state == STATE.PAUSED)
            {
                OnPauseOnResumeGame();
                // statePrevious = state;
                // state = stateNext;
                // stateNext = STATE.NONE;
                // GD.Print("[", state, " <- ", statePrevious, "] Pause button pressed. Resuming game.");
                // pause.Hide();
                // GetTree().Paused = false;
            }
            else if (state == STATE.IDLE || state == STATE.PREP || state == STATE.SHOT)
            {
                statePrevious = state;
                state = STATE.PAUSED;
                stateNext = statePrevious;
                GD.Print("[", state, " <- ", statePrevious, "] Pause button pressed. Pausing game.");
                GetTree().Paused = true;
                pause.Show();
            }
        }
        else if (state == STATE.IDLE)
        {
            if (@event.IsActionPressed("Shot"))
            {
                // Checks all devices, including mouse left click, controller bottom button, and touchscreen tap
                statePrevious = state;
                state = STATE.PREP;
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
        else if (state == STATE.PREP)
        {
            if (@event.IsActionReleased("Shot"))
            {
                statePrevious = state;
                state = STATE.SHOT;
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
        else if (state == STATE.SHOT)
        {
            // GD.Print("[SHOT] ", @event.GetClass(), " detected.");
        }
        else if (state == STATE.LOAD)
        {

        }
        else if (state == STATE.PAUSED)
        {

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
        state = STATE.IDLE;
        GD.Print("[", state, " <- ", statePrevious, "] Shot timer stopped.");
    }

    private void OnHomeOnPlayGame()
    {
        statePrevious = state;
        state = STATE.IDLE;
        GD.Print("[", state, " <- ", statePrevious, "] Playing game. Signal received from Home.");
        home.Hide();
        ResetPlayer();
        player.Show();
        background.Show();
        level.Show();
        GetTree().Paused = false;
        // make sure timer is reset
    }

    private void OnHomeOnQuitGame()
    {
        GD.Print("[", state, "] Quitting game. Signal received from Home.");
        GetTree().Quit();
    }

    private void OnPauseOnQuitGame()
    {
        GD.Print("[", state, "] Quitting game. Signal received from Pause.");
        GetTree().Quit();
    }

    private void OnPauseOnHome()
    {
        statePrevious = state;
        state = STATE.LOAD;
        GD.Print("[", state, " <- ", statePrevious, "] Home screen loading. Signal received from Pause.");
        home.Show();
        pause.Hide();
        player.Hide();
        ResetPlayer();
        background.Hide();
        level.Hide();
        GetTree().Paused = true;
        timer.Stop();
    }

    private void OnPauseOnResumeGame()
    {
        statePrevious = state;
        state = stateNext;
        stateNext = STATE.NONE;
        GD.Print("[", state, " <- ", statePrevious, "] Resuming game. Signal received from Pause.");
        pause.Hide();
        GetTree().Paused = false;
    }

    private void ResetPlayer()
    {
        player.GlobalPosition = levelStart;
        player.LinearVelocity = Vector2.Zero;
        player.AngularVelocity = 0.0f;
        player.Rotation = 0;
    }

    // public void ChangeState(STATE stateNext)
    // {
    //     statePrevious = state;
    //     state = stateNext;
    // }
}
