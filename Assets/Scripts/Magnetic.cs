using UnityEngine;

using UnityEngine;
using System.Collections.Generic;

public class MagnetEffect : MonoBehaviour
{
    public Transform magnetPoint; // Arraste o objeto ímã para esta variável no Inspetor
    public float attractForce = 10f;
    public float attractRadius = 5f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (magnetPoint == null)
            return;

        float distance = Vector3.Distance(transform.position, magnetPoint.position);
        
        if (distance < attractRadius)
        {
            Vector3 direction = (magnetPoint.position - transform.position).normalized;
            rb.AddForce(direction * attractForce, ForceMode.Force);
        }
    }
}