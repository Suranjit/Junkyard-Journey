using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

public class PartDefinitionValidationTests
{
    private const string PartsFolder = "Assets/Data/Parts";

    [Test]
    public void AllPartDefinitions_HaveValidRequiredFields()
    {
        string[] guids = AssetDatabase.FindAssets("t:PartDefinition", new[] { PartsFolder });

        Assert.Greater(guids.Length, 0, $"No PartDefinition assets found in {PartsFolder}");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            PartDefinition part = AssetDatabase.LoadAssetAtPath<PartDefinition>(path);

            Assert.IsNotNull(part, $"Could not load PartDefinition at {path}");
            Assert.IsFalse(string.IsNullOrWhiteSpace(part.partId), $"{path} has empty partId");
            Assert.IsFalse(string.IsNullOrWhiteSpace(part.displayName), $"{part.partId} has empty displayName");
            Assert.IsNotNull(part.buildPrefab, $"{part.partId} is missing buildPrefab");
            Assert.IsNotNull(part.simulationPrefab, $"{part.partId} is missing simulationPrefab");
            Assert.Greater(part.mass, 0f, $"{part.partId} mass must be greater than 0");
            Assert.Greater(part.size.x, 0f, $"{part.partId} size.x must be greater than 0");
            Assert.Greater(part.size.y, 0f, $"{part.partId} size.y must be greater than 0");
            Assert.IsNotNull(part.anchors, $"{part.partId} anchors list is null");
            Assert.Greater(part.anchors.Count, 0, $"{part.partId} must have at least one anchor");

            foreach (AnchorDefinition anchor in part.anchors)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(anchor.anchorId),
                    $"{part.partId} has an anchor with empty anchorId");
                Assert.IsNotNull(anchor.allowedConnections,
                    $"{part.partId}.{anchor.anchorId} allowedConnections is null");
                Assert.Greater(anchor.allowedConnections.Count, 0,
                    $"{part.partId}.{anchor.anchorId} must allow at least one connection type");
            }
        }
    }

    [Test]
    public void PartIds_AreUnique()
    {
        string[] guids = AssetDatabase.FindAssets("t:PartDefinition", new[] { PartsFolder });
        HashSet<string> seen = new HashSet<string>();

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            PartDefinition part = AssetDatabase.LoadAssetAtPath<PartDefinition>(path);

            Assert.IsNotNull(part, $"Could not load PartDefinition at {path}");
            Assert.IsFalse(seen.Contains(part.partId), $"Duplicate partId found: {part.partId} at {path}");
            seen.Add(part.partId);
        }
    }

    [Test]
    public void RequiredMvpParts_Exist()
    {
        string[] requiredPartIds =
        {
            "core_cockpit",
            "frame_wood",
            "wheel_small",
            "engine_basic",
            "rocket_small",
            "balloon_small"
        };

        string[] guids = AssetDatabase.FindAssets("t:PartDefinition", new[] { PartsFolder });
        HashSet<string> foundPartIds = new HashSet<string>();

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            PartDefinition part = AssetDatabase.LoadAssetAtPath<PartDefinition>(path);
            if (part != null && !string.IsNullOrWhiteSpace(part.partId))
                foundPartIds.Add(part.partId);
        }

        foreach (string requiredPartId in requiredPartIds)
        {
            Assert.IsTrue(foundPartIds.Contains(requiredPartId),
                $"Missing required MVP part: {requiredPartId}");
        }
    }
}
