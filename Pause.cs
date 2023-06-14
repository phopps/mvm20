using Godot;

public partial class Pause : CanvasLayer
{
    [Signal] public delegate void OnResumeGameEventHandler();
    [Signal] public delegate void OnQuitGameEventHandler();
    [Signal] public delegate void OnHomeEventHandler();
    private Game.GAME_STATE state;
    private Game.GAME_STATE statePrevious;
    private Game.GAME_STATE stateNext;

    public override void _Ready()
    {
        GD.Print("[", state, "] Pause ready.");
    }

    public void OnResumePressed()
    {
        GD.Print("[", state, "] Resume button pressed. Emitting signal from Pause to Main.");
        EmitSignal("OnResumeGame");
    }

    public void OnHomePressed()
    {
        GD.Print("[", state, "] Home button pressed. Emitting signal from Pause to Main.");
        EmitSignal("OnHome");
    }

    public void OnQuitPressed()
    {
        GD.Print("[", state, "] Quit button pressed. Emitting signal from Pause to Main.");
        EmitSignal("OnQuitGame");
    }

}
