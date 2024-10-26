using System.IO;
using System.Collections;
using System.Collections.Generic;
using ArabicSupport;
using UnityEngine;
using UnityEngine.UI;

public class questionhub : MonoBehaviour
{
    public Data comp;
    public gamemaster gamemaster;

    public int currentquestion = -1;

    public GameObject questionletter;
    public GameObject choices;
    
    public Text questiontext;

    public Color[] teamcolor = new Color[2];

    public void TrueAnswer(Color teamcolor){
        questionletter.GetComponent<Image>().color = teamcolor;
        questionletter.GetComponent<Button>().interactable = false;

        gameObject.SetActive(false);

        int currentteam = gamemaster.currentteam;
        gamemaster.currentteam = (currentteam+1)%2;

        gamemaster.correctlyansweredquestions.Add(currentquestion);
    }

    public void FalseAnswer(){
        gameObject.SetActive(false);

        int currentteam = gamemaster.currentteam;
        gamemaster.currentteam = (currentteam+1)%2;

        currentquestion = -1;
    }

    public void SetChoice(int choicenumber, bool correctanswer, int team, string choicetext){
        Transform choice = choices.transform.GetChild(choicenumber);
        choice.GetComponent<Button>().interactable = true;
        if(correctanswer){
            choice.GetChild(0).GetComponent<Text>().text = ArabicFixer.Fix(choicetext, true, true);

            choice.GetComponent<Image>().color = teamcolor[0];
            choice.GetComponent<Button>().onClick.RemoveAllListeners();
            choice.GetComponent<Button>().onClick.AddListener(() => {TrueAnswer(teamcolor[team]);});
        } else {
            choice.GetChild(0).GetComponent<Text>().text = ArabicFixer.Fix(choicetext, true, true);
            
            choice.GetComponent<Image>().color = teamcolor[1];
            choice.GetComponent<Button>().onClick.RemoveAllListeners();
            choice.GetComponent<Button>().onClick.AddListener(() => {FalseAnswer();});
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        comp = gamemaster.comp;

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
