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
            cablingScript.Projects.Project = new Project[0];
        }

        highestProjectIndex++; // Always increment

        Project newProject = new Project
        {
            ProjectName = "New Project " + highestProjectIndex,
            Harness = new Harness[0]
        };

        List<Project> projectList = new List<Project>(cablingScript.Projects.Project);
        projectList.Add(newProject);
        cablingScript.Projects.Project = projectList.ToArray();

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

        highestHarnessIndex++;

        Harness newHarness = new Harness
        {
            HarnessName = "New Harness " + highestHarnessIndex,
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

        highestCableIndex++;

        Cable newCable = new Cable
        {
            CableName = "New Cable " + highestCableIndex
        };

        List<Cable> cableList = new List<Cable>(selectedHarness.Cable);
        cableList.Add(newCable);
        selectedHarness.Cable = cableList.ToArray();

        SpawnCable();
    }


    public void DeleteLastProject(int X)
    {
        if (cablingScript.Projects.Project == null || cablingScript.Projects.Project.Length == 0) return;

        List<Project> projectList = new List<Project>(cablingScript.Projects.Project);
        projectList.RemoveAt(X);
        cablingScript.Projects.Project = projectList.ToArray();

        highestProjectIndex = projectList.Count > 0 ? projectList.Max(p => int.Parse(p.ProjectName.Split(' ')[2])) : 0;

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

        foreach (Transform child in ProjectCardParent)
        {
            Destroy(child.gameObject);
        }

        int projectCount = cablingScript.Projects.Project.Length;
        highestProjectIndex = projectCount > 0 ? cablingScript.Projects.Project.Max(p => int.Parse(p.ProjectName.Split(' ')[2])) : 0;

        for (int i = 0; i < projectCount; i++)
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
