using Godot;

public partial class Main : Node
{
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

    // Global autoload singleton containing important logic and data such as game, player, and level states
    private Game game;

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
        game.statePrevious = game.state;
        game.state = Game.GAME_STATE.LOAD;
        GD.Print("[", game.state, "] Main ready.");
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("Pause"))
        {
            if (game.state == Game.GAME_STATE.PAUSED)
            {
                OnPauseOnResumeGame();
            }
            else if (game.state == Game.GAME_STATE.IDLE || game.state == Game.GAME_STATE.PREP || game.state == Game.GAME_STATE.SHOT)
            {
                game.statePrevious = game.state;
                game.state = Game.GAME_STATE.PAUSED;
                game.stateNext = game.statePrevious;
                GD.Print("[", game.state, " <- ", game.statePrevious, "] Pause button pressed. Pausing game.");
                GetTree().Paused = true;
                pause.Show();
            }
        }
        else if (game.state == Game.GAME_STATE.IDLE)
        {
            if (@event.IsActionPressed("Shot"))
            {
                // Checks all devices, including mouse left click, controller bottom button, and touchscreen tap
                game.statePrevious = game.state;
                game.state = Game.GAME_STATE.PREP;
                GD.Print("[", game.state, " <- ", game.statePrevious, "] Shot button pressed.");
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
        else if (game.state == Game.GAME_STATE.PREP)
        {
            if (@event.IsActionReleased("Shot"))
            {
                game.statePrevious = game.state;
                game.state = Game.GAME_STATE.SHOT;
                GD.Print("[", game.state, " <- ", game.statePrevious, "] Shot button released. Shot timer started.");
                timer.Start();
                inputEnd = GetViewport().GetMousePosition();
                inputVector = inputStart - inputEnd;
                GD.Print("[", game.state, "] Input vector = ", inputVector.ToString());
                if (multiplier == 0) { GD.Print("[SHOT] ERROR! Multiplier can not be set to zero."); }
                impulseVector = inputVector * multiplier;
                GD.Print("[", game.state, "] Impulse vector = ", impulseVector.ToString());
                impulse.Visible = false;
                trajectory.Visible = false;
                player.ApplyCentralImpulse(impulseVector);
            }
            else if (@event is InputEventMouseMotion)
            {
                inputEnd = GetViewport().GetMousePosition();
                inputVector = inputStart - inputEnd;
                GD.Print("[", game.state, "] Input vector = ", inputVector.ToString());
                if (multiplier == 0) { GD.Print("[", game.state, "] ERROR! Multiplier can not be set to zero."); }
                impulseVector = inputVector * multiplier;
                GD.Print("[", game.state, "] Impulse vector = ", impulseVector.ToString());
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
        else if (game.state == Game.GAME_STATE.SHOT)
        {
            // GD.Print("[SHOT] ", @event.GetClass(), " detected.");
        }
        else if (game.state == Game.GAME_STATE.LOAD)
        {

        }
        else if (game.state == Game.GAME_STATE.PAUSED)
        {

        }
        else
        {
            GD.Print("[", game.state, "] Error: no player state detected.");
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
        game.statePrevious = game.state;
        game.state = Game.GAME_STATE.IDLE;
        GD.Print("[", game.state, " <- ", game.statePrevious, "] Shot timer stopped.");
    }

    private void OnHomeOnPlayGame()
    {
        game.statePrevious = game.state;
        game.state = Game.GAME_STATE.IDLE;
        GD.Print("[", game.state, " <- ", game.statePrevious, "] Playing game. Signal received from Home.");
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
        GD.Print("[", game.state, "] Quitting game. Signal received from Home.");
        GetTree().Quit();
    }

    private void OnPauseOnQuitGame()
    {
        GD.Print("[", game.state, "] Quitting game. Signal received from Pause.");
        GetTree().Quit();
    }

    private void OnPauseOnHome()
    {
        game.statePrevious = game.state;
        game.state = Game.GAME_STATE.LOAD;
        GD.Print("[", game.state, " <- ", game.statePrevious, "] Home screen loading. Signal received from Pause.");
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
        game.statePrevious = game.state;
        game.state = game.stateNext;
        game.stateNext = Game.GAME_STATE.NONE;
        GD.Print("[", game.state, " <- ", game.statePrevious, "] Resuming game. Signal received from Pause.");
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
}
