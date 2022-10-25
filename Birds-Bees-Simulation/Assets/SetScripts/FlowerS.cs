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
    int flowersNum;
    float yRotation;
    public bool isCheck;
    [SerializeField] int flowersCheck;
    void Start()
    {
        if (isCheck == true)
        {
            int id = 1;
            for (int i = 0; i < flowersCheck; i++)
            {
                InstantiateFlower(id);
                id++;
            }
        }

    }
    
    public void SetFlowersLevelSend(GameObject flowerPrifab)
    {
        int flowerId = flowerPrifab.GetComponent<DataScript>().id;
        flowerList.Remove(flowerPrifab);
        Destroy(flowerPrifab,1);
        flowersNum--;
        if (!Application.isEditor)
        {
           SetFlowersLevelWeb(flowerId);
        }           
    }
   

    public void ChangeFlowerTofruit(int flowerId)
    {
        FindObjectOfType<FruitS>().CreateNewFruit(flowerId);
    }
    public void ResetFlowerSimulation()
    {
        //var plaseF = FindObjectsOfType<PlantPlace>();
        //foreach (PlantPlace i in plaseF)
        //{
        //    i.isFree = true;
        //}

        //foreach (GameObject i in flowerList)
        //{
        //    Destroy(i);
        //}
        //flowerList.Clear();
        //flowersNum = 0;
        //foreach(GameObject i in flowerListToDelite)
        //{
        //    Destroy(i);
        //}
        //flowerListToDelite.Clear();
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
                    pl.GetComponent<PlantPlace>().isFree = true;
                    flowerList.Remove(i.gameObject);
                    Destroy(i);
                    flowersNum--;
                    break;
                }
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
                place = i.gameObject; // i = flower Plant / place = i.gameObject.transform.position
                break;
            }
            else
            {
                place = null;
            }
        }       
    }


    public void InstantiateFlower(int id)
    {
        FindFlowerPosition();
        if (flowersNum < 15 && place != null)
        {
            GameObject flower = Instantiate(flowerPrifab, place.transform.position, RundomRotation());
            flower.GetComponent<DataScript>().id = id;
            flower.GetComponent<FlowerScript>().plant = place;
            flowerList.Add(flower);
            flowersNum++;
        }
        else if(place == null)
        {
            int flow = GetRundomFlower();
            GameObject flowerDel = flowerList[flow];
            GameObject pl = flowerDel.GetComponent<FlowerScript>().plant;            
            int flowerId = flowerDel.GetComponent<DataScript>().id;
            Destroy(flowerDel);
            flowerList.Remove(flowerDel);

           
            if (!Application.isEditor)
            {
                //send to Plethora Delite flower.
                SetFlowersLevelWeb(flowerId);
            }
            flowersNum--;
            //new Flower
            GameObject flower = Instantiate(flowerPrifab, flowerDel.transform.position, RundomRotation());
            flower.GetComponent<DataScript>().id = id;
            flower.GetComponent<FlowerScript>().plant = pl;
            flowerList.Add(flower);
            flowersNum++;
        }
    }
    private int GetRundomFlower()
    {
        int rundomNumOfFlower = Random.Range(0, flowerList.Count);      
        return rundomNumOfFlower;
    }  
    public Quaternion RundomRotation()
    {
       yRotation =  Random.Range(80, 130);
       Quaternion.Euler(0, yRotation, 0);
       return Quaternion.Euler(0, yRotation, 0);
    }
}
