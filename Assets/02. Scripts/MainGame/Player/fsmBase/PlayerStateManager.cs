using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public Player player;
    public BaseState currentState;

    public PlayerStateManager(Player player)
    {
        this.player = player;
    }

    public void Update()
    {
        currentState?.HandleInput();
        currentState?.LogicUpdate();
    }

    public void Initialize(BaseState startState)
    {
        currentState = startState;
        currentState.Enter();
    }

    public void ChangeState(BaseState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
