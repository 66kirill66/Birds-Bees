using UnityEngine;
using UnityEngine.UI;

public class BirdSliderPosition : MonoBehaviour
{
    public Slider slider;
    [SerializeField] float offset;
    int birdId;

    bool sliderDown;
    float totalTime;

    private void Start()
    {
        slider.value = 100;
        GetBirdId();
    }
    private void GetBirdId()
    {
        birdId = GetComponent<DataScript>().id;
    }

    void Update()
    {
        SetBirdSliderPositionAndValue();
    }

    private void SetBirdSliderPositionAndValue()
    {
        totalTime += 1 * Time.deltaTime;
        if (totalTime > 1)
        {
            slider.value -= 1;
            int sliderV = Mathf.RoundToInt(slider.value);
            FindObjectOfType<BirdS>().SetBirdEnergySendToWeb(birdId, sliderV);
            totalTime = 0;
        }
        slider.transform.position = new Vector3(transform.position.x, transform.position.y + offset, transform.position.z);
        slider.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
