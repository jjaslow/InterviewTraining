using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonImageChanger : MonoBehaviour
{
    [SerializeField]
    Sprite buttonImage;
    [SerializeField]
    Sprite buttonImageHover;

    Image image;


    private void Start()
    {
        image = GetComponent<Image>();
        image.sprite = buttonImage;
    }

    public void SwapSprite(int i)
    {
        if (i==0)
            image.sprite = buttonImage;
        else
            image.sprite = buttonImageHover;
    }



}
