using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    private AttackType curAttackType;
    public enum AttackType
    {
        STANDING,   // idle, walk
        RUNNING,    // run
        CROUCHING   // crouching
    }

    [Header("Stat")]
    [SerializeField] private float attackCooltime;

    [Header ("Asset")]
    [SerializeField] private GameObject bulelt;
    [SerializeField] private GameObject specialBulelt;
    [SerializeField] private Transform bulletPosL;
    [SerializeField] private Transform bulletPosR;

    // controll
    private Player player;
    private float attackTimer;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        attackTimer += Time.deltaTime;
    }

    /// Attack (movement states called)
    public void Attack(AttackType type)
    {
        // run attack moveing
        if (player.playerMoveState == Player.PlayerState.Run)
        {
            player.moveX = Input.GetAxis("Horizontal");
            if (player.moveX < 0)
            {
                player.sr.flipX = true;
                player.lastDir = -1;
            }
            // right
            else if (player.moveX > 0)
            {
                player.sr.flipX = false;
                player.lastDir = 1;
            }
            player.rb.velocity = new Vector2(player.lastDir * player.runSpeed, player.rb.velocity.y);
        }

        // coolTime -> shoot
        switch (type)
        {
            case AttackType.STANDING:
                curAttackType = AttackType.STANDING;

                // shoot
                if (attackTimer >= attackCooltime)
                {
                    Shoot();
                    attackTimer = 0;
                }

                break;

            case AttackType.RUNNING:
                curAttackType = AttackType.RUNNING;
                
                // shoot
                if (attackTimer >= attackCooltime)
                {
                    Shoot();
                    attackTimer = 0;
                }
                break;

            case AttackType.CROUCHING:
                curAttackType = AttackType.CROUCHING;

                // shoot
                if (attackTimer >= attackCooltime)
                {
                    Shoot();
                    attackTimer = 0;
                }
                break;

            default:
                break;
        }
    }

    /// Shoot
    private void Shoot()
    {
        // position
        Vector3 bulletPos = (player.lastDir == -1) ? bulletPosL.position : bulletPosR.position;
        bulletPos.y = (curAttackType == AttackType.CROUCHING) ? bulletPos.y - 0.35f : bulletPos.y;

        // shoot
        GameObject playerBullet = Instantiate(bulelt, bulletPos, Quaternion.identity);
        playerBullet.GetComponent<PlayerBullet>().Init(player.lastDir, player.power);
    }

    /*---------------- ¾ÈÇÒµí -----------------------*/
    /// Special Attack (movement states called)
    public void SpecialAttack()
    {

    }

    /// Special Shoot
    private void SpecialShoot()
    {
        
    }
}
