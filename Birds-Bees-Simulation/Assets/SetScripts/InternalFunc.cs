using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InternalFunc : MonoBehaviour
{

    [DllImport("__Internal")]
    public static extern void callTNITfunction();  // globals.init function

    [DllImport("__Internal")]
    public static extern void ClickFunc(int id);
    public static bool reset;
    RaycastHit hit;
    [SerializeField] Camera mainCamera;
    void Start()
    {
        Time.timeScale = 1;
        if (!Application.isEditor)
        {
            if (reset == false)
            {
                callTNITfunction();
            }
        }
    }
    public void InitFunc()
    {
        if (!Application.isEditor)
        {
            callTNITfunction();
        }           
    }
    private void Update()
    {
        ClickingOnEntity();
    }
    private void ClickingOnEntity()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag == "Bird" || hit.transform.gameObject.tag == "Bee")
                {
                    int id = hit.transform.GetComponent<DataScript>().id;
                    Debug.Log(id);
                    if (!Application.isEditor)
                    {
                        ClickFunc(id);
                    }
                }
            }
        }
    }

    public void PausedSimulation()
    {
        if (Time.timeScale != 0)
        {
            Time.timeScale = 0;
        }
    }
    public void RunningSimulation()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }
    //public void ResetSimulation()
    //{
    //    Time.timeScale = 0;
    //    reset = true;
    //    SceneManager.LoadScene(0);
    //}
    public void ResetSimulation()
    {
        Time.timeScale = 0;
        var plaseF = FindObjectsOfType<PlantPlace>();
        foreach (PlantPlace i in plaseF)
        {
            i.isFree = true;
        }
        GetComponent<BeeS>().ResetBeeMSimulation();
        GetComponent<BirdS>().ResetBirdSimulation();
        GetComponent<TemperatureS>().ResetTempSimulation();
        GetComponent<LightDay>().ResetLightSimulation();
        GetComponent<FlowerS>().ResetFlowerSimulation();
        GetComponent<FruitS>().ResetFruitSimulation();       
    }

    public void SetLanguage(string lang)
    {
        
    }
}
