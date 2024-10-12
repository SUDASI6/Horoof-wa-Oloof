using System.IO;
using System.Collections;
using System.Collections.Generic;
using ArabicSupport;
using UnityEngine;
using UnityEngine.UI;

public class questionhub : MonoBehaviour
{
    public GameObject gamemaster;
    public GameObject questionletter;
    public GameObject choices;
    GameObject questiontext;

    public Color[] teamcolor = new Color[2];

    public void TrueAnswer(Color teamcolor){
        questionletter.GetComponent<Image>().color = teamcolor;
        questionletter.GetComponent<Button>().interactable = false;

        gameObject.GetComponent<Canvas>().enabled = false;

        int currentteam = gamemaster.GetComponent<gamemaster>().currentteam;
        gamemaster.GetComponent<gamemaster>().currentteam = (currentteam+1)%2;

        Debug.Log("True Answer!! " + ((currentteam == 0) ? "green " : "red ") + questionletter.GetComponent<hexagonbutton>().myletter);
    }

    public void FalseAnswer(){
        gameObject.GetComponent<Canvas>().enabled = false;

        int currentteam = gamemaster.GetComponent<gamemaster>().currentteam;
        gamemaster.GetComponent<gamemaster>().currentteam = (currentteam+1)%2;

        Debug.Log("False Answer!! " + ((currentteam == 0) ? "green " : "red ") + questionletter.GetComponent<hexagonbutton>().myletter);
    }

    public void SetChoice(int choicenumber, bool correctanswer, int team){
        string choicetext;
        Transform choice = choices.transform.GetChild(choicenumber);
        if(correctanswer){
            choicetext = "هذه الاجابة الصحيحة " + (choicenumber+1);
            choice.GetChild(0).GetComponent<Text>().text = ArabicFixer.Fix(choicetext, true, true);

            choice.GetComponent<Image>().color = teamcolor[0];
            choice.GetComponent<Button>().onClick.RemoveAllListeners();
            choice.GetComponent<Button>().onClick.AddListener(() => {TrueAnswer(teamcolor[team]);});
        } else {
            choicetext = "هذه الاجابة خاطئة " + (choicenumber+1);
            choice.GetChild(0).GetComponent<Text>().text = ArabicFixer.Fix(choicetext, true, true);
            
            choice.GetComponent<Image>().color = teamcolor[1];
            choice.GetComponent<Button>().onClick.RemoveAllListeners();
            choice.GetComponent<Button>().onClick.AddListener(() => {FalseAnswer();});
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        questiontext = GameObject.FindGameObjectWithTag("Question");
        gameObject.GetComponent<Canvas>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
