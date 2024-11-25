using UnityEngine;
using UnityEngine.UI;

public class ImageTimer : MonoBehaviour
{
    public float MaxTime;
    public bool Tick;

    private Image img;
    private float currentTime;
    private AudioSource audio;

    private void Start()
    {
        img = GetComponent<Image>();
        currentTime = MaxTime;
        audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        Tick = false;
        currentTime -= Time.deltaTime;

        img.fillAmount = currentTime / MaxTime;

        if(currentTime <= 0)
        {
            Tick = true;
            currentTime = MaxTime;
            audio.Play();
        }
    }
}