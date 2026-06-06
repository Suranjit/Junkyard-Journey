using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Junkyard Journey/Part Definition")]
public class PartDefinition : ScriptableObject
{
    [Header("Identity")]
    public string partId;
    public string displayName;
    public PartType partType;

    [Header("Prefabs")]
    public GameObject buildPrefab;
    public GameObject simulationPrefab;

    [Header("Physics")]
    public float mass = 1f;
    public Vector2 size = Vector2.one;
    public float breakForce = 100f;

    [Header("Build Rules")]
    public bool canRotate = true;
    public bool allowOverlap = true;

    [Header("Anchors")]
    public List<AnchorDefinition> anchors = new();
}
