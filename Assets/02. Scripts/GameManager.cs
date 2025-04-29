using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    /// MainMapInit
    private void MainMapInit()
    {
        Debug.Log("MainMapInit");

        player.PlayerInit(new Vector3(-7.7f, -4.07f, -1));
        SoundManager.Instance.SetBGM("BGM_MainMap");
        SoundManager.Instance.FadeInBGM();
    }

    // MainMap Clear
    public void MainMapClear()
    {
        player.InitPowerInit();     // power 강화 데이터 저장
        SoundManager.Instance.FadeOutBGM();
        SceneDirector.Instance.FadeOutSceneChange("BossMap");
        Invoke(nameof(BossMapInit), 10f);
    }

    // MainMap Over
    public void MainMapOver()
    {
        SceneSwitch.Instance.SceneReload();
        Invoke(nameof(MainMapInit), 0.5f);
    }

    // BossMapInit
    private void BossMapInit()
    {
        Debug.Log("BossMapInit");

        player.PlayerInit(new Vector3(-8.63f, -4.07f, -1));
        SoundManager.Instance.SetBGM("BGM_Boss");
        SoundManager.Instance.FadeInBGM();
    }

    // BossMap Over
    public void BossMapOver()
    {
        SceneSwitch.Instance.SceneReload();
        Invoke(nameof(BossMapInit), 0.5f);
    }

    // BossMap Clear
    public void BossMapClear()
    {
        SoundManager.Instance.FadeOutBGM();
        SceneDirector.Instance.FadeOutSceneChange("MainManu");
        Destroy(player.gameObject);
    }
}
