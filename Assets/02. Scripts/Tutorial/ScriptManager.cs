using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptManager : MonoBehaviour
{
    [SerializeField] string[] scripts = new string[10];
    [SerializeField] Text scriptsText;
    [SerializeField] TutorialDirector director;
    private int currentIndex = 0;

    private void Start()
    {
        StartCoroutine(TutorialScript());
    }

    IEnumerator TutorialScript()
    {
        yield return new WaitForSeconds(3f);
        director.TextBoardOn();
        
    }

    public void Scripting()
    {

    }
}
