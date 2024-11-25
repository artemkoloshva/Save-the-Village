using UnityEngine;
using UnityEngine.UI;

public class ImageChanger : MonoBehaviour
{
    public Sprite Sprite1;
    public Sprite Sprite2;

    private bool changed;

    private Image img;
    
    private void Start()
    {
        img = GetComponent<Image>();
    }

    public void ChangeSprite()
    {
        if (changed)
        {
            img.sprite = Sprite1;
        }
        else
        {
            img.sprite = Sprite2;
        }

        changed = !changed;
    }
}