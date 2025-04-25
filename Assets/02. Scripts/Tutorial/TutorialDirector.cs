using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialDirector : MonoBehaviour
{
    [SerializeField] Image fadeImage;
    [SerializeField] Image textBoardImage;

    private void Start()
    {
        fadeImage.gameObject.SetActive(true);
        textBoardImage.gameObject.SetActive(false);
        StartCoroutine(FadeIn());
    }

    /// Screen FadeIn
    IEnumerator FadeIn()
    {
        float fadeCount = 1;
        while (fadeCount > 0.001f)
        {
            fadeCount -= 0.01f;
            yield return new WaitForSeconds(0.01f);
            fadeImage.color = new Color(0, 0, 0, fadeCount);
        }

        fadeImage.gameObject.SetActive(false);

        yield return new WaitForSeconds(3f);
        textBoardImage.gameObject.SetActive(true);
        StartCoroutine(TextBoardOn());
    }

    /// Text Borad On
    IEnumerator TextBoardOn()
    {
        float fadeCount = 0;
        while (fadeCount < 0.5f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f);
            textBoardImage.color = new Color(255, 255, 255, fadeCount);
        }
    }

    /// Text Borad Off
    IEnumerator TextBoardOff()
    {
        float fadeCount = 0.5f;
        while (fadeCount > 0.001f)
        {
            fadeCount -= 0.01f;
            yield return new WaitForSeconds(0.01f);
            textBoardImage.color = new Color(255, 255, 255, fadeCount);
        }

        textBoardImage.gameObject.SetActive(false);
    }

    /// Screen FadeOut & Goto MainScene
    IEnumerator FadeOutSceneSwitch(string scenename)
    {
        fadeImage.gameObject.SetActive(true);

        float fadeCount = 0;
        while (fadeCount < 1.0f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f);
            fadeImage.color = new Color(0, 0, 0, fadeCount);
        }

        SceneSwitch.Instance.SceneSwithcing(scenename);
        yield return null;
    }
}
