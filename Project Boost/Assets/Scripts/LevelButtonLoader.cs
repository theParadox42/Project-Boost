using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButtonLoader : MonoBehaviour
{
    [SerializeField] Button LevelButtonLeft;
    [SerializeField] Button LevelButtonRight;
    [SerializeField] SceneLoader sceneLoader;
    [SerializeField] float distanceBetweenButtons = 200f;

    // Start is called before the first frame update
    void Start()
    {
        int levelsFinished = SaveProgress.RetrieveData().levelsCompleted;
        int levelCount = SceneManager.sceneCountInBuildSettings - 3;
        bool isLeft = true;
        float by = -distanceBetweenButtons / 2;
        int level = 0;
        RectTransform parentRectTransform = transform.parent.GetComponent<RectTransform>();
        for (var i = 0; i < levelCount; i++)
        {
            CreateButton(ref isLeft, ref by, ref level, parentRectTransform, levelsFinished, i);
        }
    }

    private void CreateButton(ref bool isLeft, ref float by, ref int level, RectTransform parentRectTransform, int levelsCompleted, int i)
    {
        Button newLevelButton = Instantiate(isLeft ? LevelButtonLeft : LevelButtonRight, parentRectTransform) as Button;
        var rectTransform = newLevelButton.GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector3(10f, by, 0f);
        //rectTransform.SetPositionAndRotation(new Vector3(rectTransform.position.x, by, 0f), new Quaternion());
        void loadMyScene(Button self)
        {
            int loadLevel = self.GetComponent<LevelButton>().level;
            sceneLoader.LoadSpecificScene(loadLevel);
        }
        LevelButton buttonComponent = newLevelButton.GetComponent<LevelButton>();
        buttonComponent.level = level + 1;
        buttonComponent.SetText("Level " + (level + 1).ToString());
        if(i > levelsCompleted)
        {
            newLevelButton.GetComponent<Button>().interactable = false;
            
        }

        newLevelButton.onClick.AddListener(() => loadMyScene(newLevelButton));
        level++;
        isLeft = !isLeft;

        if (isLeft)
        {
            by -= distanceBetweenButtons;
        }
    }
}
