using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class MainMenuDirector : MonoBehaviour
{
    // fade
    [SerializeField] Image fadeImage;

    // light ����
    [SerializeField] Light2D light;
    [SerializeField] float lightLimit;
    [SerializeField] float lightSpeed;

    private void Start()
    {
        Time.timeScale = 1f;        // �Ͻ����� �гη� ���ƿ� ���

        fadeImage.gameObject.SetActive(false);
        SoundManager.Instance.SetBGM("BGM_MainMenu");
        SoundManager.Instance.FadeInBGM();
        StartCoroutine(LightDirector());
    }

    IEnumerator LightDirector()
    {
        while (true)
        {
            while (light.intensity < lightLimit)
            {
                light.intensity += 0.1f;
                yield return new WaitForSeconds(lightSpeed);
            }

            while (light.intensity > 1)
            {
                light.intensity -= 0.1f;
                yield return new WaitForSeconds(lightSpeed);
            }
        }
    }

    // fadeOut �� �� ��ȯ
    public void GoToScene(string scenename)
    {
        SoundManager.Instance.FadeOutBGM();
        StartCoroutine(FadeOutSceneSwitch(scenename));
    }

    IEnumerator FadeOutSceneSwitch(string scenename)
    {
        fadeImage.gameObject.SetActive(true);

        float fadeCount = 0;
        while (fadeCount < 1.0f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.02f);
            fadeImage.color = new Color(0, 0, 0, fadeCount);
        }

        SceneSwitch.Instance.SceneSwithcing(scenename);
        yield return null;
    }
}
