using Godot;

public partial class Pause : CanvasLayer
{
    [Signal] public delegate void OnResumeGameEventHandler();
    [Signal] public delegate void OnQuitGameEventHandler();
    private Main.STATE state;
    private Main.STATE statePrevious;
    private Main.STATE stateNext;

    public override void _Ready()
    {
        statePrevious = GetNode<Main>("/root/Main").statePrevious;
        stateNext = GetNode<Main>("/root/Main").stateNext;
        state = GetNode<Main>("/root/Main").state;
        GD.Print("[", state, "] Pause ready.");
    }

    public void OnResumePressed()
    {
        state = GetNode<Main>("/root/Main").state;
        GD.Print("[", state, "] Resume button pressed. Emitting signal from Pause to Main.");
        EmitSignal("OnResumeGame");
    }

    public void OnQuitPressed()
    {
        EmitSignal("OnQuitGame");
    }

}
