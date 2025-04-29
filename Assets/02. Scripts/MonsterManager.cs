using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMapMoster;
    private GameObject monsters;

    public static MonsterManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void MonsterInit()
    {
        if(monsters != null)
        {
            Destroy(monsters);
        }

        monsters = Instantiate(mainMapMoster, mainMapMoster.transform.position, Quaternion.identity);
        monsters.transform.SetParent(this.transform);
    }

    public void MonsterClear()
    {
        Destroy(monsters);
    }
}
