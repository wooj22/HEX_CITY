
/*  Player Movement FSMÀÇ BaseState  */
/* Idle, Walk, Run, Crouch, Jump, Climb */
public class BaseState
{
    protected Player player;

    public BaseState(Player player)
    {
        this.player = player;
    }

    public virtual void Enter() { } 
    public virtual void ChangeStateLogic() { } 
    public virtual void UpdateLigic() { }
    public virtual void Exit() { } 
}
