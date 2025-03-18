using SplineMesh;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SplineController : MonoBehaviour
{
    public List<GameObject> nodeSpheres = new List<GameObject>(); // Store spheres dynamically
    private Spline spline; // Assign the spline component in Inspector
    public GameObject spherePrefab; // Assign the sphere prefab in Inspector
    private Transform sphereParent; // Parent object to hold spheres

    private float spacing = 0.2f; // Distance between spheres


    private void Start()
    {
        sphereParent = gameObject.transform;
        spline = gameObject.GetComponent<Spline>();
    }

    private void Update()
    {

        // Ensure there are at least 2 spheres
        if (nodeSpheres.Count < 2)
        {
            AddSphere(nodeSpheres.Count);
        }

        // If more spheres exist than spline nodes, remove the last node
        else if (nodeSpheres.Count < spline.nodes.Count)
        {
            spline.RemoveNode(spline.nodes[spline.nodes.Count - 1]);
            return;
        }

        // If more nodes are needed, add one
        else if (nodeSpheres.Count > spline.nodes.Count)
        {
            SplineNode newNode = new SplineNode(
                spline.nodes[spline.nodes.Count - 1].Position,
                spline.nodes[spline.nodes.Count - 1].Direction
            );
            spline.AddNode(newNode);
            return;
        }

        // Update spline to follow sphere positions
        else
        {
            for (int i = 0; i < nodeSpheres.Count; i++)
            {
                if (nodeSpheres[i] != null) // Check if the sphere still exists
                {
                    spline.nodes[i].Position = nodeSpheres[i].transform.position;
                    GameObject currenspher = nodeSpheres[i].gameObject;
                    //currenspher.GetComponent<Spherehover>().AddInteractables(i);

                    currenspher.name = "Sphere " + i;
                }
            }
        }
    }

    public void AddSphere(int i)
    {
        Vector3 position;

        // If no spheres exist, place the first one at (0,0,0)
        if (nodeSpheres.Count == 0)
        {
            position = Vector3.zero;
            // Instantiate new sphere and add it to the list
            GameObject newSphere = Instantiate(spherePrefab, position, Quaternion.identity, sphereParent);
            nodeSpheres.Add(newSphere);
        }
        else if (nodeSpheres.Count == 1)
        {
            Transform lastSphere = nodeSpheres[i - 1].transform;
            position = lastSphere.position + new Vector3(spacing, 0, 0);
            GameObject newSphere = Instantiate(spherePrefab, position, Quaternion.identity, sphereParent);
            nodeSpheres.Add(newSphere);
        }
        else
        {
            // Get last sphere's position and add spacing
            Transform lastSphere = nodeSpheres[i].transform;
            position = lastSphere.position + new Vector3(spacing, 0, 0);

            // Instantiate new sphere and add it to the list
            GameObject newSphere = Instantiate(spherePrefab, position, Quaternion.identity, sphereParent);
            nodeSpheres.Insert(i+1, newSphere);
        }

        UpdateIndexPosition();
    }

    public void UpdateIndexPosition()
    {
        for (int i = 0; i < nodeSpheres.Count; i++)
        {
            if (nodeSpheres[i] != null) // Check if the sphere still exists
            {
                GameObject currenspher = nodeSpheres[i].gameObject;
                currenspher.name = "Sphere " + i;
                XRSimpleInteractable interactable = currenspher.GetComponent<XRSimpleInteractable>();

                interactable.selectEntered.RemoveAllListeners();

                int curretpose = i;

                interactable.selectEntered.AddListener((args) => AddSphere(curretpose));
            }
        }

    }


    public void RemoveSphere()
    {
        // Ensure at least 2 spheres remain
        if (nodeSpheres.Count > 2)
        {
            GameObject lastSphere = nodeSpheres[nodeSpheres.Count - 1];
            nodeSpheres.RemoveAt(nodeSpheres.Count - 1);
            Destroy(lastSphere);
        }

        UpdateIndexPosition();
    }


    public void printposition(int i)
    {
        Debug.Log(i);
    }
}
