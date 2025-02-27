using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class List : MonoBehaviour
{
    public GameObject ScrollProject; // Project list UI
    public GameObject ScrollHarness; // Harness list UI
    public GameObject ProjectCard; // Prefab for projects and harnesses
    public Transform ProjectCardParent; // Parent for projects
    public Transform HarnessCardParent; // Parent for harnesses
    public Button BackButton; // Back button
    public Cabling cablingScript; // Reference to the Cabling script

    void Start()
    {
        SpawnProjects(); // Automatically run on Start
        BackButton.onClick.AddListener(SpawnProjects); // Assign the back button function
    }

    public void SpawnProjects()
    {
        ScrollProject.SetActive(true);
        ScrollHarness.SetActive(false);

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
                projectButton.onClick.AddListener(() => SpawnHarness(index));
            }

            Debug.Log("Spawned Project: " + projectName);
        }
    }

    public void SpawnHarness(int selectedProject)
    {
        ScrollProject.SetActive(false);
        ScrollHarness.SetActive(true);

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

        int harnessCount = cablingScript.Projects.Project[selectedProject].Harness.Length;

        for (int i = 0; i < harnessCount; i++)
        {
            string harnessName = cablingScript.Projects.Project[selectedProject].Harness[i].HarnessName;

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
        }
    }
}
