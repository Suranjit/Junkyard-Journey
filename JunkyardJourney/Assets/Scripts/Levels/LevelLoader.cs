using UnityEngine;

public static class LevelLoader
{
    public static LevelDefinition LoadFromTextAsset(TextAsset levelJson)
    {
        if (levelJson == null)
        {
            Debug.LogError("LevelLoader: levelJson is null. Assign the JSON file in the Inspector.");
            return null;
        }

        try
        {
            LevelDefinition level = JsonUtility.FromJson<LevelDefinition>(levelJson.text);

            if (level == null)
            {
                Debug.LogError("LevelLoader: Failed to parse level JSON. Check JSON syntax and field names.");
                return null;
            }

            Debug.Log($"LevelLoader: Loaded level '{level.name}' with id '{level.levelId}'.");
            return level;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"LevelLoader: Exception while parsing level JSON: {ex.Message}");
            return null;
        }
    }
}
