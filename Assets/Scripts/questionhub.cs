using System.Collections;
using System.Collections.Generic;
using ArabicSupport;
using UnityEngine;
using UnityEngine.UI;

public class questionhub : MonoBehaviour
{
    public GameObject questionletter;
    public GameObject choices;
    GameObject questiontext;

    public Color[] teamcolor = new Color[2];

    public void TrueAnswer(Color teamcolor){
        questionletter.GetComponent<Image>().color = teamcolor;
        gameObject.GetComponent<Canvas>().enabled = false;

        int currentteam = questionletter.GetComponentInParent<gamemaster>().currentteam;
        questionletter.GetComponentInParent<gamemaster>().currentteam = (currentteam+1)%2;

        Debug.Log("True Answer!!");
    }

    public void FalseAnswer(){
        gameObject.GetComponent<Canvas>().enabled = false;

        int currentteam = questionletter.GetComponentInParent<gamemaster>().currentteam;
        questionletter.GetComponentInParent<gamemaster>().currentteam = (currentteam+1)%2;

        Debug.Log("False Answer!!");
    }

    public void SetChoice(int choicenumber, bool correctanswer, int team){
        Transform choice = choices.transform.GetChild(choicenumber);
        if(correctanswer){
            choice.GetComponent<Image>().color = teamcolor[0];
            choice.GetComponent<Button>().onClick.AddListener(() => {TrueAnswer(teamcolor[team]);});
        } else {
            choice.GetComponent<Image>().color = teamcolor[1];
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
