using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Junkyard Journey/Part Catalog")]
public class PartCatalog : ScriptableObject
{
    public List<PartDefinition> parts = new();

    private Dictionary<string, PartDefinition> lookup;

    public void Initialize()
    {
        lookup = new Dictionary<string, PartDefinition>();

        foreach (PartDefinition part in parts)
        {
            if (part == null)
            {
                Debug.LogWarning("Null PartDefinition found in PartCatalog.");
                continue;
            }

            if (string.IsNullOrWhiteSpace(part.partId))
            {
                Debug.LogWarning($"PartDefinition has empty partId: {part.name}");
                continue;
            }

            if (lookup.ContainsKey(part.partId))
            {
                Debug.LogWarning($"Duplicate partId found: {part.partId}");
                continue;
            }

            lookup[part.partId] = part;
        }
    }

    public PartDefinition GetPart(string partId)
    {
        if (lookup == null)
            Initialize();

        if (lookup.TryGetValue(partId, out PartDefinition part))
            return part;

        Debug.LogError($"Part not found in catalog: {partId}");
        return null;
    }

    public bool HasPart(string partId)
    {
        if (lookup == null)
            Initialize();

        return lookup.ContainsKey(partId);
    }
}
