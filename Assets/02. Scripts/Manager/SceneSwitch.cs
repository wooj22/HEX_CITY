using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    public static SceneSwitch Instance { get; private set; }
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

    public void SceneSwithcing(string sceneName)
    {
        // 만약 Menu로 돌아간다면 GameManager, Player Destroy
        if (sceneName == "MainMenu" || sceneName == "EndMap")
        {
            if(GameManager.Instance != null)
                Destroy(GameManager.Instance.gameObject);
            if (Player.Instance != null)
                Destroy(Player.Instance.gameObject);
        }

        SceneManager.LoadScene(sceneName);
    }

    public void SceneReload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public string GetCurrentScene()
    {
        return SceneManager.GetActiveScene().name;
    }
}
