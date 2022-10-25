using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class LightDay : MonoBehaviour
{
    [DllImport("__Internal")]
    public static extern void LightValueSetWeb(int lightId, int value);  // globals.init function
    int id;
    public bool havelight;
    public bool lightCheck;  
    [SerializeField] GameObject lightCanvasSecond;
    [SerializeField] GameObject lightCanvasLest;

    private void Awake()
    {
        lightCanvasLest.SetActive(false);
        lightCanvasSecond.SetActive(false);
    }
    private void Start()
    {
        if (lightCheck == true)
        {
            Invoke("SetLightCanavas", 0.2f);
        }
    }
    public void SetLightCanavas()
    {
        havelight = true;
        if (FindObjectOfType<TemperatureS>().haveTemp == false)
        {
            lightCanvasSecond.SetActive(true);
            FindObjectOfType<MonthChanager>().TempCanvas();
        }
        if (FindObjectOfType<TemperatureS>().haveTemp == true)
        {
            lightCanvasLest.SetActive(true);
            FindObjectOfType<MonthChanager>().LightCanvas();
        }

    }
    public void LightValueSet(int value)
    {
        if (!Application.isEditor)
        {
            LightValueSetWeb(id, value);
        }
    }
    //public void ResetLightSimulation()
    //{
    //    lightCanvasLest.SetActive(false);
    //    lightCanvasSecond.SetActive(false);
    //    havelight = false;
    //    FindObjectOfType<MonthChanager>().SetPanelToOriginal();
    //}


    public void CreateLight(int lightId)
    {
        id = lightId;
        Invoke("SetLightCanavas", 0.2f);
    }
}
