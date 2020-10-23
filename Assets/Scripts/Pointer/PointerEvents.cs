using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))]
public class PointerEvents : MonoBehaviour//, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    //[SerializeField] private Color normalColor;
    //[SerializeField] private Color enterColor;
    //[SerializeField] private Color downColor;
    [SerializeField] private UnityEvent OnClick = new UnityEvent();

    Outline outline;

    bool isHovering = false;


    private void Start()
    {
        outline = GetComponent<Outline>();
        //outline.OutlineColor = Color.red;
        //outline.OutlineMode = Outline.Mode.OutlineVisible;
        //outline.OutlineWidth = 3f;
    }

    private void Update()
    {
        isHovering = false;
        //gameObject.GetComponentInChildren<Renderer>().material.color = normalColor;
        outline.enabled = false;
    }




    public void OnPointerClick()
    {
        OnClick.Invoke();
        //gameObject.GetComponentInChildren<Renderer>().material.color = downColor;
        Debug.Log("Click on " + name);
    }



    public void OnHover()
    {
        Debug.Log("Pointer EVENTS pointing at: " + gameObject.name);
        //gameObject.GetComponentInChildren<Renderer>().material.color = enterColor;
        outline.enabled = true;
        isHovering = true;
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