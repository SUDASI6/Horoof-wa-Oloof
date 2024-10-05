using System.Collections;
using System.Collections.Generic;
using ArabicSupport;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class hexagonbutton : MonoBehaviour
{
    public string myletter;
    public string question;

    public GameObject gamemaster;
    public GameObject questionpanel;
    GameObject questiontext;

    public void SetQuestion(){
        int currentteam = gamemaster.GetComponent<gamemaster>().currentteam;
        int correctchoice = Random.Range(0,3);

        question = myletter + " هذا هو السؤال";
        GameObject.FindGameObjectWithTag("Question").GetComponent<Text>().text = ArabicFixer.Fix(question, false, true);
        questionpanel.GetComponent<Canvas>().enabled = true;
        questionpanel.GetComponent<questionhub>().questionletter = gameObject;
        
        for(int i=0; i < 4; i++){
            if(i == correctchoice)
                questionpanel.GetComponent<questionhub>().SetChoice(i, true, currentteam);
            else 
                questionpanel.GetComponent<questionhub>().SetChoice(i, false, currentteam);
        }

        Debug.Log("button pressed  " + myletter);
    }

    // Start is called before the first frame update
    void Start()
    {   
        questiontext = GameObject.FindGameObjectWithTag("Question");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
