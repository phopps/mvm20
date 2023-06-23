using Godot;

public partial class Game : Node
{
    // Global autoload singleton containing important logic and data such as game, player, and level states
    public static Game game = new Game();

    // Global game state - TODO: Rename to GAME_STATE
    public enum GAME_STATE { NONE, LOAD, IDLE, PREP, SHOT, PAUSED }
    public GAME_STATE statePrevious = GAME_STATE.NONE;
    public GAME_STATE state = GAME_STATE.LOAD;
    public GAME_STATE stateNext = GAME_STATE.NONE;

    public override void _Ready()
    {
        statePrevious = state;
        state = GAME_STATE.LOAD;
        GD.Print("[", state, "] Game ready.");
    }
}
