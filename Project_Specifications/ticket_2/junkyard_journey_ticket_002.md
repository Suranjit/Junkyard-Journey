# Junkyard Journey — Ticket 002: Part Catalog + Placeholder Part Prefabs

## Ticket Summary

**Ticket ID:** JJ-002  
**Title:** Create Part Catalog and Placeholder Part Prefabs  
**Milestone:** MVP Foundation — Data-Driven Parts  
**Depends On:** JJ-001 — Load and Build Level From JSON  
**Primary Goal:** Define the first buildable parts as data assets so levels can reference parts by `partId` and Unity can resolve those IDs into usable build and simulation prefabs.

---

## Context

Ticket 1 proved that **Junkyard Journey** can load a JSON level definition and generate basic terrain, start zone, and goal zone.

Ticket 2 introduces the next foundational layer: the **part catalog**.

The game needs a reliable way to answer:

- What parts exist?
- What is each part called?
- What type of part is it?
- What prefab should appear in build mode?
- What prefab should appear in simulation mode?
- What anchors does the part expose?
- What types of bolts/connections are allowed?
- Does a level reference only valid part IDs?

This ticket does **not** implement dragging, placement, snapping, physics launch, engines, rockets, or balloons yet. It only creates the part data model, catalog, placeholder assets, and tests.

---

## Design Direction

**Junkyard Journey** uses a free-form 2D LEGO-like build system where the player bolts together janky junk parts using visible anchors and bolts.

The part catalog is the source of truth for all buildable parts.

The first MVP parts are:

1. `core_cockpit`
2. `frame_wood`
3. `wheel_small`
4. `engine_basic`
5. `rocket_small`
6. `balloon_small`

The alien mechanic character is represented by the **core cockpit**. This is the required central part of the vehicle. Later failure rules will use this part to determine whether the vehicle broke, fell, or became detached.

---

## Goals

Implement a data-driven part system using Unity `ScriptableObject` assets.

By the end of this ticket:

- Unity has a `PartDefinition` asset for each MVP part.
- Unity has a `PartCatalog` asset that references all MVP parts.
- Each part has a unique `partId`.
- Each part has placeholder build and simulation prefabs.
- Each part has one or more anchors.
- Each anchor defines allowed connection types.
- Code can resolve part IDs from a level file into `PartDefinition` objects.
- EditMode tests verify catalog lookup, asset validity, duplicate IDs, missing IDs, anchors, prefabs, and level part references.

---

## Non-Goals

This ticket does **not** include:

- Drag-and-drop building UI
- Free-form placement
- Anchor snapping behavior
- Visible bolt rendering
- Physics conversion from build mode to simulation mode
- Engine torque behavior
- Rocket thrust behavior
- Balloon lift behavior
- Glider, gear, or pulley implementation
- Runtime part configuration timers
- Save/load of player builds

Those come in later tickets.

---

## Updated Unity Directory Structure

Create or update the following folders:

```text
Assets/
  Data/
    Parts/
      PartCatalog.asset
      core_cockpit.asset
      frame_wood.asset
      wheel_small.asset
      engine_basic.asset
      rocket_small.asset
      balloon_small.asset

  Prefabs/
    Parts/
      Build/
        CoreCockpit_Build.prefab
        FrameWood_Build.prefab
        WheelSmall_Build.prefab
        EngineBasic_Build.prefab
        RocketSmall_Build.prefab
        BalloonSmall_Build.prefab

      Simulation/
        CoreCockpit_Sim.prefab
        FrameWood_Sim.prefab
        WheelSmall_Sim.prefab
        EngineBasic_Sim.prefab
        RocketSmall_Sim.prefab
        BalloonSmall_Sim.prefab

  Scripts/
    Parts/
      PartType.cs
      ConnectionType.cs
      PartAnchorType.cs
      AnchorDefinition.cs
      PartDefinition.cs
      PartCatalog.cs

  Tests/
    EditMode/
      PartCatalogTests.cs
      PartDefinitionValidationTests.cs
      LevelAvailablePartsValidationTests.cs
```

---

## Part Data Model

### Conceptual Model

```text
PartCatalog
  - List<PartDefinition>
  - Lookup by partId

PartDefinition
  - Identity
  - Prefabs
  - Physics values
  - Build rules
  - Anchors

AnchorDefinition
  - Anchor ID
  - Anchor type
  - Local position
  - Allowed connection types
```

---

## C# Implementation

### File: `Assets/Scripts/Parts/PartType.cs`

```csharp
public enum PartType
{
    Core,
    Frame,
    Wheel,
    Engine,
    Rocket,
    Balloon,
    Glider,
    Gear,
    Pulley
}
```

---

### File: `Assets/Scripts/Parts/ConnectionType.cs`

```csharp
public enum ConnectionType
{
    FixedBolt,
    AxleBolt,
    RopeBolt,
    GearMesh
}
```

---

### File: `Assets/Scripts/Parts/PartAnchorType.cs`

```csharp
public enum PartAnchorType
{
    Generic,
    Mount,
    Axle,
    EngineOutput,
    Rope,
    GearCenter
}
```

---

### File: `Assets/Scripts/Parts/AnchorDefinition.cs`

```csharp
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnchorDefinition
{
    public string anchorId;
    public PartAnchorType anchorType;
    public Vector2 localPosition;
    public List<ConnectionType> allowedConnections = new();
}
```

---

### File: `Assets/Scripts/Parts/PartDefinition.cs`

```csharp
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Junkyard Journey/Part Definition")]
public class PartDefinition : ScriptableObject
{
    [Header("Identity")]
    public string partId;
    public string displayName;
    public PartType partType;

    [Header("Prefabs")]
    public GameObject buildPrefab;
    public GameObject simulationPrefab;

    [Header("Physics")]
    public float mass = 1f;
    public Vector2 size = Vector2.one;
    public float breakForce = 100f;

    [Header("Build Rules")]
    public bool canRotate = true;
    public bool allowOverlap = true;

    [Header("Anchors")]
    public List<AnchorDefinition> anchors = new();
}
```

---

### File: `Assets/Scripts/Parts/PartCatalog.cs`

```csharp
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

        foreach (var part in parts)
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

        if (lookup.TryGetValue(partId, out var part))
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
```

---

## Placeholder Prefabs

Create very simple placeholder prefabs for both build mode and simulation mode.

For this ticket, they can be primitive placeholder objects using basic sprites or simple 2D shapes.

### Build Prefabs

Build prefabs are what the player will eventually see while assembling the machine.

```text
Assets/Prefabs/Parts/Build/CoreCockpit_Build.prefab
Assets/Prefabs/Parts/Build/FrameWood_Build.prefab
Assets/Prefabs/Parts/Build/WheelSmall_Build.prefab
Assets/Prefabs/Parts/Build/EngineBasic_Build.prefab
Assets/Prefabs/Parts/Build/RocketSmall_Build.prefab
Assets/Prefabs/Parts/Build/BalloonSmall_Build.prefab
```

Recommended placeholder components:

- `SpriteRenderer`
- Optional simple child objects showing anchor positions
- No physics required yet

### Simulation Prefabs

Simulation prefabs will later be used after the player presses Launch.

```text
Assets/Prefabs/Parts/Simulation/CoreCockpit_Sim.prefab
Assets/Prefabs/Parts/Simulation/FrameWood_Sim.prefab
Assets/Prefabs/Parts/Simulation/WheelSmall_Sim.prefab
Assets/Prefabs/Parts/Simulation/EngineBasic_Sim.prefab
Assets/Prefabs/Parts/Simulation/RocketSmall_Sim.prefab
Assets/Prefabs/Parts/Simulation/BalloonSmall_Sim.prefab
```

Recommended placeholder components:

- `SpriteRenderer`
- `Rigidbody2D` optional for now
- Collider optional for now

If physics components are not added yet, that is acceptable for Ticket 2. Later simulation tickets will refine them.

---

## Initial Part Definitions

Create each asset using:

```text
Right-click in Project window
Create → Junkyard Journey → Part Definition
```

Save the assets under:

```text
Assets/Data/Parts/
```

---

### Part: `core_cockpit`

**Asset path:**

```text
Assets/Data/Parts/core_cockpit.asset
```

**Purpose:**

The alien mechanic's cockpit/core. This is the required central part of the contraption. Later failure logic will fail the level if this part is destroyed, disconnected, stuck, or falls out of bounds.

**Recommended fields:**

```text
partId: core_cockpit
displayName: Alien Cockpit
partType: Core
mass: 2.0
size: (2.0, 1.5)
breakForce: 150
canRotate: true
allowOverlap: true
buildPrefab: CoreCockpit_Build.prefab
simulationPrefab: CoreCockpit_Sim.prefab
```

**Anchors:**

```text
left_mount
  anchorType: Mount
  localPosition: (-1.0, 0.0)
  allowedConnections: FixedBolt

right_mount
  anchorType: Mount
  localPosition: (1.0, 0.0)
  allowedConnections: FixedBolt

top_mount
  anchorType: Mount
  localPosition: (0.0, 0.75)
  allowedConnections: FixedBolt

bottom_mount
  anchorType: Mount
  localPosition: (0.0, -0.75)
  allowedConnections: FixedBolt
```

---

### Part: `frame_wood`

**Asset path:**

```text
Assets/Data/Parts/frame_wood.asset
```

**Purpose:**

Basic structural junk piece.

**Recommended fields:**

```text
partId: frame_wood
displayName: Wood Frame
partType: Frame
mass: 1.0
size: (2.0, 0.5)
breakForce: 100
canRotate: true
allowOverlap: true
buildPrefab: FrameWood_Build.prefab
simulationPrefab: FrameWood_Sim.prefab
```

**Anchors:**

```text
left
  anchorType: Mount
  localPosition: (-1.0, 0.0)
  allowedConnections: FixedBolt

right
  anchorType: Mount
  localPosition: (1.0, 0.0)
  allowedConnections: FixedBolt

top
  anchorType: Mount
  localPosition: (0.0, 0.25)
  allowedConnections: FixedBolt

bottom
  anchorType: Mount
  localPosition: (0.0, -0.25)
  allowedConnections: FixedBolt
```

---

### Part: `wheel_small`

**Asset path:**

```text
Assets/Data/Parts/wheel_small.asset
```

**Purpose:**

Basic rolling movement part.

**Recommended fields:**

```text
partId: wheel_small
displayName: Small Wheel
partType: Wheel
mass: 0.75
size: (1.0, 1.0)
breakForce: 100
canRotate: true
allowOverlap: true
buildPrefab: WheelSmall_Build.prefab
simulationPrefab: WheelSmall_Sim.prefab
```

**Anchors:**

```text
axle
  anchorType: Axle
  localPosition: (0.0, 0.0)
  allowedConnections: AxleBolt
```

---

### Part: `engine_basic`

**Asset path:**

```text
Assets/Data/Parts/engine_basic.asset
```

**Purpose:**

Basic engine. Later it will power connected wheels automatically after launch.

**Recommended fields:**

```text
partId: engine_basic
displayName: Basic Engine
partType: Engine
mass: 1.5
size: (1.5, 1.0)
breakForce: 120
canRotate: true
allowOverlap: true
buildPrefab: EngineBasic_Build.prefab
simulationPrefab: EngineBasic_Sim.prefab
```

**Anchors:**

```text
mount
  anchorType: Mount
  localPosition: (0.0, 0.0)
  allowedConnections: FixedBolt

output
  anchorType: EngineOutput
  localPosition: (0.75, 0.0)
  allowedConnections: FixedBolt
```

Note: the `output` anchor is not used yet. It prepares the system for later engine/drivetrain logic.

---

### Part: `rocket_small`

**Asset path:**

```text
Assets/Data/Parts/rocket_small.asset
```

**Purpose:**

Timed thrust part. Later the player will configure start delay and burn duration before launch.

**Recommended fields:**

```text
partId: rocket_small
displayName: Small Rocket
partType: Rocket
mass: 1.0
size: (1.5, 0.6)
breakForce: 100
canRotate: true
allowOverlap: true
buildPrefab: RocketSmall_Build.prefab
simulationPrefab: RocketSmall_Sim.prefab
```

**Anchors:**

```text
mount
  anchorType: Mount
  localPosition: (-0.75, 0.0)
  allowedConnections: FixedBolt
```

---

### Part: `balloon_small`

**Asset path:**

```text
Assets/Data/Parts/balloon_small.asset
```

**Purpose:**

Lift part. Later it will apply upward force during simulation.

**Recommended fields:**

```text
partId: balloon_small
displayName: Small Balloon
partType: Balloon
mass: 0.25
size: (1.5, 2.0)
breakForce: 80
canRotate: true
allowOverlap: true
buildPrefab: BalloonSmall_Build.prefab
simulationPrefab: BalloonSmall_Sim.prefab
```

**Anchors:**

```text
string_mount
  anchorType: Mount
  localPosition: (0.0, -1.0)
  allowedConnections: FixedBolt
```

---

## Part Catalog Asset Setup

Create the catalog asset:

```text
Right-click in Assets/Data/Parts
Create → Junkyard Journey → Part Catalog
```

Save it as:

```text
Assets/Data/Parts/PartCatalog.asset
```

Add these six assets to its `parts` list:

```text
core_cockpit.asset
frame_wood.asset
wheel_small.asset
engine_basic.asset
rocket_small.asset
balloon_small.asset
```

---

## Level Validation Integration

Ticket 2 should update level validation so that level `availableParts` references can be checked against the part catalog.

A sample level may contain:

```json
"availableParts": [
  { "partId": "core_cockpit", "count": 1 },
  { "partId": "frame_wood", "count": 8 },
  { "partId": "wheel_small", "count": 4 },
  { "partId": "engine_basic", "count": 1 }
]
```

Every `partId` above must exist in `PartCatalog.asset`.

If any level references a missing part ID, validation should fail or log a clear error.

---

## Acceptance Criteria

Ticket 2 is complete when all of the following are true:

1. `PartType.cs` exists and includes `Core`, `Frame`, `Wheel`, `Engine`, `Rocket`, `Balloon`, `Glider`, `Gear`, and `Pulley`.
2. `ConnectionType.cs` exists and includes `FixedBolt`, `AxleBolt`, `RopeBolt`, and `GearMesh`.
3. `PartAnchorType.cs` exists and includes `Generic`, `Mount`, `Axle`, `EngineOutput`, `Rope`, and `GearCenter`.
4. `AnchorDefinition.cs` exists and stores `anchorId`, `anchorType`, `localPosition`, and `allowedConnections`.
5. `PartDefinition.cs` exists as a `ScriptableObject`.
6. `PartCatalog.cs` exists as a `ScriptableObject`.
7. Six MVP `PartDefinition` assets exist:
   - `core_cockpit`
   - `frame_wood`
   - `wheel_small`
   - `engine_basic`
   - `rocket_small`
   - `balloon_small`
8. `PartCatalog.asset` exists and references all six MVP parts.
9. Each part has a non-empty `partId`.
10. Each part has a non-empty `displayName`.
11. Each part has a `buildPrefab` assigned.
12. Each part has a `simulationPrefab` assigned.
13. Each part has `mass > 0`.
14. Each part has `size.x > 0` and `size.y > 0`.
15. Each part has at least one anchor.
16. Each anchor has a non-empty `anchorId`.
17. Each anchor has at least one allowed connection type.
18. `PartCatalog.GetPart("frame_wood")` returns the correct part.
19. `PartCatalog.HasPart("wheel_small")` returns `true`.
20. `PartCatalog.HasPart("missing_part")` returns `false`.
21. Missing part IDs log a clear error.
22. Duplicate part IDs log a clear warning.
23. `level_001.json` references only part IDs that exist in the part catalog.
24. All EditMode tests pass.

---

# Test Plan

Ticket 2 should be tested with **Unity EditMode tests**.

Most of this ticket is data and lookup logic, so PlayMode tests are not required yet.

## Test Directory

Create:

```text
Assets/Tests/EditMode/
```

Add:

```text
PartCatalogTests.cs
PartDefinitionValidationTests.cs
LevelAvailablePartsValidationTests.cs
```

---

## How to Run Tests

In Unity:

```text
Window → General → Test Runner
```

Then choose:

```text
EditMode → Run All
```

Expected test groups:

```text
PartCatalogTests
PartDefinitionValidationTests
LevelAvailablePartsValidationTests
```

All tests should pass before Ticket 2 is marked complete.

---

## Test File 1: `PartCatalogTests.cs`

**Path:**

```text
Assets/Tests/EditMode/PartCatalogTests.cs
```

**Purpose:**

Tests pure in-memory `PartCatalog` behavior without relying on project assets.

```csharp
using NUnit.Framework;
using UnityEngine;

public class PartCatalogTests
{
    private PartDefinition CreatePart(string partId, string displayName, PartType type)
    {
        var part = ScriptableObject.CreateInstance<PartDefinition>();
        part.partId = partId;
        part.displayName = displayName;
        part.partType = type;
        part.mass = 1f;
        part.size = Vector2.one;
        part.breakForce = 100f;
        part.canRotate = true;
        part.allowOverlap = true;

        part.anchors.Add(new AnchorDefinition
        {
            anchorId = "mount",
            anchorType = PartAnchorType.Mount,
            localPosition = Vector2.zero,
            allowedConnections = new System.Collections.Generic.List<ConnectionType>
            {
                ConnectionType.FixedBolt
            }
        });

        return part;
    }

    [Test]
    public void GetPart_ReturnsCorrectPart_WhenPartIdExists()
    {
        var catalog = ScriptableObject.CreateInstance<PartCatalog>();

        var frame = CreatePart("frame_wood", "Wood Frame", PartType.Frame);
        var wheel = CreatePart("wheel_small", "Small Wheel", PartType.Wheel);

        catalog.parts.Add(frame);
        catalog.parts.Add(wheel);
        catalog.Initialize();

        var result = catalog.GetPart("frame_wood");

        Assert.IsNotNull(result);
        Assert.AreEqual("frame_wood", result.partId);
        Assert.AreEqual("Wood Frame", result.displayName);
        Assert.AreEqual(PartType.Frame, result.partType);
    }

    [Test]
    public void HasPart_ReturnsTrue_WhenPartExists()
    {
        var catalog = ScriptableObject.CreateInstance<PartCatalog>();

        var wheel = CreatePart("wheel_small", "Small Wheel", PartType.Wheel);

        catalog.parts.Add(wheel);
        catalog.Initialize();

        Assert.IsTrue(catalog.HasPart("wheel_small"));
    }

    [Test]
    public void HasPart_ReturnsFalse_WhenPartDoesNotExist()
    {
        var catalog = ScriptableObject.CreateInstance<PartCatalog>();

        var wheel = CreatePart("wheel_small", "Small Wheel", PartType.Wheel);

        catalog.parts.Add(wheel);
        catalog.Initialize();

        Assert.IsFalse(catalog.HasPart("rocket_small"));
    }

    [Test]
    public void GetPart_ReturnsNull_WhenPartDoesNotExist()
    {
        var catalog = ScriptableObject.CreateInstance<PartCatalog>();

        var frame = CreatePart("frame_wood", "Wood Frame", PartType.Frame);

        catalog.parts.Add(frame);
        catalog.Initialize();

        LogAssert.Expect(LogType.Error, "Part not found in catalog: rocket_small");

        var result = catalog.GetPart("rocket_small");

        Assert.IsNull(result);
    }

    [Test]
    public void Initialize_LogsWarning_WhenDuplicatePartIdExists()
    {
        var catalog = ScriptableObject.CreateInstance<PartCatalog>();

        var partA = CreatePart("frame_wood", "Wood Frame A", PartType.Frame);
        var partB = CreatePart("frame_wood", "Wood Frame B", PartType.Frame);

        catalog.parts.Add(partA);
        catalog.parts.Add(partB);

        LogAssert.Expect(LogType.Warning, "Duplicate partId found: frame_wood");

        catalog.Initialize();

        var result = catalog.GetPart("frame_wood");

        Assert.IsNotNull(result);
        Assert.AreEqual("Wood Frame A", result.displayName);
    }
}
```

---

## Test File 2: `PartDefinitionValidationTests.cs`

**Path:**

```text
Assets/Tests/EditMode/PartDefinitionValidationTests.cs
```

**Purpose:**

Validates the actual `PartDefinition` assets created in the Unity project.

```csharp
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

        Assert.Greater(
            guids.Length,
            0,
            $"No PartDefinition assets found in {PartsFolder}"
        );

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            PartDefinition part = AssetDatabase.LoadAssetAtPath<PartDefinition>(path);

            Assert.IsNotNull(part, $"Could not load PartDefinition at {path}");

            Assert.IsFalse(
                string.IsNullOrWhiteSpace(part.partId),
                $"{path} has empty partId"
            );

            Assert.IsFalse(
                string.IsNullOrWhiteSpace(part.displayName),
                $"{part.partId} has empty displayName"
            );

            Assert.IsNotNull(
                part.buildPrefab,
                $"{part.partId} is missing buildPrefab"
            );

            Assert.IsNotNull(
                part.simulationPrefab,
                $"{part.partId} is missing simulationPrefab"
            );

            Assert.Greater(
                part.mass,
                0f,
                $"{part.partId} mass must be greater than 0"
            );

            Assert.Greater(
                part.size.x,
                0f,
                $"{part.partId} size.x must be greater than 0"
            );

            Assert.Greater(
                part.size.y,
                0f,
                $"{part.partId} size.y must be greater than 0"
            );

            Assert.IsNotNull(
                part.anchors,
                $"{part.partId} anchors list is null"
            );

            Assert.Greater(
                part.anchors.Count,
                0,
                $"{part.partId} must have at least one anchor"
            );

            foreach (AnchorDefinition anchor in part.anchors)
            {
                Assert.IsFalse(
                    string.IsNullOrWhiteSpace(anchor.anchorId),
                    $"{part.partId} has an anchor with empty anchorId"
                );

                Assert.IsNotNull(
                    anchor.allowedConnections,
                    $"{part.partId}.{anchor.anchorId} allowedConnections is null"
                );

                Assert.Greater(
                    anchor.allowedConnections.Count,
                    0,
                    $"{part.partId}.{anchor.anchorId} must allow at least one connection type"
                );
            }
        }
    }

    [Test]
    public void PartIds_AreUnique()
    {
        string[] guids = AssetDatabase.FindAssets("t:PartDefinition", new[] { PartsFolder });

        var seen = new System.Collections.Generic.HashSet<string>();

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            PartDefinition part = AssetDatabase.LoadAssetAtPath<PartDefinition>(path);

            Assert.IsNotNull(part, $"Could not load PartDefinition at {path}");

            Assert.IsFalse(
                seen.Contains(part.partId),
                $"Duplicate partId found: {part.partId} at {path}"
            );

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

        var foundPartIds = new System.Collections.Generic.HashSet<string>();

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            PartDefinition part = AssetDatabase.LoadAssetAtPath<PartDefinition>(path);

            if (part != null && !string.IsNullOrWhiteSpace(part.partId))
            {
                foundPartIds.Add(part.partId);
            }
        }

        foreach (string requiredPartId in requiredPartIds)
        {
            Assert.IsTrue(
                foundPartIds.Contains(requiredPartId),
                $"Missing required MVP part: {requiredPartId}"
            );
        }
    }
}
```

---

## Test File 3: `LevelAvailablePartsValidationTests.cs`

**Path:**

```text
Assets/Tests/EditMode/LevelAvailablePartsValidationTests.cs
```

**Purpose:**

Verifies that level JSON files reference only parts that exist in the `PartCatalog`.

This test assumes:

```text
Assets/Data/Parts/PartCatalog.asset
Assets/Data/Levels/level_001.json
```

```csharp
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

        Assert.IsNotNull(
            catalog,
            $"PartCatalog not found at {CatalogPath}"
        );

        catalog.Initialize();

        TextAsset levelText = AssetDatabase.LoadAssetAtPath<TextAsset>(LevelPath);

        Assert.IsNotNull(
            levelText,
            $"Level JSON not found at {LevelPath}"
        );

        LevelDefinition level = JsonUtility.FromJson<LevelDefinition>(levelText.text);

        Assert.IsNotNull(level, "Failed to parse level JSON");

        Assert.IsNotNull(
            level.availableParts,
            "level.availableParts is null"
        );

        Assert.Greater(
            level.availableParts.Count,
            0,
            "level.availableParts must contain at least one part"
        );

        foreach (var availablePart in level.availableParts)
        {
            Assert.IsFalse(
                string.IsNullOrWhiteSpace(availablePart.partId),
                "Level contains availablePart with empty partId"
            );

            Assert.IsTrue(
                catalog.HasPart(availablePart.partId),
                $"Level references missing partId: {availablePart.partId}"
            );

            Assert.Greater(
                availablePart.count,
                0,
                $"Level part {availablePart.partId} count must be greater than 0"
            );
        }
    }
}
```

---

## Required LevelDefinition Support

The `LevelAvailablePartsValidationTests` test assumes that the level model includes available parts.

If not already present from Ticket 1, add or verify the following:

```csharp
using System;
using System.Collections.Generic;

[Serializable]
public class AvailablePartDefinition
{
    public string partId;
    public int count;
}
```

And in `LevelDefinition`:

```csharp
public List<AvailablePartDefinition> availableParts;
```

---

# Manual Test Checklist

In addition to automated tests, manually verify the following in Unity.

## Catalog Manual Check

1. Open `Assets/Data/Parts/PartCatalog.asset`.
2. Confirm it contains exactly these six MVP parts:
   - `core_cockpit`
   - `frame_wood`
   - `wheel_small`
   - `engine_basic`
   - `rocket_small`
   - `balloon_small`
3. Confirm there are no empty slots in the catalog list.

## Part Asset Manual Check

For each `PartDefinition` asset:

1. Confirm `partId` is correct and lowercase snake_case.
2. Confirm `displayName` is readable.
3. Confirm `partType` is correct.
4. Confirm `buildPrefab` is assigned.
5. Confirm `simulationPrefab` is assigned.
6. Confirm `mass > 0`.
7. Confirm `size.x > 0` and `size.y > 0`.
8. Confirm at least one anchor is defined.
9. Confirm every anchor has an `anchorId`.
10. Confirm every anchor has at least one allowed connection type.

## Level Manual Check

1. Open `Assets/Data/Levels/level_001.json`.
2. Check the `availableParts` array.
3. Confirm every listed `partId` exists in `PartCatalog.asset`.
4. Run the game scene.
5. Confirm there are no missing part errors in the Console.

---

# Common Failure Cases

## Test fails: `No PartDefinition assets found`

Check that assets are saved under:

```text
Assets/Data/Parts
```

and that they were created from:

```text
Create → Junkyard Journey → Part Definition
```

---

## Test fails: `Missing required MVP part`

Check the exact `partId` string. It must match exactly:

```text
core_cockpit
frame_wood
wheel_small
engine_basic
rocket_small
balloon_small
```

---

## Test fails: missing prefab

Assign a placeholder build prefab and simulation prefab to the relevant `PartDefinition`.

---

## Test fails: anchor count is zero

Add at least one anchor to the part.

For example, `wheel_small` should have:

```text
anchorId: axle
anchorType: Axle
allowedConnections: AxleBolt
```

---

## Test fails: level references missing part

Either:

1. Add the missing part to the catalog, or
2. Change `level_001.json` to use a valid part ID.

---

# Definition of Done

Ticket 2 is done when:

- All part system scripts compile.
- All six MVP part assets exist.
- All placeholder build prefabs exist.
- All placeholder simulation prefabs exist.
- `PartCatalog.asset` references all six MVP part assets.
- Level available parts resolve through `PartCatalog`.
- All EditMode tests pass.
- Manual checks show no missing part IDs, missing prefabs, empty anchors, or duplicate part IDs.

---

# Next Ticket

After Ticket 2 is complete, move to:

## Ticket 3: Build Inventory UI

**Goal:** Read the loaded level's `availableParts`, resolve them through the `PartCatalog`, and display selectable part buttons/icons for the player.

Ticket 3 should not yet implement full dragging/snapping. It should only prove that the level inventory can appear in the UI based on JSON + catalog data.

