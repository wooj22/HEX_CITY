using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState : MonoBehaviour
{
    protected Player player;
    protected PlayerStateManager stateManager;

    protected BaseState(Player player, PlayerStateManager stateManager)
    {
        this.player = player;
        this.stateManager = stateManager;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void HandleInput() { }
    public virtual void LogicUpdate() { }
}
