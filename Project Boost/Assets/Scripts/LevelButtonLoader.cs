using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButtonLoader : MonoBehaviour
{
    [SerializeField] Button LevelButtonLeft;
    [SerializeField] Button LevelButtonRight;
    [SerializeField] SceneLoader sceneLoader;

    // Start is called before the first frame update
    void Start()
    {
        int levelCount = SceneManager.sceneCountInBuildSettings - 3;
        bool isLeft = true;
        float by = 50f + 19.75f;
        int level = 0;
        RectTransform parentRectTransform = transform.parent.GetComponent<RectTransform>();
        for (var i = 0; i < levelCount; i++)
        {
            CreateButton(ref isLeft, ref by, ref level, parentRectTransform);
        }
    }

    private void CreateButton(ref bool isLeft, ref float by, ref int level, RectTransform parentRectTransform)
    {
        Button newLevelButton = Instantiate(isLeft ? LevelButtonLeft : LevelButtonRight, parentRectTransform) as Button;
        var rectTransform = newLevelButton.GetComponent<RectTransform>();
        rectTransform.position = new Vector3(rectTransform.position.x, by, 0f);
        void loadMyScene(Button self)
        {
            int loadLevel = self.GetComponent<LevelButton>().level;
            sceneLoader.LoadSpecificScene(loadLevel);
        }
        LevelButton buttonComponent = newLevelButton.GetComponent<LevelButton>();
        buttonComponent.level = level + 1;
        buttonComponent.SetText("Level " + (level + 1).ToString());
        newLevelButton.onClick.AddListener(() => loadMyScene(newLevelButton));
        level++;
        isLeft = !isLeft;
        if (isLeft)
        {
            by -= 100f;
        }
    }
}
