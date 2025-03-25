using SplineMesh;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.UIElements;
using System.Data;
using TMPro;

public class SplineController : MonoBehaviour
{

    private List listScript;
    public List<GameObject> nodeSpheres = new List<GameObject>(); // Store spheres dynamically
    private Spline spline; // Assign the spline component in Inspector
    public GameObject spherePrefab; // Assign the sphere prefab in Inspector
    private Transform sphereParent; // Parent object to hold spheres
    private GameObject lastCreatedSphere;
    public GameObject nodeEntryPrefab; // Assign the Node UI prefab in the Inspect
    private List<GameObject> nodeEntries = new List<GameObject>(); // Store UI elements

    private float spacing = 0.2f; // Distance between spheres

    public static bool isNodeupdate = false;

    private void Start()
    {
        sphereParent = gameObject.transform;
        spline = gameObject.GetComponent<Spline>();

        listScript = FindObjectOfType<List>();
    }


    private void Update()
    {
        /*for (int i = 0; i < nodeSpheres.Count; i++)
        {
            if (nodeSpheres[i] != null)
            {
                UpdateNodeUI(i);
            }
        }*/

        // Ensure there are at least 2 spheres
        if (nodeSpheres.Count < 2)
        {
            AddSphere(nodeSpheres.Count);
        }

        if (spline.nodes.Count == 0)
        {
            SplineNode firstNode = new SplineNode(transform.position, transform.forward);
            spline.AddNode(firstNode);
        }

        // If more spheres exist than spline nodes, remove the last node
        else if (nodeSpheres.Count < spline.nodes.Count)
        {
            SplineNode clearNode = spline.nodes[0];
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
                    spline.nodes[i].Position = transform.InverseTransformPoint(nodeSpheres[i].transform.position);
                    GameObject currenspher = nodeSpheres[i].gameObject;
                    //currenspher.GetComponent<Spherehover>().AddInteractables(i);

                    currenspher.name = "Sphere " + i;
                }
            }
        }

        if (isNodeupdate) // Check if the flag is true
        {
            for (int i = 0; i < nodeEntries.Count; i++) // Iterate through nodes
            {
                UpdateNodeUI(i);
            }

            isNodeupdate = false; // Reset flag after updating
        }
    }

    public void AddSphere(int i)
    {
        Vector3 position;

        // If no spheres exist, place the first one at (0,0,0)
        if (nodeSpheres.Count == 0)
        {
            position = spline.nodes.Count > 0 ? spline.nodes[0].Position : transform.position;
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
            nodeSpheres.Insert(i + 1, newSphere);
        }

        GameObject newEntry = Instantiate(nodeEntryPrefab);
        nodeEntries.Add(newEntry);
        //UpdateNodeUI(nodeEntries.Count - 1);

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


                XRSimpleInteractable addinteractable = currenspher.transform.Find("Add").GetComponent<XRSimpleInteractable>();

                XRSimpleInteractable removeinteractable = currenspher.transform.Find("Remove").GetComponent<XRSimpleInteractable>();


                addinteractable.selectEntered.RemoveAllListeners();
                removeinteractable.selectEntered.RemoveAllListeners();


                int curretpose = i;

                addinteractable.selectEntered.AddListener((args) => AddSphere(curretpose));
                removeinteractable.selectEntered.AddListener((args) => RemoveSphere(curretpose));

                //UpdateNodeUI(i);
            }

        }
    }

    private void UpdateNodeUI(int index)
    {
        if (index < nodeEntries.Count && index < nodeSpheres.Count)
        {
            TextMeshProUGUI text = nodeEntries[index].GetComponentInChildren<TextMeshProUGUI>();
            Vector3 pos = nodeSpheres[index].transform.position;
            text.text = $"Node {index}: X = {pos.x:F2}, Y = {pos.y:F2}, Z = {pos.z:F2}";

            if (listScript != null)
            {
                nodeEntries[index].transform.SetParent(listScript.nodeCardParent, false);
            }
        }
    }

    public void RemoveSphere(int i)
    {
        if (i < 0 || i >= nodeSpheres.Count)
            return; // Prevent out-of-range errors

        GameObject sphereToRemove = nodeSpheres[i];
        nodeSpheres.RemoveAt(i);
        Destroy(sphereToRemove);

        Destroy(nodeEntries[i]); // Remove corresponding UI entry
        nodeEntries.RemoveAt(i);

        UpdateIndexPosition();
    }

}

