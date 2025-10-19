using UnityEngine;

public class InactivityTimer : MonoBehaviour
{
    [Tooltip("Nome da cena espec�fica para carregar. Se deixado em branco, carregar� a pr�xima cena da lista do GameManager.")]
    [SerializeField] private string specificSceneName;
    public float inactiveTimeLimit = 7f; // tempo necess�rio sem movimento
    private float inactivityTimer = 0f;
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
                        GameManager.Instance.LoadNextScene();
                    }
                }
            }


        }

    private void LoadTargetScene()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance n�o foi encontrado. O gatilho n�o pode funcionar.");
            return;
        }
        GameManager.Instance.LoadSceneByName(specificSceneName != "" ? specificSceneName : null);

    }

}
