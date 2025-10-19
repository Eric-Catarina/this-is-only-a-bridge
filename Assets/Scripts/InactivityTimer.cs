using UnityEngine;

public class InactivityTimer : MonoBehaviour
{
    [Tooltip("Nome da cena específica para carregar. Se deixado em branco, carregará a próxima cena da lista do GameManager.")]
    [SerializeField] private string specificSceneName;
    public float inactiveTimeLimit = 7f; // tempo necessário sem movimento
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
                        Debug.LogError("GameManager.Instance está null!");
                    }
                    else
                    {
                        Debug.Log("Carregando próxima cena...");
                        GameManager.Instance.LoadNextScene();
                    }
                }
            }


        }

    private void LoadTargetScene()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance não foi encontrado. O gatilho não pode funcionar.");
            return;
        }
        GameManager.Instance.LoadSceneByName(specificSceneName != "" ? specificSceneName : null);

    }

}
