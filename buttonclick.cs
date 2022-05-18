using System.Collections;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using System.Data;
using UnityEngine.UI;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.EventSystems;



public class buttonclick : MonoBehaviour
{
  public Text mosqName;
  public Text mosqID;
  public Text mosqRegion;
  public Text mosqCity;
  public Text mosqStreet;

  public String mosqName1;
  public String mosqRegion1;
  public String mosqCity1;
  public String mosqStreet1;
  public GameObject MosqueDeatilsPanel;
   
  

    // Start is called before the first frame update
    void Start()
    { 
    Button btn = GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   void TaskOnClick(){
   string mosque = GetComponentInChildren<Text>().text + "";   
   string[] mosqueid = mosque.Split("\t\t");
   //Debug.Log(mosqueid[1]);
   GameObject.Find("SceneController").GetComponent<DBCode>().mosqnum = int.Parse(mosqueid[1]);
   GameObject.Find("SceneController").GetComponent<DBCode>().searchMosqgrid();

	}

}
