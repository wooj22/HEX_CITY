using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{   
    public Player player;

    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        MainMapInit();
        SoundManager.Instance.SetBGM("BGM_MainMap");
        SoundManager.Instance.FadeInBGM();
    }

    /// MainMap Init - player, monster 초기화
    private void MainMapInit()
    {
        player.PlayerInit(new Vector3(-7.7f, -4.07f, -1));
        MonsterManager.Instance.MonsterInit();

        SceneDirector.Instance.FadeIn();
    }

    /// Player Die 로그라이크
    public void PlayerDie()
    {
        string sceneName = SceneSwitch.Instance.GetCurrentScene();
        if(sceneName == "MainMap")
        {
            MainMapOver();
        }
        else if (sceneName == "BossMap")
        {
            BossMapOver();
        }
    }

    /// MainMap Over
    public void MainMapOver()
    {
        Invoke(nameof(MainMapInit), 0.5f);
    }

    /// MainMap Clear
    public void MainMapClear()
    {
        SoundManager.Instance.FadeOutBGM();
        SceneDirector.Instance.FadeOutSceneChange("BossMap");

        // main -> boss 최초 1회 로드
        Invoke(nameof(PlayerBossMapSet), 4.5f);
        Invoke(nameof(BossMapInit), 4.7f);
    }

    /// Player Boss Map Setting (1회)
    private void PlayerBossMapSet()
    {
        player.PlayerBossMapSetting();
        GameObject.Find("Virtual Camera").GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = player.transform;
    }

    /// BossMapInit - player, boss 초기화
    private void BossMapInit()
    {
        player.PlayerInit(new Vector3(-8.63f, -4.07f, -1));
        MonsterManager.Instance.MonsterInit();

        SceneDirector.Instance.FadeIn();
        SoundManager.Instance.SetBGM("BGM_Boss");
        SoundManager.Instance.FadeInBGM();
    }

    /// BossMap Over
    public void BossMapOver()
    {
        Invoke(nameof(BossMapInit), 0.5f);
    }

    /// BossMap Clear - BossController Called
    public void BossMapClear()
    {
        SoundManager.Instance.FadeOutBGM();
        SceneDirector.Instance.FadeOutSceneChange("MainManu");
    }
}
