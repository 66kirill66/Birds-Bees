using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class BirdS : MonoBehaviour
{
    [DllImport("__Internal")]
    public static extern void BirdMeetFruit(int birdId, int fruitId);
    [DllImport("__Internal")]
    public static extern void SetBirdEnergyWeb(int birdId, int value);

    [SerializeField] GameObject birdPrifab;
    [SerializeField] GameObject birdPrifabTo;
    public List<GameObject> birdsList = new List<GameObject>();
    float PosX, PosY, PosZ;
    int birdsNum;
    public bool isCheck;
    [SerializeField] int birdCheck;
    public float timerToFindFruit = 5;
    bool findFruit;
    GameObject bird;
    int num;

    bool find;

    public class BirdData
    {
        public int sliderNewValue;
        public int birdId;
        public static BirdData CreateFromJSON(string json)
        {
            BirdData birdSDataData = JsonUtility.FromJson<BirdData>(json);
            return birdSDataData;
        }
    }
    void Start()
    {
        timerToFindFruit = 5;
        if (isCheck == true)
        {
            int id = 1;
            for (int i = 0; i < birdCheck; i++)
            {
                InstantiateBird(id);
                id++;
            }
        }
    }
    private void Update()
    {
        if (birdsList.Count != 0)
        {
            if (timerToFindFruit >= 0)
            {
                timerToFindFruit -= Time.deltaTime;
            }
            else if (timerToFindFruit <= 0)
            {
                bird = GetRundomBird();
                if (bird != null)
                {                    
                    bird.GetComponent<FlyMoovment>().BirdMoveToFruit();
                }
                timerToFindFruit = 5;
            }
        }     
    }
    public void SetBirdEnergySendToWeb(int birdId, int value)
    {
        if (!Application.isEditor)
        {
            SetBirdEnergyWeb(birdId, value);
        }
    }
    public void DeliteBird(int birdId)
    {
        foreach (GameObject i in birdsList)
        {
            if (i.GetComponent<DataScript>().id == birdId)
            {
                Destroy(i);
                birdsList.Remove(i);
                break;
            }
        }
    }
    private GameObject GetRundomBird()
    {
        int max = birdsList.Count;
        num = Random.Range(0, max);
        return birdsList[num];
    }

    public void SetBirdSliderValue(string jsonData)
    {
        BirdData data = BirdData.CreateFromJSON(jsonData);
        int sliderVal = data.sliderNewValue;
        int birdId = data.birdId;
        foreach (GameObject i in birdsList)
        {
            if (i.GetComponent<DataScript>().id == birdId)
            {
                i.GetComponent<BirdSliderPosition>().slider.value = sliderVal;
                break;
            }
        }
    }  
    public void BirdMeetFruitrWeb(int birdId, int fruitId)
    {
        if (!Application.isEditor)
        {
            BirdMeetFruit(birdId, fruitId);
        }
    }
    private Vector3 RundomPointToInst()
    {

        PosX = Random.Range(120, 200);
        PosY = Random.Range(14, 30);
        PosZ = 150;
        Vector3 createPos = new Vector3(PosX, PosY, PosZ);
        return createPos;
    }
    public int GetNumberToInstRundomBird()
    {
        int numOfBird = Random.Range(0, 6);
        return numOfBird;
    }
    public void InstantiateBird(int id)
    {
        int birdNum = GetNumberToInstRundomBird();
        if (birdNum > 3)
        {
            GameObject birdPTo = Instantiate(birdPrifabTo, RundomPointToInst(), birdPrifab.transform.rotation);
            birdPTo.AddComponent<DataScript>().id = id;
            birdsList.Add(birdPTo);
            birdsNum++;
        }
        else if (birdNum <= 3)
        {
            GameObject birdP = Instantiate(birdPrifab, RundomPointToInst(), birdPrifab.transform.rotation);
            birdP.AddComponent<DataScript>().id = id;
            birdsList.Add(birdP);
            birdsNum++;
        }
    }
    public void ResetBirdSimulation()
    {
        if (birdsList.Count != 0)
        {
            foreach (GameObject i in birdsList)
            {
                Destroy(i);
            }
        }
        birdsList.Clear();
        birdsNum = 0;
        timerToFindFruit = 5;
        findFruit = false;
    }
}
