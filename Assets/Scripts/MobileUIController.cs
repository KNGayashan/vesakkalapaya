using UnityEngine;

public class MobileUIController : MonoBehaviour
{

    public float minWidthToShow = 600f; // Threshold width
    public GameObject targetObject; 
    void Update()
    {
        // Check if screen width is below threshold
        if (Screen.width < minWidthToShow)
        {
            if (targetObject.activeSelf) targetObject.SetActive(false);
        }
        else
        {
            if (!targetObject.activeSelf) targetObject.SetActive(true);
        }
    }
}