using UnityEngine;
using UnityEngine.SceneManagement;

public class DetectTriggerOnColison : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            GameManager.Instance.RestartScene();
        }
    }
}
