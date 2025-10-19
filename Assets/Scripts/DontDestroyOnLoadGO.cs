using UnityEngine;

public class DontDestroyOnLoadGO : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
