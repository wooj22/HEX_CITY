using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MainMenuDirector : MonoBehaviour
{
    // light ����
    [SerializeField] Light2D light;
    [SerializeField] float lightLimit;
    [SerializeField] float lightSpeed;

    private void Start()
    {
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
}
