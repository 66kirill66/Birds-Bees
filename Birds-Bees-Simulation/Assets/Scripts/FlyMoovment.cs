using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyMoovment : MonoBehaviour
{
    bool fly;
    float PosX, PosY, PosZ;
    float speed = 0.1f;

    Vector3 endPos;
    [SerializeField] float yBeeOfsset;
    [SerializeField] GameObject hive;

    public GameObject flowerPrifab;
    public GameObject fruitPrifab;

    public bool isBird;
    public int beeId;
    public int birdId;
    int flowerId;
    int fruitId;
    bool isHaive;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        fly = true;
        StartCoroutine(FlyRange());
        if (isBird == false)
        {
            beeId = GetComponent<DataScript>().id;
        }
        if (isBird == true)
        {
            birdId = GetComponent<DataScript>().id;
        }
    }
    private void FixedUpdate()
    {
        LookAtPoint();
    }

    public void BeeMoveToFlower()
    {
        if (fly == true)
        {
            fly = false;
            StopAllCoroutines();
            FindFlowerPosition();
        }
        if (flowerPrifab != null)
        {
            MoveToFlower(endPos);
        }
        else
        {
            FindObjectOfType<BeeS>().timerToFindFlower = 5;
            fly = true;
            StartCoroutine(FlyRange());
        }
    }
    public void BirdMoveToFruit()
    {
        if (fly == true)
        {
            fly = false;
            StopAllCoroutines();
            FindFruitPosition();
        }
        if (fruitPrifab != null)
        {
            MoveToFruit(endPos);
        }
        else
        {
            FindObjectOfType<BirdS>().timerToFindFruit = 5;
            fly = true;
            StartCoroutine(FlyRange());
        }
    }
    
    public void BeeGoToHive()
    {
        fly = false;

        isHaive = true;
        StartCoroutine(HivePlacee());
    }
    private IEnumerator HivePlacee()
    {
        fly = false;
        while (isHaive == true)
        {
            endPos = hive.transform.position;
            Vector3 startPos = transform.position;
            float travel = 0;
            while (travel < 1)
            {
                travel += Time.deltaTime * speed;
                transform.position = Vector3.Lerp(startPos, endPos, travel);
                yield return new WaitForEndOfFrame();
                if( ReturnDist() < 1)
                {
                    anim.enabled = false;
                }
            }
        }
    }
    public void BeeBackToFly()
    {
        isHaive = false;
        StopAllCoroutines();
        fly = true;
        anim.enabled = true;
        StartCoroutine(FlyRange());
    }

    private IEnumerator FlyRange()
    {
        while (fly == true)
        {
            RundomPointToFly();
            endPos = new Vector3(PosX, PosY - yBeeOfsset, PosZ);                
            if (ReturnDist() < 40)
            {
                RundomPointToFly();
                endPos = new Vector3(PosX, PosY - yBeeOfsset, PosZ);
            }
            if (ReturnDist() > 40)
            {
                if (PosX > transform.position.x)
                {
                    anim.SetBool("Move Left", true);
                }
                else
                {
                    anim.SetBool("Move Left", false);
                }
                Vector3 startPos = transform.position;
                float travel = 0;
                while (travel < 1)
                {
                    travel += Time.deltaTime * speed;
                    transform.position = Vector3.Lerp(startPos, endPos, travel);
                    yield return new WaitForEndOfFrame();
                }
            }
           
        }
    }

    private void FindFlowerPosition()
    {
        if (FindObjectOfType<FlowerS>().flowerList.Count != 0)
        {
            foreach (GameObject i in FindObjectOfType<FlowerS>().flowerList)
            {
                if (i.GetComponent<FlowerScript>().haveBee == false)
                {
                    i.GetComponent<FlowerScript>().haveBee = true;
                    flowerId = i.GetComponent<DataScript>().id;                   
                    flowerPrifab = i.gameObject;
                    endPos = i.GetComponent<FlowerScript>().beePocToCome.transform.position;
                    //flowerDelCheck = i;
                    break;
                }               
            }           
        }    
    }
    private void MoveToFlower(Vector3 endPos)
    {
        Vector3 startPos = transform.position;
        transform.position = Vector3.Lerp(startPos, endPos, 0.5f * Time.deltaTime);
        if (ReturnDist() < 1f && fly == false)
        {
            //FindObjectOfType<FlowerS>().SetFlowersLevelSend(flowerDelCheck);
            // GetComponent<SliderPosition>().slider.value += 100;
            flowerPrifab.GetComponent<FlowerScript>().haveBee = false;
            flowerPrifab = null;           
            FindObjectOfType<BeeS>().timerToFindFlower = 5;
            FindObjectOfType<BeeS>().BeeMeetFlowerWeb(beeId, flowerId);  // web
            fly = true;
            StartCoroutine(FlyRange());           
        }
    }
    private void FindFruitPosition()
    {
        if (FindObjectOfType<FruitS>().fruitList.Count != 0)
        {
            foreach (GameObject i in FindObjectOfType<FruitS>().fruitList)
            {
                if (i.GetComponent<FruitLogick>().haveBird == false)
                {
                    fruitId = i.GetComponent<DataScript>().id;
                    i.GetComponent<FruitLogick>().haveBird = true;                  
                    fruitPrifab = i;
                    endPos = i.transform.position;
                    break;
                }               
            }
        }       
    }
    private void MoveToFruit(Vector3 endPos)
    {
        Vector3 startPos = transform.position;
        transform.position = Vector3.Lerp(startPos, endPos, 0.5f * Time.deltaTime);
        if (ReturnDist() < 1 && fly == false)
        {
            // GetComponent<SliderPosition>().slider.value += 100;
            fruitPrifab.GetComponent<FruitLogick>().haveBird = false;
            fruitPrifab = null;
            FindObjectOfType<BirdS>().timerToFindFruit = 5;
            FindObjectOfType<BirdS>().BirdMeetFruitrWeb(birdId, fruitId); /////
            fly = true;
            StartCoroutine(FlyRange());
        }
    }
    private float ReturnDist()
    {
        float dis = Vector3.Distance(transform.position, endPos);
        return dis;
    }
    private void RundomPointToFly()
    {
        PosX = Random.Range(120, 200);
        PosY = Random.Range(14, 26);
        PosZ = 150;
    }

    private void LookAtPoint()
    {
        Vector3 direction = endPos - transform.position;
        if (direction != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 3 * Time.deltaTime);  // from.rotation, to.rotation, speed * Time.time
        }
    }
}
