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

        // animation flip 재적용
        //if (player.lastDir == -1) player.sr.flipX = true;
        //else if (player.lastDir == 1) player.sr.flipX = false;

        // 돌아가는거 확인. 근데 moveX에 따른 이동은 안댐. 정 안되면 여기 이동로직 작성
        //if (Input.GetKey(KeyCode.LeftArrow))
        //    player.sr.flipX = true;
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
        bulletPos.y = (curAttackType == AttackType.CROUCHING) ? bulletPos.y - 0.35f : bulletPos.y;

        // shoot
        GameObject playerBullet = Instantiate(bulelt, bulletPos, Quaternion.identity);
        playerBullet.GetComponent<PlayerBullet>().Init(player.lastDir, player.power);
    }

    /// Special Shoot
    private void SpecialShoot()
    {
        
    }
}
