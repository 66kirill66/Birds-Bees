using UnityEngine;
using UnityEngine.UI;

public class SliderPosition : MonoBehaviour
{
    public Slider slider;
    [SerializeField] float offset;
    int beeId;
    public int value;
    public int carrentValue;
    public bool sliderDown = true;

    private void Start()
    {

        slider.value = 100;
        carrentValue = 100;
        GetBeeId();
    }
    private void GetBeeId()
    {
        beeId = GetComponent<DataScript>().id;
    }

    void Update()
    {
        if(sliderDown == true)
        {
            if (slider.value > 0)
            {
                slider.value -= 4 * Time.deltaTime;

                if (carrentValue - 1 > slider.value)
                {
                    value = Mathf.RoundToInt(slider.value);
                    carrentValue = value;
                    FindObjectOfType<BeeS>().SetBeeEnergySendToWeb(beeId, value);
                }
            }
        }       
        slider.transform.position = new Vector3(transform.position.x, transform.position.y + offset, transform.position.z);
        slider.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
