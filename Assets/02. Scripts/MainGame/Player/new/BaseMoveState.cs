
/*  Player Movement FSM�� BaseState  */
/* Idle, Walk, Run, Crouch, Jump, Climb */
public class BaseMoveState
{
    protected Player player;

    public BaseMoveState(Player player)
    {
        this.player = player;
    }

    public virtual void Enter() { } 
    public virtual void HandleInput() { } 
    public virtual void LogicUpdate() { }
    public virtual void Exit() { } 
}
