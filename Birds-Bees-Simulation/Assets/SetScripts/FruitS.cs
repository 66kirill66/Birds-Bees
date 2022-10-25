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
            int id = 1;
            for (int i = 0; i < fruitCheck; i++)
            {
                InstantiateFruitCheck(id);
                id++;
            }
        }    
    }
    public void SetFruitsLevelSend(int fruitId)
    {
        if (!Application.isEditor)
        {
            SetFruitsLevelWeb(fruitId);
        }          
    }
    public void ResetFruitSimulation()
    {
        //var plaseF = FindObjectsOfType<PlantPlace>();
        //foreach (PlantPlace i in plaseF)
        //{
        //    i.isFree = true;
        //}
        //foreach (GameObject i in fruitList)
        //{
        //    Destroy(i);
        //}
        //fruitList.Clear();
        //fruitsNum = 0;
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
    public void InstantiateFruitCheck(int id)
    {
        if(fruitsNum < 15)
        {
            FindFlowerPosition();
            if (place != null)
            {
                GameObject fruit = Instantiate(fruitPrifab, new Vector3(place.transform.position.x, place.transform.position.y - 3, place.transform.position.z), fruitPrifab.transform.rotation);
                fruit.GetComponent<DataScript>().id = id;
                fruit.GetComponent<FruitLogick>().plant = place; // to Set plant Free after destroy the fruit
                fruitList.Add(fruit);
                fruitsNum++;
            }
            else
            {
                SetFruitsLevelSend(id);
                fruitsNum--;
            }
        }      
    }
    private void FindFlowerPosition()
    {
        var plaseF = FindObjectsOfType<PlantPlace>();
        foreach (PlantPlace i in plaseF)
        {
            if (i.isFree == true)
            {
                i.isFree = false;
                place = i.gameObject;
                break;
            }
            else
            {
                place = null;
            }
        }
    }
    private int GetRundomFruit()
    {
        int rundomNumOfFruit = Random.Range(0, fruitList.Count);
        return rundomNumOfFruit;
    }


    public void InstantiateFruit(string json)
    {
        FruitMData data = FruitMData.CreateFromJSON(json);
        if (data.flowerId != -1 && fruitsNum < 15)
        {           
            foreach (GameObject i in FindObjectOfType<FlowerS>().flowerList)
            {
                if (data.flowerId == i.GetComponent<DataScript>().id)
                {
                    GameObject fruit = Instantiate(fruitPrifab, new Vector3(i.transform.position.x, i.transform.position.y - 3, i.transform.position.z), fruitPrifab.transform.rotation);
                    fruit.GetComponent<DataScript>().id = data.fruitId;
                    fruitList.Add(fruit);
                    GameObject pl = i.GetComponent<FlowerScript>().plant;
                    fruit.GetComponent<FruitLogick>().plant = pl;
                    fruitsNum++;
                    FindObjectOfType<FlowerS>().SetFlowersLevelSend(i); // destroy Flower web and set list
                    break;
                }
            }
        }
        FindFlowerPosition();
        if (data.flowerId == -1 && fruitsNum < 15 && place != null)
        {
            GameObject fruit = Instantiate(fruitPrifab,new Vector3(place.transform.position.x, place.transform.position.y -3, place.transform.position.z), fruitPrifab.transform.rotation);
            fruit.GetComponent<DataScript>().id = data.fruitId;
            fruit.GetComponent<FruitLogick>().plant = place; // to Set plant Free after destroy the fruit
            fruitList.Add(fruit);
            fruitsNum++;
        }
        else if(place == null)
        {
            int fruitN = GetRundomFruit();
            GameObject fruitDel = fruitList[fruitN];
            GameObject pl = fruitDel.GetComponent<FruitLogick>().plant;
            int fruitId = fruitDel.GetComponent<DataScript>().id;
            Destroy(fruitDel);
            fruitList.Remove(fruitDel);


            if (!Application.isEditor)
            {
                //send to Plethora Delite fruit.
                SetFruitsLevelSend(fruitId);
                fruitsNum--;
            }          
            //new fruit
            GameObject fruit = Instantiate(fruitPrifab, fruitDel.transform.position, fruitPrifab.transform.rotation);
            fruit.GetComponent<DataScript>().id = data.fruitId;
            fruit.GetComponent<FruitLogick>().plant = pl;
            fruitList.Add(fruit);
            fruitsNum++;
        }
    }    
}
