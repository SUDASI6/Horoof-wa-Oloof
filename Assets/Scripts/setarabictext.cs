﻿using System.Collections;
using System.Collections.Generic;
using ArabicSupport;
using UnityEngine;
using UnityEngine.UI;

public class setarabictext : MonoBehaviour
{
    public string text;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Text>().text = ArabicFixer.Fix(text, true, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
