using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static OVRInput;

public class PhysicsPointer : MonoBehaviour
{

    public float defaultLength = 5f;
    private LineRenderer lineRenderer = null;
    bool isPressing = false;
    bool pressingTransitioning = false;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void LateUpdate()
    {
        UpdateLength();
    }

    private void UpdateLength()
    {
        lineRenderer.SetPosition(0, transform.position + (transform.forward * .05f));
        lineRenderer.SetPosition(1, CalculateEnd());
    }

    private Vector3 CalculateEnd()
    {
        RaycastHit hit = CreateForwardRaycast();
        Vector3 endPosition = DefaultEnd(defaultLength);

        if(hit.collider)
        {
            endPosition = hit.point;
            TakeAction(hit);
        }

        //IInteractable other = hit.collider.gameObject.GetComponent<IInteractable>();
        //if (other != null)
        //    TakeAction(other);

        return endPosition;
    }

    private RaycastHit CreateForwardRaycast()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        Physics.Raycast(ray, out hit, defaultLength);
        return hit;
    }

    private Vector3 DefaultEnd(float length)
    {
        return transform.position + (transform.forward * length);
    }


    private void TakeAction(RaycastHit hit)
    {
        PointerEvents pe = hit.collider.gameObject.GetComponent<PointerEvents>();
        if (pe == null)
            return;

        pe.OnHover();

        if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) > .6f && !isPressing)
        {
            pe.OnPointerClick();
            isPressing = true;
        }

        if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) < .4f && isPressing && !pressingTransitioning)
        {
            StartCoroutine(ReAllowPressing());
            pe.OnPointerRelease();
        }


    }

    IEnumerator ReAllowPressing()
    {
        pressingTransitioning = true;
        yield return new WaitForSeconds(0.5f);
        isPressing = false;
        pressingTransitioning = false;
    }

}



