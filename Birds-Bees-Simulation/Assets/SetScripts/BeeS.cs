using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class BeeS : MonoBehaviour
{
    [DllImport("__Internal")]
    public static extern void BeeMeetFlower(int beeId, int flowerId);

    [DllImport("__Internal")]
    public static extern void SetBeeEnergyWeb(int beeId, int value);

    [SerializeField] GameObject beePrifab;
    public List<GameObject> beeList = new List<GameObject>();
    public List<GameObject> beeFlowerList = new List<GameObject>();
    public List<GameObject> beeInHaiveList = new List<GameObject>();
    float PosX, PosY, PosZ;
    int beesNum;
    public bool isCheck;
    [SerializeField] int beesCheck;
    GameObject bee;
    public GameObject beeDestroyEf;
    public float timerToFindFlower = 5;

    // DataClasss
    public class BeeMData
    {
        public int sliderNewValue;
        public string stateNewValue;
        public int beeId;
        public static BeeMData CreateFromJSON(string json)
        {
            BeeMData beeMDataData = JsonUtility.FromJson<BeeMData>(json);
            return beeMDataData;
        }
    }

    void Start()
    {
        timerToFindFlower = 5;
        if (isCheck == true)
        {
            int id = 1;
            for (int i = 0; i < beesCheck; i++)
            {
                InstantiateBee(id);
                id++;
            }
        }
    }
    private void Update()
    {
        TimerToFindFlower();
    }

    private void TimerToFindFlower()
    {
        if (beeList.Count != 0)
        {
            if (timerToFindFlower >= 0)
            {
                timerToFindFlower -= Time.deltaTime;
            }
            else if (timerToFindFlower <= 0)
            {
                if(FindObjectOfType<FlowerS>().flowerList.Count != 0)
                {
                    bee = GetRundomBee();
                    if (bee != null)
                    {
                        bee.GetComponent<BeeLogick>().BeeMoveToFlower();
                        bee = null;
                    }                   
                }
                timerToFindFlower = 5;
            }
        }
    }

    public void OnPollinates(int id)
    {
        foreach(GameObject i in beeFlowerList)
        {
            if(i.GetComponent<DataScript>().id == id)
            {
                i.GetComponent<BeeLogick>().beeParticle.Play();
                break;
            }
        }
        foreach (GameObject i in beeList)
        {
            if (i.GetComponent<DataScript>().id == id)
            {
                i.GetComponent<BeeLogick>().beeParticle.Play();
                break;
            }
        }
    }
    public void OnStateUpdate(string jsonData)
    {
        BeeMData data = BeeMData.CreateFromJSON(jsonData);
        string state = data.stateNewValue;
        int beesId = data.beeId;
        if(beeList.Count != 0)
        {
            foreach (GameObject i in beeList)
            {
                if (i.GetComponent<DataScript>().id == beesId)
                {
                    i.GetComponent<DataScript>().beeState = state;
                    if(state == "hibernate")
                    {
                        i.GetComponent<BeeSliderPosition>().sliderDown = false;
                        beeList.Remove(i);
                        beeInHaiveList.Add(i);
                        // Check if Bee active / hibernate after Update state
                        BeeHibernateCheck(i);
                        break;
                    }                   
                }
            }
        }
        if (beeFlowerList.Count != 0)
        {
            foreach (GameObject i in beeFlowerList)
            {
                if (i.GetComponent<DataScript>().id == beesId)
                {
                    i.GetComponent<DataScript>().beeState = state;
                    if (state == "hibernate")
                    {
                        i.GetComponent<BeeSliderPosition>().sliderDown = false;
                        beeFlowerList.Remove(i);
                        beeInHaiveList.Add(i);
                        // Check if Bee active / hibernate after Update state
                        BeeHibernateCheck(i);
                        break;
                    }
                }
            }
        }
        if (beeInHaiveList.Count != 0)
        {
            foreach (GameObject i in beeInHaiveList)
            {
                if (i.GetComponent<DataScript>().id == beesId)
                {
                    i.GetComponent<DataScript>().beeState = state;
                    if (state == "active")
                    {
                        i.GetComponent<BeeSliderPosition>().sliderDown = true;
                        beeInHaiveList.Remove(i);
                        beeList.Add(i);
                        // Check if Bee active / hibernate after Update state
                        BeeHibernateCheck(i);
                        break;
                    }                    
                }
            }
        }            
    }
    public void BeeHibernateCheck(GameObject bee)
    {
        if(bee.GetComponent<DataScript>().beeState == "hibernate")
        {            
            bee.GetComponent<BeeLogick>().BeeGoToHive();                  
        }
        else if (bee.GetComponent<DataScript>().beeState == "active")
        {           
            bee.GetComponent<BeeLogick>().BeeBackToFly();                  
        }
    }
    public void DeliteBee(int beeId)
    {
        if (beeFlowerList.Count != 0)
        {
            foreach (GameObject i in beeFlowerList)
            {
                if (i.GetComponent<DataScript>().id == beeId)
                {
                    beeFlowerList.Remove(i);
                    Vector3 p = i.transform.position;
                    Destroy(i);
                    GameObject ef = Instantiate(beeDestroyEf, p, transform.rotation);
                    Destroy(ef, 0.5f);
                    break;
                }
            }
        }
        if (beeList.Count != 0)
        {
            foreach (GameObject i in beeList)
            {
                if (i.GetComponent<DataScript>().id == beeId)
                {
                    beeList.Remove(i);
                    Vector3 p = i.transform.position;
                    Destroy(i);
                    GameObject ef = Instantiate(beeDestroyEf, p, transform.rotation);                  
                    Destroy(ef, 0.5f);
                    break;
                }
            }
        }                
    }
    public void SetBeeListFlowerFly(GameObject bee)
    {        
        beeFlowerList.Remove(bee);
        beeList.Add(bee);
    }
    public void BeeMeetFlowerWeb(int beeId, int flowerId)
    {
        if (!Application.isEditor)
        {
            BeeMeetFlower(beeId, flowerId);
        }
    }
   
    public void SetBeeEnergySendToWeb(int beeId, int value)
    {
        if (!Application.isEditor)
        {
            SetBeeEnergyWeb(beeId, value);
        }
    }

    public void SetBeeSliderValue(string jsonData)
    {
        BeeMData data = BeeMData.CreateFromJSON(jsonData);
        int sliderVal = data.sliderNewValue;
        int beesId = data.beeId;
        foreach (GameObject i in beeList)
        {
            if (i.GetComponent<DataScript>().id == beesId)
            {
                i.GetComponent<BeeSliderPosition>().slider.value = sliderVal;              
                break;
            }
        }
        foreach (GameObject i in beeFlowerList)
        {
            if (i.GetComponent<DataScript>().id == beesId)
            {
                i.GetComponent<BeeSliderPosition>().slider.value = sliderVal;
                break;
            }
        }
        foreach (GameObject i in beeInHaiveList)
        {
            if (i.GetComponent<DataScript>().id == beesId)
            {
                i.GetComponent<BeeSliderPosition>().slider.value = sliderVal;
                break;
            }
        }
    }
    private GameObject GetRundomBee()
    {
        GameObject bee;
        int max = beeList.Count;
        int rundomBeeNum = Random.Range(0, max);
        bee = beeList[rundomBeeNum];
        beeFlowerList.Add(bee);
        beeList.Remove(bee);
        return bee;
    }
    private Vector3 RundomPointToInst()
    {

        PosX = Random.Range(120, 200);
        PosY = Random.Range(14, 30);
        PosZ = 150;
        Vector3 createPos = new Vector3(PosX, PosY, PosZ);
        return createPos;
    }
    public void InstantiateBee(int id)
    {       
        GameObject beeP = Instantiate(beePrifab, RundomPointToInst(), beePrifab.transform.rotation);
        beeP.AddComponent<DataScript>().id = id;
        beeList.Add(beeP);
        beesNum++;
    }
    public void ResetBeeMSimulation()
    {
        if (beeList.Count != 0)
        {
            foreach (GameObject i in beeList)
            {
                Destroy(i);
            }
        }
        beeList.Clear();
        if (beeInHaiveList.Count != 0)
        {
            foreach (GameObject i in beeInHaiveList)
            {
                Destroy(i);
            }
        }
        beeInHaiveList.Clear();
        if (beeFlowerList.Count != 0)
        {
            foreach (GameObject i in beeFlowerList)
            {
                Destroy(i);
            }
        }
        beeFlowerList.Clear();
        beesNum = 0;
        timerToFindFlower = 5;
    }
}
