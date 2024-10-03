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

    public GameObject questionpanel;
    GameObject questiontext;

    public void SetQuestion(){
        question = myletter + " هذا هو السؤال";
        GameObject.FindGameObjectWithTag("Question").GetComponent<Text>().text = ArabicFixer.Fix(question, false, false);
        questionpanel.GetComponent<Canvas>().enabled = true;
        questionpanel.GetComponent<questionhub>().questionletter = gameObject;


        Debug.Log("button pressed");
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
