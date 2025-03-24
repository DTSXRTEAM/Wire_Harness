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
    public GameObject cablePrefab;

    public Cabling cablingScript;
    public int highestProjectIndex = 0;
    public int highestHarnessIndex = 0;
    public int highestCableIndex = 0;


    void Start()
    {
        SpawnProjects();
    }

    public void AddNewProject()
    {
        if (cablingScript.Projects.Project == null)
        {
            cablingScript.Projects.Project = new List<Project>();
        }

        highestProjectIndex++; // Always increment

        Project newProject = new Project
        {
            ProjectName = "New Project " + highestProjectIndex,
            Harness = new List<Harness>()
        };

        cablingScript.Projects.Project.Add(newProject);

        SpawnProjects();
    }


    public void AddNewHarness()
    {
        if (cablingScript.Projects.Project == null || cablingScript.Projects.Project.Count == 0) return;

        Project selectedProject = cablingScript.Projects.Project[currentProject];

        highestHarnessIndex++;

        Harness newHarness = new Harness
        {
            HarnessName = "New Harness " + highestHarnessIndex,
            Cable = new List<Cable>()
        };

        selectedProject.Harness.Add(newHarness);
        SpawnHarness();
    }


    public void AddNewCable()
    {
        if (cablingScript.Projects.Project.Count == 0) return;

        Harness selectedHarness = cablingScript.Projects.Project[currentProject].Harness[currentHarness];

        highestCableIndex++;

        Cable newCable = new Cable
        {
            CableName = "New Cable " + highestCableIndex
        };

        selectedHarness.Cable.Add(newCable);
        SpawnCablePrefab();
        SpawnCable();
    }


    public void DeleteLastProject(int index)
    {
        if (cablingScript.Projects.Project.Count == 0) return;
        cablingScript.Projects.Project.RemoveAt(index);
        highestProjectIndex = cablingScript.Projects.Project.Count;
        SpawnProjects();
    }

    public void DeleteLastHarness(int index)
    {
        if (cablingScript.Projects.Project[currentProject].Harness.Count == 0) return;
        cablingScript.Projects.Project[currentProject].Harness.RemoveAt(index);
        SpawnHarness();
    }

    public void DeleteLastCable(int index)
    {
        if (cablingScript.Projects.Project[currentProject].Harness[currentHarness].Cable.Count == 0) return;
        cablingScript.Projects.Project[currentProject].Harness[currentHarness].Cable.RemoveAt(index);
        SpawnCable();
    }

    public void SpawnProjects()
    {
        ScrollProject.SetActive(true);
        ScrollHarness.SetActive(false);
        ScrollCable.SetActive(false);
        cableProperties.SetActive(false);

        foreach (Transform child in ProjectCardParent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < cablingScript.Projects.Project.Count; i++)
        {
            string projectName = cablingScript.Projects.Project[i].ProjectName;
            GameObject newProject = Instantiate(ProjectCard, ProjectCardParent);
            newProject.name = projectName;

            TextMeshProUGUI tmpText = newProject.GetComponentInChildren<TextMeshProUGUI>();
            if (tmpText != null) tmpText.text = projectName;

            Button projectButton = newProject.GetComponent<Button>();
            if (projectButton != null)
            {
                int index = i;
                projectButton.onClick.AddListener(() => UpdateCurrentProject(index));
            }

            Button deleteButton = newProject.transform.Find("Button")?.GetComponent<Button>();
            if (deleteButton != null)
            {
                int index = i;
                deleteButton.onClick.AddListener(() => DeleteLastProject(index));
            }
        }
    }

    public void UpdateCurrentProject(int index)
    {
        currentProject = index;
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

        for (int i = 0; i < cablingScript.Projects.Project[currentProject].Harness.Count; i++)
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

    private void UpdateCurrentHarness(int index)
    {
        currentHarness = index;
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

        for (int i = 0; i < cablingScript.Projects.Project[currentProject].Harness[currentHarness].Cable.Count; i++)
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
    public void SpawnCablePrefab()
    {
        if (cablePrefab != null)
        {
            Instantiate(cablePrefab);
        }
    }
    private void UpdateCurrentCable(int index)
    {
        currentCable = index;
        SpawnNode();
    }

    public void SpawnNode()
    {

        ScrollProject.SetActive(false);
        ScrollHarness.SetActive(false);
        ScrollCable.SetActive(false);
        cableProperties.SetActive(true);
    }
}
