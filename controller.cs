using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class controller : MonoBehaviour
{
    public Text[] Times; 
    public GameObject DisplayInoPage,KinectController,homePageEN;
    
    // to print current date
    public Text dateUI;
    public Text dateUIAR;
    public Text prayerName;
    // Start is called before the first frame update
    void Start()
    {
    }

    System.DateTime theTime = System.DateTime.Now;
    String currentdate =  System.DateTime.Now.ToString("dddd, dd MMMM yyyy");


    int[] prayerTimesLooperHours = new int[5];
    int[] prayerTimesLooperMinutes = new int[5];
    int[] prayerTimesLooperHoursMinutes = new int[5];


    
    // Update is called once per frame
    void Update()
    {   
        dateUI.text = currentdate;
        dateUIAR.text = currentdate;
        theTime = System.DateTime.Now;
        /*  alfajar 05:42 - 05:52 
            alduhor 12:37 - 12:47
            alasur  15:55 - 16:05
            almaghreb 18:23 - 18:33
            alesha    19:46 - 19:56
        */

     // here we open the comment if we want to turn on the kniect and start tracking

     
        for (int i = 0; i < 4; i++){
            // print(Times[i].text.ToString());
            string[] splitArray =  Times[i].text.ToString().Split(":");
            prayerTimesLooperHours[i] = int.Parse(splitArray[0]);           
            if (splitArray.Length>1) 
                prayerTimesLooperMinutes[i] = int.Parse(splitArray[1]);
            else
            prayerTimesLooperMinutes[i] = 0;
            prayerTimesLooperHoursMinutes[i] = prayerTimesLooperHours[i] * 60 + prayerTimesLooperMinutes[i];

        }

        int currentTimeInMinutes = theTime.Hour * 60 + theTime.Minute ;

        if (currentTimeInMinutes - prayerTimesLooperHoursMinutes[0] > 10 && currentTimeInMinutes - prayerTimesLooperHoursMinutes[0] < 25 )
        {
            print("fagr");
            DisplayInoPage.SetActive(true);
            KinectController.SetActive(true);
            homePageEN.SetActive(false);
            prayerName.text = "Alfajar Prayer";

           // MaxRak3a=2;
        }else
        if (currentTimeInMinutes - prayerTimesLooperHoursMinutes[1] > 10 && currentTimeInMinutes - prayerTimesLooperHoursMinutes[1] < 25 )
        {
            print("zohr");
            DisplayInoPage.SetActive(true);
            KinectController.SetActive(true);
            homePageEN.SetActive(false);
            prayerName.text = "Alduhor Prayer";
            // MaxRak3a=2;
        }else
        if (currentTimeInMinutes - prayerTimesLooperHoursMinutes[2] > 10 && currentTimeInMinutes - prayerTimesLooperHoursMinutes[2] < 25 )
        {
            print("Asr");
            DisplayInoPage.SetActive(true);
            KinectController.SetActive(true);
            homePageEN.SetActive(false);
            prayerName.text = "Alasr Prayer";
            // MaxRak3a=2;
        }else
        if (currentTimeInMinutes - prayerTimesLooperHoursMinutes[3] > 10 && currentTimeInMinutes - prayerTimesLooperHoursMinutes[3] < 25 )
        {
            print("magrib");
            DisplayInoPage.SetActive(true);
            KinectController.SetActive(true);
            homePageEN.SetActive(false);
            prayerName.text = "Almaghreb Prayer";
            // MaxRak3a=2;
        }else
        if (currentTimeInMinutes - prayerTimesLooperHoursMinutes[4] > 10 && currentTimeInMinutes - prayerTimesLooperHoursMinutes[4] < 25 )
        {
            print("Eshaa");
            DisplayInoPage.SetActive(true);
            KinectController.SetActive(true);
            homePageEN.SetActive(false);
            prayerName.text = "Alesha Prayer";
            // MaxRak3a=2;
        }
        else{
            if (DisplayInoPage.activeInHierarchy){
                KinectController.SetActive(false);
                DisplayInoPage.SetActive(false);                
                homePageEN.SetActive(true);
            }
        }         
    } 

     public void disableCamera(){
         //   KinectController.SetActive(false);
             DisplayInoPage.SetActive(true);
        }

            public void openFajr(){
            print("fagr");
            homePageEN.SetActive(false);
           // DisplayInoPage.SetActive(true);
            KinectController.SetActive(true);
            
            prayerName.text = "Alfajar Prayer";
        }

}
