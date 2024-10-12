using System.IO;
using System.Collections;
using System.Collections.Generic;
using ArabicSupport;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gamemaster : MonoBehaviour
{
    public Data competition;

    public int currentteam = 0;

    public string currentcomp;
    List<string> letters = new List<string>() {"أ","ب","ت","ث","ج","ح","خ","د","ذ","ر","ز","س","ش","ص","ض","ط","ظ","ع","غ","ف","ق","ك","ل","م","ن","ه","و","ي"};
    
    public GameObject hexagon;
    public GameObject questionpanel;

    public Image teamturn;

    public Color[] colors = new Color[2];

    // Start is called before the first frame update
    void Start()
    {
        string jsoncontent = File.ReadAllText(Application.dataPath + "/CompetitionToLoadInfo.json");
        Data compinfo = JsonUtility.FromJson<Data>(jsoncontent);

        currentcomp = compinfo.compname;

        jsoncontent = File.ReadAllText(Application.dataPath + "/Competitions/" + currentcomp + ".json");
        competition = JsonUtility.FromJson<Data>(jsoncontent);

        // Set Letters On Hexagons
        for(int i=0; i < 25; i++){
            hexagonbutton hexbutton = hexagon.transform.GetChild(i).GetComponent<hexagonbutton>();
            Text text = hexagon.transform.GetChild(i).GetChild(0).GetComponent<Text>();
            int x = Random.Range(0,28-i);

            hexbutton.questionpanel = questionpanel;
            hexbutton.myletter = letters[x];
            text.text = ArabicFixer.Fix(letters[x], true, true);

            letters.Remove(letters[x]);

            hexbutton.competition = competition;
            hexbutton.currentcomp = competition.compname;
            hexbutton.questions = competition.questions;
            hexbutton.numberofquestions = competition.questions.Length;
            
            Debug.Log(hexbutton.myletter);
        }
    }

    // Update is called once per frame
    void Update()
    {
        teamturn.color = colors[currentteam];
    }
}
