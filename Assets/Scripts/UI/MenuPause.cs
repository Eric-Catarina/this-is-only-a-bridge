using UnityEngine;

public class MenuPause : MonoBehaviour
{
    public GameObject menuObject;

    void Update()
    {
        ControlCanvas();
    }

    void ControlCanvas()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuObject.activeSelf)
            {
                Time.timeScale = 1f;
                menuObject.SetActive(false);
            }
            else
            {
                Time.timeScale = 0f;
                menuObject.SetActive(true);
            }
        }
    }
}
