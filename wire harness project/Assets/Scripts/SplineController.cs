using SplineMesh;
using UnityEngine;

public class SplineController : MonoBehaviour
{
    public Spline spline;

    public GameObject nodessphers;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spline.nodes[0].Position = nodessphers.transform.position;

        
    }
}
