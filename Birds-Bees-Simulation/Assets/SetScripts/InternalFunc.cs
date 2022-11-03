using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class InternalFunc : MonoBehaviour
{

    [DllImport("__Internal")]
    public static extern void callTNITfunction();  // globals.init function

    [DllImport("__Internal")]
    public static extern void ClickFunc(int id);
    RaycastHit hit;
    [SerializeField] Camera mainCamera;
    public List<GameObject> FreePlantPlace = new List<GameObject>();

    public void Awake()
    {
        PlantPlaceListFinder();
    }
    void Start()
    {
        Time.timeScale = 1;
        if (!Application.isEditor)
        {
            callTNITfunction();
        }
    }
    private void PlantPlaceListFinder()
    {
        var plaseF = FindObjectsOfType<PlantPlace>();
        foreach (PlantPlace i in plaseF)
        {
            if (i.isFree == true)
            {
                FreePlantPlace.Add(i.gameObject);
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
    
    public void ResetSimulation()
    {
        Time.timeScale = 0;
        var plaseF = FindObjectsOfType<PlantPlace>();
        foreach (PlantPlace i in plaseF)
        {
            i.isFree = true;
        }
        FreePlantPlace.Clear();
        GetComponent<BeeS>().ResetBeeMSimulation();
        GetComponent<BirdS>().ResetBirdSimulation();
        GetComponent<TemperatureS>().ResetTempSimulation();
        GetComponent<LightDay>().ResetLightSimulation();
        GetComponent<FlowerS>().ResetFlowerSimulation();
        GetComponent<FruitS>().ResetFruitSimulation();
        PlantPlaceListFinder();
    }

    public void SetLanguage(string lang)
    {
        
    }
}
