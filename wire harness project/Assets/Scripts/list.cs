using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class List : MonoBehaviour
{
    public int currentProject;
    public int currentHarness;
    public int currentCable;

    public GameObject ScrollProject;
    public GameObject ScrollHarness;
    public GameObject ScrollCable;
    public GameObject cableProperties;
    public GameObject ProjectCard;
    public GameObject CablePropertiescard;
    public Transform ProjectCardParent;
    public Transform HarnessCardParent;
    public Transform CableCardParent;
    public Transform cablePropertiesParent;

    public Cabling cablingScript;
   


    void Start()
    {
        SpawnProjects();
    }

    public void AddNewProject()
    {
        // Ensure the Projects array is initialized
        if (cablingScript.Projects.Project == null)
        {
            cablingScript.Projects.Project = new Project[0];
        }

        // Create a new project with an empty harness list
        Project newProject = new Project();
        newProject.ProjectName = "New Project " + (cablingScript.Projects.Project.Length + 1);
        newProject.Harness = new Harness[0]; // Initialize the Harness array

        // Add new project to the array
        List<Project> projectList = new List<Project>(cablingScript.Projects.Project);
        projectList.Add(newProject);
        cablingScript.Projects.Project = projectList.ToArray();

        // Update UI
        SpawnProjects();
    }

    public void AddNewHarness()
    {
        if (cablingScript.Projects.Project == null || cablingScript.Projects.Project.Length == 0) return;

        Project selectedProject = cablingScript.Projects.Project[currentProject];

        if (selectedProject.Harness == null)
        {
            selectedProject.Harness = new Harness[0];
        }

        Harness newHarness = new Harness
        {
            HarnessName = "New Harness " + (selectedProject.Harness.Length + 1),
            Cable = new Cable[0]
        };

        List<Harness> harnessList = new List<Harness>(selectedProject.Harness);
        harnessList.Add(newHarness);
        selectedProject.Harness = harnessList.ToArray();

        SpawnHarness();
    }

    public void AddNewCable()
    {
        if (cablingScript.Projects.Project.Length == 0) return;

        Harness selectedHarness = cablingScript.Projects.Project[currentProject].Harness[currentHarness];

        Cable newCable = new Cable
        {
            CableName = "New Cable " + (selectedHarness.Cable.Length + 1)
        };

        List<Cable> cableList = new List<Cable>(selectedHarness.Cable);
        cableList.Add(newCable);
        selectedHarness.Cable = cableList.ToArray();

        SpawnCable();
    }

    public void DeleteLastProject(int X)
    {
        Debug.Log("delete project " + X);

        if (cablingScript.Projects.Project == null || cablingScript.Projects.Project.Length == 0)
        {
            Debug.LogWarning("No projects to delete.");
            return;
        }

        // Convert array to a list for easy removal
        List<Project> projectList = new List<Project>(cablingScript.Projects.Project);

        // Remove the last project
        projectList.RemoveAt(X);

        // Convert back to an array
        cablingScript.Projects.Project = projectList.ToArray();

        // Update UI
        SpawnProjects();
    }
    public void DeleteLastHarness(int X)
    {
        if (cablingScript.Projects.Project[currentProject].Harness.Length == 0) return;

        List<Harness> harnessList = new List<Harness>(cablingScript.Projects.Project[currentProject].Harness);
        harnessList.RemoveAt(X);
        cablingScript.Projects.Project[currentProject].Harness = harnessList.ToArray();

        SpawnHarness();
    }

    public void DeleteLastCable(int X)
    {
        if (cablingScript.Projects.Project[currentProject].Harness[currentHarness].Cable.Length == 0) return;

        List<Cable> cableList = new List<Cable>(cablingScript.Projects.Project[currentProject].Harness[currentHarness].Cable);
        cableList.RemoveAt(X);
        cablingScript.Projects.Project[currentProject].Harness[currentHarness].Cable = cableList.ToArray();

        SpawnCable();
    }

    public void SpawnProjects()
    {
        ScrollProject.SetActive(true);
        ScrollHarness.SetActive(false);
        ScrollCable.SetActive(false);
        cableProperties.SetActive(false);

        if (ProjectCard == null || ProjectCardParent == null || cablingScript == null)
        {
            Debug.LogError("Missing references! Assign Project Prefab, Content Parent, and Cabling Script.");
            return;
        }

        // Clear previous projects to avoid duplicates
        foreach (Transform child in ProjectCardParent)
        {
            Destroy(child.gameObject);
        }

        int projectCount = cablingScript.Projects.Project.Length;

        for (int i = 0; i < projectCount; i++)
        {
            string projectName = cablingScript.Projects.Project[i].ProjectName;

            // Instantiate the prefab
            GameObject newProject = Instantiate(ProjectCard, ProjectCardParent);
            newProject.name = projectName; // Assign unique name

            // Find and update the text
            TextMeshProUGUI tmpText = newProject.GetComponentInChildren<TextMeshProUGUI>();
            if (tmpText != null)
            {
                tmpText.text = projectName;
            }

            // Assign button click dynamically
            Button projectButton = newProject.GetComponent<Button>();


            if (projectButton != null)
            {
                int currentindex = i;
                Debug.Log("project" + projectName);
                projectButton.onClick.AddListener(() => UpdateCurrentProject(currentindex));
            }

            Transform Deletbutton = projectButton.gameObject.transform.Find("Button");

            //GameObject Deletbutton = GameObject.Find("Button");

            Button deleteButton = Deletbutton.gameObject.GetComponent<Button>();

            if (deleteButton != null)
            {
                int currentindex = i;
                //Debug.Log("project" + projectName);
                deleteButton.onClick.AddListener(() => DeleteLastProject(currentindex));
            }

        }
    }



    public void UpdateCurrentProject(int x)
    {
        currentProject = x;
        SpawnHarness();
    }

    public void SpawnHarness()
    {
        ScrollProject.SetActive(false);
        ScrollHarness.SetActive(true);
        ScrollCable.SetActive(false);
        cableProperties.SetActive(false);

        foreach (Transform child in HarnessCardParent)
        {
            Destroy(child.gameObject);
        }

        int harnessCount = cablingScript.Projects.Project[currentProject].Harness.Length;

        for (int i = 0; i < harnessCount; i++)
        {
            string harnessName = cablingScript.Projects.Project[currentProject].Harness[i].HarnessName;
            GameObject newHarness = Instantiate(ProjectCard, HarnessCardParent);
            newHarness.name = harnessName;

            TextMeshProUGUI tmpText = newHarness.GetComponentInChildren<TextMeshProUGUI>();
            if (tmpText != null) tmpText.text = harnessName;

            Button harnessButton = newHarness.GetComponent<Button>();
            if (harnessButton != null)
            {
                int index = i;
                harnessButton.onClick.AddListener(() => UpdateCurrentHarness(index));
            }

            Button deleteButton = newHarness.transform.Find("Button")?.GetComponent<Button>();
            if (deleteButton != null)
            {
                int index = i;
                deleteButton.onClick.AddListener(() => DeleteLastHarness(index));
            }
        }
    }

    private void UpdateCurrentHarness(int x)
    {
        currentHarness = x;
        SpawnCable();
    }

    public void SpawnCable()
    {
        ScrollProject.SetActive(false);
        ScrollHarness.SetActive(false);
        ScrollCable.SetActive(true);
        cableProperties.SetActive(false);

        foreach (Transform child in CableCardParent)
        {
            Destroy(child.gameObject);
        }

        int cableCount = cablingScript.Projects.Project[currentProject].Harness[currentHarness].Cable.Length;

        for (int i = 0; i < cableCount; i++)
        {
            string cableName = cablingScript.Projects.Project[currentProject].Harness[currentHarness].Cable[i].CableName;
            GameObject newCable = Instantiate(ProjectCard, CableCardParent);
            newCable.name = cableName;

            TextMeshProUGUI tmpText = newCable.GetComponentInChildren<TextMeshProUGUI>();
            if (tmpText != null) tmpText.text = cableName;

            Button cableButton = newCable.GetComponent<Button>();
            if (cableButton != null)
            {
                int index = i;
                cableButton.onClick.AddListener(() => UpdateCurrentCable(index));
            }

            Button deleteButton = newCable.transform.Find("Button")?.GetComponent<Button>();
            if (deleteButton != null)
            {
                int index = i;
                deleteButton.onClick.AddListener(() => DeleteLastCable(index));
            }
        }
    }

    private void UpdateCurrentCable(int x)
    {
        currentCable = x;
        SpawnCable();
    }
}
