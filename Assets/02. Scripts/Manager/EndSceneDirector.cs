using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSceneDirector : MonoBehaviour
{
    void Start()
    {
        SceneDirector.Instance.FadeIn();
        StartCoroutine(End());
    }

    private IEnumerator End()
    {
        yield return new WaitForSeconds(5f);
        SceneDirector.Instance.FadeOutSceneChange("MainMenu");
    }
}
