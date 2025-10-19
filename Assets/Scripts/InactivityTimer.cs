using UnityEngine;
using UnityEngine.SceneManagement;

public class InactivityTimer : MonoBehaviour
{
    [Tooltip("Nome da cena específica para carregar. Se deixado em branco, carregará a próxima cena da lista do GameManager.")]
    [SerializeField] private string specificSceneName;
    public float inactiveTimeLimit = 7f; // tempo necessário sem movimento
    private float inactivityTimer = 0f;
    
    public void LoadNextScene()
    {
        if (LevelDeathManager.Instance != null)
            LevelDeathManager.Instance.MarkLevelPassed();

        int currentIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentIndex +1);
    }

    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
       
    }
    void Update()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Debug.Log($"Input: H={horizontal}, V={vertical}");

            if (Mathf.Abs(horizontal) > 0.01f || Mathf.Abs(vertical) > 0.01f)
            {
                inactivityTimer = 0f;
            }
            else
            {
                inactivityTimer += Time.deltaTime;
                Debug.Log($"Inatividade: {inactivityTimer:F2}");

                if (inactivityTimer >= inactiveTimeLimit)
                {
                    if (GameManager.Instance == null)
                    {
                        Debug.LogError("GameManager.Instance está null!");
                    }
                    else
                    {
                        Debug.Log("Carregando próxima cena...");
                        LoadNextScene();
                    
                }
                }
            }


        }

}
