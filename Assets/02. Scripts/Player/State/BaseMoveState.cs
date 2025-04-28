
/*  Player Movement FSMÀÇ BaseState  */
/* Idle, Walk, Run, Crouch, Jump, Climb */
public class BaseMoveState
{
    protected Player player;

    public BaseMoveState(Player player)
    {
        this.player = player;
    }

    public virtual void Enter() { } 
    public virtual void ChangeStateLogic() { } 
    public virtual void UpdateLigic() { }
    public virtual void Exit() { } 
}
