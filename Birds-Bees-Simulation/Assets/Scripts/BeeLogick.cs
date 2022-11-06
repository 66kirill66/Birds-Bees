using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeLogick : MonoBehaviour
{
    float PosX, PosY, PosZ;
    float speed = 0.2f;
    Vector3 endPos;
    [SerializeField] GameObject hive;
    public GameObject flowerPrifab;
    public GameObject flowerToCome;
    public int beeId;
    int flowerId;
    //Move Booleans
    bool isHaive;
    bool isFlower;
    bool fly;
    Animator anim;
    public ParticleSystem beeParticle;
    void Start()
    {
        anim = GetComponent<Animator>();
        fly = true;
        StartCoroutine(FlyRange());
        GetBeeId();
    }
    private void FixedUpdate()
    {
        LookAtPoint();      
    }
    private void GetBeeId()
    {
        beeId = GetComponent<DataScript>().id;
    }
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
                    flowerPrifab = i;
                    flowerToCome = i;
                    endPos = i.GetComponent<FlowerScript>().beePocToCome.transform.position;
                    break;
                }
            }
        }
    }
    public void BeeGoToFlower()
    {
        isHaive = false;
        fly = false;
        StopAllCoroutines();        
        isFlower = true;
        StartCoroutine(FlowerTransform());
    }
    public void BeeGoToHive()
    {
        StopAllCoroutines();
        isHaive = true;
        StartCoroutine(HivePlacee());
    }
    private IEnumerator FlyRange()
    {
        while (fly == true)
        {
            RundomPointToFly();
            endPos = new Vector3(PosX, PosY, PosZ);
            if (ReturnDist() < 40)
            {
                RundomPointToFly();
                endPos = new Vector3(PosX, PosY, PosZ);
            }
            if (ReturnDist() > 40)
            {               
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
                if (ReturnDist() < 1)
                {
                    anim.enabled = false;
                }
            }
        }
    }
    private IEnumerator FlowerTransform()
    {
        while (isFlower == true)
        {
            Vector3 startPos = transform.position;
            float travel = 0;
            while (travel < 1)
            {
                travel += Time.deltaTime * 0.3f;
                CheckIfFlowerNotDelited();
                transform.position = Vector3.Lerp(startPos, new Vector3(endPos.x - 0.2f, endPos.y, endPos.z), travel);
                yield return new WaitForEndOfFrame();
            }
            FindObjectOfType<BeeS>().BeeMeetFlowerWeb(beeId, flowerId);  // web             
            Invoke("NulledllFowerPrifab", 4f);
            Invoke("BeeBackToFly", 1);
            isFlower = false;
        }       
    }
    private void NulledllFowerPrifab()
    {      
        if (flowerPrifab != null)
        {
            if (flowerPrifab.GetComponent<FlowerScript>() != null)
            {
                flowerPrifab.GetComponent<FlowerScript>().haveBee = false;
            }           
            flowerPrifab = null;
            flowerToCome = null;
        }
        FindObjectOfType<BeeS>().SetBeeListFlowerFly(gameObject);
    }
    private void CheckIfFlowerNotDelited()
    {
        if (flowerToCome == null)
        {
            BeeBackToFly();
            FindObjectOfType<BeeS>().SetBeeListFlowerFly(gameObject);
            flowerToCome = null;
            flowerPrifab = null;           
        }
    }
    public void BeeBackToFly()
    {
        isHaive = false;
        isFlower = false;
        StopAllCoroutines();    
        fly = true;
        anim.enabled = true;
        StartCoroutine(FlyRange());
    }
    private void RundomPointToFly()
    {
        PosX = Random.Range(120, 200);
        PosY = Random.Range(15, 19);
        PosZ = 150;
    }
    private float ReturnDist()
    {
        float dis = Vector3.Distance(transform.position, endPos);
        return dis;
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
