using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class LevelSerializationTests
{
    private const string ValidLevelJson = @"{
        ""schemaVersion"": ""1.0"",
        ""levelId"": ""junkyard_escape_001"",
        ""name"": ""Wake Up, Scraphead"",
        ""description"": ""The first escape route."",
        ""theme"": ""junkyard"",
        ""difficulty"": 1,
        ""worldBounds"": { ""minX"": -10, ""maxX"": 90, ""minY"": -25, ""maxY"": 35 },
        ""startZone"": {
            ""position"": { ""x"": 0, ""y"": 3 },
            ""width"": 12,
            ""height"": 8
        },
        ""goal"": {
            ""type"": ""reach_zone"",
            ""position"": { ""x"": 75, ""y"": 3 },
            ""width"": 8,
            ""height"": 8
        },
        ""terrain"": [
            {
                ""id"": ""main_ground"",
                ""type"": ""polyline"",
                ""surface"": ""scrap_dirt"",
                ""points"": [
                    { ""x"": -10, ""y"": 0 },
                    { ""x"": 20,  ""y"": 0 },
                    { ""x"": 40,  ""y"": 2 },
                    { ""x"": 60,  ""y"": 0 },
                    { ""x"": 90,  ""y"": 0 }
                ]
            }
        ]
    }";

    // -------------------------------------------------------------------------
    // Deserialization — top-level fields
    // -------------------------------------------------------------------------

    [Test]
    public void Deserialize_ValidJson_ReturnsNonNull()
    {
        LevelDefinition level = JsonUtility.FromJson<LevelDefinition>(ValidLevelJson);
        Assert.IsNotNull(level, "Expected deserialized level to not be null.");
    }

    [Test]
    public void Deserialize_ValidJson_LevelIdIsCorrect()
    {
        LevelDefinition level = JsonUtility.FromJson<LevelDefinition>(ValidLevelJson);
        Assert.AreEqual("junkyard_escape_001", level.levelId);
    }

    [Test]
    public void Deserialize_ValidJson_NameIsCorrect()
    {
        LevelDefinition level = JsonUtility.FromJson<LevelDefinition>(ValidLevelJson);
        Assert.AreEqual("Wake Up, Scraphead", level.name);
    }

    [Test]
    public void Deserialize_ValidJson_SchemaVersionIsCorrect()
    {
        LevelDefinition level = JsonUtility.FromJson<LevelDefinition>(ValidLevelJson);
        Assert.AreEqual("1.0", level.schemaVersion);
    }

    [Test]
    public void Deserialize_ValidJson_DifficultyIsCorrect()
    {
        LevelDefinition level = JsonUtility.FromJson<LevelDefinition>(ValidLevelJson);
        Assert.AreEqual(1, level.difficulty);
    }

    // -------------------------------------------------------------------------
    // Deserialization — world bounds
    // -------------------------------------------------------------------------

    [Test]
    public void Deserialize_ValidJson_WorldBoundsNotNull()
    {
        LevelDefinition level = JsonUtility.FromJson<LevelDefinition>(ValidLevelJson);
        Assert.IsNotNull(level.worldBounds);
    }

    [Test]
    public void Deserialize_ValidJson_WorldBoundsValuesCorrect()
    {
        LevelDefinition level = JsonUtility.FromJson<LevelDefinition>(ValidLevelJson);
        Assert.AreEqual(-10f, level.worldBounds.minX);
        Assert.AreEqual(90f,  level.worldBounds.maxX);
        Assert.AreEqual(-25f, level.worldBounds.minY);
        Assert.AreEqual(35f,  level.worldBounds.maxY);
    }

    // -------------------------------------------------------------------------
    // Deserialization — start zone
    // -------------------------------------------------------------------------

    [Test]
    public void Deserialize_ValidJson_StartZoneNotNull()
    {
        LevelDefinition level = JsonUtility.FromJson<LevelDefinition>(ValidLevelJson);
        Assert.IsNotNull(level.startZone);
    }

    [Test]
    public void Deserialize_ValidJson_StartZonePositionCorrect()
    {
        LevelDefinition level = JsonUtility.FromJson<LevelDefinition>(ValidLevelJson);
        Assert.AreEqual(0f, level.startZone.position.x);
        Assert.AreEqual(3f, level.startZone.position.y);
    }

    [Test]
    public void Deserialize_ValidJson_StartZoneSizeCorrect()
    {
        LevelDefinition level = JsonUtility.FromJson<LevelDefinition>(ValidLevelJson);
        Assert.AreEqual(12f, level.startZone.width);
        Assert.AreEqual(8f,  level.startZone.height);
    }

    // -------------------------------------------------------------------------
    // Deserialization — goal
    // -------------------------------------------------------------------------

    [Test]
    public void Deserialize_ValidJson_GoalNotNull()
    {
        LevelDefinition level = JsonUtility.FromJson<LevelDefinition>(ValidLevelJson);
        Assert.IsNotNull(level.goal);
    }

    [Test]
    public void Deserialize_ValidJson_GoalTypeCorrect()
    {
        LevelDefinition level = JsonUtility.FromJson<LevelDefinition>(ValidLevelJson);
        Assert.AreEqual("reach_zone", level.goal.type);
    }

    [Test]
    public void Deserialize_ValidJson_GoalPositionCorrect()
    {
        LevelDefinition level = JsonUtility.FromJson<LevelDefinition>(ValidLevelJson);
        Assert.AreEqual(75f, level.goal.position.x);
        Assert.AreEqual(3f,  level.goal.position.y);
    }

    // -------------------------------------------------------------------------
    // Deserialization — terrain
    // -------------------------------------------------------------------------

    [Test]
    public void Deserialize_ValidJson_TerrainNotNullAndNotEmpty()
    {
        LevelDefinition level = JsonUtility.FromJson<LevelDefinition>(ValidLevelJson);
        Assert.IsNotNull(level.terrain);
        Assert.AreEqual(1, level.terrain.Count);
    }

    [Test]
    public void Deserialize_ValidJson_TerrainIdCorrect()
    {
        LevelDefinition level = JsonUtility.FromJson<LevelDefinition>(ValidLevelJson);
        Assert.AreEqual("main_ground", level.terrain[0].id);
    }

    [Test]
    public void Deserialize_ValidJson_TerrainHasFivePoints()
    {
        LevelDefinition level = JsonUtility.FromJson<LevelDefinition>(ValidLevelJson);
        Assert.AreEqual(5, level.terrain[0].points.Count);
    }

    [Test]
    public void Deserialize_ValidJson_TerrainFirstPointCorrect()
    {
        LevelDefinition level = JsonUtility.FromJson<LevelDefinition>(ValidLevelJson);
        Vector2Definition first = level.terrain[0].points[0];
        Assert.AreEqual(-10f, first.x);
        Assert.AreEqual(0f,   first.y);
    }

    [Test]
    public void Deserialize_ValidJson_TerrainSurfaceCorrect()
    {
        LevelDefinition level = JsonUtility.FromJson<LevelDefinition>(ValidLevelJson);
        Assert.AreEqual("scrap_dirt", level.terrain[0].surface);
    }

    // -------------------------------------------------------------------------
    // Vector2Definition.ToVector2
    // -------------------------------------------------------------------------

    [Test]
    public void Vector2Definition_ToVector2_ConvertsCorrectly()
    {
        Vector2Definition def = new Vector2Definition { x = 5f, y = -3f };
        Vector2 result = def.ToVector2();
        Assert.AreEqual(5f,  result.x);
        Assert.AreEqual(-3f, result.y);
    }

    // -------------------------------------------------------------------------
    // Malformed JSON
    // -------------------------------------------------------------------------

    [Test]
    public void Deserialize_EmptyJson_ReturnsDefaultObject()
    {
        LevelDefinition level = JsonUtility.FromJson<LevelDefinition>("{}");
        Assert.IsNotNull(level);
        Assert.IsTrue(string.IsNullOrEmpty(level.levelId),
            "Expected levelId to be null or empty when not present in JSON.");
        Assert.IsTrue(level.terrain == null || level.terrain.Count == 0,
            "Expected terrain to be null or empty when not present in JSON.");
    }
}
