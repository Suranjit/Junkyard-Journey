using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    [Header("Scene Roots")]
    public Transform levelRoot;
    public Transform terrainRoot;
    public Transform startGoalRoot;

    [Header("Builders")]
    public TerrainBuilder2D terrainBuilder;

    [Header("Zone Colors")]
    public Color startZoneColor = new Color(0f, 1f, 0f, 0.25f);
    public Color goalZoneColor = new Color(1f, 0.9f, 0f, 0.25f);

    public void BuildLevel(LevelDefinition level)
    {
        if (level == null)
        {
            Debug.LogError("LevelBuilder: Cannot build null level.");
            return;
        }

        ClearRoot(terrainRoot);
        ClearRoot(startGoalRoot);

        terrainBuilder.BuildTerrain(level.terrain, terrainRoot);
        SpawnStartZone(level.startZone);
        SpawnGoalZone(level.goal);

        Debug.Log($"LevelBuilder: Finished building level '{level.name}'.");
    }

    private void SpawnStartZone(StartZoneDefinition startZone)
    {
        if (startZone == null)
        {
            Debug.LogWarning("LevelBuilder: startZone is null, skipping.");
            return;
        }

        GameObject startObject = CreateZone(
            "StartZone",
            startZone.position.ToVector2(),
            startZone.width,
            startZone.height,
            startZoneColor);

        startObject.transform.SetParent(startGoalRoot, false);
    }

    private void SpawnGoalZone(GoalDefinition goal)
    {
        if (goal == null)
        {
            Debug.LogWarning("LevelBuilder: goal is null, skipping.");
            return;
        }

        GameObject goalObject = CreateZone(
            "GoalZone",
            goal.position.ToVector2(),
            goal.width,
            goal.height,
            goalZoneColor);

        goalObject.tag = "GoalZone";
        goalObject.transform.SetParent(startGoalRoot, false);
    }

    private GameObject CreateZone(string zoneName, Vector2 position, float width, float height, Color color)
    {
        GameObject zone = new GameObject(zoneName);
        zone.transform.position = new Vector3(position.x, position.y, 0f);

        SpriteRenderer spriteRenderer = zone.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = CreateWhiteSprite();
        spriteRenderer.color = color;
        spriteRenderer.sortingOrder = 0;
        zone.transform.localScale = new Vector3(width, height, 1f);

        BoxCollider2D collider = zone.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;

        return zone;
    }

    private Sprite CreateWhiteSprite()
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, Color.white);
        texture.Apply();

        return Sprite.Create(
            texture,
            new Rect(0, 0, 1, 1),
            new Vector2(0.5f, 0.5f),
            1f);
    }

    private void ClearRoot(Transform root)
    {
        if (root == null)
            return;

        for (int i = root.childCount - 1; i >= 0; i--)
        {
            Destroy(root.GetChild(i).gameObject);
        }
    }
}
