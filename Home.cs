using Godot;

public partial class Home : CanvasLayer
{
    [Signal] public delegate void OnPlayGameEventHandler();
    [Signal] public delegate void OnQuitGameEventHandler();
    private Game.GAME_STATE state;

    public override void _Ready()
    {
        GD.Print("[", state, "] Home ready.");
    }

    public void OnPlayPressed()
    {
        GD.Print("[", state, "] Play button pressed. Emitting signal from Home to Main.");
        EmitSignal("OnPlayGame");
    }

    public void OnQuitPressed()
    {
        GD.Print("[", state, "] Quit button pressed. Emitting signal from Home to Main.");
        EmitSignal("OnQuitGame");
    }
}
