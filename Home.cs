using Godot;

public partial class Home : CanvasLayer
{
    [Signal] public delegate void OnPlayGameEventHandler();
    [Signal] public delegate void OnQuitGameEventHandler();
    private Main.STATE state;

    public override void _Ready()
    {
        state = GetNode<Main>("/root/Main").state;
        GD.Print("[", state, "] Home ready.");
    }

    public void OnPlayPressed()
    {
        state = GetNode<Main>("/root/Main").state;
        GD.Print("[", state, "] Play button pressed. Emitting signal from Home to Main.");
        EmitSignal("OnPlayGame");
    }

    public void OnQuitPressed()
    {
        state = GetNode<Main>("/root/Main").state;
        GD.Print("[", state, "] Quit button pressed. Emitting signal from Home to Main.");
        EmitSignal("OnQuitGame");
    }
}
