using System.IO;
using System.Collections;
using System.Collections.Generic;
using ArabicSupport;
using UnityEngine;
using UnityEngine.UI;

public class hexagonbutton : MonoBehaviour
{
    public Data competition;

    public int numberofquestions;

    public string myletter;
    public string question;
    public string currentcomp;
    public string[] questions;

    public GameObject gamemaster;
    public GameObject questionpanel;
    GameObject questiontext;

    public void SetQuestion(){
        int currentteam = gamemaster.GetComponent<gamemaster>().currentteam;
        int correctchoice = Random.Range(0,3);
        int randomquestionindex = Random.Range(0, numberofquestions);

        question = questions[randomquestionindex];
        GameObject.FindGameObjectWithTag("Question").GetComponent<Text>().text = ArabicFixer.Fix(question, true, true);
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

        competition = gamemaster.GetComponent<gamemaster>().competition;
        currentcomp = competition.compname;
        questions = competition.questions;
        numberofquestions = questions.Length;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
