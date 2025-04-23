using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Stat")]
    [SerializeField] public int hp;
    [SerializeField] public int power;
    //[SerializeField] public float speed;

    [Header ("State")]
    [SerializeField] public PlayerState curState = PlayerState.IDLE;
    [SerializeField] public PlayerState preState;
    [SerializeField] public PlayerMoveState moveState;
    [SerializeField] public PlayerAttackState attackState;
    [SerializeField] private bool isLadderIn;
    [SerializeField] private bool isChargeMax;

    public enum PlayerState { IDLE, MOVE, CLIMB, ATTACK, HIT, DIE, NONE }
    public enum PlayerMoveState { WALK, RUN, JUMP, NONE }
    public enum PlayerAttackState { ATTACK, RUNATTACK, SPECIALATTACK, NONE }

    [Header("Key Bindings")]
    [SerializeField] public KeyCode moveL = KeyCode.LeftArrow;
    [SerializeField] public KeyCode moveR = KeyCode.RightArrow;
    [SerializeField] public KeyCode run = KeyCode.LeftShift;
    [SerializeField] public KeyCode climbUp = KeyCode.UpArrow;
    [SerializeField] public KeyCode climbDown = KeyCode.DownArrow;
    [SerializeField] public KeyCode jump = KeyCode.Space;
    [SerializeField] public KeyCode attack = KeyCode.F;
    [SerializeField] public KeyCode specialAttack = KeyCode.G;


    private void Update()
    {
        StateUpdate();
    }

    void StateUpdate()
    {
        // move
        if ((Input.GetKey(moveL) || Input.GetKey(moveL)) && 
            curState != PlayerState.ATTACK && 
            curState != PlayerState.CLIMB &&
            curState != PlayerState.HIT)
        {
            preState = curState;
            curState = PlayerState.MOVE;

            // move state setting
            if (Input.GetKey(run))
                moveState = PlayerMoveState.RUN;
            else
                moveState = PlayerMoveState.WALK;
        }

        // climb      * 사다리와 닿아있는지 검사 추가 필요
        if ((Input.GetKey(climbDown) || Input.GetKey(climbUp) && isLadderIn) {
            preState = curState;
            curState = PlayerState.CLIMB;
        }

        // attack
        if (Input.GetKey(attack) && (curState == PlayerState.IDLE || curState == PlayerState.MOVE))
        {
            preState = curState;
            curState = PlayerState.ATTACK;

            // attack state setting
            if (moveState == PlayerMoveState.RUN)
                attackState = PlayerAttackState.RUNATTACK;
            else
                attackState = PlayerAttackState.ATTACK;
        }

        // special attack
        if (Input.GetKey(specialAttack) && isChargeMax &&
            (curState == PlayerState.IDLE || curState == PlayerState.MOVE))
        {
            preState = curState;
            curState = PlayerState.ATTACK;
            attackState = PlayerAttackState.SPECIALATTACK;
        }

        if()
    }

    /*-------------- event ---------------*/
    private void Hit(int damage)
    {
        hp -= damage;

        if (hp < 0)
        {
            hp = 0;
            preState = curState;
            curState = PlayerState.DIE;
        }
        else
        {
            preState = curState;
            curState = PlayerState.HIT;
        }
    }
}
