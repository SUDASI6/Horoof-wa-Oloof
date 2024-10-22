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
    
    public CustomInputField questioninput;
    public CustomInputField compnameinput;
    
    public CustomButton returnloadbtn;
    public CustomButton returnnewbtn;
    public CustomButton addquestionbtn;
    public CustomButton savecompbtn;
    public CustomButton deletequestionbtn;
    public CustomButton duplicatequestionbtn;
    public Button newcompbtn;
    public Button deletecompbtn;
    public Button editcompbtn;
    public Button entercompbtn;

    public Text questioninputvisible;

    IEnumerator WaitForFrameEnd(){
        yield return new WaitForEndOfFrame();
    }

    public void NewCompPanel(){
        startpanel.SetActive(false);
        newpanel.SetActive(true);
        loadpanel.SetActive(false);

        eventsys.SetSelectedGameObject(questioninput.gameObject, new BaseEventData(eventsys));

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

        eventsys.SetSelectedGameObject(returnloadbtn.gameObject, new BaseEventData(eventsys));

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

        if(fileinfo.Length != 0){
            StartCoroutine(WaitForFrameEnd());
            eventsys.SetSelectedGameObject(compspanel.transform.GetChild(0).GetChild(0).gameObject, new BaseEventData(eventsys));
        }
    }

    public void SelectComp(GameObject pressedbtn){
        currentcomp = int.Parse(pressedbtn.name.Remove(0, pressedbtn.name.Length-1));
        eventsys.SetSelectedGameObject(editcompbtn.gameObject, new BaseEventData(eventsys));
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

        eventsys.SetSelectedGameObject(null, new BaseEventData(eventsys));

        string jsoncontent = File.ReadAllText(Application.dataPath + "/Competitions/" + compnames[currentcomp] + ".json");
        Data data = JsonUtility.FromJson<Data>(jsoncontent);

        compnameinput.text = data.compname;
        foreach(string question in data.questions){
            int x=0;
            questioninput.text = question;
            foreach(Transform choice in choices.transform){
                choice.GetComponent<CustomInputField>().text = data.choices[currentquestionnum*4 + x];
                x++;
            }
            AddQuestion(questioninputvisible);
        }

        if(data.questions.Length != 0){
            Debug.Log("select question0");
            StartCoroutine(WaitForFrameEnd());
            eventsys.SetSelectedGameObject(questionspanel.transform.GetChild(0).GetChild(0).gameObject, new BaseEventData(eventsys));
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
            choice.GetComponent<CustomInputField>().text = "";
        
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

        eventsys.SetSelectedGameObject(newcompbtn.gameObject, new BaseEventData(eventsys));
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
            choices.transform.GetChild(i).GetComponent<CustomInputField>().text = choiceslist[currentquestion*4 + i];
        AddQuestion(questioninputvisible);

        questionbtnpressed = false;
    }

    public void EditQuestion(GameObject toeditquestion){
        Debug.Log("Editing Question " + toeditquestion.GetComponentInChildren<Text>().text + "...");        
        currentquestion = int.Parse(toeditquestion.name.Remove(0, toeditquestion.name.Length-1));

        questioninput.text = questionslist[currentquestion];
        for(int i=0; i < 4; i++)
            choices.transform.GetChild(i).GetComponent<CustomInputField>().text = choiceslist[currentquestion*4 + i];        
        
        questionbtnpressed = true;

        eventsys.SetSelectedGameObject(questioninput.gameObject, new BaseEventData(eventsys));
    }

    public void AddQuestion(Text question){
        foreach(Transform choice in choices.transform){
            choiceslist.Add(choice.GetComponent<CustomInputField>().text);
        }
        questionslist.Add(questioninput.text);
        
        GameObject newquestion = Instantiate(questionprefab);
        newquestion.name = "question" + currentquestionnum;
        newquestion.transform.SetParent(questionspanel.transform.GetChild(0));
        
        newquestion.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
        newquestion.GetComponent<CustomButton>().rightSelectable = questioninput;
        newquestion.GetComponent<CustomButton>().onClick.RemoveAllListeners();
        newquestion.GetComponent<CustomButton>().onClick.AddListener(() => {EditQuestion(newquestion);});
        newquestion.GetComponentInChildren<Text>().text = question.text;

        foreach(Transform choice in choices.transform)
            choice.GetComponent<CustomInputField>().text = "";
        questioninput.text = "";
        currentquestionnum++;

        if(currentquestionnum == 0){
            StartCoroutine(WaitForFrameEnd());
            returnnewbtn.downSelectable = newquestion.GetComponent<CustomButton>();
            addquestionbtn.leftSelectable = newquestion.GetComponent<CustomButton>();

        }        

        questionbtnpressed = false;
    }

    public void UpdateArabicText(CustomInputField input){
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
        if((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.Tab)){
            Selectable previous = eventsys.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();

            if(previous != null){
                CustomInputField previousCustomInputField = previous.GetComponent<CustomInputField>();

                if(previousCustomInputField != null){
                    previousCustomInputField.OnPointerClick(new PointerEventData(eventsys));
                }


                eventsys.SetSelectedGameObject(previous.gameObject, new BaseEventData(eventsys));
            } else Debug.Log("no previous element");
        } else if(Input.GetKeyDown(KeyCode.Tab)){
            Selectable next = eventsys.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();

            if(next != null){
                CustomInputField nextCustomInputField = next.GetComponent<CustomInputField>();

                if(nextCustomInputField != null){
                    nextCustomInputField.OnPointerClick(new PointerEventData(eventsys));
                }

                eventsys.SetSelectedGameObject(next.gameObject, new BaseEventData(eventsys));
            } else Debug.Log("no next element");
        }

        #region Interactable Buttons Manager
            
            int choiceswritten = 0;
            
            foreach(Transform choice in choices.transform){
                if(choice.GetComponent<CustomInputField>().text != "" && choice.GetComponent<CustomInputField>().text != null)
                    choiceswritten++;
            }
            
            if(!questionbtnpressed){
                deletequestionbtn.interactable = false;
                duplicatequestionbtn.interactable = false;
            } else {
                deletequestionbtn.interactable = true;
                duplicatequestionbtn.interactable = true;
            }
            if(questioninput.text == "" || choiceswritten < 2 || choices.transform.GetChild(0).GetComponent<CustomInputField>().text == ""){
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
        #endregion
    }
}