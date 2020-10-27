using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class PointerEvents : MonoBehaviour//, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    //[SerializeField] private Color normalColor;
    //[SerializeField] private Color enterColor;
    //[SerializeField] private Color downColor;
    [SerializeField] public UnityEvent OnClick = new UnityEvent();

    Outline outline;
    Image image;
    Renderer rend;
    Color startColor;

    bool isHovering = false;


    private void Start()
    {
        outline = GetComponent<Outline>();
        rend = GetComponent<Renderer>();
        image = GetComponent<Image>();

        if(rend!=null)
            startColor = rend.material.color;

        //outline.OutlineColor = Color.red;
        //outline.OutlineMode = Outline.Mode.OutlineVisible;
        //outline.OutlineWidth = 3f;
    }

    private void Update()
    {
        isHovering = false;
        //gameObject.GetComponentInChildren<Renderer>().material.color = normalColor;

        if(outline!=null)
            outline.enabled = false;
        else if (image!=null)
        {
            image.GetComponent<UIButtonImageChanger>().SwapSprite(0);
        }
        else
        {
            rend.material.color = startColor;
        }
    }


    public void OnHover()
    {
        Debug.Log("Pointer EVENTS pointing at: " + gameObject.name);
        //gameObject.GetComponentInChildren<Renderer>().material.color = enterColor;
        isHovering = true;

        if(outline!=null)
            outline.enabled = true;
        else if (image != null)
        {
            image.GetComponent<UIButtonImageChanger>().SwapSprite(1);
        }
        else
        {
            rend.material.color = new Color(1, 0, 0, .6f);
        }
    }



    public void OnPointerClick()
    {
        OnClick.Invoke();
        //gameObject.GetComponentInChildren<Renderer>().material.color = downColor;
        Debug.Log("Clicked on " + name);
    }


    internal void OnPointerRelease()
    {
        //Debug.Log("Released click on " + name);
    }
}



/*
     public void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.GetComponent<Renderer>().material.color = enterColor;
        Debug.Log("Enter");
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.GetComponent<Renderer>().material.color = normalColor;
        Debug.Log("Exit");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        gameObject.GetComponent<Renderer>().material.color = downColor;
        Debug.Log("Down");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        gameObject.GetComponent<Renderer>().material.color = enterColor;
        Debug.Log("Up");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick.Invoke();
        Debug.Log("Click");
    } 
 
 */