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
    GameObject placeChangeToFree;
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
        if (place == null && FindObjectOfType<InternalFunc>().FreePlantPlace.Count == 0  )
        {
            //int fruitN = GetRundomFruit();
            //GameObject fruitDel = fruitList[fruitN];
            GameObject fruitDel = GetRundomFruit();
            if(fruitDel != null)
            {
                GameObject pl = fruitDel.GetComponent<FruitLogick>().plant;
                fruitList.Remove(fruitDel);
                Destroy(fruitDel);
                fruitsNum--;

                //new fruit
                GameObject fruit = Instantiate(fruitPrifab, fruitDel.transform.position, fruitPrifab.transform.rotation);
                fruit.GetComponent<FruitLogick>().plant = pl;
                fruitList.Add(fruit);
                fruitsNum++;
            }           
        }
        if (place != null)
        {
            GameObject fruit = Instantiate(fruitPrifab, new Vector3(place.transform.position.x, place.transform.position.y - 3, place.transform.position.z), fruitPrifab.transform.rotation);
            fruit.GetComponent<FruitLogick>().plant = place; // to Set plant Free after destroy the fruit
            fruitList.Add(fruit);
            fruitsNum++;
            place = null;
        }
        else { return; }
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
                    fruitList.Remove(i.gameObject);
                    Destroy(i);
                    GameObject pl = i.GetComponent<FruitLogick>().plant;
                    pl.GetComponent<PlantPlace>().isFree = true;
                    FindObjectOfType<InternalFunc>().FreePlantPlace.Add(pl);
                    fruitsNum--;
                    break;
                }
            }
        }
    }
    public void BirdEatFruit(int fruitId) 
    {
        foreach (GameObject i in fruitList)
        {
            if (i.GetComponent<DataScript>().id == fruitId)
            {
                placeChangeToFree = i.GetComponent<FruitLogick>().plant;
                fruitList.Remove(i);
                Destroy(i, 1f);                              
                Invoke("SetPlaceFree", 3);
                SetFruitsLevelSend(fruitId);
                fruitsNum--;
                break;
            }
        }
    }
    private void SetPlaceFree()
    {
        placeChangeToFree.GetComponent<PlantPlace>().isFree = true;
        FindObjectOfType<InternalFunc>().FreePlantPlace.Add(placeChangeToFree);
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
        if (data.flowerId == -1 )
        {
            FindFruitPosition();
            if (FindObjectOfType<InternalFunc>().FreePlantPlace.Count == 0 && place == null)
            {
                //int fruitN = GetRundomFruit();
                //GameObject fruitDel = fruitList[fruitN];
                GameObject fruitDel = GetRundomFruit();
                if (fruitDel != null)
                {                    
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
                    fruit.GetComponent<FruitLogick>().ef.SetActive(true);
                    fruitList.Add(fruit);
                    fruitsNum++;
                }
                else { SetFruitsLevelSend(data.fruitId); }
            }
            if(place != null)
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
        GameObject fruit = null;
        if (fruitList.Count != 0) 
        {
            foreach (GameObject i in fruitList)
            {
                if (i.GetComponent<FruitLogick>().haveBird == false)
                {
                    fruit = i;
                    break;
                }
            }          
        }
        return fruit;
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
