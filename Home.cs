using Godot;

public partial class Home : CanvasLayer
{
    [Signal] public delegate void OnPlayGameEventHandler();
    [Signal] public delegate void OnQuitGameEventHandler();

    public void OnPlayPressed()
    {
        GD.Print("Play button pressed. Emitting signal from Home to Main.");
        EmitSignal("OnPlayGame");
    }

    public void OnQuitPressed()
    {
        GD.Print("Quit button pressed. Emitting signal from Home to Main.");
        EmitSignal("OnQuitGame");
    }
}
