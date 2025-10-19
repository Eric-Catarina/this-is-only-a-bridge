using UnityEngine;
using UnityEngine.SceneManagement;

public class InactivityTimer : MonoBehaviour
{
    [Tooltip("Nome da cena espec�fica para carregar. Se deixado em branco, carregar� a pr�xima cena da lista do GameManager.")]
    [SerializeField] private string specificSceneName;
    public float inactiveTimeLimit = 7f; // tempo necess�rio sem movimento
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
                        Debug.LogError("GameManager.Instance est� null!");
                    }
                    else
                    {
                        Debug.Log("Carregando pr�xima cena...");
                        LoadNextScene();
                    
                }
                }
            }


        }

}
