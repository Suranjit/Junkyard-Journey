using System.Collections.Generic;
using NUnit.Framework;

public class LevelValidationTests
{
    // -------------------------------------------------------------------------
    // Helpers
    // -------------------------------------------------------------------------

    private LevelDefinition BuildValidLevel()
    {
        return new LevelDefinition
        {
            schemaVersion = "1.0",
            levelId = "junkyard_escape_001",
            name = "Wake Up, Scraphead",
            theme = "junkyard",
            difficulty = 1,
            worldBounds = new WorldBoundsDefinition
            {
                minX = -10f, maxX = 90f,
                minY = -25f, maxY = 35f
            },
            startZone = new StartZoneDefinition
            {
                position = new Vector2Definition { x = 0f, y = 3f },
                width = 12f,
                height = 8f
            },
            goal = new GoalDefinition
            {
                type = "reach_zone",
                position = new Vector2Definition { x = 75f, y = 3f },
                width = 8f,
                height = 8f
            },
            terrain = new List<TerrainDefinition>
            {
                new TerrainDefinition
                {
                    id = "main_ground",
                    type = "polyline",
                    surface = "scrap_dirt",
                    points = new List<Vector2Definition>
                    {
                        new Vector2Definition { x = -10f, y = 0f },
                        new Vector2Definition { x = 90f,  y = 0f }
                    }
                }
            }
        };
    }

    // -------------------------------------------------------------------------
    // Happy path
    // -------------------------------------------------------------------------

    [Test]
    public void Validate_ValidLevel_ReturnsTrue()
    {
        LevelDefinition level = BuildValidLevel();
        bool result = LevelValidator.Validate(level, out List<string> errors);
        Assert.IsTrue(result, "Expected valid level to pass validation.");
        Assert.AreEqual(0, errors.Count, "Expected no validation errors.");
    }

    // -------------------------------------------------------------------------
    // Null / missing top-level fields
    // -------------------------------------------------------------------------

    [Test]
    public void Validate_NullLevel_ReturnsFalse()
    {
        bool result = LevelValidator.Validate(null, out List<string> errors);
        Assert.IsFalse(result);
        Assert.IsTrue(errors.Count > 0);
    }

    [Test]
    public void Validate_MissingLevelId_ReturnsError()
    {
        LevelDefinition level = BuildValidLevel();
        level.levelId = null;
        LevelValidator.Validate(level, out List<string> errors);
        Assert.IsTrue(errors.Exists(e => e.Contains("levelId")),
            "Expected error about missing levelId.");
    }

    [Test]
    public void Validate_MissingName_ReturnsError()
    {
        LevelDefinition level = BuildValidLevel();
        level.name = "";
        LevelValidator.Validate(level, out List<string> errors);
        Assert.IsTrue(errors.Exists(e => e.Contains("name")),
            "Expected error about missing name.");
    }

    [Test]
    public void Validate_MissingWorldBounds_ReturnsError()
    {
        LevelDefinition level = BuildValidLevel();
        level.worldBounds = null;
        LevelValidator.Validate(level, out List<string> errors);
        Assert.IsTrue(errors.Exists(e => e.Contains("worldBounds")),
            "Expected error about missing worldBounds.");
    }

    [Test]
    public void Validate_MissingStartZone_ReturnsError()
    {
        LevelDefinition level = BuildValidLevel();
        level.startZone = null;
        LevelValidator.Validate(level, out List<string> errors);
        Assert.IsTrue(errors.Exists(e => e.Contains("startZone")),
            "Expected error about missing startZone.");
    }

    [Test]
    public void Validate_MissingGoal_ReturnsError()
    {
        LevelDefinition level = BuildValidLevel();
        level.goal = null;
        LevelValidator.Validate(level, out List<string> errors);
        Assert.IsTrue(errors.Exists(e => e.Contains("goal")),
            "Expected error about missing goal.");
    }

    // -------------------------------------------------------------------------
    // Terrain validation
    // -------------------------------------------------------------------------

    [Test]
    public void Validate_NullTerrain_ReturnsError()
    {
        LevelDefinition level = BuildValidLevel();
        level.terrain = null;
        LevelValidator.Validate(level, out List<string> errors);
        Assert.IsTrue(errors.Exists(e => e.Contains("terrain")),
            "Expected error about missing terrain.");
    }

    [Test]
    public void Validate_EmptyTerrainList_ReturnsError()
    {
        LevelDefinition level = BuildValidLevel();
        level.terrain = new List<TerrainDefinition>();
        LevelValidator.Validate(level, out List<string> errors);
        Assert.IsTrue(errors.Exists(e => e.Contains("terrain")),
            "Expected error about empty terrain list.");
    }

    [Test]
    public void Validate_TerrainWithOnePoint_ReturnsError()
    {
        LevelDefinition level = BuildValidLevel();
        level.terrain[0].points = new List<Vector2Definition>
        {
            new Vector2Definition { x = 0f, y = 0f }
        };
        LevelValidator.Validate(level, out List<string> errors);
        Assert.IsTrue(errors.Exists(e => e.Contains("main_ground")),
            "Expected error referencing terrain id 'main_ground'.");
    }

    [Test]
    public void Validate_TerrainWithNullPoints_ReturnsError()
    {
        LevelDefinition level = BuildValidLevel();
        level.terrain[0].points = null;
        LevelValidator.Validate(level, out List<string> errors);
        Assert.IsTrue(errors.Exists(e => e.Contains("main_ground")),
            "Expected error referencing terrain id 'main_ground'.");
    }

    // -------------------------------------------------------------------------
    // Bounds validation
    // -------------------------------------------------------------------------

    [Test]
    public void Validate_StartZoneOutsideBounds_ReturnsError()
    {
        LevelDefinition level = BuildValidLevel();
        level.startZone.position = new Vector2Definition { x = -999f, y = 0f };
        LevelValidator.Validate(level, out List<string> errors);
        Assert.IsTrue(errors.Exists(e => e.Contains("startZone")),
            "Expected error about startZone outside world bounds.");
    }

    [Test]
    public void Validate_GoalOutsideBounds_ReturnsError()
    {
        LevelDefinition level = BuildValidLevel();
        level.goal.position = new Vector2Definition { x = 999f, y = 0f };
        LevelValidator.Validate(level, out List<string> errors);
        Assert.IsTrue(errors.Exists(e => e.Contains("goal")),
            "Expected error about goal outside world bounds.");
    }

    // -------------------------------------------------------------------------
    // Multiple errors
    // -------------------------------------------------------------------------

    [Test]
    public void Validate_MultipleInvalidFields_ReturnsAllErrors()
    {
        LevelDefinition level = BuildValidLevel();
        level.levelId = null;
        level.goal = null;
        level.terrain[0].points = new List<Vector2Definition>
        {
            new Vector2Definition { x = 0f, y = 0f }
        };

        LevelValidator.Validate(level, out List<string> errors);
        Assert.GreaterOrEqual(errors.Count, 3,
            "Expected at least 3 errors for 3 invalid fields.");
    }
}
