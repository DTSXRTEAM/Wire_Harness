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
    public GameObject ProjectCard; // Prefab for projects and harnesses
    public Transform ProjectCardParent; // Parent for projects
    public Transform HarnessCardParent; // Parent for harnesses
    public Transform CableCardParent; // Parent for harnesses
    //public Button BackButton; // Back button
    public Cabling cablingScript; // Reference to the Cabling script

    void Start()
    {
        SpawnProjects(); // Automatically run on Start
       // BackButton.onClick.AddListener(SpawnProjects); // Assign the back button function
    }
    
    public void SpawnProjects()
    {
        ScrollProject.SetActive(true);
        ScrollHarness.SetActive(false);
        ScrollCable.SetActive(false);

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
                int index = i; // Capture index for delegate
                projectButton.onClick.AddListener(() => SpawnHarness());
            }

            else
            {
                projectButton.onClick.AddListener(() => SpawnProjects());
            }

            Debug.Log("Spawned Project: " + projectName);
        }
    }

    private void UpdateCurrentProject(int x)
    {
        currentProject = x;
        SpawnHarness();
    }
    public void SpawnHarness()
    {
 
        ScrollProject.SetActive(false);
        ScrollHarness.SetActive(true);
        ScrollCable.SetActive(false);

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
                HarnessButton.onClick.AddListener(() => SpawnCable());
            }

            else
            {
                HarnessButton.onClick.AddListener(() => SpawnHarness());
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
        }
    }
}
