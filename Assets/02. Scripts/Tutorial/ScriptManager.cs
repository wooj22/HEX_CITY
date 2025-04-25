using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class ScriptManager : MonoBehaviour
{
    [SerializeField] TutorialDirector director;
    [SerializeField] Text scriptsText;
    [SerializeField] float typingSpeed;     // 타이핑 속도
    [SerializeField] float waitTime;        // 한 줄 보여주는 시간
    [SerializeField] string[] scripts = new string[10];
    private string currentText = "";

    private IEnumerator tutorialCo;

    private void Start()
    {
        StartCoroutine(TutorialScript());
    }

    /// 전체 스크립트 관할 코루틴
    IEnumerator TutorialScript()
    {
        yield return new WaitForSeconds(3f);
        director.TextBoardOn();

        foreach (string line in scripts)
        {
            // 타이핑
            yield return StartCoroutine(TypeLine(line));
            yield return new WaitForSeconds(waitTime);

            // 지우기
            yield return StartCoroutine(EraseText());
        }

        yield return new WaitForSeconds(0.5f);
        director.TextBoardOff();
        yield return new WaitForSeconds(1f);
        director.FadeOut("MainGame");
    }

    /// 타이핑 효과
    IEnumerator TypeLine(string line)
    {
        for (int i = 0; i < line.Length; i++)
        {
            currentText = line.Substring(0, i + 1);
            scriptsText.text = currentText;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    /// 현재 라인 erase
    IEnumerator EraseText()
    {
        while (currentText.Length > 0)
        {
            currentText = currentText.Substring(0, currentText.Length - 1);
            scriptsText.text = currentText;
            yield return new WaitForSeconds(typingSpeed * 0.5f);
        }
    }
}
