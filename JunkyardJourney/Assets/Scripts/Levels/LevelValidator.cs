using System.Collections.Generic;
using UnityEngine;

public static class LevelValidator
{
    public static bool Validate(LevelDefinition level, out List<string> errors)
    {
        errors = new List<string>();

        if (level == null)
        {
            errors.Add("Level is null.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(level.levelId))
            errors.Add("Level is missing levelId.");

        if (string.IsNullOrWhiteSpace(level.name))
            errors.Add("Level is missing name.");

        if (level.worldBounds == null)
            errors.Add("Level is missing worldBounds.");

        if (level.startZone == null)
            errors.Add("Level is missing startZone.");

        if (level.goal == null)
            errors.Add("Level is missing goal.");

        if (level.terrain == null || level.terrain.Count == 0)
        {
            errors.Add("Level must contain at least one terrain definition.");
        }
        else
        {
            foreach (TerrainDefinition terrain in level.terrain)
            {
                if (terrain.points == null || terrain.points.Count < 2)
                {
                    errors.Add($"Terrain '{terrain.id}' must contain at least two points.");
                }
            }
        }

        if (level.worldBounds != null && level.startZone != null)
            ValidatePointInsideBounds("startZone", level.startZone.position, level.worldBounds, errors);

        if (level.worldBounds != null && level.goal != null)
            ValidatePointInsideBounds("goal", level.goal.position, level.worldBounds, errors);

        return errors.Count == 0;
    }

    private static void ValidatePointInsideBounds(
        string label,
        Vector2Definition point,
        WorldBoundsDefinition bounds,
        List<string> errors)
    {
        if (point == null)
        {
            errors.Add($"{label} position is missing.");
            return;
        }

        bool inside =
            point.x >= bounds.minX &&
            point.x <= bounds.maxX &&
            point.y >= bounds.minY &&
            point.y <= bounds.maxY;

        if (!inside)
            errors.Add($"{label} position ({point.x}, {point.y}) is outside world bounds.");
    }
}
