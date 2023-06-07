using Godot;

public partial class Pause : CanvasLayer
{
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

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("Pause") && state == Main.STATE.PAUSED)
        {
            statePrevious = state;
            state = stateNext;
            stateNext = Main.STATE.NONE;
            GD.Print("[", state, " <- ", statePrevious, "] Pause button pressed. Unpausing game.");
            Hide();
            GetTree().Paused = false;
        }
    }
}
