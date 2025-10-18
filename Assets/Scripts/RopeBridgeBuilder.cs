using UnityEngine;
using System;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class RopeBridgeBuilder : MonoBehaviour
{
    [Header("Bridge Structure")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private int plankCount = 20;

    [Header("Plank Properties")]
    [SerializeField] private GameObject plankPrefab;
    [SerializeField, Range(0f, 1f)] private float missingPlankChance = 0.1f;

    [Header("Physics Properties")]
    [Tooltip("How much the bridge sags in the middle.")]
    [SerializeField] private float sag = 0f;
    [SerializeField] private float plankMass = 0f;
    [SerializeField] private float jointSpring = 0f;
    [SerializeField] private float jointDamper = 0f;

    [Header("Rope Visuals")]
    [SerializeField] private Material ropeMaterial;
    [SerializeField] private float ropeWidth = 0.1f;

    public event Action OnBridgeGenerated;

    private const string BRIDGE_ROOT_NAME = "GeneratedRopeBridge";
    private const string LEFT_ROPE_ANCHOR_NAME = "RopeAnchor_Left";
    private const string RIGHT_ROPE_ANCHOR_NAME = "RopeAnchor_Right";

    [ContextMenu("Generate Bridge")]
    public void Generate()
    {
        ClearBridge();
        if (!AreSettingsValid()) return;

        var bridgeRoot = new GameObject(BRIDGE_ROOT_NAME);
        bridgeRoot.transform.SetParent(transform, false);

        var leftRopePoints = new List<Transform> { startPoint };
        var rightRopePoints = new List<Transform> { startPoint };

        Vector3 bridgeDirection = endPoint.position - startPoint.position;
        Vector3 step = bridgeDirection / (plankCount + 1);

        Rigidbody previousRigidbody = CreateAnchor(startPoint, bridgeRoot.transform, "StartAnchor");

        for (int i = 0; i < plankCount; i++)
        {
            float progress = (float)(i + 1) / (plankCount + 1);
            Vector3 position = startPoint.position + (step * (i + 1));
            
            float curve = (1 - Mathf.Pow(2 * progress - 1, 2));
            position.y -= curve * sag;

            Quaternion rotation = Quaternion.LookRotation(step.normalized);

            if (UnityEngine.Random.value < missingPlankChance && i > 0 && i < plankCount -1)
            {
                var dummyPoint = new GameObject($"DummyPoint_{i}").transform;
                dummyPoint.position = position;
                dummyPoint.SetParent(bridgeRoot.transform);
                leftRopePoints.Add(dummyPoint);
                rightRopePoints.Add(dummyPoint);
                continue;
            }

            GameObject plankInstance = Instantiate(plankPrefab, position, rotation, bridgeRoot.transform);
            plankInstance.name = $"Plank_{i}";
            
            Rigidbody currentRigidbody = plankInstance.GetComponent<Rigidbody>();
            if (currentRigidbody == null)
            {
                currentRigidbody = plankInstance.AddComponent<Rigidbody>();
            }
            currentRigidbody.mass = plankMass;
            
            ConnectToPrevious(previousRigidbody, currentRigidbody);
            previousRigidbody = currentRigidbody;

            Transform leftAnchor = plankInstance.transform.Find(LEFT_ROPE_ANCHOR_NAME);
            Transform rightAnchor = plankInstance.transform.Find(RIGHT_ROPE_ANCHOR_NAME);
            
            if(leftAnchor == null || rightAnchor == null)
            {
                 Debug.LogError($"O Prefab da prancha precisa ter GameObjects filhos chamados '{LEFT_ROPE_ANCHOR_NAME}' e '{RIGHT_ROPE_ANCHOR_NAME}'. Abortando.", plankPrefab);
                 ClearBridge();
                 return;
            }

            leftRopePoints.Add(leftAnchor);
            rightRopePoints.Add(rightAnchor);
        }

        Rigidbody endAnchor = CreateAnchor(endPoint, bridgeRoot.transform, "EndAnchor");
        ConnectToPrevious(previousRigidbody, endAnchor);

        leftRopePoints.Add(endPoint);
        rightRopePoints.Add(endPoint);

        CreateRopeVisualizer("LeftRope", bridgeRoot.transform, leftRopePoints);
        CreateRopeVisualizer("RightRope", bridgeRoot.transform, rightRopePoints);

        OnBridgeGenerated?.Invoke();
    }
    
    [ContextMenu("Clear Existing Bridge")]
    public void ClearBridge()
    {
        Transform existingBridge = transform.Find(BRIDGE_ROOT_NAME);
        if (existingBridge != null)
        {
            if (Application.isPlaying)
                Destroy(existingBridge.gameObject);
            else
                DestroyImmediate(existingBridge.gameObject);
        }
    }

    private void ConnectToPrevious(Rigidbody previous, Rigidbody current)
    {
        HingeJoint joint = current.gameObject.AddComponent<HingeJoint>();
        joint.connectedBody = previous;
        joint.anchor = new Vector3(0, 0, -plankPrefab.transform.localScale.z / 2f);
        joint.axis = new Vector3(1, 0, 0);
        
        var spring = new JointSpring
        {
            spring = jointSpring,
            damper = jointDamper
        };
        joint.spring = spring;
        joint.useSpring = true;
    }

    private Rigidbody CreateAnchor(Transform anchorTransform, Transform parent, string name)
    {
        var anchorObject = new GameObject(name);
        anchorObject.transform.SetPositionAndRotation(anchorTransform.position, anchorTransform.rotation);
        anchorObject.transform.SetParent(parent);
        var rb = anchorObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        return rb;
    }

    private void CreateRopeVisualizer(string name, Transform parent, List<Transform> points)
    {
        var ropeObject = new GameObject(name);
        ropeObject.transform.SetParent(parent);
        var visualizer = ropeObject.AddComponent<RopeVisualizer>();
        visualizer.Initialize(points, ropeMaterial, ropeWidth);
    }
    
    private bool AreSettingsValid()
    {
        if (startPoint == null || endPoint == null || plankPrefab == null)
        {
            Debug.LogError("Start Point, End Point e Plank Prefab devem ser atribuídos.");
            return false;
        }
        return true;
    }
}