using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButton : MonoBehaviour
{
    public int level;
    private void Start()
    {
        
    }
    public void SetText(string text)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = text;
    }
}
