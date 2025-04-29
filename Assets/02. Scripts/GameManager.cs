using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int _palyerHp;
    private int _playerCharge;

    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
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

        SoundManager.Instance.SetBGM("BGM_MainMap");
        SoundManager.Instance.FadeInBGM();
    }

    // BossMapInit
    private void BossMapInit()
    {
        Debug.Log("BossMapInit");
    }

    // MainMap Over
    public void MainMapOver()
    {
        SceneSwitch.Instance.SceneReload();
        Invoke(nameof(MainMapInit), 0.5f);
    }

    // MainMap Clear
    public void MainMapClear()
    {
        SoundManager.Instance.FadeOutBGM();
        SceneDirector.Instance.FadeOutSceneChange("BossMap");
        Invoke(nameof(BossMapInit), 0.5f);
    }

    // BossMap Over
    public void BossMapOver()
    {

    }

    // BossMap Clear
    public void BossMapClear()
    {

    }
}
