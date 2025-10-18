using UnityEngine;

public class LevadicaScript : MonoBehaviour
{
    public Vector3 rotationAmount = new Vector3(30, 0, 0);

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transform.Rotate(rotationAmount);
        }
    }
}