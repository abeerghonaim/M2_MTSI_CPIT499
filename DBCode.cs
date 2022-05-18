using System.Collections;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using System.Data;
using UnityEngine.UI;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DBCode: MonoBehaviour {
  public UnityEngine.UI.Button button1;

  public GameObject admin_management,StartPage,adminLogin,homePageEN,homePageAR,imamLogin,
  SuccessAdd_Panel,FailAdd_Panel,FailSearchMosq,MosqueDeatilsPanel,prayerbutton_Panel,
  ControlPrayerPanel,failAddSubPrayer,successAddSubPrayer,failDeleteSubPrayer,successDeleteSubPrayer, mosquesListPanel;

  //public Canvas mosquesListPanel;
  public float buttonHeight;
  public int numOfMosques;
  public int numOfMosques1;
  public int mosqiddd;
  

  public ToggleGroup toggleInstance;
  public Text id_InputField,password_InputField,errorMsg_text,
  id_InputFieldImam, errorMsg_textImam,adminNameAcc,adminEmailAcc,searchMosq,cityName,SubPrayer,OblPrayer;

  public String name,email;
  public int searchMosq1;
  public static int imamidstatic;

  public Text addMosqueName,addRegion,addCity,addStreet,mosqueName,bigMosqueName,idMoqsue,address;

  public String mosqueName1,bigMosqueName1,address1;
  public int idMoqsue1;

  public Text mosqueNameAR,bigMosqueNameAR,addressAR,cityNameAR,idMoqsueAR;

  public String mosqueName1AR,bigMosqueName1AR,address1AR,cityName1AR;
  public int idMoqsue1AR;

  // mosque deatils
  public int mosqID1;
  public int mosqID1Prayer;
  public Text mosqName,mosqID,mosqRegion,mosqCity,mosqStreet,mosqName101,mosqStreet101, mosqCity101, mosqRegion101;

  public String mosqName1,mosqRegion1,mosqCity1, mosqStreet1;

  public String mosqNameInsert;

  // update mosque
  public Text mosqNameUP,mosqRegionUP,mosqCityUP,mosqStreetUP;

  public String mosqNameUP1,mosqRegionUP1,mosqCityUP1,mosqStreetUP1,cityName1;

  public ArrayList sublogatryArrList = new ArrayList();
  List < MosqueArrayOfObject > mosqueArrList = new List < MosqueArrayOfObject > ();
  public string id;
  public Button addPrayerBtn, deletePrayerBtn;
  GUIStyle style;

  public Button[] newButton;
  // Start is called before the first frame update
  void Start() {
    ReadDatabase();
  //  mosquesLists();
  }

  string conn = "";
  IDbConnection dbconn;
  void ReadDatabase() {

    conn = "URI=file:" + Application.dataPath + "/MTSIDatabase.db"; //Path to database.
    try {
      dbconn = (IDbConnection) new SqliteConnection(conn);
      dbconn.Open(); //Open connection to the database.
    }

    catch(Mono.Data.Sqlite.SqliteException ex) {
       EditorUtility.DisplayDialog("Error", ex.Message, "Ok");    
    }

    IDbCommand dbcmd = dbconn.CreateCommand();
    string sqlQuery = "SELECT adminID, adminName, adminPass FROM admin";
    dbcmd.CommandText = sqlQuery;
    IDataReader reader = dbcmd.ExecuteReader();

    while (reader.Read()) {
      int id = reader.GetInt32(0);
      string pass = reader.GetString(2);
    }
    reader.Close();
    reader = null;
    dbcmd.Dispose();
    dbcmd = null;
    dbconn.Close();
    dbconn = null;

  }

  public void mosquesLists() {
    mosqueArrList.Clear();

    try {
    dbconn = (IDbConnection) new SqliteConnection(conn);
    dbconn.Open(); //Open connection to the database.
    IDbCommand dbcmd = dbconn.CreateCommand();

    String sqlQuery = "SELECT * FROM mosque";
    dbcmd.CommandText = sqlQuery;
    IDataReader reader = dbcmd.ExecuteReader();

    while (reader.Read()) {
      int mosid = reader.GetInt32(0);
      string mosname = reader.GetString(1);
      mosqueArrList.Add(new MosqueArrayOfObject() {
        mosqueName = mosname,
        mosqueID = mosid
      });
    }

    print("after reader in mosquelists " + mosqueArrList.Count);


     buttonHeight = button1.GetComponent < RectTransform > ().rect.height;
     numOfMosques = mosqueArrList.Count;

    if (numOfMosques > 0) {
      button1.gameObject.SetActive(true);
      button1.GetComponentInChildren < Text > ().text = "\t\t" + mosqueArrList[0].mosqueID.ToString() + "\t\t\t\t" + mosqueArrList[0].mosqueName;
    }

    if (numOfMosques > 1) newButton = new Button[numOfMosques - 1];
    for (int i = 1; i < mosqueArrList.Count; i++) {
      newButton[i - 1] = Instantiate(button1) as Button;
      newButton[i - 1].transform.parent = prayerbutton_Panel.transform;
      newButton[i - 1].transform.localPosition = button1.transform.localPosition;
      newButton[i - 1].transform.position = button1.transform.position;
      (newButton[i - 1].transform as RectTransform).anchoredPosition = (button1.transform as RectTransform).anchoredPosition;
      newButton[i - 1].transform.localScale = new Vector3(1, 1, 1);
      Vector3 pos = newButton[i - 1].transform.position - new Vector3(0, 24 * (i + 1), 0);
      newButton[i - 1].transform.position = pos;
      newButton[i - 1].GetComponentInChildren < Text > ().text = "\t\t" + mosqueArrList[i].mosqueID.ToString() + "\t\t\t\t" + mosqueArrList[i].mosqueName; // here we made changes
      
    }

    reader.Close();
    reader = null;
    dbcmd.Dispose();
    dbcmd = null;
    dbconn.Close();
    dbconn = null; 
    }
    
    // This excepttion will appear if the numOfMosques variable is less than the actual numbers of mosques in the DB.
    catch (NullReferenceException ex) {
    EditorUtility.DisplayDialog("Error", ex.Message, "Ok");  
    }
    catch(Mono.Data.Sqlite.SqliteException ex) {
    EditorUtility.DisplayDialog("Error", ex.Message, "Ok");    
    }
  }

  public void ValidateAdmin() {
    // This method will check if the admin's id and password matched in db or not.
    try {

      IDbCommand dbcmd = null;
      IDataReader reader = null;
      bool found = false;
      int adminid = 0;
      String adminpass = "";
      adminid = int.Parse(id_InputField.text);
      adminpass = password_InputField.text;
      dbconn = (IDbConnection) new SqliteConnection(conn);
      dbconn.Open(); //Open connection to the database.
      dbcmd = dbconn.CreateCommand();
      // to retrive the information from db
      String sqlQuery = "SELECT * FROM admin WHERE adminID = " + adminid + " AND adminPass = '" + adminpass + "'";
      dbcmd.CommandText = sqlQuery;
      reader = dbcmd.ExecuteReader();
      found = false;
      // to read the found info from db and assigned to the variables.
      while (reader.Read()) {
        int id = reader.GetInt32(0);
        String pass = reader.GetString(2);
        name = reader.GetString(1);
        email = reader.GetString(3);
        found = true;
      }
      // closing the reader and the connection of db.
      reader.Close();
      reader = null;
      dbcmd.Dispose();
      dbcmd = null;
      dbconn.Close();
      dbconn = null;
      /* if there is no match between the entered info and the stored one in db, 
      an error message will be displayed. */
      if (!found) errorMsg_text.text = "Wrong ID or Password!!";

      else {
        StartPage.SetActive(false);
        adminLogin.SetActive(false);
        admin_management.SetActive(true);
        imamLogin.SetActive(false);
        homePageEN.SetActive(false);
        homePageAR.SetActive(false);
        SuccessAdd_Panel.SetActive(false);
        FailAdd_Panel.SetActive(false);
        accountInfoAdmin();
        mosquesLists();
      
      }
    }
    catch(FormatException ex) {
      EditorUtility.DisplayDialog("Error", ex.Message, "Ok");
    }
    catch(Mono.Data.Sqlite.SqliteException ex) {
    EditorUtility.DisplayDialog("Error", ex.Message, "Ok");    
    }
  }

  public void InsertIntoAdmin() {
    try {    
    dbconn = (IDbConnection) new SqliteConnection(conn);
    dbconn.Open(); //Open connection to the database.
    IDbCommand dbcmd = dbconn.CreateCommand();

    String sqlQuery = "INSERT INTO admin VALUES (4, 'rafaaa') ";
    dbcmd.CommandText = sqlQuery;
    IDataReader reader = dbcmd.ExecuteReader();
    reader.Close();
    reader = null;
    dbcmd.Dispose();
    dbcmd = null;
    dbconn.Close();
    dbconn = null;
    }
    catch(Mono.Data.Sqlite.SqliteException ex) {
    EditorUtility.DisplayDialog("Error", ex.Message, "Ok");    
    }
  }
  int imamID = 0;
  public void ValidateImam() {
    // This method will check if the imam's id entered by the user is matched in db or not.
    try {


      // int imamID = 0;
      
      imamID = int.Parse(id_InputFieldImam.text);
      dbconn = (IDbConnection) new SqliteConnection(conn);
      dbconn.Open(); //Open connection to the database.
      IDbCommand dbcmd = dbconn.CreateCommand();
      String sqlQuery = "";
      // to retrive the information from db
      sqlQuery = "SELECT * FROM mosque WHERE mosqueID = " + imamID;
      dbcmd.CommandText = sqlQuery;
      IDataReader reader = dbcmd.ExecuteReader();
      bool found = false;
      // to read the found info from db and assigned to the variables.
      while (reader.Read()) {
        idMoqsue1 = reader.GetInt32(0);
        mosqueName1 = reader.GetString(1);
        bigMosqueName1 = reader.GetString(1);
        address1 = reader.GetString(3) + " - " + reader.GetString(4);
        cityName1 = reader.GetString(3);
        idMoqsue1AR = reader.GetInt32(0);
        mosqueName1AR = reader.GetString(5);
        bigMosqueName1AR = reader.GetString(5);
        address1AR = reader.GetString(6) + " - " + reader.GetString(7) + " - " + reader.GetString(8);
        cityName1AR = reader.GetString(7);
        found = true;
      }

       imamidstatic = imamID;
       print( "id " + imamidstatic);
  
      // closing the reader and the connection of db.
      reader.Close();
      reader = null;
      dbcmd.Dispose();
      dbcmd = null;
      dbconn.Close();
      dbconn = null;
      /* if there is no match between the entered info and the stored one in db, 
      an error message will be displayed. */
      if (!found) errorMsg_textImam.text = "Wrong ID!!";

      else {
        adminLogin.SetActive(false);
        admin_management.SetActive(false);
        StartPage.SetActive(false);
        imamLogin.SetActive(false);
        homePageAR.SetActive(false);
        SuccessAdd_Panel.SetActive(false);
        FailAdd_Panel.SetActive(false);
        homePageEN.SetActive(true);
        displayMosqueInfo();
        displayMosqueInfoAR();
      }
    }

    catch(FormatException imamidEX) {
      EditorUtility.DisplayDialog("Error", "You enter wrong format", "Ok");
    }
    catch(Mono.Data.Sqlite.SqliteException ex) {
    EditorUtility.DisplayDialog("Error", ex.Message, "Ok");    
    }
  }

  public void accountInfoAdmin() {
    adminNameAcc.text = name;
    adminEmailAcc.text = email;
  }

  
  public static int imamIDstatic(){
   // imamidstatic = 1230;
    return imamidstatic;
  }

  public void insertMosque() {
   try {

    dbconn = (IDbConnection) new SqliteConnection(conn);
    dbconn.Open(); //Open connection to the database.
    IDbCommand dbcmd = dbconn.CreateCommand();
    String sqlQuery;
    IDataReader reader = null;
  

    if (addMosqueName.text != "" && addRegion.text != "" && addCity.text != "" && addStreet.text != "") {
      sqlQuery = "INSERT INTO mosque (mosqueName, region, city, street) VALUES (' " + addMosqueName.text + "','" + addRegion.text + "','" + addCity.text + "','" + addStreet.text + "')";
      dbcmd.CommandText = sqlQuery;
      reader = dbcmd.ExecuteReader(); 

      mosqueArrList.Add(new MosqueArrayOfObject() {
      mosqueName = addMosqueName.text   
      });

        while (reader.Read()) {
        mosqiddd = reader.GetInt32(0);
      }

      print("new mosue 0 " + mosqiddd);
    // print("new mosque " + mosqueArrList[].mosqueID);

      mosqNameInsert = addMosqueName.text;
      for (int m = 0; m < newButton.Count() ; m++) {
      Destroy(newButton[m].GetComponentInChildren<Button>().gameObject);
  }

     reader.Close();
     reader = null;
     dbcmd.Dispose();
     dbcmd = null;
     dbconn.Close();
     dbconn = null;

     print("after add " + mosqueArrList.Count);
     SuccessAdd_Panel.SetActive(true);
     mosquesLists();
     
        
    }
  
    else {
      FailAdd_Panel.SetActive(true); 
    }
   }

   catch(FormatException ex) {
   EditorUtility.DisplayDialog("Error", ex.Message, "Ok");
   }
   catch(Mono.Data.Sqlite.SqliteException ex) {
   EditorUtility.DisplayDialog("Error", ex.Message, "Ok");    
   }
  }

   public void insertObligaroryPrayers(int idmosq) {

    dbconn = (IDbConnection) new SqliteConnection(conn);
    dbconn.Open(); //Open connection to the database.
    IDbCommand dbcmd = dbconn.CreateCommand();
    String sqlQuery;
    IDataReader reader = null;
    String [] arrPrayers = new String [5] {"Alfajr", "Alduhur", "Alasr", "Almaghrib", "Alisha"};

    for (int i= 0; i < arrPrayers.Count(); i++) {
      sqlQuery = "INSERT INTO prayerMosque (prayername) VALUES (' " + arrPrayers[i] + "') WHERE mosqueid = " + idmosq;
      dbcmd.CommandText = sqlQuery;
      reader = dbcmd.ExecuteReader(); 
    }

     reader.Close();
     reader = null;
     dbcmd.Dispose();
     dbcmd = null;
     dbconn.Close();
     dbconn = null;      
  
   }


  public void displayMosqueInfo() {
    mosqueName.text = mosqueName1;
    bigMosqueName.text = bigMosqueName1;
    idMoqsue.text = idMoqsue1 + " ";
    address.text = address1;
    cityName.text = cityName1;
  }

  public void displayMosqueInfoAR() {
    idMoqsueAR.text = idMoqsue1AR + " ";
    mosqueNameAR.GetComponent < ArabicText > ().Text = mosqueName1AR;
    bigMosqueNameAR.GetComponent < ArabicText > ().Text = bigMosqueName1AR;
    addressAR.GetComponent < ArabicText > ().Text = address1AR;
    cityNameAR.GetComponent < ArabicText > ().Text = cityName1AR;
    print( "id in AR" + imamidstatic);

  }

  public void searchMosqClick() {
     searchMosque(int.Parse(searchMosq.text));   
  }

  public int mosqnum = 0;
  public void searchMosqgrid() {
    searchMosque(mosqnum);
    //searchMosq.text = mosqnum.ToString();
    searchMosq.text=mosqnum+"";
    Debug.Log(mosqnum);
   // printSublegatoryPrayers_buttonOnClick(mosqnum);
  }

  public void searchMosque(int searchMosq1) {
    
    try {
      dbconn = (IDbConnection) new SqliteConnection(conn);
      dbconn.Open(); //Open connection to the database.
    
      IDbCommand dbcmd = dbconn.CreateCommand();

      String sqlQuery = "SELECT * FROM mosque WHERE mosqueID = " + searchMosq1;
      dbcmd.CommandText = sqlQuery;
      IDataReader reader = dbcmd.ExecuteReader();
      bool found = false;
      while (reader.Read()) {
        mosqID1 = reader.GetInt32(0);
        mosqName1 = reader.GetString(1);
        mosqRegion1 = reader.GetString(2);
        mosqCity1 = reader.GetString(3);
        mosqStreet1 = reader.GetString(4);
        found = true;
      }

      if (!found) {
       FailSearchMosq.SetActive(true);
      } else {

        MosqueDeatilsPanel.SetActive(true);
        mosqID.text = mosqID1 + "";
        mosqName.text = mosqName1;
        mosqName101.text = mosqName1;
        mosqRegion.text = mosqRegion1;
        mosqCity.text = mosqCity1;
        mosqStreet.text = mosqStreet1;
        mosqStreet101.text = mosqStreet1;
        mosqCity101.text = mosqCity1;
        mosqRegion101.text = mosqRegion1;

      }

      printSublegatoryPrayers();
      printObligaroryPrayers();

    }

    catch(FormatException Ex) {
      EditorUtility.DisplayDialog("Error", Ex.Message, "Ok");
    }
    catch(IndexOutOfRangeException Ex) {
      EditorUtility.DisplayDialog("Error", Ex.Message, "Ok");
    }
    catch(Mono.Data.Sqlite.SqliteException ex) {
      EditorUtility.DisplayDialog("Error", ex.Message, "Ok");    
    }

  }

  public void DeleteMosque() {
    FailSearchMosq.SetActive(false);

    try {

    dbconn = (IDbConnection) new SqliteConnection(conn);
    dbconn.Open(); //Open connection to the database.
    IDbCommand dbcmd = dbconn.CreateCommand();
    String sqlQuery;
    IDataReader reader = null;

    sqlQuery = "DELETE FROM mosque WHERE mosqueID = " + mosqID1;
    dbcmd.CommandText = sqlQuery;
    reader = dbcmd.ExecuteReader();
    // after deleting the prayer form db, we should also delete it from the arraylist
    
    mosqueArrList.Remove(new MosqueArrayOfObject() {
    mosqueName = addMosqueName.text
    });

      for (int m = 0; m < newButton.Count() ; m++) {
      Destroy(newButton[m].GetComponentInChildren<Button>().gameObject);
  } 
    reader.Close();
    reader = null;
    dbcmd.Dispose();
    dbcmd = null;
    dbconn.Close();
    dbconn = null;
    // call the method of mosques list to be updated.
    mosquesLists();
    }
    catch(Mono.Data.Sqlite.SqliteException ex) {
      EditorUtility.DisplayDialog("Error", ex.Message, "Ok");    
    }
  }

  public void UpdateInfoMosque() {
    try {

    // dbconn = (IDbConnection) new SqliteConnection(conn);
    // dbconn.Open(); //Open connection to the database.
    // IDbCommand dbcmd = dbconn.CreateCommand();
    // String sqlQuery;
    // IDataReader reader = null;

    if (mosqNameUP.text != "" ) {  
      
    dbconn = (IDbConnection) new SqliteConnection(conn);
    dbconn.Open(); //Open connection to the database.
    IDbCommand dbcmd = dbconn.CreateCommand();
    String sqlQuery;
    IDataReader reader = null;
    mosqNameUP1 = mosqNameUP.text;

    sqlQuery = "UPDATE mosque SET mosqueName= '" + mosqNameUP1+ "' WHERE mosqueID= " + mosqID1;
    dbcmd.CommandText = sqlQuery;
    reader = dbcmd.ExecuteReader();
    reader.Close();
    reader = null;
    dbcmd.Dispose();
    dbcmd = null;
    dbconn.Close();
    dbconn = null;
 
    mosqName101.text = mosqNameUP.text;
    } 
  
    if(mosqRegionUP.text != "" ) {
      
    dbconn = (IDbConnection) new SqliteConnection(conn);
    dbconn.Open(); //Open connection to the database.
    IDbCommand dbcmd = dbconn.CreateCommand();
    String sqlQuery;
    IDataReader reader = null;

    mosqRegionUP1 = mosqRegionUP.text;
    sqlQuery = "UPDATE mosque SET region = '" + mosqRegionUP1 + "' WHERE mosqueID= " + mosqID1;
    dbcmd.CommandText = sqlQuery;
    reader = dbcmd.ExecuteReader();

    reader.Close();
    reader = null;
    dbcmd.Dispose();
    dbcmd = null;
    dbconn.Close();
    dbconn = null;

    mosqRegion101.text = mosqRegionUP.text;

    }

    if(mosqCityUP.text != "" ) {

      
    dbconn = (IDbConnection) new SqliteConnection(conn);
    dbconn.Open(); //Open connection to the database.
    IDbCommand dbcmd = dbconn.CreateCommand();
    String sqlQuery;
    IDataReader reader = null;

    mosqCityUP1 = mosqCityUP.text;
    sqlQuery = "UPDATE mosque SET city = '" + mosqCityUP1 + "' WHERE mosqueID= " + mosqID1;
    dbcmd.CommandText = sqlQuery;
    reader = dbcmd.ExecuteReader();

    reader.Close();
    reader = null;
    dbcmd.Dispose();
    dbcmd = null;
    dbconn.Close();
    dbconn = null;

    mosqCity101.text = mosqCityUP.text;
    }
    
    if(mosqStreetUP.text != "" ) {
      
    dbconn = (IDbConnection) new SqliteConnection(conn);
    dbconn.Open(); //Open connection to the database.
    IDbCommand dbcmd = dbconn.CreateCommand();
    String sqlQuery;
    IDataReader reader = null;

    mosqStreetUP1 = mosqStreetUP.text;
    sqlQuery = "UPDATE mosque SET street = '" + mosqStreetUP1 + "' WHERE mosqueID= " + mosqID1;
    dbcmd.CommandText = sqlQuery;
    reader = dbcmd.ExecuteReader();

    mosqStreet101.text = mosqStreetUP.text;
    reader.Close();
    reader = null;
    dbcmd.Dispose();
    dbcmd = null;
    dbconn.Close();
    dbconn = null;
    }

  }
    catch(Mono.Data.Sqlite.SqliteException ex) {
      EditorUtility.DisplayDialog("Error", ex.Message, "Ok");    
    }
    catch(FormatException Ex) {
      EditorUtility.DisplayDialog("Error", Ex.Message, "Ok");
    }

  }

  public void printSublegatoryPrayers() {

    sublogatryArrList.Clear();
    searchMosq1 = int.Parse(searchMosq.text);
    dbconn = (IDbConnection) new SqliteConnection(conn);
    dbconn.Open(); //Open connection to the database.
    IDbCommand dbcmd = dbconn.CreateCommand();

    String sqlQuery = "SELECT a.prayername, b.prayerName FROM prayerMosque a, prayerTypes b WHERE a.mosqueid =" + searchMosq1 + " AND a.prayername = b.prayerName  AND b.type = 'Sunnah'" + "ORDER BY b.prayerName ASC";
    dbcmd.CommandText = sqlQuery;
    IDataReader reader = dbcmd.ExecuteReader();

    bool found = false;
    String list = "";

    int i = 0;

    while (reader.Read()) {
      list += reader.GetString(0) + "\n";
      sublogatryArrList.Add(reader.GetString(0));
      found = true;
      i++;
    }

    reader.Close();
    reader = null;
    dbcmd.Dispose();
    dbcmd = null;
    dbconn.Close();
    dbconn = null;
    SubPrayer.text = list;

    if (!found) {
      FailSearchMosq.SetActive(true);
    } else {

      MosqueDeatilsPanel.SetActive(true);
      mosqID.text = mosqID1 + "";
      mosqName.text = mosqName1;
      mosqRegion.text = mosqRegion1;
      mosqCity.text = mosqCity1;
      mosqStreet.text = mosqStreet1;
    }
  }

  public void printObligaroryPrayers() {

    searchMosq1 = int.Parse(searchMosq.text);
    dbconn = (IDbConnection) new SqliteConnection(conn);
    dbconn.Open(); //Open connection to the database.
    IDbCommand dbcmd = dbconn.CreateCommand();
    String sqlQuery = "SELECT a.prayername, b.prayerName FROM prayerMosque a, prayerTypes b WHERE a.mosqueid =" + searchMosq1 + " AND a.prayername = b.prayerName  AND b.type = 'Fredah'" + "ORDER BY b.prayerName ASC";
    dbcmd.CommandText = sqlQuery;
    IDataReader reader = dbcmd.ExecuteReader();

    bool found = false;
    String list = "";
    while (reader.Read()) {
      list += reader.GetString(0) + " \n ";
      found = true;
    }

    reader.Close();
    reader = null;
    dbcmd.Dispose();
    dbcmd = null;
    dbconn.Close();
    dbconn = null;

    OblPrayer.text = list;
    if (!found) {
      FailSearchMosq.SetActive(true);
     // insertObligaroryPrayers(searchMosq1);

    } else {
      MosqueDeatilsPanel.SetActive(true);
      mosqID.text = mosqID1 + "";
      mosqName.text = mosqName1;
      mosqRegion.text = mosqRegion1;
      mosqCity.text = mosqCity1;
      mosqStreet.text = mosqStreet1;
    }
  }

  public void addPrayer() {
    printSublegatoryPrayers();
    ControlPrayerPanel.SetActive(false);
    successAddSubPrayer.SetActive(false);
    failAddSubPrayer.SetActive(false);
    successDeleteSubPrayer.SetActive(false);
    failDeleteSubPrayer.SetActive(false);

    IDataReader reader = null;
    dbconn = (IDbConnection) new SqliteConnection(conn);
    dbconn.Open(); //Open connection to the database.
    IDbCommand dbcmd = dbconn.CreateCommand();
    Toggle[] toggles = toggleInstance.GetComponentsInChildren < Toggle > ();
    bool check = false;
    bool check2 = false;

    foreach(var t in toggles) {
      if (t.isOn) {
        if (sublogatryArrList.Count == 0) {
          check = true;
        } else {
          for (int j = 0; j < sublogatryArrList.Count; j++) {
            if (! (String.Equals(sublogatryArrList[j], t.gameObject.transform.GetChild(1).GetComponent < Text > ().text))) {
              check = true; // if there is no match between selected prayer and the prayer in db
              continue;
            } else {
              check = false;
              break;
            }
          }
        }
        if (check) {
          String sqlQuery = "INSERT INTO prayerMosque (prayername, mosqueid) VALUES ('" + t.gameObject.transform.GetChild(1).GetComponent < Text > ().text + "'," + searchMosq1 + ")";
          dbcmd.CommandText = sqlQuery;
          reader = dbcmd.ExecuteReader();
          reader.Close();
          reader = null;
          dbcmd.Dispose();
          dbcmd = null;
          dbconn.Close();
          dbconn = null;
          printSublegatoryPrayers();
          check2 = true;
          break;

        } else {
          printSublegatoryPrayers();
          check2 = false;
          break;
        }
      }
      if (check2) {
        break;
      }
    }
    if (check2) {
      ControlPrayerPanel.SetActive(true);
      successAddSubPrayer.SetActive(true);
    }
    else {
      ControlPrayerPanel.SetActive(true);
      failAddSubPrayer.SetActive(true);
    }
  }
  IDataReader reader = null;
  IDbCommand dbcmd;
  public void deletePrayer() {
    ControlPrayerPanel.SetActive(false);
    successDeleteSubPrayer.SetActive(false);
    failDeleteSubPrayer.SetActive(false);

    reader = null;
    Button add;

    dbconn = (IDbConnection) new SqliteConnection(conn);
    dbconn.Open(); //Open connection to the database.
    dbcmd = dbconn.CreateCommand();
    Toggle[] toggles = toggleInstance.GetComponentsInChildren < Toggle > ();

    bool exist = false;
    foreach(var t in toggles) {
      if (t.isOn) {
        if (sublogatryArrList.Count == 0) {} else {
          for (int j = 0; j < sublogatryArrList.Count; j++) {
            if (String.Equals(sublogatryArrList[j], t.gameObject.transform.GetChild(1).GetComponent < Text > ().text)) {
             // print(sublogatryArrList[j]);
              String sqlQuery = "DELETE FROM prayerMosque WHERE mosqueid = " + searchMosq1 + " AND prayername='" + t.gameObject.transform.GetChild(1).GetComponent < Text > ().text + "'";
              dbcmd.CommandText = sqlQuery;
              reader = dbcmd.ExecuteReader();
              reader.Close();
              ControlPrayerPanel.SetActive(true);
              successDeleteSubPrayer.SetActive(true);
              successAddSubPrayer.SetActive(false);
              failAddSubPrayer.SetActive(false);
              failDeleteSubPrayer.SetActive(false);
              exist = true;
            }
          }
        }
      }
    }

    if (!exist) {
      ControlPrayerPanel.SetActive(true);
      failDeleteSubPrayer.SetActive(true);
      successAddSubPrayer.SetActive(false);
      failAddSubPrayer.SetActive(false);
      successDeleteSubPrayer.SetActive(false);
    }
    reader = null;
    dbcmd.Dispose();
    dbcmd = null;
    dbconn.Close();
    dbconn = null;
    printSublegatoryPrayers();
  }

  RaycastHit hit;

  // Update is called once per frame
  //public UnityEngine.UI.Button button1;
  void Update() {
    if (Input.GetMouseButtonDown(0)) {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      if (Physics.Raycast(ray, out hit, 2f)) {
        //   Debug.Log("object that was hit: " + hit.collider.gameObject);
      }
    }
  }

  public void printSublegatoryPrayers_buttonOnClick(int mosqnum) {

    sublogatryArrList.Clear();
    searchMosq1 =mosqnum;
    dbconn = (IDbConnection) new SqliteConnection(conn);
    dbconn.Open(); //Open connection to the database.
    IDbCommand dbcmd = dbconn.CreateCommand();

    String sqlQuery = "SELECT a.prayername, b.prayerName FROM prayerMosque a, prayerTypes b WHERE a.mosqueid =" + searchMosq1 + " AND a.prayername = b.prayerName  AND b.type = 'Sunnah'" + "ORDER BY b.prayerName ASC";
    dbcmd.CommandText = sqlQuery;
    IDataReader reader = dbcmd.ExecuteReader();

    bool found = false;
    String list = "";

    int i = 0;

    while (reader.Read()) {
      list += reader.GetString(0) + "\n";
      sublogatryArrList.Add(reader.GetString(0));
      found = true;
      i++;
    }

    reader.Close();
    reader = null;
    dbcmd.Dispose();
    dbcmd = null;
    dbconn.Close();
    dbconn = null;
    SubPrayer.text = list;

    if (!found) {
      FailSearchMosq.SetActive(true);
    } else {

      MosqueDeatilsPanel.SetActive(true);
      mosqID.text = mosqID1 + "";
      mosqName.text = mosqName1;
      mosqRegion.text = mosqRegion1;
      mosqCity.text = mosqCity1;
      mosqStreet.text = mosqStreet1;
    }
  }
}