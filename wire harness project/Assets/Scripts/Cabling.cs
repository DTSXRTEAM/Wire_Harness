using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Projects
{
    public Project[] Project;
}

[System.Serializable]
public class Project
{
    public string ProjectName;
    public Harness[] Harness;
}

[System.Serializable]
public class Harness
{
    public string HarnessName;
    public Cable[] Cable;
}

[System.Serializable]
public class Cable
{
    public string CableName;
    public Node[] Node;
}

[System.Serializable]
public class Node
{
    public string NodeName;
}
public class Cabling : MonoBehaviour
{
    public Projects Projects = new Projects();

    // Start is called before the first frame update
    void Start()
    {
        /*int totalNodes = 0;

        for (int i = 1; i < Projects.Project.Length; i++)
        {
            for (int j = 0; j < Projects.Project[i].Harness.Length; j++)
            {
                if (Projects.Project[i].Harness[j].Cable != null)
                {
                    totalNodes += Projects.Project[i].Harness[j].Cable.Length;
                }

            }
            //Debug.Log(Projects.Project[i].Harness[1].Cable[1].Node);
        }
        Debug.Log("Total number of nodes: " + totalNodes);*/
    }

    // Update is called once per frame
    void Update()
    {

    }
}
