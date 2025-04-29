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
        StartCoroutine(FadeIn());
        SoundManager.Instance.SetBGM("BGM_Tutorial");
        SoundManager.Instance.FadeInBGM();
    }

    public void TextBoardOn() { StartCoroutine(TextBoardOnCo()); }
    public void TextBoardOff() { StartCoroutine(TextBoardOffCo()); }
    public void FadeOut(string name) 
    { 
        SoundManager.Instance.FadeOutBGM();
        StartCoroutine(FadeOutSceneSwitch(name)); 
    }

    /// Text Borad On
    private IEnumerator TextBoardOnCo()
    {
        float fadeCount = 0;
        textBoardImage.gameObject.SetActive(true);

        while (fadeCount < 0.5f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f);
            textBoardImage.color = new Color(255, 255, 255, fadeCount);
        }
    }

    /// Text Borad Off
    private IEnumerator TextBoardOffCo()
    {
        float fadeCount = 0.5f;
        textBoardImage.gameObject.SetActive(true);

        while (fadeCount > 0.001f)
        {
            fadeCount -= 0.01f;
            yield return new WaitForSeconds(0.01f);
            textBoardImage.color = new Color(255, 255, 255, fadeCount);
        }

        textBoardImage.gameObject.SetActive(false);
    }

    /// Screen FadeIn
    private IEnumerator FadeIn()
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

    /// Screen FadeOut & Goto MainScene
    private IEnumerator FadeOutSceneSwitch(string scenename)
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
