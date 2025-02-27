using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class list : MonoBehaviour
{

        public GameObject projectPrefab; // Prefab to instantiate
        public Transform contentParent;  // Parent where prefab will be instantiated
        public Cabling cablingScript;    // Reference to the Cabling script

        void Start()
        {
            SpawnProjects(); // Automatically run on Start
        }

        void SpawnProjects()
        {
            if (projectPrefab == null || contentParent == null || cablingScript == null)
            {
                Debug.LogError("Missing references! Assign Project Prefab, Content Parent, and Cabling Script.");
                return;
            }

            // Check how many projects are assigned in the Cabling script
            int projectCount = cablingScript.Projects.Project.Length;

            for (int i = 0; i < projectCount; i++)
            {
                string projectName = cablingScript.Projects.Project[i].ProjectName;

                // Instantiate the prefab inside Content Parent
                GameObject newProject = Instantiate(projectPrefab, contentParent);
                newProject.name = projectName;  // Assign unique name to GameObject

                // Find and update the Text inside the prefab
                TextMeshProUGUI tmpText = newProject.GetComponentInChildren<TextMeshProUGUI>();
                if (tmpText != null)
                {
                    tmpText.text = projectName;  // Update TMP Text
                }

                //

                Debug.Log("Spawned Project: " + projectName);
            }
        }
    }
