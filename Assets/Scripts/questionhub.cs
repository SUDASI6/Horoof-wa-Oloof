using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class questionhub : MonoBehaviour
{
    public GameObject questionletter;
    GameObject questiontext;

    public Color correct, wrong;

    public void TrueAnswer(){
        questionletter.GetComponent<Image>().color = correct;
        gameObject.GetComponent<Canvas>().enabled = false;

        Debug.Log("True Answer!!");
    }

    public void FalseAnswer(){
        questionletter.GetComponent<Image>().color = wrong;
        gameObject.GetComponent<Canvas>().enabled = false;

        Debug.Log("False Answer!!");
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
