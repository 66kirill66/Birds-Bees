using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class FlowerS : MonoBehaviour
{
    [DllImport("__Internal")]
    public static extern void SetFlowersLevelWeb(int flowerId);

    public List<GameObject> flowerList = new List<GameObject>();
    public  GameObject place;
    [SerializeField] GameObject flowerPrifab;
    float yRotation;
    public bool isCheck;
    [SerializeField] int flowersCheck;
    
    void Start()
    {       
        if (isCheck == true)
        {
            InvokeRepeating("Ch", 3, 4);
            int id = 1;
            for (int i = 0; i < flowersCheck; i++)
            {
                InstantiateFlower(id);
                id++;
            }
        }      
    }   
    
    private void Ch()
    {
        FindFlowerPosition();     
        if (FindObjectOfType<InternalFunc>().FreePlantPlace.Count == 0 && place == null)
        {
            if (flowerList.Count > 7)
            {
                GameObject flowerToDel = GetRundomFlower();
                if (flowerToDel != null)
                {
                    GameObject pl = flowerToDel.GetComponent<FlowerScript>().plant; // bug 
                    int flowerId = flowerToDel.GetComponent<DataScript>().id;
                    Destroy(flowerToDel);
                    flowerList.Remove(flowerToDel);
                    
                    //new Flower
                    GameObject flower = Instantiate(flowerPrifab, flowerToDel.transform.position, RundomRotation());
                    flower.GetComponent<FlowerScript>().plant = pl;
                    flowerList.Add(flower);
                }
            }           
            else
            {
                ////new
                GameObject fruitDel = FindObjectOfType<FruitS>().GetRundomFruit();
                if (fruitDel != null)
                {
                    GameObject pl = fruitDel.GetComponent<FruitLogick>().plant;
                    int fruitId = fruitDel.GetComponent<DataScript>().id;
                    Destroy(fruitDel);
                    // fruitsNum--;
                    if (!Application.isEditor)
                    {
                        //send to Plethora Delite fruit.
                        FindObjectOfType<FruitS>().SetFruitsLevelSend(fruitId);
                    }
                    //new flower
                    GameObject flowerP = Instantiate(flowerPrifab, pl.transform.position, RundomRotation());
                    flowerP.GetComponent<FlowerScript>().plant = pl;
                    flowerList.Add(flowerP);
                }
            }
        }
        if (place != null)
        {
            GameObject flower = Instantiate(flowerPrifab, place.transform.position, RundomRotation());
            flower.GetComponent<FlowerScript>().plant = place;
            flowerList.Add(flower);
            place = null;
        }
    }
    public void SetFlowersLevelSendObj(GameObject flowerPrifab)
    {
        int flowerId = flowerPrifab.GetComponent<DataScript>().id;
        flowerList.Remove(flowerPrifab);
        Destroy(flowerPrifab,1);
        if (!Application.isEditor)
        {
           SetFlowersLevelWeb(flowerId);
        }           
    }
    public void SetFlowersLevelSendId(int flowerId)
    {
        if (!Application.isEditor)
        {
            SetFlowersLevelWeb(flowerId);
        }
    }
    public void ChangeFlowerTofruit(int flowerId)
    {
        FindObjectOfType<FruitS>().CreateNewFruit(flowerId);
    }
    
    public void DestroyFlower(int flowerId)
    {
        if (flowerList.Count != 0)
        {
            foreach (GameObject i in flowerList)
            {
                if (i.GetComponent<DataScript>().id == flowerId)
                {
                    GameObject pl = i.GetComponent<FlowerScript>().plant;                   
                    flowerList.Remove(i.gameObject);
                    pl.GetComponent<PlantPlace>().isFree = true;
                    FindObjectOfType<InternalFunc>().FreePlantPlace.Add(pl);
                    Destroy(i);
                    break;
                }
            }
        }
    }
    private void FindFlowerPosition()
    {
        if(FindObjectOfType<InternalFunc>().FreePlantPlace.Count != 0)
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
    
    public void InstantiateFlower(int id)
    {
        FindFlowerPosition();
        if (FindObjectOfType<InternalFunc>().FreePlantPlace.Count == 0 && place == null)
        {
            if(flowerList.Count > 10)
            {
                GameObject flowerToDel =  GetRundomFlower();
                if (flowerToDel != null)
                {
                    GameObject pl = flowerToDel.GetComponent<FlowerScript>().plant;
                    int flowerId = flowerToDel.GetComponent<DataScript>().id;
                    Destroy(flowerToDel);
                    if (!Application.isEditor)
                    {
                        //send to Plethora Delite flower.
                        SetFlowersLevelWeb(flowerId);
                    }
                    //new Flower
                    GameObject flower = Instantiate(flowerPrifab, flowerToDel.transform.position, RundomRotation());
                    flower.GetComponent<DataScript>().id = id;
                    flower.GetComponent<FlowerScript>().plant = pl;
                    flowerList.Add(flower);
                }
            }          
            else
            {
                ////new
                GameObject fruitDel = FindObjectOfType<FruitS>().GetRundomFruit();
                if (fruitDel != null)
                {
                    GameObject pl = fruitDel.GetComponent<FruitLogick>().plant;
                    int fruitId = fruitDel.GetComponent<DataScript>().id;
                    Destroy(fruitDel);
                   // fruitsNum--;
                    if (!Application.isEditor)
                    {
                        //send to Plethora Delite fruit.
                        FindObjectOfType<FruitS>().SetFruitsLevelSend(fruitId);
                    }
                    //new flower
                    GameObject flower = Instantiate(flowerPrifab, pl.transform.position, RundomRotation());
                    flower.GetComponent<DataScript>().id = id;
                    flower.GetComponent<FlowerScript>().plant = pl;
                    flowerList.Add(flower);
                }                
            }
        }
        if (place != null)
        {
            GameObject flower = Instantiate(flowerPrifab, place.transform.position, RundomRotation());
            flower.GetComponent<DataScript>().id = id;
            flower.GetComponent<FlowerScript>().plant = place;
            flowerList.Add(flower);
            place = null;
        }
    }
    public GameObject GetRundomFlower()
    {
        GameObject n = null;
        foreach (GameObject i in flowerList)
        {
            if (i.GetComponent<FlowerScript>().haveBee == false)
            {
                n = i;
                flowerList.Remove(i);
                break;
            }
        }
        return n;
    }
    public Quaternion RundomRotation()
    {
       yRotation =  Random.Range(80, 130);
       Quaternion.Euler(0, yRotation, 0);
       return Quaternion.Euler(0, yRotation, 0);
    }
    public void ResetFlowerSimulation()
    {
        foreach (GameObject i in flowerList)
        {
            Destroy(i);
        }
        flowerList.Clear();
    }
}
