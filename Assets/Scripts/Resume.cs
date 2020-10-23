using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Resume : MonoBehaviour
{
    bool isFacing = false;
    Vector3 startPosition;

    GameObject button;
    [SerializeField]
    GameObject desk;

    private void Start()
    {
        isFacing = false;
        startPosition = gameObject.transform.position;
        button = transform.GetChild(0).gameObject;
        button.SetActive(false);
    }

    public void OnClickResume()
    {
        if(!isFacing)
        {
            GetComponent<PointerEvents>().enabled = false;
            GetComponent<Outline>().enabled = false;
            button.SetActive(true);
            isFacing = true;
            desk.SetActive(false);

            //transform.LookAt(Camera.main.transform);
            transform.position = Camera.main.transform.position - new Vector3(0, 0, .75f);
            transform.localScale *= 2.5f;
            transform.eulerAngles += new Vector3(90, 0, 0);
        }
        else
        {

        }
        
    }

    public void ReturnToDesk()
    {
        Debug.Log("RETURN TO DESK");

        transform.localScale /= 2.5f;
        transform.eulerAngles -= new Vector3(90, 0, 0);
        transform.position = startPosition;

        button.SetActive(false);
        desk.SetActive(true);
        GetComponent<PointerEvents>().enabled = true;
        GetComponent<Outline>().enabled = true;
        isFacing = false;
    }




}


