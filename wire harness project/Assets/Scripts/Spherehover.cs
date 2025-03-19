using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class Spherehover : MonoBehaviour
{

    private GameObject child1;
    private GameObject child2;
    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;
    private Coroutine disableCoroutine;
    private SplineController splineController;

    //public void AddInteractables(int sphereListLocation)
    public void Awake()
    {
        // Automatically assign first and second children
        if (transform.childCount >= 2)
        {
            child1 = transform.GetChild(0).gameObject;
            child2 = transform.GetChild(1).gameObject;
        }
        else
        {
            Debug.LogError("Not enough children! Need at least 2.");
            return;
        }

        // Ensure Rigidbody is attached
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.useGravity = false;
        rb.isKinematic = true;


        // Ensure XRGrabInteractable is attached
        grabInteractable = GetComponent<XRGrabInteractable>() ?? gameObject.AddComponent<XRGrabInteractable>();

      
        // Find the existing SplineController in the scene
        splineController = FindObjectOfType<SplineController>();
        if (splineController == null)
        {
            Debug.LogError("SplineController not found in the scene!");
            return;
        }

       
        // Hide children initially
        child1.SetActive(false);
        child2.SetActive(false);

        // Subscribe to hover events
        grabInteractable.hoverEntered.AddListener(OnHoverEnter);
        grabInteractable.hoverExited.AddListener(OnHoverExit);
    }

    private void OnHoverEnter(HoverEnterEventArgs args)
    {
        if (disableCoroutine != null)
            StopCoroutine(disableCoroutine);

        child1.SetActive(true);
        child2.SetActive(true);
    }

    private void OnHoverExit(HoverExitEventArgs args)
    {
        if (disableCoroutine != null)
            StopCoroutine(disableCoroutine);

        disableCoroutine = StartCoroutine(DisableAfterDelay(5f));
    }

    private IEnumerator DisableAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        child1.SetActive(false);
        child2.SetActive(false);
    }

   

}