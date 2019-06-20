using UnityEngine;

public class ProgressComponent : MonoBehaviour
{
    public void ResetProgress()
    {
        SaveProgress.DestroyData();
    }
}
