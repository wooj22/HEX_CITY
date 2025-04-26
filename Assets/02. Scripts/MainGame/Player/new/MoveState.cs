using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  Player Movement FSMÀÇ BaseState  */
/* Idle, Walk, Run, Crouch, Jump, Climb */
public class MoveState : MonoBehaviour
{
    protected Player2 player;

    public MoveState(Player2 player)
    {
        this.player = player;
    }

    public virtual void Enter() { } 
    public virtual void HandleInput() { } 
    public virtual void LogicUpdate() { }
    public virtual void Exit() { } 
}
