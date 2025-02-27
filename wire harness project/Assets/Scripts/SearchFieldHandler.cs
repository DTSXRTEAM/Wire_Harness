using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SearchFieldHandler : MonoBehaviour
{
    public TMP_InputField searchInput;
    public Image searchIcon;

    void Start()
    {
        searchInput.onValueChanged.AddListener(OnSearchTextChanged);
    }

    void OnSearchTextChanged(string text)
    {
        searchIcon.enabled = string.IsNullOrEmpty(text); // Disable image when typing
    }
}
