using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneDirector : MonoBehaviour
{
    [SerializeField] Image fadeImage;

    public static SceneDirector Instance { get; private set; }
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

    private void Start()
    {
        StartCoroutine(FadeInCo());
    }

    public void FadeOutSceneChange(string name) { StartCoroutine(FadeOutSceneSwitch(name)); }

    /// FadeIn 
    private IEnumerator FadeInCo()
    {
        float fadeCount = 1;
        fadeImage.gameObject.SetActive(true);

        while (fadeCount > 0.001f)
        {
            fadeCount -= 0.01f;
            yield return new WaitForSeconds(0.01f);
            fadeImage.color = new Color(0, 0, 0, fadeCount);
        }

        fadeImage.gameObject.SetActive(false);
    }

    /// Screen FadeOut & Goto MainScene - ∫∏Ω∫æ¿ ¿Ãµø
    private IEnumerator FadeOutSceneSwitch(string scenename)
    {
        fadeImage.gameObject.SetActive(true);

        float fadeCount = 0;
        while (fadeCount < 1.0f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.02f);
            fadeImage.color = new Color(0, 0, 0, fadeCount);
        }

        yield return new WaitForSeconds(2f);
        SceneSwitch.Instance.SceneSwithcing(scenename);
        yield return null;
    }
}
