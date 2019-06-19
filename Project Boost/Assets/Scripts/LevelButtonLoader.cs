using System.Collections;
using System.Collections.Generic;
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
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        bool isLeft = false;
        float by = 50f;
        int level = 0;
        for(var i = 0; i < sceneCount; i++)
        {
            print(sceneCount);
            Button newLevelButton = Instantiate(isLeft ? LevelButtonLeft : LevelButtonRight, transform.parent.GetComponent<RectTransform>()) as Button;
            var rectTransform = newLevelButton.GetComponent<RectTransform>();
            rectTransform.position = new Vector3(rectTransform.position.x, by, 0f);
            void loadMyScene()
            {
                sceneLoader.LoadSpecificScene(level + 1);
            }
            newLevelButton.onClick.AddListener(loadMyScene);
            level++;
            isLeft = !isLeft;
            if (!isLeft)
            {
                by -= 100f;
            }
        }
    }

}
