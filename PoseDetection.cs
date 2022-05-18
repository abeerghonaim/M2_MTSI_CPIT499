using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.rfilkov.kinect;
using UnityEngine.UI;

public class PoseDetection : MonoBehaviour
{
    public GameObject iconStand,stand,iconRko3,rko3, iconSgod,sgod;
    public Text raka3aNumber;
    public GameObject imamLeftFoot,imamLeftKnee,imamLeftHip,imamRightFoot,imamRightKnee,imamRightHip,imamHipCenter,imamNeck,
    imamLeftHand,imamRightHand,imamLeftShoulder, imamRightShoulder , imamHead;

    public GameObject KinectController;
// Start is called before the first frame update

    float topfromdepth = 0, topStandingRak3a1=0, topStandingTakperRak3a1=0 , topSeatedRak3a1;
    
    void Start()
    {     
    }

// declare variablies 
    int Raka3a = 0;
    int standCount = 0, rkou3Count = 0, sitCount = 0, sgodCount = 0;
    Vector3 headStanding = new Vector3(0,0,0),headSeting = new Vector3(0,0,0); float headYRko3 = -1;
    float hipYStanding = -1, hipYRko3 = -1;
    string pose = "";
    int handStatus = 0,handStatusPrevious = 0; float handPositionTime = 0;
    float lastseenSiting = 0;
    float lastChangeTimer = 0, minChangeTimer = 1; 
    float timeFromRko3toRak3aCount = 0; 
// Update is called once per frame
int MaxRak3a=4;
    void Update()
    {

System.DateTime theTime = System.DateTime.Now;
/*  alfajar 05:42 - 05:52 
    alduhor 12:37 - 12:47
    alasur  15:55 - 16:05
    almaghreb 18:23 - 18:33
    alesha    19:46 - 19:56
*/

/*
if (theTime.Hour == 22 && theTime.Minute > 30 && theTime.Minute < 34 )
{
MaxRak3a=2;
}else if(theTime.Hour == 05 && theTime.Minute > 42 && theTime.Minute < 52){
    MaxRak3a=4;
}else if(theTime.Hour == 12 && theTime.Minute > 37 && theTime.Minute < 47){
    MaxRak3a=4;
}else if(theTime.Hour == 15 && theTime.Minute > 30 && theTime.Minute < 34){
    MaxRak3a=3;
}
else if(theTime.Hour == 22 && theTime.Minute > 30 && theTime.Minute < 34){
    MaxRak3a=4;
} 
*/


float standingRko3 = - Mathf.Abs(imamHead.transform.position.x - imamHipCenter.transform.position.x)
        + Mathf.Abs(imamHead.transform.position.y - imamHipCenter.transform.position.y)
        - Mathf.Abs(imamHead.transform.position.z - imamHipCenter.transform.position.z);  /// .38   ->  -.0xxx
       
float lefthandref = Mathf.Abs(imamLeftHand.transform.position.y - imamLeftShoulder.transform.position.y);
float righthandref = Mathf.Abs(imamRightHand.transform.position.y - imamRightShoulder.transform.position.y);



if (Mathf.Abs(lefthandref - righthandref) > .0001){
    topfromdepth = KinectController.GetComponent<KinectManager>().ytoplast;

if (standingRko3 > .3 && (Mathf.Abs(topfromdepth - topStandingTakperRak3a1)<25 || topStandingTakperRak3a1==0) && lastChangeTimer > minChangeTimer && string.Compare(pose,"Standing") != 0)
{
    pose = "Standing"; // inrease -- > from ruku to stand or standing  
    if (Raka3a == 0){
        topStandingRak3a1 = topfromdepth;
        standCount = 1;
    }
    if (Raka3a > 0 && standCount == Raka3a && rkou3Count == Raka3a && sgodCount == Raka3a && sitCount == Raka3a && timeFromRko3toRak3aCount > 30)
    {
        if (MaxRak3a > Raka3a){
            Raka3a++;
        standCount = Raka3a;
        }
    }
lastChangeTimer = 0;
}
else if (standingRko3 < .1 && lastChangeTimer > minChangeTimer && string.Compare(pose,"Rko3") != 0 )
    pose = "Rko3"; // from stand to ruku -- > decrease
    rkou3Count = standCount;
    if(rkou3Count < Raka3a){
        timeFromRko3toRak3aCount = 0;
    }
}

// 
if (lefthandref > .35 && righthandref > .35 && Mathf.Abs(lefthandref - righthandref) > .0001){ // the avatar in start has the same values, which is not noraml in human life
    // print("hand straight");
    handStatus = 1;
}else if (lefthandref < .35 && righthandref < .35 && lefthandref > .1 && righthandref > .1 && Mathf.Abs(lefthandref - righthandref) > .0001){
    // print("hand fatiha");
    handStatus = 2;
}else if (lefthandref < .1 && righthandref < .1 && Mathf.Abs(lefthandref - righthandref) > .0001){
    // print("hand takbear");
    handStatus = 3;
}else{
    // print("hand undefined");
    handStatus = -1;
}

// to check if there is any changes in the postition
if  (handStatusPrevious != handStatus){
    handStatusPrevious = handStatus;
    handPositionTime=0;
}

handPositionTime += Time.deltaTime;
if (Raka3a == 0){
    if ((Raka3a == 0 || handStatus == 3) && string.Compare(pose,"Standing") == 0 &&  (handStatus == 2 || handStatus == 3) && handPositionTime > 0.25 ){
    
        Raka3a = 1;
        topStandingTakperRak3a1 = topfromdepth;
        headStanding = imamHead.transform.position ; 
        hipYStanding = imamHipCenter.transform.position.y;
        headSeting = new Vector3(0,0,0);

    }
}


    if (Raka3a > 0 && standCount == Raka3a && rkou3Count == Raka3a &&
        topfromdepth - topStandingTakperRak3a1  > 150 && lastChangeTimer > minChangeTimer && string.Compare(pose,"siting") != 0
        )
    {
        headSeting = imamHead.transform.position;
        pose = "siting";
        if (Raka3a == 1){
            topSeatedRak3a1 = topfromdepth;
        }
        lastseenSiting = 0;
        sitCount = standCount;
        lastChangeTimer = 0;
    }
    lastseenSiting += Time.deltaTime;

    if ((lastseenSiting <3f || string.Compare(pose,"sgod") == 0) && Mathf.Abs(lefthandref - righthandref) < .0001 && topfromdepth - topStandingTakperRak3a1  > 150 && lastChangeTimer > minChangeTimer && string.Compare(pose,"sgod") != 0){
        
        pose = "sgod";
        sgodCount = standCount;
        lastChangeTimer = 0;
    }

    lastChangeTimer += Time.deltaTime;
    timeFromRko3toRak3aCount += Time.deltaTime;
}

void OnGUI(){
    // GUI.Label(new Rect(10, 10, 100, 20), Raka3a.ToString());
    // print(Raka3a.ToString());
    raka3aNumber.text = Raka3a.ToString();
    // GUI.Label(new Rect(10, 30, 100, 20), pose.ToString());
    // print(pose.ToString());
    if (string.Compare(pose,"Standing") == 0){
        iconStand.SetActive(true);stand.SetActive(true);iconRko3.SetActive(false);rko3.SetActive(false); iconSgod.SetActive(false);sgod.SetActive(false);       
    }
    if (string.Compare(pose,"Rko3") == 0){
        iconStand.SetActive(false);stand.SetActive(false);iconRko3.SetActive(true);rko3.SetActive(true); iconSgod.SetActive(false);sgod.SetActive(false);       
    }
    if (string.Compare(pose,"sgod") == 0){
        iconStand.SetActive(false);stand.SetActive(false);iconRko3.SetActive(false);rko3.SetActive(false); iconSgod.SetActive(true);sgod.SetActive(true);       
    }
  //  if (GUI.Button(new Rect(10, 70, 50, 30), "Click"))
    //        Debug.Log("Clicked the button with text");
}
}