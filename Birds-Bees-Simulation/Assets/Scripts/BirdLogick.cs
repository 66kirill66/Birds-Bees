using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdLogick : MonoBehaviour
{
    float PosX, PosY, PosZ;
    float speed = 0.2f;
    Vector3 endPos;
    public GameObject fruitPrifab;
    public GameObject fruitToCome;
    public int birdId;
    int fruitId;

    bool isFruit;
    bool fly;
    // Start is called before the first frame update
    void Start()
    {
        fly = true;
        StartCoroutine(FlyRange());
        GetBirdId();
    }
    private void FixedUpdate()
    {
        LookAtPoint();
    }
    private void GetBirdId()
    {
        birdId = GetComponent<DataScript>().id;
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
    public void BirdBackToFly()
    {
        StopAllCoroutines();
        isFruit = false;        
        fly = true;
        StartCoroutine(FlyRange());
        FindObjectOfType<BirdS>().SetBirdListFruitFly(gameObject);
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
                    fruitToCome = i;
                    endPos = i.transform.position;  // Fruit Place
                    break;
                }
            }
        }
    }
    private void CheckIfFruitNotDelited()
    {
        if (fruitToCome == null)
        {
            BirdBackToFly();
            // FindObjectOfType<FruitS>().fruitList.Remove(fruitEndPos);// Check
            fruitToCome = null;
            fruitPrifab = null;
        }
    }
    public void BirdGoToFruit()
    {
        fly = false;
        StopAllCoroutines();
        isFruit = true;
        StartCoroutine(FruitTransform());
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
                CheckIfFruitNotDelited();
                transform.position = Vector3.Lerp(startPos, new Vector3(endPos.x - 0.2f, endPos.y, endPos.z), travel);
                yield return new WaitForEndOfFrame();                 
            }
            FindObjectOfType<BirdS>().BirdMeetFruitrWeb(birdId, fruitId);
            Invoke("NulledlFruitPrifab", 4f);
            Invoke("BirdBackToFly",1);                        
            isFruit = false;
        }
    }
    private void NulledlFruitPrifab()
    {
        if (fruitPrifab != null)
        {           
            if (fruitPrifab.GetComponent<FruitLogick>() != null)
            {
                fruitPrifab.GetComponent<FruitLogick>().haveBird = false;                
            }
            fruitPrifab = null;
            fruitToCome = null;
        }
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
