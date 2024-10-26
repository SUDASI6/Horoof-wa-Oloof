using System.IO;
using System.Collections;
using System.Collections.Generic;
using ArabicSupport;
using UnityEngine;
using UnityEngine.UI;

public class hexagonbutton : MonoBehaviour
{
    public Data comp;
    public gamemaster gamemaster;
    public questionhub questionhub;

    public string myletter;
    public string question;

    public GameObject questionpanel;

    public void SetQuestion(){
        int currentteam = gamemaster.currentteam;
        int emptychoices = 0;

        bool newquestion = false;
        int randomquestionindex = -1;
        int timer = 10000;

        // Check if there is a non asked question
        while(!newquestion && gamemaster.askedquestions.Count < comp.questions.Length){
            randomquestionindex = Random.Range(0, comp.questions.Length);

            newquestion = true;
            foreach(int questionindex in gamemaster.askedquestions){
                if(randomquestionindex == questionindex){
                    newquestion = false;
                    break;
                }
            }

            if(newquestion){
                questionhub.currentquestion = randomquestionindex;
                gamemaster.askedquestions.Add(randomquestionindex);
                break;
            } else questionhub.currentquestion = -1;

            timer--;
            if(timer <= 0){
                Debug.Log("unable to find a non asked question after 10000 tries");
                break;
            }
        }
        timer = 10000;
        
        // Check if there is a not correctly answered question
        while(!newquestion && gamemaster.correctlyansweredquestions.Count < comp.questions.Length){
            randomquestionindex = Random.Range(0, comp.questions.Length);

            newquestion = true;
            foreach(int questionindex in gamemaster.correctlyansweredquestions){
                if(randomquestionindex == questionindex){
                    newquestion = false;
                    break;
                }
            }

            if(newquestion){
                questionhub.currentquestion = randomquestionindex;
                break;
            } else questionhub.currentquestion = -1;

            timer--;
            if(timer <= 0){
                Debug.Log("unable to find a non answered question after 10000 tries");
                break;
            }
        }

        if(newquestion){
            for(int i=0; i < 4; i++){
                if(comp.choices[randomquestionindex*4 + i] == "") emptychoices++;

                questionhub.choices.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "";
                questionhub.choices.transform.GetChild(i).GetComponent<Button>().interactable = false;
                questionhub.choices.transform.GetChild(i).GetComponent<Image>().color = Color.white;
                questionhub.choices.transform.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
            }
            int correctchoice = Random.Range(0,4-emptychoices);

            question = comp.questions[randomquestionindex];
            questionhub.questiontext.text = ArabicFixer.Fix(question, true, true);
            questionhub.questionletter = gameObject;
            
            questionpanel.SetActive(true);

            List<string> choosenchoices = new List<string>() {comp.choices[randomquestionindex*4]};
            // Set Correct Choice
            questionhub.SetChoice(correctchoice, true, currentteam, comp.choices[randomquestionindex*4]);
            // Set Other Choices
            for(int i=0; i < 4-emptychoices; i++){
                if(i != correctchoice){
                    int randchoice = 1;
                    bool choosen = true;
                    
                    int time = 10000;
                    while(choosen){
                        choosen = false;
                        time--;
                        if(time <= 0){
                            choosen = true;
                            Debug.Log("Couldn't Find A " + (i+1) + " Not Choosen Choice After 10000 Tries");
                            break;
                        }

                        randchoice = Random.Range(1,4-emptychoices);
                        if(comp.choices[randomquestionindex*4 + randchoice] == "")
                            choosen = true;

                        foreach(string choosenchoice in choosenchoices)
                            if(comp.choices[randomquestionindex*4 + randchoice] == choosenchoice)
                                choosen = true;
                    }
                    if(!choosen){
                        string choicetext = comp.choices[randomquestionindex*4 + randchoice];
                        
                        choosenchoices.Add(choicetext);
                        questionhub.SetChoice(i, false, currentteam, choicetext);
                    } else Debug.Log("Choice " + (i+1) + " Is Empty");
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
