using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Level Data")]
    public TextAsset levelJson;

    [Header("Scene References")]
    public LevelBuilder levelBuilder;

    private void Start()
    {
        LoadAndBuildLevel();
    }

    public void LoadAndBuildLevel()
    {
        LevelDefinition level = LevelLoader.LoadFromTextAsset(levelJson);

        if (level == null)
            return;

        if (!LevelValidator.Validate(level, out List<string> validationErrors))
        {
            Debug.LogError("GameManager: Level validation failed:");
            foreach (string error in validationErrors)
                Debug.LogError($"  - {error}");
            return;
        }

        Debug.Log("GameManager: Level validation passed.");
        levelBuilder.BuildLevel(level);
    }
}
