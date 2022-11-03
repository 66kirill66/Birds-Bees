using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyMoovment : MonoBehaviour
{
    bool fly;
    float PosX, PosY, PosZ;
    float speed = 0.1f;
    [SerializeField] float yBeeOfsset;

    Vector3 endPos; 
    [SerializeField] GameObject hive;
    // GameObjects  - End Point
    public GameObject flowerPrifab;
    public GameObject fruitPrifab;
    public bool isBird;
    //Id`S
    public int beeId;
    public int birdId;
    int flowerId;
    int fruitId;
    //Move Booleans
    bool isHaive;
    bool isFlower;
    bool isFruit;
    // Anim And Particle
    Animator anim;
    public ParticleSystem beeParticle;
   

    void Start()
    {
        anim = GetComponent<Animator>();
        fly = true;
        StartCoroutine(FlyRange());
        GetIdSOfObjects();
    }
    void FixedUpdate()
    {
        LookAtPoint();      
    }
    

    private void GetIdSOfObjects()
    {
        if (isBird == false)
        {
            beeId = GetComponent<DataScript>().id;
        }
        if (isBird == true)
        {
            birdId = GetComponent<DataScript>().id;
        }
    } // Id to Send ;

    
    // Colling From Main Script
    public void BeeMoveToFlower()
    {
        StopAllCoroutines();
        FindFlowerPosition();
        if (flowerPrifab != null)
        {
            BeeGoToFlower();
        }
        else
        {
            BeeBackToFly();
        }
       
    }
    public void BirdMoveToFruit()
    {
        StopAllCoroutines();
        FindFruitPosition();
        if (fruitPrifab != null)
        {
            BirdGoToFruit();
        }
        else
        {
            BirdBackToFly();
        }
    }
    public void BeeGoToHive()
    {
        StopAllCoroutines();
        isHaive = true;
        StartCoroutine(HivePlacee());
    }
    public void BeeBackToFly()
    {
        StopAllCoroutines();
        isHaive = false;
        isFlower = false;
        fly = true;
        anim.enabled = true;
        StartCoroutine(FlyRange());
    }
    public void BirdBackToFly()
    {       
        StopAllCoroutines();
        isFruit = false;
        fly = true;
        StartCoroutine(FlyRange());
    }
    public void BeeGoToFlower()
    {
        StopAllCoroutines();
        isHaive = false;
        fly = false;       
        StartCoroutine(FlowerTransform());
    }
    public void BirdGoToFruit()
    {
        StopAllCoroutines();
        isFruit = true;
        StartCoroutine(FruitTransform());
    }
    // loop Func
    private IEnumerator FlowerTransform()
    {
        while (isFlower == true)
        {         
            Vector3 startPos = transform.position;
            float travel = 0;
            while (travel < 1)
            {
                travel += Time.deltaTime * 0.3f;
                transform.position = Vector3.Lerp(startPos, new Vector3(endPos.x - 0.2f, endPos.y, endPos.z), travel);
                yield return new WaitForEndOfFrame();
                if (ReturnDist() < 1f)
                {
                    if (flowerPrifab != null)
                    {                       
                        FindObjectOfType<BeeS>().BeeMeetFlowerWeb(beeId, flowerId);  // web
                        //flowerPrifab.GetComponent<FlowerScript>().haveBee = false;
                        //flowerPrifab = null;
                        Invoke("BeeBackToFly", 0.8f);
                        Invoke("NulledllFowerPrifab", 4f);
                        //beeParticle.Play();
                    }
                    
                }
            }
            //Invoke("BeeBackToFly", 0.8f);
            //BeeBackToFly();
        }
    }
    private IEnumerator FruitTransform()
    {
        while (isFruit == true)
        {
            Vector3 startPos = transform.position;
            float travel = 0;
            while (travel < 1)
            {
                travel += Time.deltaTime * 0.3f;
                transform.position = Vector3.Lerp(startPos, new Vector3(endPos.x - 0.2f, endPos.y, endPos.z), travel);
                yield return new WaitForEndOfFrame();
                if (ReturnDist() < 1f)
                {
                    if(fruitPrifab != null)
                    {
                        FindObjectOfType<BirdS>().BirdMeetFruitrWeb(birdId, fruitId);                       
                    }                   
                }              
            }
            Invoke("BirdBackToFly", 0.8f);
            Invoke("NulledlFruitPrifab", 4f);
            //Invoke("BirdBackToFly", 0.8f);
        }
    }
    private void NulledlFruitPrifab()
    {
        if (fruitPrifab != null)
        {
            fruitPrifab.GetComponent<FruitLogick>().haveBird = false;
            fruitPrifab = null;
        }
    }
    private void NulledllFowerPrifab()
    {
        if (flowerPrifab != null)
        {

            flowerPrifab.GetComponent<FlowerScript>().haveBee = false;
            flowerPrifab = null;
        }
    }
    private IEnumerator HivePlacee()
    {
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
                //if (PosX > transform.position.x)
                //{
                //    anim.SetBool("Move Left", true);
                //}
                //else
                //{
                //    anim.SetBool("Move Left", false);
                //}
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
    // Other Func
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
                    break;
                }               
            }           
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
                    endPos = i.transform.position;  // Fruit Place
                    break;
                }               
            }
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
