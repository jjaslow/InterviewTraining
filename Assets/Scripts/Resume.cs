using RPG.Dialogue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Resume : MonoBehaviour
{
    bool isFacing = false;
    Vector3 startPosition;

    GameObject button;
    GameObject sections;
    [SerializeField]
    GameObject desk;
    [SerializeField]
    GameObject candidate;

    PlayerConversant playerConversant;




    private void Start()
    {
        playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();

        isFacing = false;
        startPosition = gameObject.transform.position;
        button = transform.GetChild(0).gameObject;
        sections = transform.GetChild(1).gameObject;
        button.SetActive(false);
        sections.SetActive(false);
    }

    public void OnClickResume()
    {
        if(!isFacing && !playerConversant.IsActive())
        {
            GetComponent<Collider>().enabled = false;
            GetComponent<Outline>().enabled = false;
            GetComponent<PointerEvents>().enabled = false;
            button.SetActive(true);
            sections.SetActive(true);
            isFacing = true;
            desk.SetActive(false);
            candidate.SetActive(false);

            //transform.LookAt(Camera.main.transform);
            transform.position = Camera.main.transform.position - new Vector3(0, 0, .85f);
            transform.localScale *= 2.65f;
            transform.eulerAngles += new Vector3(90, 0, 0);
        }

        
    }

    public void ReturnToDesk()
    {
        Debug.Log("RETURN TO DESK");

        transform.localScale /= 2.65f;
        transform.eulerAngles -= new Vector3(90, 0, 0);
        transform.position = startPosition;

        button.SetActive(false);
        sections.SetActive(false);
        desk.SetActive(true);
        candidate.SetActive(true);
        GetComponent<Collider>().enabled = true;
        GetComponent<Outline>().enabled = true;
        GetComponent<PointerEvents>().enabled = true;
        isFacing = false;
    }




}


