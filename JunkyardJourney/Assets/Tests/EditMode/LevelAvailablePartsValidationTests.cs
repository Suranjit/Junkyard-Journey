using NUnit.Framework;
using UnityEditor;
using UnityEngine;

public class LevelAvailablePartsValidationTests
{
    private const string CatalogPath = "Assets/Data/Parts/PartCatalog.asset";
    private const string LevelPath = "Assets/Data/Levels/level_001.json";

    [Test]
    public void Level001_AvailableParts_ExistInPartCatalog()
    {
        PartCatalog catalog = AssetDatabase.LoadAssetAtPath<PartCatalog>(CatalogPath);
        Assert.IsNotNull(catalog, $"PartCatalog not found at {CatalogPath}");
        catalog.Initialize();

        TextAsset levelText = AssetDatabase.LoadAssetAtPath<TextAsset>(LevelPath);
        Assert.IsNotNull(levelText, $"Level JSON not found at {LevelPath}");

        LevelDefinition level = JsonUtility.FromJson<LevelDefinition>(levelText.text);
        Assert.IsNotNull(level, "Failed to parse level JSON");
        Assert.IsNotNull(level.availableParts, "level.availableParts is null");
        Assert.Greater(level.availableParts.Count, 0, "level.availableParts must contain at least one part");

        foreach (AvailablePartDefinition availablePart in level.availableParts)
        {
            Assert.IsFalse(string.IsNullOrWhiteSpace(availablePart.partId),
                "Level contains availablePart with empty partId");
            Assert.IsTrue(catalog.HasPart(availablePart.partId),
                $"Level references missing partId: {availablePart.partId}");
            Assert.Greater(availablePart.count, 0,
                $"Level part {availablePart.partId} count must be greater than 0");
        }
    }
}
