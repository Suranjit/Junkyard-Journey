using System;
using System.Collections.Generic;

[Serializable]
public class LevelDefinition
{
    public string schemaVersion;
    public string levelId;
    public string name;
    public string description;
    public string theme;
    public int difficulty;

    public WorldBoundsDefinition worldBounds;
    public StartZoneDefinition startZone;
    public GoalDefinition goal;
    public List<TerrainDefinition> terrain;
    public List<AvailablePartDefinition> availableParts;
}

[Serializable]
public class AvailablePartDefinition
{
    public string partId;
    public int count;
}

[Serializable]
public class WorldBoundsDefinition
{
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
}

[Serializable]
public class StartZoneDefinition
{
    public Vector2Definition position;
    public float width;
    public float height;
}

[Serializable]
public class GoalDefinition
{
    public string type;
    public Vector2Definition position;
    public float width;
    public float height;
}

[Serializable]
public class TerrainDefinition
{
    public string id;
    public string type;
    public List<Vector2Definition> points;
    public string surface;
}

[Serializable]
public class Vector2Definition
{
    public float x;
    public float y;

    public UnityEngine.Vector2 ToVector2()
    {
        return new UnityEngine.Vector2(x, y);
    }
}
