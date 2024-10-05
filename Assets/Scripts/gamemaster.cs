using System.Collections;
using System.Collections.Generic;
using ArabicSupport;
using UnityEngine;
using UnityEngine.UI;

public class gamemaster : MonoBehaviour
{
    public int currentteam = 0;

    List<string> letters = new List<string>() {"أ","ب","ت","ث","ج","ح","خ","د","ذ","ر","ز","س","ش","ص","ض","ط","ظ","ع","غ","ف","ق","ك","ل","م","ن","ه","و","ي"};
    private string[] usedletters = new string[28];
    
    public GameObject hexagon;
    public GameObject questionpanel;

    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i < 25; i++){
            hexagonbutton hexbutton = hexagon.transform.GetChild(i).GetComponent<hexagonbutton>();
            Text text = hexagon.transform.GetChild(i).GetChild(0).GetComponent<Text>();
            int x = Random.Range(0,28-i);

            hexbutton.questionpanel = questionpanel;
            hexbutton.myletter = letters[x];
            text.text = ArabicFixer.Fix(letters[x], false, true);

            letters.Remove(letters[x]);
            
            Debug.Log(hexbutton.myletter);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
