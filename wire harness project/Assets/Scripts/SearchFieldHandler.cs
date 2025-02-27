using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SearchFieldHandler : MonoBehaviour
{
    [System.Serializable]
    public class SearchPanel
    {
        public TMP_InputField searchInputField; // Input field
        public GameObject searchIcon;           // Search icon
    }

    public SearchPanel[] searchPanels; // Array for multiple panels

    void Start()
    {
        foreach (var panel in searchPanels)
        {
            if (panel.searchInputField != null && panel.searchIcon != null)
            {
                panel.searchInputField.onValueChanged.AddListener((text) => OnSearchTextChanged(panel, text));
            }
        }
    }

    void OnSearchTextChanged(SearchPanel panel, string text)
    {
        panel.searchIcon.SetActive(string.IsNullOrEmpty(text));
    }
}
