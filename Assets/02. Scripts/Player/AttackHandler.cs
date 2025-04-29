using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    [SerializeField] private AttackType curAttackType;
    public enum AttackType
    {
        STANDING,   // idle, walk
        RUNNING,    // run
        CROUCHING   // crouching
    }

    [Header("Stat")]
    [SerializeField] private float attackCooltime;

    [Header ("Asset")]
    [SerializeField] private GameObject buleltPrefab;
    [SerializeField] private GameObject specialBulelt;
    [SerializeField] private Transform bulletPosL;
    [SerializeField] private Transform bulletPosR;
    [SerializeField] private Transform bulletParent;

    // bullet pulling
    private List<GameObject> bulletPool = new List<GameObject>();
    private List<GameObject> specialBulletPool = new List<GameObject>();
    private int poolSize = 30;
    private int specialPoolSize = 5;

    // controll
    private Player player;
    private float attackTimer;

    private void Awake()
    {
        AttackHandlerInit();
    }

    private void Update()
    {
        attackTimer += Time.deltaTime;
    }

    public void AttackHandlerInit()
    {
        player = GetComponent<Player>();
        bulletParent = GameObject.Find("Bullets").transform;
        BulletPuling();
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
    }

    /// Special Attack (movement states called)
    public void SpecialAttack(AttackType type)
    {
        // coolTime -> shoot
        switch (type)
        {
            case AttackType.STANDING:
                curAttackType = AttackType.STANDING;

                // shoot
                if (player.isChargeMax) SpecialShoot();
                else PlayerUIManager.Instance.ChargeShortFall();
                break;

            case AttackType.RUNNING:
                curAttackType = AttackType.RUNNING;

                // shoot
                if (player.isChargeMax) SpecialShoot();
                else PlayerUIManager.Instance.ChargeShortFall();
                break;

            case AttackType.CROUCHING:
                curAttackType = AttackType.CROUCHING;

                // shoot
                if (player.isChargeMax) SpecialShoot();
                else PlayerUIManager.Instance.ChargeShortFall();
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
        GameObject bullet = ActivateBullet();
        if(bullet != null)
        {
            bullet.transform.position = bulletPos;
            bullet.GetComponent<PlayerBullet>().Init(player.lastDir, player.power);
            bullet.SetActive(true);
            SoundManager.Instance.PlaySFX("SFX_Shoot");
        }
    }

    /// Special Shoot
    private void SpecialShoot()
    {
        // position
        Vector3 bulletPos = (player.lastDir == -1) ? bulletPosL.position : bulletPosR.position;
        bulletPos.y = (curAttackType == AttackType.CROUCHING) ? bulletPos.y - 0.35f : bulletPos.y;

        GameObject bullet = AtctiveSpecialBullet();
        if (bullet != null)
        {
            bullet.transform.position = bulletPos;
            bullet.GetComponent<PlayerBullet>().Init(player.lastDir, player.power * 2);     // damage 2น่
            bullet.SetActive(true);
            SoundManager.Instance.PlaySFX("SFX_SpecialShoot");
            player.ChargeInit();
        }
    }

    /// Bullet Object Pooling
    private void BulletPuling()
    {
        bulletPool.Clear();
        specialBulletPool.Clear();

        // bullet pool
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(buleltPrefab);
            bullet.SetActive(false);
            bullet.transform.SetParent(bulletParent);
            bulletPool.Add(bullet);
        }

        // special bullet pool
        for (int i = 0; i < specialPoolSize; i++)
        {
            GameObject specialBullet = Instantiate(specialBulelt);
            specialBullet.SetActive(false);
            specialBullet.transform.SetParent(bulletParent);
            specialBulletPool.Add(specialBullet);
        }
    }

    /// Activate Bullet
    private GameObject ActivateBullet()
    {
        for(int i = 0; i < bulletPool.Count; i++)
        {
            if (!bulletPool[i].activeSelf)
            {
                return bulletPool[i];
            }
        }
        return null;
    }

    /// Activate Special Bullet
    private GameObject AtctiveSpecialBullet()
    {
        for (int i = 0; i < specialBulletPool.Count; i++)
        {
            if (!specialBulletPool[i].activeSelf)
            {
                return specialBulletPool[i];
            }
        }
        return null;
    }
}
