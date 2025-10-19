using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UIElements;

public class LevadicaScript : MonoBehaviour
{
    [Header("Rotation Controller")]
    [SerializeField] private GameObject targetObject1;
    [SerializeField] private GameObject targetObject2;

    [Tooltip("Rotação alvo em ângulos de Euler (graus)")]
    public Vector3 targetEulerAngles1;
    public Vector3 targetEulerAngles2;

    public float rotationDuration = 1.0f;

    private bool isRotating = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isRotating)
        {
            StartCoroutine(RotateObjects());
        }
    }

    IEnumerator RotateObjects()
    {
        isRotating = true;

        Quaternion initialRotation1 = targetObject1.transform.rotation;
        Quaternion targetRotation1 = Quaternion.Euler(targetEulerAngles1);

        Quaternion initialRotation2 = targetObject2.transform.rotation;
        Quaternion targetRotation2 = Quaternion.Euler(targetEulerAngles2);

        float elapsedTime = 0f;

        while (elapsedTime < rotationDuration)
        {
            float t = elapsedTime / rotationDuration;

            targetObject1.transform.rotation = Quaternion.Slerp(initialRotation1, targetRotation1, t);
            targetObject2.transform.rotation = Quaternion.Slerp(initialRotation2, targetRotation2, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Garantir rotação final exata
        targetObject1.transform.rotation = targetRotation1;
        targetObject2.transform.rotation = targetRotation2;

        isRotating = false;
    }

}