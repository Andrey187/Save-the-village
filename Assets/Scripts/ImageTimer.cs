using UnityEngine;
using UnityEngine.UI;

public class ImageTimer : MonoBehaviour
{
    [SerializeField] private float MaxTime;
    public bool Tick;

    private Image img;
    private float currentTime;

    void Start()
    {
        img = GetComponent<Image>();
        currentTime = MaxTime;
    }

    void Update()
    {
        Tick = false;
        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            Tick = true;
            currentTime = MaxTime;
        }

        img.fillAmount = currentTime / MaxTime;
    }
}
