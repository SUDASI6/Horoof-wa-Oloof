using System.IO;
using System.Collections;
using System.Collections.Generic;
using ArabicSupport;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class startmenu : MonoBehaviour
{
    bool questionbtnpressed = false;

    int currentquestionnum = 0;
    int currentcompnum = 0;
    public int currentquestion;
    int currentcomp;

    public List<string> questionslist = new List<string>();
    public List<string> choiceslist = new List<string>();
    public List<string> compnames = new List<string>();

    public GameObject startpanel;
    public GameObject newpanel;
    public GameObject loadpanel;
    public GameObject questionspanel;
    public GameObject compspanel;
    public GameObject choices;

    public GameObject questionprefab;
    public GameObject compprefab;
    
    public EventSystem eventsys;
    
    public InputField questioninput;
    public InputField compnameinput;
    
    public Button newcompbtn;
    public Button savecompbtn;
    public Button addquestionbtn;
    public Button deletequestionbtn;
    public Button duplicatequestionbtn;
    public Button deletecompbtn;
    public Button editcompbtn;
    public Button entercompbtn;

    public Text questioninputvisible;

    public void NewCompPanel(){
        startpanel.SetActive(false);
        newpanel.SetActive(true);
        loadpanel.SetActive(false);

        eventsys.SetSelectedGameObject(questioninput.gameObject);

        GridLayoutGroup questionscontent = questionspanel.transform.GetChild(0).GetComponent<GridLayoutGroup>();
        float widthfactor = questionspanel.GetComponent<RectTransform>().rect.width/450f;
        float heightfactor = questionspanel.GetComponent<RectTransform>().rect.height/430f;
        
        questionscontent.cellSize = new Vector2(430*widthfactor, 30*heightfactor);
        questionscontent.spacing = new Vector2(0*widthfactor, 7.5f*heightfactor);
        RectOffset offset = new RectOffset();
        offset.left = (int)(10 * widthfactor);
        offset.right = offset.left;
        offset.top = offset.left;
        offset.bottom = offset.left;
        questionscontent.padding = offset;
    }

    public void LoadCompPanel(){
        startpanel.SetActive(false);
        newpanel.SetActive(false);
        loadpanel.SetActive(true);
        currentcomp = -1;

        eventsys.SetSelectedGameObject(entercompbtn.gameObject);

        GridLayoutGroup compscontent = compspanel.transform.GetChild(0).GetComponent<GridLayoutGroup>();
        float widthfactor = compspanel.GetComponent<RectTransform>().rect.width/700f;
        float heightfactor = compspanel.GetComponent<RectTransform>().rect.height/300f;

        compscontent.cellSize = new Vector2(250*widthfactor, 85*heightfactor);
        RectOffset offset = new RectOffset();
        offset.left = (int)(25 * widthfactor);
        offset.right = offset.left;
        offset.top = (int)(40  * heightfactor);
        offset.bottom = offset.top;
        compscontent.padding = offset;
        compscontent.spacing = new Vector2(75*widthfactor, 50*heightfactor);

        FileInfo[] fileinfo = new DirectoryInfo(Application.dataPath + "/Competitions/").GetFiles("*.json");
        compnames.Clear();
        foreach(FileInfo file in fileinfo){
            GameObject newcomp = Instantiate(compprefab);

            string compname = file.Name.Remove(file.Name.Length - 5, 5);
            compnames.Add(compname);

            newcomp.name = "comp" + currentcompnum;
            newcomp.transform.SetParent(compspanel.transform.GetChild(0));

            newcomp.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
            newcomp.transform.GetChild(0).GetComponent<Text>().text = ArabicFixer.Fix(compname, true, true);

            newcomp.GetComponent<Button>().onClick.RemoveAllListeners();
            newcomp.GetComponent<Button>().onClick.AddListener(() => {SelectComp(newcomp);});

            currentcompnum++;
        }
    }

    public void SelectComp(GameObject pressedbtn){
        currentcomp = int.Parse(pressedbtn.name.Remove(0, pressedbtn.name.Length-1));
    }

    public void DeleteComp(){
        string compname = compnames[currentcomp];
        
        if(!File.Exists(Application.dataPath + "/Competitions/" + compname + ".json")){
            Debug.Log("No file with path: { " + Application.dataPath + "/Competitions/" + compname + ".json }" + " exists!");
        } else {
            File.Delete(Application.dataPath + "/Competitions/" + compname + ".json");
            File.Delete(Application.dataPath + "/Competitions/" + compname + ".json.meta");
            
            Debug.Log("There is a file with name " + compname + " and it has been deleted!");
        }

        ReturnButton();
        LoadCompPanel();
    }

    public void EditComp(){
        Debug.Log("Editing Competition " + compnames[currentcomp] + "...");

        ReturnButton();
        NewCompPanel();

        string jsoncontent = File.ReadAllText(Application.dataPath + "/Competitions/" + compnames[currentcomp] + ".json");
        Data data = JsonUtility.FromJson<Data>(jsoncontent);

        compnameinput.text = data.compname;
        foreach(string question in data.questions){
            int x=0;
            questioninput.text = question;
            foreach(Transform choice in choices.transform){
                choice.GetComponent<InputField>().text = data.choices[currentquestionnum*4 + x];
                x++;
            }
            AddQuestion(questioninputvisible);
        }
    }

    public void EnterComp(){
        Debug.Log("Scene Changed To: Main, With Competition: " + compnames[currentcomp]);

        Data comptoloadinfo = new Data();
        comptoloadinfo.compname = compnames[currentcomp];

        string jsoncontent = JsonUtility.ToJson(comptoloadinfo);
        File.WriteAllText(Application.dataPath + "/CompetitionToLoadInfo.json", jsoncontent);

        SceneManager.LoadScene("Main");
    }

    public void SaveComp(){
        Data data = new Data();

        data.compname = compnameinput.text;
        data.questions = questionslist.ToArray();
        data.choices = choiceslist.ToArray();

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
        foreach(Transform choice in choices.transform)
            choice.GetComponent<InputField>().text = "";
        
        currentquestionnum = 0;
        currentcompnum = 0;
        questionslist.Clear();
        choiceslist.Clear();
        questioninput.text = "";
        compnameinput.text = "";
        questionbtnpressed = false;

        startpanel.SetActive(true);
        newpanel.SetActive(false);
        loadpanel.SetActive(false);

        eventsys.SetSelectedGameObject(newcompbtn.gameObject);
    }

    public void DeleteQuestion(){
        questionslist.RemoveAt(currentquestion);
        for(int i=0; i < 4; i++)
            choiceslist.RemoveAt(currentquestion*4);
        Destroy(questionspanel.transform.GetChild(0).GetChild(currentquestion).gameObject);

        foreach(Transform question in questionspanel.transform.GetChild(0)){
            int oldname = int.Parse(question.name.Remove(0,question.name.Length-1));
            if(oldname > currentquestion) 
                question.name = "question" + (oldname-1);
        }

        currentquestionnum--;
        questionbtnpressed = false;
    }
    
    public void DuplicateQuestion(){
        questioninput.text = questionslist[currentquestion];
        for(int i=0; i < 4; i++)
            choices.transform.GetChild(i).GetComponent<InputField>().text = choiceslist[currentquestion*4 + i];
        AddQuestion(questioninputvisible);

        questionbtnpressed = false;
    }

    public void EditQuestion(GameObject toeditquestion){
        Debug.Log("Editing Question " + toeditquestion.GetComponentInChildren<Text>().text + "...");        
        currentquestion = int.Parse(toeditquestion.name.Remove(0, toeditquestion.name.Length-1));

        questioninput.text = questionslist[currentquestion];
        for(int i=0; i < 4; i++)
            choices.transform.GetChild(i).GetComponent<InputField>().text = choiceslist[currentquestion*4 + i];        
        
        questionbtnpressed = true;
    }

    public void AddQuestion(Text question){
        foreach(Transform choice in choices.transform){
            choiceslist.Add(choice.GetComponent<InputField>().text);
        }
        questionslist.Add(questioninput.text);
        
        GameObject newquestion = Instantiate(questionprefab);
        newquestion.name = "question" + currentquestionnum;
        newquestion.transform.SetParent(questionspanel.transform.GetChild(0));
        
        newquestion.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
        newquestion.GetComponent<Button>().onClick.RemoveAllListeners();
        newquestion.GetComponent<Button>().onClick.AddListener(() => {EditQuestion(newquestion);});
        newquestion.GetComponentInChildren<Text>().text = question.text;

        foreach(Transform choice in choices.transform)
            choice.GetComponent<InputField>().text = "";
        questioninput.text = "";
        currentquestionnum++;
        
        Debug.Log("Question Added: " + question.text);

        questionbtnpressed = false;
    }

    public void UpdateArabicText(InputField input){
        foreach(Transform child in input.transform){
            if(child.name == "VisibleText"){
                child.GetComponent<Text>().text = ArabicFixer.Fix(input.text, true, true);
            }
        }
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

        // Navigation nav = new Navigation();
        // nav.selectOnDown = questioninput;
        // deletecompbtn.navigation = nav;
    }

    // Update is called once per frame
    void Update()
    {
        int choiceswritten = 0;
        
        foreach(Transform choice in choices.transform){
            if(choice.GetComponent<InputField>().text != "" && choice.GetComponent<InputField>().text != null)
                choiceswritten++;
        }
        
        if(!questionbtnpressed){
            deletequestionbtn.interactable = false;
            duplicatequestionbtn.interactable = false;
        } else {
            deletequestionbtn.interactable = true;
            duplicatequestionbtn.interactable = true;
        }
        if(questioninput.text == "" || choiceswritten < 2 || choices.transform.GetChild(0).GetComponent<InputField>().text == ""){
            addquestionbtn.interactable = false;
        } else {
            addquestionbtn.interactable = true;
        }
        if(currentcomp == -1){
            deletecompbtn.interactable = false;
            editcompbtn.interactable = false;
            entercompbtn.interactable = false;
        } else {
            deletecompbtn.interactable = true;
            editcompbtn.interactable = true;
            entercompbtn.interactable = true;
        }
        if(compnameinput.text == "" || questionslist.Count == 0){
            savecompbtn.interactable = false;
        } else {
            savecompbtn.interactable = true;
        }
    }
}