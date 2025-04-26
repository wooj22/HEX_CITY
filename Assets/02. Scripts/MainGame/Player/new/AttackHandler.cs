using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    private AttackType curentAttackType;
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

    private Player2 player;

    private void Awake()
    {
        player = GetComponent<Player2>();
    }

    /// Attack (movement states called)
    public void Attack(AttackType type)
    {
        player.isAttack = true;

        switch (type)
        {
            case AttackType.STANDING:
                curentAttackType = AttackType.STANDING;
                // animation
                // shoot
                if (player.attackTimer >= attackCooltime)
                {
                    Shoot();
                    player.attackTimer = 0;
                }

                break;

            case AttackType.RUNNING:
                curentAttackType = AttackType.STANDING;
                // animation
                // shoot
                if (player.attackTimer >= attackCooltime)
                {
                    Shoot();
                    player.attackTimer = 0;
                }
                break;

            case AttackType.CROUCHING:
                curentAttackType = AttackType.STANDING;
                // animation
                // shoot
                if (player.attackTimer >= attackCooltime)
                {
                    Shoot();
                    player.attackTimer = 0;
                }
                break;

            default:
                break;
        }
    }

    /// Special Attack (movement states called)
    public void SpecialAttack()
    {
        
    }

    /// Shoot
    private void Shoot()
    {
        // position
        Vector3 bulletPos = (player.lastDir == -1) ? bulletPosL.position : bulletPosR.position;
        bulletPos.y = (curentAttackType == AttackType.CROUCHING) ? bulletPos.y - 0.5f : bulletPos.y;

        // shoot
        GameObject playerBullet = Instantiate(bulelt, bulletPos, Quaternion.identity);
        playerBullet.GetComponent<PlayerBullet>().Init(player.lastDir, player.power);
    }

    /// Special Shoot
    private void SpecialShoot()
    {
        
    }
}
