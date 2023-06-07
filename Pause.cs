using Godot;

public partial class Pause : CanvasLayer
{
    private string state;

    public override void _Ready()
    {
        state = GetNode<Main>("/root/Main").state;
        GD.Print("[", state, "] Pause ready.");
    }
}
