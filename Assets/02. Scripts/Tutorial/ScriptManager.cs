using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class ScriptManager : MonoBehaviour
{
    [SerializeField] TutorialDirector director;
    [SerializeField] Text scriptsText;
    [SerializeField] float typingSpeed;     // Ÿ���� �ӵ�
    [SerializeField] float waitTime;        // �� �� �����ִ� �ð�
    [SerializeField] string[] scripts = new string[10];
    private string currentText = "";

    private IEnumerator tutorialCo;

    private void Start()
    {
        StartCoroutine(TutorialScript());
    }

    /// ��ü ��ũ��Ʈ ���� �ڷ�ƾ
    IEnumerator TutorialScript()
    {
        yield return new WaitForSeconds(3f);
        director.TextBoardOn();

        foreach (string line in scripts)
        {
            // Ÿ����
            yield return StartCoroutine(TypeLine(line));
            yield return new WaitForSeconds(waitTime);

            // �����
            yield return StartCoroutine(EraseText());
        }

        yield return new WaitForSeconds(0.5f);
        director.TextBoardOff();
        yield return new WaitForSeconds(1f);
        director.FadeOut("MainGame");
    }

    /// Ÿ���� ȿ��
    IEnumerator TypeLine(string line)
    {
        for (int i = 0; i < line.Length; i++)
        {
            currentText = line.Substring(0, i + 1);
            scriptsText.text = currentText;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    /// ���� ���� erase
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
