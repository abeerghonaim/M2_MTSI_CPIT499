using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    public void Exit(){
        Application.Quit();
        Debug.Log("Quit!");
    }
}
