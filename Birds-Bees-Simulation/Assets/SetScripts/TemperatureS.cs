using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class TemperatureS : MonoBehaviour
{
    [DllImport("__Internal")]
    public static extern void TempValueSetWeb(int tempId, int value);  // globals.init function
    [SerializeField] GameObject tempCanvas;
    public int id;
    public bool haveTemp;
    public bool tempCheck;

    private void Awake()
    {      
        tempCanvas.SetActive(false);
    }
    private void Start()
    {
        if(tempCheck == true)
        {
            SetTempCanavas();
        }      
    }
    public void SetTempCanavas()
    {
        haveTemp = true;
        tempCanvas.SetActive(true);
        if (FindObjectOfType<LightDay>().havelight == false)
        {
            FindObjectOfType<MonthChanager>().TempCanvas();
        }
    }
    public void ResetTempSimulation()
    {           
        FindObjectOfType<MonthChanager>().SetPanelToOriginal();
        tempCanvas.SetActive(false);
        haveTemp = false;
    }

    public void SetTemperatureVal(int value)
    {
        if (!Application.isEditor)
        {
            TempValueSetWeb(id, value);
        }           
    }
    public void CreateTemp(int id)
    {
        this.id = id;
        SetTempCanavas();
    }
}
