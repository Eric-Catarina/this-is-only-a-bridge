using UnityEngine;

public class WheelController : MonoBehaviour
{
    [HideInInspector] public WheelCollider WheelCollider;
    public Transform[] wheelModels;
    public bool steerable = false;
    public bool motorized = false;

    private Vector3 wheelPosition;
    Vector3 positionOffset = new Vector3(0, 0.05f, 0);
    private Quaternion wheelRotation;

    void Awake()
    {
        WheelCollider = GetComponent<WheelCollider>();
    }

    void Update()
    {
        if (WheelCollider == null) return;
        
        WheelCollider.GetWorldPose(out wheelPosition, out wheelRotation);

        for (int i = 0; i < wheelModels.Length; i++)
        {
            if (wheelModels[i] != null)
            {
                wheelModels[i].position = wheelPosition - positionOffset;
                wheelModels[i].rotation = wheelRotation;
            }
        }
    }
}