using System.IO;
using System.Collections;
using System.Collections.Generic;
using ArabicSupport;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class startmenu : MonoBehaviour
{
    int currentquestionnum = 0;
    int currentcompnum = 0;

    public List<string> questionslist = new List<string>();


    public GameObject startpanel;
    public GameObject newpanel;
    public GameObject loadpanel;
    public GameObject questionspanel;
    public GameObject compspanel;

    public GameObject questionprefab;
    public GameObject compprefab;
    
    public InputField questioninput;
    public InputField comptitleinput;
    
    public Button savecompbtn;
    public Button addquestionbtn;


    public void NewCompPanel(){
        startpanel.SetActive(false);
        newpanel.SetActive(true);
        loadpanel.SetActive(false);
    }

    public void LoadCompPanel(){
        startpanel.SetActive(false);
        newpanel.SetActive(false);
        loadpanel.SetActive(true);

        FileInfo[] fileinfo = new DirectoryInfo(Application.dataPath + "/Competitions/").GetFiles("*.json");
        foreach(FileInfo file in fileinfo){
            GameObject newcomp = Instantiate(compprefab);
            string compname = file.Name.Remove(file.Name.Length - 5, 5);

            newcomp.name = "comp" + currentcompnum;
            newcomp.transform.SetParent(compspanel.transform.GetChild(0));

            newcomp.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
            newcomp.transform.GetChild(0).GetComponent<Text>().text = ArabicFixer.Fix(compname, true, true);

            newcomp.GetComponent<Button>().onClick.RemoveAllListeners();
            newcomp.GetComponent<Button>().onClick.AddListener(() => {LoadComp(compname);});

            currentcompnum++;
            Debug.Log(file);
        }
    }

    public void LoadComp(string compname){
        Debug.Log("Scene Changed To: Main");

        Data comptoloadinfo = new Data();
        comptoloadinfo.compname = compname;

        string jsoncontent = JsonUtility.ToJson(comptoloadinfo);
        File.WriteAllText(Application.dataPath + "/CompetitionToLoadInfo.json", jsoncontent);


        jsoncontent = File.ReadAllText(Application.dataPath + "/Competitions/" + compname + ".json");
        Data data = JsonUtility.FromJson<Data>(jsoncontent);

        Debug.Log(ArabicFixer.Fix(data.compname,true,true));
        foreach(string question in data.questions){
            Debug.Log(question);
        }

        SceneManager.LoadScene("Main");
    }

    public void SaveComp(){
        Data data = new Data();

        data.compname = comptitleinput.text;
        data.questions = questionslist.ToArray();

        string jsoncontent = JsonUtility.ToJson(data);
        File.WriteAllText(Application.dataPath + "/Competitions/" + data.compname + ".json", jsoncontent);

        Debug.Log("Competition Saved with Name: " + data.compname);
        ReturnButton();
    }

    public void ReturnButton(){
        foreach(Transform child in questionspanel.transform.GetChild(0))
            Destroy(child.gameObject);
        foreach(Transform child in compspanel.transform.GetChild(0))
            Destroy(child.gameObject);
        
        currentquestionnum = 0;
        currentcompnum = 0;
        questioninput.text = "";
        comptitleinput.text = "";

        startpanel.SetActive(true);
        newpanel.SetActive(false);
        loadpanel.SetActive(false);
    }

    public void AddQuestion(Text question){
        GameObject newquestion = Instantiate(questionprefab);

        newquestion.name = "question" + currentquestionnum;
        newquestion.transform.SetParent(questionspanel.transform.GetChild(0));
        
        newquestion.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
        newquestion.GetComponent<Text>().text = question.text;

        questionslist.Add(questioninput.text);
        questioninput.text = "";
        currentquestionnum++;
        
        Debug.Log("Question Added: " + question.text);
    }

    public void UpdateArabicText(InputField input){
        input.transform.GetChild(3).GetComponent<Text>().text = ArabicFixer.Fix(input.text, true, true);
    }
    
    void Awake() 
    {
        startpanel.SetActive(true);
        newpanel.SetActive(true);
        loadpanel.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        ReturnButton();
    }

    // Update is called once per frame
    void Update()
    {
        if(comptitleinput.text == "" || questionslist.Count == 0){
            savecompbtn.interactable = false;
        } else {
            savecompbtn.interactable = true;
        }
        if(questioninput.text == ""){
            addquestionbtn.interactable = false;
        } else {
            addquestionbtn.interactable = true;
        }
    }
}