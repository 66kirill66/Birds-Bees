using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonthChanager : MonoBehaviour
{
    [SerializeField] Material sky1;
    [SerializeField] Material sky2;
    [SerializeField] Material sky3;
    [SerializeField] Material sky4;

    [SerializeField] GameObject snow;
    [SerializeField] GameObject gras;
    //Read month position
    [SerializeField] GameObject readPanel;  // ToReset Read Panel
    Vector3 readP;

    int month;
    int currentMonth;
    [SerializeField] List<GameObject> monthPosition = new List<GameObject>();
    [SerializeField] List<GameObject> tempObjects = new List<GameObject>();
    [SerializeField] List<GameObject> lightObjectsOne = new List<GameObject>();
    [SerializeField] List<GameObject> lightObjectsTo = new List<GameObject>();

    bool isHot;
    private void Awake()
    {             
        readP = readPanel.transform.localScale;
        month = 3;
    }

    void Start()
    {
        currentMonth = month;
        monthPosition[month].gameObject.SetActive(true);
    }
    public void TempCanvas()
    {      
        for (int i = 0; i < 12; i++)
        {
            monthPosition[i].gameObject.transform.localScale = new Vector3(0.8f, 2f, gameObject.transform.localScale.z);
        }       
    }
    public void LightCanvas()
    {
        for (int i = 0; i < 12; i++)
        {
            monthPosition[i].gameObject.transform.localScale = new Vector3(0.8f, 3.2f, gameObject.transform.localScale.z);
        }
    }

    public void SetPanelToOriginal()
    {
        foreach (GameObject i in monthPosition)
        {
            i.gameObject.transform.localScale = readP;
            i.SetActive(false);
        }
        month = 3;
        currentMonth = month;
        monthPosition[month].gameObject.SetActive(true);
        //SetTemp To Original
        //if (GetComponent<TemperatureS>().haveTemp == true)
        //{
        //    tempObjects[3].GetComponentInChildren<Text>().text = 15.ToString();
        //    tempObjects[4].GetComponentInChildren<Text>().text = 25.ToString();
        //    tempObjects[5].GetComponentInChildren<Text>().text = 23.ToString();
        //    tempObjects[6].GetComponentInChildren<Text>().text = 23.ToString();
        //    tempObjects[7].GetComponentInChildren<Text>().text = 20.ToString();
        //    tempObjects[8].GetComponentInChildren<Text>().text = 15.ToString();
        //    tempObjects[9].GetComponentInChildren<Text>().text = 6.ToString();
        //    tempObjects[10].GetComponentInChildren<Text>().text = 3.ToString();
        //    tempObjects[11].GetComponentInChildren<Text>().text = 3.ToString();
        //}
        if(tempObjects.Count != 0)
        {
            tempObjects[3].GetComponentInChildren<Text>().text = 15.ToString();
            tempObjects[4].GetComponentInChildren<Text>().text = 25.ToString();
            tempObjects[5].GetComponentInChildren<Text>().text = 23.ToString();
            tempObjects[6].GetComponentInChildren<Text>().text = 23.ToString();
            tempObjects[7].GetComponentInChildren<Text>().text = 20.ToString();
            tempObjects[8].GetComponentInChildren<Text>().text = 15.ToString();
            tempObjects[9].GetComponentInChildren<Text>().text = 6.ToString();
            tempObjects[10].GetComponentInChildren<Text>().text = 3.ToString();
            tempObjects[11].GetComponentInChildren<Text>().text = 3.ToString();
        }
    }
    void Update()
    {
        SetSkyBox();
        SetBgPanel();
    }
    private void SetSkyBox()
    {
        if (month == 12)
        {
            month = 0;
        }
        else if(month == 3)
        {
            snow.SetActive(false);
            gras.SetActive(true);
            RenderSettings.skybox = sky1;
        }
        else if (month == 6)
        {
            RenderSettings.skybox = sky2;
        }
        else if (month == 8)
        {
            RenderSettings.skybox = sky3;
        }
        else if (month > 10 || month < 3)
        {
            RenderSettings.skybox = sky4;
            snow.SetActive(true);
            gras.SetActive(false);
        }
    }

    public void ChanageMonth()
    {
        month++;       
    }
    public void SetTemperatureValue() // Button
    {
        if(isHot == false)
        {
            tempObjects[3].GetComponentInChildren<Text>().text = 16.ToString();
            tempObjects[4].GetComponentInChildren<Text>().text = 27.ToString();
            tempObjects[5].GetComponentInChildren<Text>().text = 25.ToString();
            tempObjects[6].GetComponentInChildren<Text>().text = 25.ToString();
            tempObjects[7].GetComponentInChildren<Text>().text = 23.ToString();
            tempObjects[8].GetComponentInChildren<Text>().text = 16.ToString();
            tempObjects[9].GetComponentInChildren<Text>().text = 7.ToString();
            tempObjects[10].GetComponentInChildren<Text>().text = 5.ToString();
            tempObjects[11].GetComponentInChildren<Text>().text = 4.ToString();
            Text t = tempObjects[month].GetComponentInChildren<Text>();
            int tempNum = System.Convert.ToInt32(t.text);
            FindObjectOfType<TemperatureS>().SetTemperatureVal(tempNum);
            isHot = true;
        }
        else
        {
            tempObjects[3].GetComponentInChildren<Text>().text = 15.ToString();
            tempObjects[4].GetComponentInChildren<Text>().text = 25.ToString();
            tempObjects[5].GetComponentInChildren<Text>().text = 23.ToString();
            tempObjects[6].GetComponentInChildren<Text>().text = 23.ToString();
            tempObjects[7].GetComponentInChildren<Text>().text = 20.ToString();
            tempObjects[8].GetComponentInChildren<Text>().text = 15.ToString();
            tempObjects[9].GetComponentInChildren<Text>().text = 6.ToString();
            tempObjects[10].GetComponentInChildren<Text>().text = 3.ToString();
            tempObjects[11].GetComponentInChildren<Text>().text = 3.ToString();
            Text t = tempObjects[month].GetComponentInChildren<Text>();
            int tempNum = System.Convert.ToInt32(t.text);
            FindObjectOfType<TemperatureS>().SetTemperatureVal(tempNum);
            isHot = false;
        }        
    }
    private void SetBgPanel()
    {
        if (currentMonth != month)
        {
            monthPosition[month].gameObject.SetActive(true);           
            if (month != 0)
            {
                monthPosition[month - 1].gameObject.SetActive(false);
            }
            if (month == 0)
            {
                monthPosition[11].gameObject.SetActive(false);
            }
            if(FindObjectOfType<TemperatureS>().haveTemp == true)
            {
                Text t = tempObjects[month].GetComponentInChildren<Text>();
                int tempValue = System.Convert.ToInt32(t.text);
                FindObjectOfType<TemperatureS>().SetTemperatureVal(tempValue);              
            }
            if (FindObjectOfType<LightDay>().havelight == true && FindObjectOfType<TemperatureS>().haveTemp == false)
            {
                Text lightText = lightObjectsOne[month].GetComponentInChildren<Text>();
                int lightValue = System.Convert.ToInt32(lightText.text);
                FindObjectOfType<LightDay>().LightValueSet(lightValue);
            }
            if (FindObjectOfType<LightDay>().havelight == true && FindObjectOfType<TemperatureS>().haveTemp == true)
            {
                Text lightText = lightObjectsTo[month].GetComponentInChildren<Text>();
                int lightValue = System.Convert.ToInt32(lightText.text);
                FindObjectOfType<LightDay>().LightValueSet(lightValue);
            }               
            currentMonth = month;
        }
    }      
}
