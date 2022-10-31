using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class FruitS : MonoBehaviour
{
    [DllImport("__Internal")]
    public static extern void SetFruitsLevelWeb(int fruitId);

    [DllImport("__Internal")]
    public static extern void CreateRequestNewFruit(int FlowerId);

    public List<GameObject> fruitList = new List<GameObject>();
    [SerializeField] GameObject fruitPrifab;
    GameObject place;
    int fruitsNum;
    public bool isCheck;
    [SerializeField] int fruitCheck;
    public class FruitMData
    {
        public int fruitId;
        public int flowerId;
        public FruitMData(int id, int placeId)
        {
            fruitId = id;
            flowerId = placeId;
        }
        public static FruitMData CreateFromJSON(string json)
        {
            FruitMData fruitData = JsonUtility.FromJson<FruitMData>(json);
            return fruitData;
        }
    }
    void Start()
    {
        if (isCheck == true)
        {
            InvokeRepeating("InstantiateFruitCheck", 3, 4);
            int id = 1;
            for (int i = 0; i < fruitCheck; i++)
            {
                InstantiateFruitCheck();
                id++;
            }
        }    
    }
    public void InstantiateFruitCheck()
    {
        FindFruitPosition();
        if (place != null)
        {
            GameObject fruit = Instantiate(fruitPrifab, new Vector3(place.transform.position.x, place.transform.position.y - 3, place.transform.position.z), fruitPrifab.transform.rotation);
            fruit.GetComponent<FruitLogick>().plant = place; // to Set plant Free after destroy the fruit
            fruitList.Add(fruit);
            fruitsNum++;
            place = null;
        }
        else if (FindObjectOfType<InternalFunc>().FreePlantPlace.Count == 0 && place == null)
        {
            //int fruitN = GetRundomFruit();
            //GameObject fruitDel = fruitList[fruitN];
            GameObject fruitDel = GetRundomFruit();
            GameObject pl = fruitDel.GetComponent<FruitLogick>().plant;
            int fruitId = fruitDel.GetComponent<DataScript>().id;
            if (!Application.isEditor)
            {
                //send to Plethora Delite fruit.
                SetFruitsLevelSend(fruitId);
            }
            Destroy(fruitDel);
            fruitList.Remove(fruitDel);
            fruitsNum--;
            //new fruit
            GameObject fruit = Instantiate(fruitPrifab, fruitDel.transform.position, fruitPrifab.transform.rotation);
            fruit.GetComponent<FruitLogick>().plant = pl;
            fruitList.Add(fruit);
            fruitsNum++;
        }
    }

    public void SetFruitsLevelSend(int fruitId)
    {
        if (!Application.isEditor)
        {
            SetFruitsLevelWeb(fruitId);
        }          
    }  
    public void DestroyFruit(int fruitId)
    {
        if (fruitList.Count != 0)
        {
            foreach (GameObject i in fruitList)
            {
                if (i.GetComponent<DataScript>().id == fruitId)
                {
                    GameObject pl = i.GetComponent<FruitLogick>().plant;
                    pl.GetComponent<PlantPlace>().isFree = true;
                    FindObjectOfType<InternalFunc>().FreePlantPlace.Add(pl);
                    fruitList.Remove(i.gameObject);
                    Destroy(i);
                    fruitsNum--;
                    break;
                }
            }
        }
    }
    public void BirdEatFruit(int fruitId) 
    {
        if(fruitList.Count != 0)
        {
            foreach (GameObject i in fruitList)
            {
                if (i.GetComponent<DataScript>().id == fruitId)
                {
                    GameObject pl = i.GetComponent<FruitLogick>().plant;
                    pl.GetComponent<PlantPlace>().isFree = true;
                    FindObjectOfType<InternalFunc>().FreePlantPlace.Add(pl);
                    fruitList.Remove(i.gameObject);
                    Destroy(i);
                    SetFruitsLevelSend(fruitId);
                    fruitsNum--;
                    break;
                }
            }
        }       
    }
    public void CreateNewFruit(int flowerId)   // send receptor Id 
    {
        if (!Application.isEditor)
        {
            CreateRequestNewFruit(flowerId);
        }
    }

    private void FindFruitPosition()
    {
        if (FindObjectOfType<InternalFunc>().FreePlantPlace.Count != 0)
        {
            foreach (GameObject i in FindObjectOfType<InternalFunc>().FreePlantPlace)
            {
                if (i.GetComponent<PlantPlace>().isFree == true)
                {
                    i.GetComponent<PlantPlace>().isFree = false;
                    place = i;
                    FindObjectOfType<InternalFunc>().FreePlantPlace.Remove(i);
                    break;
                }
            }
        }
    }
    

    public void InstantiateFruit(string json)
    {
        FruitMData data = FruitMData.CreateFromJSON(json);
        if (data.flowerId != -1) 
        {           
            foreach (GameObject FlowerPrifab in FindObjectOfType<FlowerS>().flowerList)
            {
                if (data.flowerId == FlowerPrifab.GetComponent<DataScript>().id)
                {
                    GameObject fruit = Instantiate(fruitPrifab, new Vector3(FlowerPrifab.transform.position.x, FlowerPrifab.transform.position.y - 3, FlowerPrifab.transform.position.z), fruitPrifab.transform.rotation);
                    fruit.GetComponent<DataScript>().id = data.fruitId;
                    fruitList.Add(fruit);
                    GameObject pl = FlowerPrifab.GetComponent<FlowerScript>().plant;
                    fruit.GetComponent<FruitLogick>().plant = pl;
                    // destroy Flower web and set list
                    FindObjectOfType<FlowerS>().SetFlowersLevelSend(FlowerPrifab); 
                    fruitsNum++;                    
                    break;
                }
            }
        }
        if(data.flowerId == -1)
        {
            FindFruitPosition();
            if (FindObjectOfType<InternalFunc>().FreePlantPlace.Count == 0 && place == null)
            {
                //int fruitN = GetRundomFruit();
                //GameObject fruitDel = fruitList[fruitN];
                GameObject fruitDel = GetRundomFruit();
                GameObject pl = fruitDel.GetComponent<FruitLogick>().plant;
                int fruitId = fruitDel.GetComponent<DataScript>().id;
                Destroy(fruitDel);
                fruitList.Remove(fruitDel);
                fruitsNum--;
                if (!Application.isEditor)
                {
                    //send to Plethora Delite fruit.
                    SetFruitsLevelSend(fruitId);
                }
                //new fruit
                GameObject fruit = Instantiate(fruitPrifab, fruitDel.transform.position, fruitPrifab.transform.rotation);
                fruit.GetComponent<DataScript>().id = data.fruitId;
                fruit.GetComponent<FruitLogick>().plant = pl;
                fruitList.Add(fruit);
                fruitsNum++;
            }
            if (data.flowerId == -1 && place != null)
            {
                GameObject fruit = Instantiate(fruitPrifab, new Vector3(place.transform.position.x, place.transform.position.y - 3, place.transform.position.z), fruitPrifab.transform.rotation);
                fruit.GetComponent<DataScript>().id = data.fruitId;
                fruit.GetComponent<FruitLogick>().plant = place; // to Set plant Free after destroy the fruit
                fruitList.Add(fruit);
                fruitsNum++;
                place = null;
            }           
        }
    }
    private GameObject GetRundomFruit()
    {
        GameObject f = null;
        if (fruitList.Count != 0) 
        {
            foreach (GameObject i in fruitList)
            {
                if (i.GetComponent<FruitLogick>().haveBird == false)
                {
                    f = i;
                    break;
                }
            }          
        }
        return f;
    }
    //private int GetRundomFruit()
    //{
    //    int rundomNumOfFruit = Random.Range(0, fruitList.Count);
    //    return rundomNumOfFruit;
    //}
    public void ResetFruitSimulation()
    {
        foreach (GameObject i in fruitList)
        {
            Destroy(i);
        }
        fruitList.Clear();
        fruitsNum = 0;
    }
}
