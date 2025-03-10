using SplineMesh;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public class SplineController : MonoBehaviour
{
    public GameObject[] nodeSpheres; // Assign your sphere prefab in Inspector
    public Spline spline; // Assign the spline component in Inspector


    public void Start()
    {
        

    }


    private void Update()
    {

        if (nodeSpheres.Length < spline.nodes.Count)
        {
            spline.RemoveNode(spline.nodes[spline.nodes.Count - 1]);
            return;
        }

        

           if(nodeSpheres.Length > spline.nodes.Count)
            {
                SplineNode newnode = new SplineNode(spline.nodes[spline.nodes.Count -1].Position, spline.nodes[spline.nodes.Count-1].Direction);
                spline.AddNode(newnode);
            return;
            }


        for (int i = 0; i < nodeSpheres.Length; i++)
        {
            spline.nodes[i].Position = nodeSpheres[i].transform.position;
        }
    }
}
