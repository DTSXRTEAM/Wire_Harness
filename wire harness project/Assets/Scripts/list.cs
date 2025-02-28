using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class List : MonoBehaviour
{
    public int currentProject;
    public int currentHarness;
    public int currentCable;
    


    public GameObject ScrollProject; // Project list UI
    public GameObject ScrollHarness; // Harness list UI
    public GameObject ScrollCable;
    public GameObject cableProperties;
    public GameObject ProjectCard; // Prefab for projects and harnesses
    public GameObject CablePropertiescard;
    public Transform ProjectCardParent; // Parent for projects
    public Transform HarnessCardParent; // Parent for harnesses
    public Transform CableCardParent; // Parent for harnesses
    public Transform cablePropertiesParent;
    //public Button BackButton; // Back button
    public Cabling cablingScript; // Reference to the Cabling script

    void Start()
    {
        SpawnProjects(); // Automatically run on Start
                         // BackButton.onClick.AddListener(SpawnProjects); // Assign the back button function
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

        // Get the selected project
        Project selectedProject = cablingScript.Projects.Project[currentProject];

        // Ensure the Harness array is initialized
        if (selectedProject.Harness == null)
        {
            selectedProject.Harness = new Harness[0];
        }

        // Create a new harness
        Harness newHarness = new Harness();
        newHarness.HarnessName = "New Harness " + (selectedProject.Harness.Length + 1);
        newHarness.Cable = new Cable[0]; // Initialize cable array

        // Add new harness to the project
        List<Harness> harnessList = new List<Harness>(selectedProject.Harness);
        harnessList.Add(newHarness);
        selectedProject.Harness = harnessList.ToArray();

        // Update UI
        SpawnHarness();
    }

    public void AddNewCable()
    {
        if (cablingScript.Projects.Project.Length == 0) return;

        // Get current harness
        Harness selectedHarness = cablingScript.Projects.Project[currentProject].Harness[currentHarness];

        // Create a new cable
        Cable newCable = new Cable();
        newCable.CableName = "New Cable " + (selectedHarness.Cable.Length + 1);

        // Add new cable to the harness
        List<Cable> cableList = new List<Cable>(selectedHarness.Cable);
        cableList.Add(newCable);
        selectedHarness.Cable = cableList.ToArray();

        // Update UI
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
                Debug.Log("project"  + projectName);
                projectButton.onClick.AddListener(() => UpdateCurrentProject(currentindex));
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

        if (ProjectCard == null || HarnessCardParent == null || cablingScript == null)
        {
            Debug.LogError("Missing references! Assign Project Prefab, Content Parent, and Cabling Script.");
            return;
        }

        // Clear previous harnesses to avoid duplication
        foreach (Transform child in HarnessCardParent)
        {
            Destroy(child.gameObject);
        }

        int harnessCount = cablingScript.Projects.Project[currentProject].Harness.Length;

        for (int i = 0; i < harnessCount; i++)
        {
            string harnessName = cablingScript.Projects.Project[currentProject].Harness[i].HarnessName;

            // Instantiate harness UI
            GameObject newHarness = Instantiate(ProjectCard, HarnessCardParent);
            newHarness.name = harnessName;

            // Update harness name in the UI
            TextMeshProUGUI tmpText = newHarness.GetComponentInChildren<TextMeshProUGUI>();
            if (tmpText != null)
            {
                tmpText.text = harnessName;
            }

            Debug.Log("Spawned Harness: " + harnessName);
            Button HarnessButton = newHarness.GetComponent<Button>();
            if (HarnessButton != null)
            {
                int index = i; // Capture index for delegate
                HarnessButton.onClick.AddListener(() => UpdateCurrentHarness(index));
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

        if (ProjectCard == null || CableCardParent == null || cablingScript == null)
        {
            Debug.LogError("Missing references! Assign Project Prefab, Content Parent, and Cabling Script.");
            return;
        }

        // Clear previous harnesses to avoid duplication
        foreach (Transform child in CableCardParent)
        {
            Destroy(child.gameObject);
        }

        int cableCount = cablingScript.Projects.Project[currentProject].Harness[currentHarness].Cable.Length;

        for (int i = 0; i < cableCount; i++)
        {
            string cableName = cablingScript.Projects.Project[currentProject].Harness[currentHarness].Cable[i].CableName; ;

            // Instantiate harness UI
            GameObject newCable = Instantiate(ProjectCard, CableCardParent);
            newCable.name = cableName;

            // Update harness name in the UI
            TextMeshProUGUI tmpText = newCable.GetComponentInChildren<TextMeshProUGUI>();
            if (tmpText != null)
            {
                tmpText.text = cableName;
            }

            Debug.Log("Spawned Harness: " + cableName);

            Button CableButton = newCable.GetComponent<Button>();
            if (CableButton != null)
            {
                int index = i; // Capture index for delegate
                CableButton.onClick.AddListener(() => UpdateCurrentCable(index));
            }

        }
    }

      
    


    private void UpdateCurrentCable(int x)
    {
        currentCable = x;
        SpawnCablProperties();
    }

    public void SpawnCablProperties()
    {
        ScrollProject.SetActive(false);
        ScrollHarness.SetActive(false);
        ScrollCable.SetActive(false);
        cableProperties.SetActive(true);

        if (CablePropertiescard  == null || cablePropertiesParent == null || cablingScript == null)
        {
            Debug.LogError("Missing references! Assign Project Prefab, Content Parent, and Cabling Script.");
            return;
        }

        // Clear previous harnesses to avoid duplication
        foreach (Transform child in cablePropertiesParent)
        {
            Destroy(child.gameObject);
        }

        int cablePropertiesCount = cablingScript.Projects.Project[currentProject].Harness[currentHarness].Cable[currentCable].Node.Length;

        for (int i = 0; i < cablePropertiesCount; i++)
        {
            string NodeName = cablingScript.Projects.Project[currentProject].Harness[currentHarness].Cable[currentCable].Node[i].NodeName;

            // Instantiate cable properties UI
            GameObject newCableProperties = Instantiate(CablePropertiescard, cablePropertiesParent);
            newCableProperties.name = NodeName;

            // Get the TextMeshProUGUI component from the instantiated object
            TextMeshProUGUI tmpText = newCableProperties.GetComponentInChildren<TextMeshProUGUI>();
            if (tmpText != null)
            {
                tmpText.text = NodeName;
            }

            Debug.Log("Spawned Cable Property: " + NodeName);


        }

    }
}