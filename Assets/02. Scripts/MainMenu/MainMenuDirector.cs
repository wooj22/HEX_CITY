using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class MainMenuDirector : MonoBehaviour
{
    // fade
    [SerializeField] Image fadeImage;

    // light 연출
    [SerializeField] Light2D light;
    [SerializeField] float lightLimit;
    [SerializeField] float lightSpeed;

    private void Start()
    {
        fadeImage.gameObject.SetActive(false);
        StartCoroutine(lightDirector());
    }

    IEnumerator lightDirector()
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

    // fadeOut 후 씬 전환
    public void GoToScene(string scenename)
    {
        StartCoroutine(FadeOutCo(scenename));
    }

    IEnumerator FadeOutCo(string scenename)
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
