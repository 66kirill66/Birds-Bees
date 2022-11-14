using UnityEngine;
using UnityEngine.UI;

public class BeeSliderPosition : MonoBehaviour
{
    public Slider slider;
    [SerializeField] float offset;
    int beeId;
    public int value;
    public float totalTime;
    public bool sliderDown;

    private void Start()
    {
        //slider.value = 100;
        totalTime = 0;
        GetBeeId();
        sliderDown = true;
    }
    private void GetBeeId()
    {
        beeId = GetComponent<DataScript>().id;
    }

    void Update()
    {
        SetBeeSliderPositionAndValue();
    }

    private void SetBeeSliderPositionAndValue()
    {
        if (sliderDown == true && slider.value != 0)
        {
            totalTime += 1 * Time.deltaTime;
            if (totalTime > 1)
            {
                slider.value -= 2;
                value = Mathf.RoundToInt(slider.value);
                FindObjectOfType<BeeS>().SetBeeEnergySendToWeb(beeId, value);
                totalTime = 0;
            }
        }
        slider.transform.position = new Vector3(transform.position.x, transform.position.y + offset, transform.position.z);
        slider.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
