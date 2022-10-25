using UnityEngine;
using UnityEngine.UI;

public class BirdSliderPosition : MonoBehaviour
{
    public Slider slider;
    [SerializeField] float offset;
    int birdId;

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
        slider.value -= 4 * Time.deltaTime;

        slider.transform.position = new Vector3(transform.position.x, transform.position.y + offset, transform.position.z);
        slider.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
