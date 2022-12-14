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

    public class DataTemp
    {
       public int temperatureId;
       public int tempValue;
        public static DataTemp CreateFromJSON(string json)
        {
            DataTemp tempDataJ = JsonUtility.FromJson<DataTemp>(json);
            return tempDataJ;
        }
    }

    private void Awake()
    {      
        tempCanvas.SetActive(false);
        haveTemp = false;
}
    private void Start()
    {
        if(tempCheck == true)
        {
            SetTempCanavas();
        }      
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
    public void SetTempCanavas()
    {
        haveTemp = true;
        tempCanvas.SetActive(true);
        FindObjectOfType<MonthChanager>().TempCanvas();       
        FindObjectOfType<MonthChanager>().Invoke("SetTempOnStart", 1f);
    }
    public void ResetTempSimulation()
    {       
        tempCanvas.SetActive(false);
        haveTemp = false;
        FindObjectOfType<MonthChanager>().SetPanelToOriginal();
    }
}
