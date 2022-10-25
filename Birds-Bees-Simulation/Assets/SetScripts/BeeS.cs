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

    public float timerToFindFlower = 5;
    [SerializeField] GameObject beePrifab;
    public List<GameObject> beeList = new List<GameObject>();
    public List<GameObject> beeInHaiveList = new List<GameObject>();
    float PosX, PosY, PosZ;
    int beesNum;
    public bool isCheck;
    bool find;
    [SerializeField] int beesCheck;
    GameObject bee;
    int num;
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
        if(beeList.Count != 0)
        {
            if (timerToFindFlower >= 0)
            {
                find = false;
                timerToFindFlower -= Time.deltaTime;
            }
            else if (timerToFindFlower <= 0)
            {
                if (find == false)
                {
                    bee = GetRundomBee();
                    find = true;
                }
                if (bee != null)
                {
                    bee.GetComponent<FlyMoovment>().BeeMoveToFlower();
                }
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
                        i.GetComponent<SliderPosition>().sliderDown = false;
                        beeList.Remove(i);
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
                        i.GetComponent<SliderPosition>().sliderDown = true;
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
            bee.GetComponent<FlyMoovment>().BeeGoToHive();                  
        }
        else if (bee.GetComponent<DataScript>().beeState == "active")
        {           
            bee.GetComponent<FlyMoovment>().BeeBackToFly();                  
        }
    }
    public void DeliteBee(int beeId)
    {
        foreach(GameObject i in beeList)
        {
            if(i.GetComponent<DataScript>().id == beeId)
            {
                Destroy(i);
                beeList.Remove(i);
                break;
            }
        }
    }

    private GameObject GetRundomBee()
    {
        int max = beeList.Count;
        num = Random.Range(0, max);
        return beeList[num];
    }
    public void SetBeeEnergySendToWeb(int beeId,int value)
    {
        if (!Application.isEditor)
        {
            SetBeeEnergyWeb(beeId, value);
        }          
    }
    public void BeeMeetFlowerWeb(int beeId, int flowerId)
    {
        if (!Application.isEditor)
        {
            BeeMeetFlower(beeId, flowerId);
        }
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
        beesNum = 0;

        FindObjectOfType<MonthChanager>().SetPanelToOriginal();
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
                i.GetComponent<SliderPosition>().slider.value += sliderVal;
                int currentVal = Mathf.RoundToInt(i.GetComponent<SliderPosition>().slider.value);;
                SetBeeEnergySendToWeb(beesId, currentVal);
                break;
            }
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


    public void InstantiateBee(int id)
    {       
        GameObject beeP = Instantiate(beePrifab, RundomPointToInst(), beePrifab.transform.rotation);
        beeP.AddComponent<DataScript>().id = id;
        beeList.Add(beeP);
        beesNum++;
    }
}
