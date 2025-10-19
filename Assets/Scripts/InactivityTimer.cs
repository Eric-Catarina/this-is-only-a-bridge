using UnityEngine;

public class InactivityTimer : MonoBehaviour
{
    public float inactiveTimeLimit = 7f; // tempo necessário sem movimento
    private float inactivityTimer = 0f;

    void Update()
    {
        // Captura os eixos padrão
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Se houver qualquer movimento, zera o cronômetro
        if (Mathf.Abs(horizontal) > 0.01f || Mathf.Abs(vertical) > 0.01f)
        {
            inactivityTimer = 0f;
        }
        else
        {
            // Incrementa o tempo de inatividade
            inactivityTimer += Time.deltaTime;

            // (Opcional) Debug do tempo atual
            Debug.Log($"Inatividade: {inactivityTimer:F2} segundos");

            if (inactivityTimer >= inactiveTimeLimit)
            {
                GameManager.Instance.LoadNextScene();
                // Jogador ficou inativo por 7 segundos
                //Debug.Log("Jogador inativo por 7 segundos!");
                // Aqui você pode chamar uma função, ativar algo, etc.
            }
        }
    }
}
