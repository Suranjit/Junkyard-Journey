# Junkyard Journey — Game Implementation Plan v0.1

## 1. Product Summary

### Game Title

**Junkyard Journey**

### Genre

2D physics-based contraption puzzle game.

### Target Platform

- Primary: **iOS**
- Future: **Android**
- Engine: **Unity 2D**
- Orientation: **Landscape**

### Business Model

- Initial release: **Free**
- Monetization: Not included in MVP
- Future possibilities: cosmetic skins, level packs, optional ads, daily challenges, or premium unlocks

### Core Fantasy

The player is an **alien mechanic** trapped in a gigantic junkyard on Earth. To escape, they must build ridiculous, unstable, junk-powered contraptions using parts like wheels, engines, rockets, balloons, gliders, gears, and pulleys.

The game is about experimenting, failing hilariously, improving the design, and eventually escaping one junkyard section at a time.

### One-Line Pitch

**Junkyard Journey** is a cartoony 2D physics puzzle game where an alien mechanic bolts together janky machines from scrap parts to escape a massive junkyard.

---

## 2. Locked Product Decisions

| Area | Decision |
|---|---|
| Main character | Alien mechanic |
| Orientation | Landscape |
| Build system | Free-form placement |
| Connection system | Visible bolts and anchors, like 2D LEGO junk |
| Part overlap | Allowed, within reason, to preserve janky cartoon style |
| Run control | No player control during simulation |
| Pre-launch control | Engines and rockets can use configurable timers |
| Failure conditions | Vehicle breaks, gets stuck, falls out of bounds, or core is destroyed |
| First world | Junkyard only |
| Story wrapper | Alien mechanic is trapped in junkyard and trying to escape |
| Initial price | Free |
| Level generation | Programmatic, config/spec-driven, AI-assisted/generated |

---

## 3. Game Identity

### Tone

The tone should be:

- Cartoony
- Chaotic
- Weird sci-fi junkyard comedy
- Janky and expressive
- Funny when things fail
- Original visual style, with energetic adult-animation influence but not copying any specific show

### Main Character

The player character is an **alien mechanic**.

Possible working character details:

- Crash-landed on Earth
- Ship parts scattered across a giant junkyard
- Must build escape machines from scrap
- Speaks through funny short text bubbles
- Can serve as the “core cockpit” of the contraption

### Story Premise

The alien mechanic crash-lands into an enormous junkyard. Their spaceship is wrecked, the exit is far away, and the only way out is to assemble junk into makeshift machines.

Each level represents another route through the junkyard.

### Story Structure for MVP

World 1: **The Junkyard**

Suggested level zones:

1. Scrap Yard Entrance — tutorial levels
2. Rust Hill — slopes and uneven terrain
3. Crate Canyon — ramps and gaps
4. Balloon Basin — lift-based puzzles
5. Rocket Dump — timed thrust puzzles
6. Crusher Row — early hazards, likely post-MVP
7. Escape Gate — final world challenge

---

## 4. Core Gameplay Loop

```text
Choose level
↓
Inspect terrain, obstacles, goal, available parts
↓
Build free-form contraption using anchors and bolts
↓
Configure timers for engines/rockets if needed
↓
Press Launch
↓
Watch simulation with no player control
↓
Win, fail, break, get stuck, or fall
↓
Return to build mode and improve design
↓
Earn stars/scrap/unlocks
↓
Continue the escape journey
```

---

## 5. MVP Scope

### MVP Goal

Prove that the core loop is fun:

> Build a janky machine, launch it, watch it succeed or fail, then revise it.

### MVP Includes

- Landscape Unity 2D project
- Config-driven level loading from JSON
- Free-form build mode
- Visible anchors and bolts
- Slight part overlap allowed
- Conversion from build mode to physics simulation
- No run-time player control
- Pre-launch timers for engine/rocket parts
- Basic failure detection
- Basic win detection
- Local level progression
- 10–15 junkyard levels
- Offline AI-generated level specs validated before shipping

### MVP Parts

1. **Alien Core Cockpit**
   - Required part
   - Represents alien mechanic
   - If destroyed, detached, or lost, player fails

2. **Frame Wood / Scrap Beam**
   - Basic structure
   - Has multiple anchor points

3. **Wheel Small**
   - Provides rolling movement
   - Uses axle anchor

4. **Engine Basic**
   - Powers connected wheels
   - Can use start delay and duration

5. **Rocket Small**
   - Applies thrust along its facing direction
   - Uses configurable start delay and burn duration

6. **Balloon Small**
   - Applies constant upward lift
   - Useful for lift-assist puzzles

### Post-MVP Parts

7. **Glider Wing**
   - Passive lift based on speed and angle

8. **Gear**
   - Transfers and modifies torque
   - Start with simplified gear logic, not tooth-level physical simulation

9. **Pulley**
   - Rope/constraint-based mechanics
   - Start with simplified distance joints

---

## 6. Unity Folder Structure

Recommended project layout:

```text
JunkyardJourney/
  UnityProject/
    Assets/
      Art/
        Characters/
          AlienMechanic/
        Parts/
          Frames/
          Wheels/
          Engines/
          Rockets/
          Balloons/
          Gliders/
          Gears/
          Pulleys/
        Environments/
          Junkyard/
        UI/
        Effects/
        Placeholder/

      Audio/
        Music/
        SFX/
          UI/
          Physics/
          Engines/
          Rockets/
          Breaks/

      Materials/
        Physics2D/
        Visuals/

      Prefabs/
        Core/
        Parts/
          BuildPrefabs/
          SimulationPrefabs/
        Level/
          Terrain/
          Obstacles/
          Hazards/
          Pickups/
          Goal/
          StartZone/
        UI/
        Effects/

      Resources/
        Levels/
          world_01_junkyard/
        PartDefinitions/

      Scenes/
        Boot.unity
        MainMenu.unity
        LevelSelect.unity
        Game.unity
        DevSandbox.unity

      ScriptableObjects/
        Parts/
        PhysicsMaterials/
        GameConfig/

      Scripts/
        Core/
          GameManager.cs
          GameState.cs
          ServiceLocator.cs
          Constants.cs

        Data/
          SerializableVector2.cs
          SerializableVector3.cs
          JsonUtilityWrapper.cs

        Levels/
          LevelDefinition.cs
          LevelLoader.cs
          LevelValidator.cs
          LevelBuilder.cs
          LevelManager.cs
          TerrainBuilder2D.cs
          StartZoneSpawner.cs
          GoalZoneSpawner.cs
          ObstacleSpawner.cs
          HazardSpawner.cs
          PickupSpawner.cs
          LevelProgressionManager.cs

        BuildSystem/
          BuildModeManager.cs
          FreeformPlacementManager.cs
          BuildPart.cs
          AnchorPoint.cs
          AnchorSnapSystem.cs
          BoltConnection.cs
          BoltManager.cs
          BuildInventory.cs
          BuildInventoryUI.cs
          BuildSerializer.cs
          BuildValidator.cs
          PartPlacementPreview.cs
          PartRotationController.cs
          PartConfigurationPanel.cs

        Parts/
          PartDefinition.cs
          PartType.cs
          PartCatalog.cs
          PartInstanceData.cs
          PartSettings.cs
          EngineSettings.cs
          RocketSettings.cs
          WheelPart.cs
          EnginePart.cs
          RocketPart.cs
          BalloonPart.cs
          GliderPart.cs
          AlienCorePart.cs
          BreakablePart.cs

        Simulation/
          SimulationManager.cs
          VehicleAssembler.cs
          SimulationPart.cs
          BoltJointFactory.cs
          PhysicsConnectionGraph.cs
          FailureDetector.cs
          StuckDetector.cs
          OutOfBoundsDetector.cs
          CoreIntegrityDetector.cs
          GoalDetector.cs
          SimulationResult.cs
          TemplateVehicleSolver.cs

        Camera/
          CameraController.cs
          BuildCameraController.cs
          SimulationCameraController.cs

        UI/
          MainMenuUI.cs
          LevelSelectUI.cs
          GameHUD.cs
          WinScreenUI.cs
          FailScreenUI.cs
          PauseMenuUI.cs
          TimerSliderUI.cs
          PartPaletteButton.cs

        Save/
          SaveManager.cs
          PlayerProgressData.cs
          LevelSaveData.cs
          BuildSaveData.cs

        Audio/
          AudioManager.cs
          SfxPlayer.cs
          MusicManager.cs

        Effects/
          EffectsManager.cs
          ImpactEffectSpawner.cs
          RocketFlameEffect.cs
          BoltBreakEffect.cs
          DustTrailEffect.cs

      Tests/
        EditMode/
          LevelValidationTests.cs
          BuildSerializationTests.cs
          PartCatalogTests.cs
        PlayMode/
          VehicleAssemblyTests.cs
          GoalDetectionTests.cs
          FailureDetectionTests.cs

  tools/
    levelgen/
      README.md
      generate_level.py
      validate_level.py
      repair_level.py
      batch_generate.py
      score_difficulty.py
      schema/
        level_schema_v1.json
      prompts/
        system_prompt.txt
        beginner_level_prompt.txt
        repair_prompt.txt
      generated/
        raw/
        valid/
        invalid/
        approved/

  docs/
    product_brief.md
    implementation_plan.md
    level_spec_v1.md
    art_direction.md
    technical_architecture.md
```

---

## 7. C# Class List

### Core

#### `GameManager.cs`

Owns high-level game flow.

Responsibilities:

- Bootstraps the game
- Tracks current game state
- Switches between menu, build, simulation, win, and fail states
- Coordinates managers

#### `GameState.cs`

Enum for major game states.

```csharp
public enum GameState
{
    Boot,
    MainMenu,
    LevelSelect,
    LoadingLevel,
    BuildMode,
    Simulating,
    Win,
    Fail,
    Paused
}
```

---

### Level System

#### `LevelDefinition.cs`

Serializable C# representation of the JSON level spec.

Contains:

- Metadata
- Story
- Build system settings
- Simulation settings
- Failure rules
- World bounds
- Start zone
- Goal
- Terrain
- Obstacles
- Available parts
- Star objectives
- Validation hints

#### `LevelLoader.cs`

Loads level JSON from `Resources`, `StreamingAssets`, or future remote source.

MVP recommendation:

- Load from `Resources/Levels`

Later:

- Move to Addressables or backend-delivered JSON

#### `LevelValidator.cs`

Validates loaded level specs before building the scene.

Checks:

- Required fields
- Bounds
- Start/goal validity
- Terrain validity
- Available parts
- Difficulty rules
- Failure rules
- Required capabilities

#### `LevelBuilder.cs`

Builds a playable Unity scene from a valid `LevelDefinition`.

Calls:

- `TerrainBuilder2D`
- `StartZoneSpawner`
- `GoalZoneSpawner`
- `ObstacleSpawner`
- `HazardSpawner`
- `PickupSpawner`
- `BuildInventory`

#### `TerrainBuilder2D.cs`

Creates 2D terrain from polyline points.

MVP implementation:

- `EdgeCollider2D` for physics
- `LineRenderer` for visuals

Later:

- SpriteShape terrain
- Mesh-filled terrain
- Surface-specific physics materials

#### `LevelManager.cs`

Manages current level, progression, and reloads.

Responsibilities:

- Load selected level
- Restart level
- Advance to next level
- Record completion

---

### Build System

#### `BuildModeManager.cs`

Controls entry, exit, and state of build mode.

Responsibilities:

- Enable build UI
- Disable simulation physics
- Track placed parts and bolts
- Initiate launch

#### `FreeformPlacementManager.cs`

Handles free-form dragging, dropping, rotation, and placement.

Responsibilities:

- Pointer/touch input
- Part drag previews
- Rotation gestures or buttons
- Overlap checks
- Build zone checks
- Valid/janky/invalid placement feedback

#### `BuildPart.cs`

Represents a part while in build mode.

Contains:

- Instance ID
- Part definition
- Position
- Rotation
- Anchor points
- Optional settings

#### `AnchorPoint.cs`

Represents an attach point on a build part.

Contains:

- Anchor ID
- Local position
- Anchor type
- Whether occupied
- Visual marker reference

#### `AnchorSnapSystem.cs`

Finds nearby compatible anchors and snaps parts together.

Responsibilities:

- Detect nearest anchor
- Check compatibility
- Show highlight
- Suggest bolt type
- Snap moved part into position

#### `BoltConnection.cs`

Serializable connection between two part anchors.

Contains:

- Bolt ID
- Part A
- Anchor A
- Part B
- Anchor B
- Bolt type
- Break force

#### `BoltManager.cs`

Creates, stores, removes, and visualizes bolts.

Responsibilities:

- Create visible bolt sprite
- Store connection data
- Remove bolts when parts are deleted
- Send bolt list to `VehicleAssembler`

#### `BuildInventory.cs`

Tracks parts available in the current level.

Responsibilities:

- Part counts
- Used counts
- Remaining counts

#### `BuildInventoryUI.cs`

Visual UI for available parts.

Responsibilities:

- Part buttons
- Remaining counts
- Drag creation
- Disabled state when count is exhausted

#### `PartConfigurationPanel.cs`

Allows pre-launch configuration for configurable parts.

MVP settings:

- Engine start delay
- Engine direction
- Engine duration
- Rocket start delay
- Rocket burn duration

---

### Parts

#### `PartDefinition.cs`

ScriptableObject defining part metadata.

Example fields:

```csharp
public class PartDefinition : ScriptableObject
{
    public string partId;
    public string displayName;
    public PartType partType;
    public Sprite icon;
    public GameObject buildPrefab;
    public GameObject simulationPrefab;
    public float mass;
    public Vector2 size;
    public List<AnchorDefinition> anchors;
    public bool canRotate;
    public bool allowOverlap;
    public int defaultDurability;
}
```

#### `PartType.cs`

```csharp
public enum PartType
{
    AlienCore,
    Frame,
    Wheel,
    Engine,
    Rocket,
    Balloon,
    Glider,
    Gear,
    Pulley,
    Weight
}
```

#### `PartCatalog.cs`

Central lookup for all known parts.

Responsibilities:

- Validate part IDs
- Return part definitions
- Support editor/debug lookup

#### `AlienCorePart.cs`

Represents the alien mechanic cockpit/core during simulation.

Responsibilities:

- Track whether core is intact
- Trigger fail on destruction/out-of-bounds
- Serve as camera follow target

#### `WheelPart.cs`

Controls wheel physics.

Responsibilities:

- Configure wheel joint
- Receive motor torque
- Apply friction settings

#### `EnginePart.cs`

Powers connected wheels.

MVP simplification:

- Engine powers all wheels connected in the same contraption graph

Later:

- Engine powers wheels through gears and drivetrain connections

#### `RocketPart.cs`

Applies timed thrust.

Responsibilities:

- Start after delay
- Burn for duration
- Apply force in forward direction
- Trigger flame particles and sound

#### `BalloonPart.cs`

Applies constant upward force.

Responsibilities:

- Add lift during simulation
- Optional pop/damage behavior later

---

### Simulation

#### `SimulationManager.cs`

Controls simulation mode.

Responsibilities:

- Launch build
- Start physics
- Track simulation timer
- Trigger win/fail
- Reset back to build mode

#### `VehicleAssembler.cs`

Converts build-mode parts and bolts into physics-mode objects.

Responsibilities:

- Spawn simulation prefabs
- Copy positions/rotations/settings
- Add Rigidbody2D/Collider2D if needed
- Create joints from bolt connections
- Build physics connection graph

#### `SimulationPart.cs`

Base class for simulation parts.

Contains:

- Instance ID
- Part definition
- Rigidbody2D
- Health/durability
- Connection data

#### `BoltJointFactory.cs`

Creates Unity 2D joints based on bolt type.

MVP joint types:

- Fixed bolt → `FixedJoint2D`
- Axle bolt → `HingeJoint2D` or `WheelJoint2D`
- Breakable bolt → joint with break force

#### `PhysicsConnectionGraph.cs`

Tracks which parts are connected.

Used for:

- Engine-to-wheel power propagation
- Core integrity checks
- Break detection
- Future gear/pulley systems

#### `FailureDetector.cs`

Aggregates all failure rules.

Calls:

- `StuckDetector`
- `OutOfBoundsDetector`
- `CoreIntegrityDetector`

#### `StuckDetector.cs`

Fails if the alien core makes insufficient progress for a configured duration.

#### `OutOfBoundsDetector.cs`

Fails if the alien core exits world bounds.

#### `CoreIntegrityDetector.cs`

Fails if alien core is destroyed or isolated from the vehicle.

#### `GoalDetector.cs`

Detects when the alien core or vehicle enters the goal zone.

---

### Save System

#### `SaveManager.cs`

Handles local persistence.

MVP:

- Completed levels
- Stars earned
- Last build per level

#### `BuildSaveData.cs`

Stores part placements, rotations, settings, and bolts.

#### `PlayerProgressData.cs`

Stores progression and unlocks.

---

## 8. JSON Schema v1

This is a practical first version of the schema. It can be refined as systems are implemented.

File path:

```text
tools/levelgen/schema/level_schema_v1.json
```

```json
{
  "$schema": "https://json-schema.org/draft/2020-12/schema",
  "$id": "https://junkyardjourney.game/schemas/level_schema_v1.json",
  "title": "Junkyard Journey Level Schema v1",
  "type": "object",
  "required": [
    "schemaVersion",
    "levelId",
    "name",
    "theme",
    "difficulty",
    "story",
    "buildSystem",
    "simulationMode",
    "failureRules",
    "worldBounds",
    "startZone",
    "goal",
    "terrain",
    "availableParts",
    "buildConstraints",
    "starObjectives",
    "validationHints"
  ],
  "properties": {
    "schemaVersion": {
      "type": "string",
      "const": "1.0"
    },
    "levelId": {
      "type": "string",
      "pattern": "^[a-z0-9_\\-]+$"
    },
    "name": {
      "type": "string",
      "minLength": 1
    },
    "description": {
      "type": "string"
    },
    "theme": {
      "type": "string",
      "enum": ["junkyard"]
    },
    "difficulty": {
      "type": "integer",
      "minimum": 1,
      "maximum": 10
    },
    "estimatedDurationSeconds": {
      "type": "number",
      "minimum": 1
    },
    "story": {
      "type": "object",
      "required": ["chapter", "introText", "outroText"],
      "properties": {
        "chapter": { "type": "integer", "minimum": 1 },
        "introText": { "type": "string" },
        "outroText": { "type": "string" }
      },
      "additionalProperties": false
    },
    "buildSystem": {
      "type": "object",
      "required": ["placementMode", "connectionMode", "allowVisualOverlap"],
      "properties": {
        "placementMode": { "type": "string", "const": "freeform" },
        "connectionMode": { "type": "string", "const": "anchor_bolts" },
        "allowVisualOverlap": { "type": "boolean" }
      },
      "additionalProperties": false
    },
    "simulationMode": {
      "type": "object",
      "required": ["playerControlDuringRun", "partTimersEnabled", "activationMode"],
      "properties": {
        "playerControlDuringRun": { "type": "boolean", "const": false },
        "partTimersEnabled": { "type": "boolean" },
        "activationMode": {
          "type": "string",
          "enum": ["automatic_on_launch", "timed_preconfigured"]
        }
      },
      "additionalProperties": false
    },
    "failureRules": {
      "type": "object",
      "required": [
        "failOnCoreDestroyed",
        "failOnStuck",
        "failOnOutOfBounds",
        "stuckSeconds",
        "minimumProgressDistance"
      ],
      "properties": {
        "failOnCoreDestroyed": { "type": "boolean" },
        "failOnStuck": { "type": "boolean" },
        "failOnOutOfBounds": { "type": "boolean" },
        "stuckSeconds": { "type": "number", "minimum": 1 },
        "minimumProgressDistance": { "type": "number", "minimum": 0.1 }
      },
      "additionalProperties": false
    },
    "worldBounds": {
      "$ref": "#/$defs/worldBounds"
    },
    "startZone": {
      "$ref": "#/$defs/zone"
    },
    "goal": {
      "type": "object",
      "required": ["type", "position", "width", "height"],
      "properties": {
        "type": { "type": "string", "enum": ["reach_zone"] },
        "position": { "$ref": "#/$defs/vector2" },
        "width": { "type": "number", "minimum": 0.1 },
        "height": { "type": "number", "minimum": 0.1 }
      },
      "additionalProperties": false
    },
    "terrain": {
      "type": "array",
      "minItems": 1,
      "items": {
        "type": "object",
        "required": ["id", "type", "points", "surface"],
        "properties": {
          "id": { "type": "string" },
          "type": { "type": "string", "const": "polyline" },
          "points": {
            "type": "array",
            "minItems": 2,
            "items": { "$ref": "#/$defs/vector2" }
          },
          "surface": {
            "type": "string",
            "enum": ["scrap_dirt", "metal", "rubber", "mud"]
          }
        },
        "additionalProperties": false
      }
    },
    "obstacles": {
      "type": "array",
      "items": { "$ref": "#/$defs/placedObject" }
    },
    "hazards": {
      "type": "array",
      "items": { "$ref": "#/$defs/placedObject" }
    },
    "pickups": {
      "type": "array",
      "items": { "$ref": "#/$defs/pickup" }
    },
    "availableParts": {
      "type": "array",
      "minItems": 1,
      "items": {
        "type": "object",
        "required": ["partId", "count"],
        "properties": {
          "partId": { "type": "string" },
          "count": { "type": "integer", "minimum": 1 }
        },
        "additionalProperties": false
      }
    },
    "buildConstraints": {
      "type": "object",
      "required": ["maxParts", "maxWidth", "maxHeight"],
      "properties": {
        "maxParts": { "type": "integer", "minimum": 1 },
        "maxWidth": { "type": "number", "minimum": 1 },
        "maxHeight": { "type": "number", "minimum": 1 }
      },
      "additionalProperties": false
    },
    "physicsSettings": {
      "type": "object",
      "properties": {
        "gravityScale": { "type": "number" },
        "wind": { "$ref": "#/$defs/vector2" },
        "terrainFriction": { "type": "number", "minimum": 0 },
        "terrainBounciness": { "type": "number", "minimum": 0 }
      },
      "additionalProperties": false
    },
    "starObjectives": {
      "type": "array",
      "minItems": 1,
      "items": {
        "type": "object",
        "required": ["stars", "type"],
        "properties": {
          "stars": { "type": "integer", "minimum": 1, "maximum": 3 },
          "type": {
            "type": "string",
            "enum": ["reach_goal", "finish_under_time", "use_at_most_parts", "collect_scrap"]
          },
          "seconds": { "type": "number", "minimum": 1 },
          "count": { "type": "integer", "minimum": 1 }
        },
        "additionalProperties": false
      }
    },
    "validationHints": {
      "type": "object",
      "required": ["requiredCapabilities", "expectedSolutionTypes", "playerControlDuringRun"],
      "properties": {
        "requiredCapabilities": {
          "type": "array",
          "items": {
            "type": "string",
            "enum": ["rolling", "thrust", "lift_assist", "gliding", "climbing", "stability"]
          }
        },
        "expectedSolutionTypes": {
          "type": "array",
          "items": {
            "type": "string",
            "enum": ["basic_cart", "rocket_cart", "balloon_assisted_cart", "glider_cart"]
          }
        },
        "playerControlDuringRun": {
          "type": "boolean",
          "const": false
        }
      },
      "additionalProperties": false
    }
  },
  "additionalProperties": false,
  "$defs": {
    "vector2": {
      "type": "object",
      "required": ["x", "y"],
      "properties": {
        "x": { "type": "number" },
        "y": { "type": "number" }
      },
      "additionalProperties": false
    },
    "worldBounds": {
      "type": "object",
      "required": ["minX", "maxX", "minY", "maxY"],
      "properties": {
        "minX": { "type": "number" },
        "maxX": { "type": "number" },
        "minY": { "type": "number" },
        "maxY": { "type": "number" }
      },
      "additionalProperties": false
    },
    "zone": {
      "type": "object",
      "required": ["position", "width", "height"],
      "properties": {
        "position": { "$ref": "#/$defs/vector2" },
        "width": { "type": "number", "minimum": 0.1 },
        "height": { "type": "number", "minimum": 0.1 }
      },
      "additionalProperties": false
    },
    "placedObject": {
      "type": "object",
      "required": ["id", "type", "position"],
      "properties": {
        "id": { "type": "string" },
        "type": { "type": "string" },
        "position": { "$ref": "#/$defs/vector2" },
        "rotation": { "type": "number" },
        "size": {
          "type": "object",
          "properties": {
            "width": { "type": "number", "minimum": 0.1 },
            "height": { "type": "number", "minimum": 0.1 }
          },
          "additionalProperties": false
        },
        "isDynamic": { "type": "boolean" }
      },
      "additionalProperties": false
    },
    "pickup": {
      "type": "object",
      "required": ["id", "type", "position", "value"],
      "properties": {
        "id": { "type": "string" },
        "type": { "type": "string", "enum": ["scrap"] },
        "position": { "$ref": "#/$defs/vector2" },
        "value": { "type": "integer", "minimum": 1 }
      },
      "additionalProperties": false
    }
  }
}
```

---

## 9. Sample Level JSON

File path:

```text
Assets/Resources/Levels/world_01_junkyard/junkyard_escape_001.json
```

```json
{
  "schemaVersion": "1.0",
  "levelId": "junkyard_escape_001",
  "name": "Wake Up, Scraphead",
  "description": "The alien mechanic wakes up in a junk pile and needs to build something that rolls.",
  "theme": "junkyard",
  "difficulty": 1,
  "estimatedDurationSeconds": 20,

  "story": {
    "chapter": 1,
    "introText": "Your ship is toast, your tools are missing, and the exit gate is way over there. Bolt some trash together and try to look professional.",
    "outroText": "It rolled. Barely. But barely is still engineering."
  },

  "buildSystem": {
    "placementMode": "freeform",
    "connectionMode": "anchor_bolts",
    "allowVisualOverlap": true
  },

  "simulationMode": {
    "playerControlDuringRun": false,
    "partTimersEnabled": true,
    "activationMode": "timed_preconfigured"
  },

  "failureRules": {
    "failOnCoreDestroyed": true,
    "failOnStuck": true,
    "failOnOutOfBounds": true,
    "stuckSeconds": 8,
    "minimumProgressDistance": 1.0
  },

  "worldBounds": {
    "minX": -10,
    "maxX": 90,
    "minY": -25,
    "maxY": 35
  },

  "startZone": {
    "position": { "x": 0, "y": 3 },
    "width": 12,
    "height": 8
  },

  "goal": {
    "type": "reach_zone",
    "position": { "x": 75, "y": 3 },
    "width": 8,
    "height": 8
  },

  "terrain": [
    {
      "id": "main_ground",
      "type": "polyline",
      "points": [
        { "x": -10, "y": 0 },
        { "x": 20, "y": 0 },
        { "x": 40, "y": 2 },
        { "x": 60, "y": 0 },
        { "x": 90, "y": 0 }
      ],
      "surface": "scrap_dirt"
    }
  ],

  "obstacles": [
    {
      "id": "crate_001",
      "type": "crate",
      "position": { "x": 48, "y": 3 },
      "rotation": 0,
      "size": { "width": 2, "height": 2 },
      "isDynamic": true
    }
  ],

  "hazards": [],

  "pickups": [
    {
      "id": "scrap_001",
      "type": "scrap",
      "position": { "x": 35, "y": 4 },
      "value": 10
    }
  ],

  "availableParts": [
    { "partId": "alien_core_cockpit", "count": 1 },
    { "partId": "frame_wood", "count": 8 },
    { "partId": "wheel_small", "count": 4 },
    { "partId": "engine_basic", "count": 1 }
  ],

  "buildConstraints": {
    "maxParts": 12,
    "maxWidth": 10,
    "maxHeight": 7
  },

  "physicsSettings": {
    "gravityScale": 1.0,
    "wind": { "x": 0, "y": 0 },
    "terrainFriction": 0.8,
    "terrainBounciness": 0.05
  },

  "starObjectives": [
    { "stars": 1, "type": "reach_goal" },
    { "stars": 2, "type": "finish_under_time", "seconds": 20 },
    { "stars": 3, "type": "use_at_most_parts", "count": 8 }
  ],

  "validationHints": {
    "requiredCapabilities": ["rolling"],
    "expectedSolutionTypes": ["basic_cart"],
    "playerControlDuringRun": false
  }
}
```

---

## 10. First 10 Engineering Tickets

### Ticket 1 — Create Unity 2D Project Foundation

**Goal:** Set up the base Unity project for iOS-first 2D development.

Tasks:

- Create Unity 2D project
- Set orientation to landscape
- Create base scenes: `Boot`, `MainMenu`, `LevelSelect`, `Game`, `DevSandbox`
- Create folder structure
- Add Git repository
- Add `.gitignore` for Unity
- Configure iOS build target

Acceptance criteria:

- Project opens cleanly
- Scenes exist
- Game scene can run in editor
- iOS build target is configured

---

### Ticket 2 — Implement LevelDefinition Data Model

**Goal:** Create C# serializable classes matching JSON schema v1.

Tasks:

- Create `LevelDefinition.cs`
- Create nested definition classes:
  - `StoryDefinition`
  - `BuildSystemDefinition`
  - `SimulationModeDefinition`
  - `FailureRulesDefinition`
  - `WorldBoundsDefinition`
  - `ZoneDefinition`
  - `GoalDefinition`
  - `TerrainDefinition`
  - `AvailablePartDefinition`
  - `BuildConstraintsDefinition`
  - `PhysicsSettingsDefinition`
  - `StarObjectiveDefinition`
  - `ValidationHintsDefinition`
- Create `SerializableVector2.cs`

Acceptance criteria:

- Sample JSON can deserialize into `LevelDefinition`
- All required fields are represented

---

### Ticket 3 — Implement LevelLoader

**Goal:** Load level JSON from `Resources/Levels`.

Tasks:

- Create `LevelLoader.cs`
- Load TextAsset by level ID/path
- Deserialize JSON to `LevelDefinition`
- Add error handling for missing files and invalid JSON
- Add debug logging

Acceptance criteria:

- `junkyard_escape_001.json` loads successfully
- Invalid level ID reports clear error

---

### Ticket 4 — Implement LevelValidator v1

**Goal:** Validate level data before building the scene.

Tasks:

- Create `LevelValidator.cs`
- Validate required fields
- Validate world bounds
- Validate start and goal inside world bounds
- Validate terrain has at least two points
- Validate part IDs against `PartCatalog`
- Validate player control is false
- Validate theme is junkyard
- Validate difficulty range

Acceptance criteria:

- Valid sample level passes
- Invalid sample level returns useful error list
- Game scene refuses to build invalid level

---

### Ticket 5 — Build Terrain From JSON

**Goal:** Generate level terrain from polyline JSON.

Tasks:

- Create `TerrainBuilder2D.cs`
- Spawn terrain GameObject
- Add `EdgeCollider2D`
- Add `LineRenderer` for visual placeholder
- Apply basic physics material based on surface type

Acceptance criteria:

- Terrain appears in scene from JSON points
- Vehicle/physics objects collide with terrain
- Editing JSON terrain points changes generated terrain

---

### Ticket 6 — Spawn Start Zone, Goal Zone, and Camera Bounds

**Goal:** Build basic playable level layout from JSON.

Tasks:

- Create `StartZoneSpawner.cs`
- Create `GoalZoneSpawner.cs`
- Add trigger collider for goal
- Add simple goal flag/portal placeholder
- Configure camera bounds from world bounds

Acceptance criteria:

- Start zone appears
- Goal zone appears
- Goal trigger can detect alien core entry

---

### Ticket 7 — Create PartDefinition and PartCatalog

**Goal:** Create data-driven part definitions.

Tasks:

- Create `PartDefinition.cs` ScriptableObject
- Create `PartType.cs`
- Create `PartCatalog.cs`
- Create initial part definitions:
  - `alien_core_cockpit`
  - `frame_wood`
  - `wheel_small`
  - `engine_basic`
- Define anchor points for each part

Acceptance criteria:

- PartCatalog can look up part definitions by ID
- LevelValidator can confirm available part IDs are valid

---

### Ticket 8 — Implement Build Inventory UI

**Goal:** Show available parts for the current level.

Tasks:

- Create `BuildInventory.cs`
- Create `BuildInventoryUI.cs`
- Create part palette buttons
- Show icon and count
- Disable button when count reaches zero

Acceptance criteria:

- Part palette matches level JSON availableParts
- Counts decrease when part is placed
- Counts increase when part is removed

---

### Ticket 9 — Implement Free-Form Part Placement

**Goal:** Allow player to place, drag, rotate, and delete parts in build mode.

Tasks:

- Create `BuildModeManager.cs`
- Create `FreeformPlacementManager.cs`
- Create `BuildPart.cs`
- Support drag from palette
- Support dragging placed parts
- Support rotation button
- Support delete button
- Enforce build zone bounds
- Allow mild overlap

Acceptance criteria:

- Player can place alien core, frame, wheels, and engine
- Player can move and rotate parts
- Player can delete parts
- Parts stay within build zone

---

### Ticket 10 — Implement Anchor Snap and Visible Bolts v1

**Goal:** Connect parts using visible anchors and bolts.

Tasks:

- Create `AnchorPoint.cs`
- Create `AnchorSnapSystem.cs`
- Create `BoltConnection.cs`
- Create `BoltManager.cs`
- Show anchor markers while dragging
- Highlight compatible nearby anchors
- Snap part when dropped near compatible anchor
- Create visible bolt sprite
- Store connection data

Acceptance criteria:

- Wheel axle can snap to frame anchor
- Frame can snap to frame
- Engine can snap to frame
- Visible bolts appear after connection
- Bolt data is available to `VehicleAssembler`

---

## 11. AI Level-Generation Pipeline

### Guiding Principle

GenAI should generate **candidate level specs**, not Unity scenes directly.

Unity should only consume specs that have passed deterministic validation.

### Pipeline Overview

```text
Prompt + schema + design constraints
↓
GenAI generates raw JSON level candidate
↓
JSON parse validation
↓
JSON schema validation
↓
Rule-based gameplay validation
↓
Part/capability validation
↓
Optional repair loop
↓
Template vehicle simulation validation
↓
Difficulty scoring
↓
Human review/playtest
↓
Approved level JSON shipped with game
```

### Recommended MVP Approach

Use **offline AI-assisted generation** during development.

Do not generate levels live inside the iOS app for MVP.

Reasons:

- Lower complexity
- No backend needed initially
- No runtime AI cost
- Easier App Store review
- Easier to guarantee quality
- Easier to playtest levels before release

### Tooling Folder

```text
tools/levelgen/
  generate_level.py
  validate_level.py
  repair_level.py
  batch_generate.py
  score_difficulty.py
  schema/
    level_schema_v1.json
  prompts/
    system_prompt.txt
    beginner_level_prompt.txt
    repair_prompt.txt
  generated/
    raw/
    valid/
    invalid/
    approved/
```

### Generation Script Responsibilities

#### `generate_level.py`

- Load prompt template
- Insert desired difficulty, part list, and archetype
- Call GenAI model
- Save raw JSON response

#### `validate_level.py`

- Parse JSON
- Validate against JSON schema
- Apply custom gameplay rules
- Validate known part IDs
- Validate required capabilities
- Save to `valid` or `invalid`

#### `repair_level.py`

- Take invalid level and validation errors
- Ask GenAI to return corrected JSON
- Re-run validation

#### `batch_generate.py`

- Generate many candidates
- Validate all candidates
- Produce summary report

#### `score_difficulty.py`

- Estimate difficulty based on:
  - Terrain length
  - Slope complexity
  - Gaps
  - Obstacles
  - Hazards
  - Part constraints
  - Required capabilities

### Prompt Rules

Every generated level must obey:

- Theme is `junkyard`
- Placement mode is `freeform`
- Connection mode is `anchor_bolts`
- Visual overlap is allowed
- Player control during run is false
- Part timers are enabled
- Level includes story intro/outro text
- Level uses known part IDs only
- Level has exactly one start zone and one goal
- Level must be solvable without real-time input
- Difficulty must match constraints
- Easy levels should not include hazards

### Example Beginner Prompt

```text
You are generating a level for a 2D physics puzzle game called Junkyard Journey.

The player is an alien mechanic trapped in a giant cartoon junkyard. The player builds janky machines from scrap parts to escape.

Return only valid JSON matching Level Schema v1.

Rules:
- theme must be "junkyard"
- difficulty must be 1
- placementMode must be "freeform"
- connectionMode must be "anchor_bolts"
- allowVisualOverlap must be true
- playerControlDuringRun must be false
- partTimersEnabled must be true
- activationMode must be "timed_preconfigured"
- no hazards
- terrain length between 70 and 95 units
- terrain should be mostly flat with one small bump
- available parts must include:
  - alien_core_cockpit x1
  - frame_wood x8
  - wheel_small x4
  - engine_basic x1
- requiredCapabilities must include "rolling"
- expectedSolutionTypes must include "basic_cart"
- include funny but short introText and outroText in the voice of a chaotic alien mechanic story
```

### Validation Rules v1

Structural checks:

- Required fields exist
- `schemaVersion` is `1.0`
- `theme` is `junkyard`
- `playerControlDuringRun` is `false`
- Start and goal are inside world bounds
- Terrain has at least two points
- Available parts are known
- Star objectives are valid

Gameplay checks:

- Difficulty 1 cannot contain hazards
- Difficulty 1 must include rolling capability
- Rolling levels must include at least:
  - `alien_core_cockpit`
  - `frame_wood`
  - `wheel_small`
  - `engine_basic`
- Goal must be to the right of start
- Level length must match difficulty range
- Start zone must be near terrain surface
- Goal zone must be near terrain surface
- Terrain slope must not exceed difficulty-specific limit

Capability checks:

- `rolling` requires wheels and engine
- `thrust` requires rocket
- `lift_assist` requires balloon
- `gliding` requires glider

### Template Solver v1

After core simulation exists, create automated template vehicles:

1. Basic cart
2. Rocket cart
3. Balloon-assisted cart
4. Glider cart

Validation flow:

```text
For each template matching expectedSolutionTypes:
  Build vehicle automatically
  Run simulation
  Check if alien core reaches goal
If any template succeeds:
  Mark level as likely solvable
Else:
  Reject or require human review
```

For MVP, this can come after the first playable Unity prototype.

---

## 12. MVP Milestone Checklist

### Milestone 0 — Project Setup

Goal: Create a clean Unity foundation.

Checklist:

- [ ] Unity 2D project created
- [ ] Git repo initialized
- [ ] Folder structure created
- [ ] Base scenes created
- [ ] Landscape orientation configured
- [ ] iOS build target configured
- [ ] Placeholder art folder created

Exit criteria:

- Project runs in editor
- Empty game scene loads successfully

---

### Milestone 1 — Data-Driven Level Loader

Goal: Load JSON level and create terrain/start/goal.

Checklist:

- [ ] `LevelDefinition.cs`
- [ ] `LevelLoader.cs`
- [ ] `LevelValidator.cs`
- [ ] `TerrainBuilder2D.cs`
- [ ] `StartZoneSpawner.cs`
- [ ] `GoalZoneSpawner.cs`
- [ ] Sample level JSON added
- [ ] Terrain generated from JSON
- [ ] Goal trigger generated from JSON

Exit criteria:

- Editing JSON terrain changes the Unity level
- Invalid level specs fail with clear messages

---

### Milestone 2 — Part Catalog and Build Inventory

Goal: Define parts and show them in build UI.

Checklist:

- [ ] `PartDefinition.cs`
- [ ] `PartType.cs`
- [ ] `PartCatalog.cs`
- [ ] Initial part ScriptableObjects
- [ ] Build inventory from level JSON
- [ ] Part palette UI
- [ ] Counts update when placing/removing parts

Exit criteria:

- Level-specific parts appear in UI with correct counts

---

### Milestone 3 — Free-Form Build Mode

Goal: Place and manipulate parts freely.

Checklist:

- [ ] Drag part from palette
- [ ] Move placed parts
- [ ] Rotate parts
- [ ] Delete parts
- [ ] Build zone bounds
- [ ] Mild overlap allowed
- [ ] Placement preview feedback

Exit criteria:

- Player can assemble a rough static contraption layout

---

### Milestone 4 — Anchors and Visible Bolts

Goal: Connect parts with visible 2D LEGO-like anchor/bolt system.

Checklist:

- [ ] Anchor definitions per part
- [ ] Anchor visual markers
- [ ] Anchor compatibility checks
- [ ] Snap preview
- [ ] Bolt creation
- [ ] Bolt deletion
- [ ] Bolt data serialization

Exit criteria:

- Player can visibly bolt wheels and frames together

---

### Milestone 5 — Build-to-Simulation Conversion

Goal: Convert built contraption into physics objects.

Checklist:

- [ ] `VehicleAssembler.cs`
- [ ] Spawn simulation prefabs from build parts
- [ ] Copy positions and rotations
- [ ] Create joints from bolts
- [ ] Enter simulation mode
- [ ] Reset back to build mode

Exit criteria:

- A bolted contraption falls/moves as a connected physics object

---

### Milestone 6 — Self-Driving Cart

Goal: Make the first complete playable level.

Checklist:

- [ ] Wheel physics
- [ ] Engine part behavior
- [ ] Engine powers connected wheels
- [ ] Alien core follows vehicle
- [ ] Camera follows alien core
- [ ] Goal detection
- [ ] Win screen

Exit criteria:

- Player can build a cart that drives itself to the goal

---

### Milestone 7 — Failure Conditions

Goal: Detect meaningful failures.

Checklist:

- [ ] Out-of-bounds failure
- [ ] Stuck failure
- [ ] Core destroyed/detached failure
- [ ] Fail screen
- [ ] Retry button
- [ ] Return-to-build button

Exit criteria:

- Game reliably fails when vehicle breaks, gets stuck, or falls

---

### Milestone 8 — Rocket and Balloon Strategy

Goal: Add richer contraption solutions.

Checklist:

- [ ] Rocket part
- [ ] Rocket start delay
- [ ] Rocket burn duration
- [ ] Rocket thrust direction from rotation
- [ ] Balloon part
- [ ] Constant lift behavior
- [ ] Timer configuration UI
- [ ] Gap/lift test levels

Exit criteria:

- Player can solve a gap level using timed rocket or balloon assistance

---

### Milestone 9 — AI Level Generation Tool v1

Goal: Generate and validate level specs outside Unity.

Checklist:

- [ ] JSON schema file
- [ ] `generate_level.py`
- [ ] `validate_level.py`
- [ ] `repair_level.py`
- [ ] Prompt templates
- [ ] Batch generation script
- [ ] 20 candidate levels generated
- [ ] 10 valid levels selected

Exit criteria:

- At least 10 AI-assisted JSON levels can be loaded by Unity

---

### Milestone 10 — MVP TestFlight Candidate

Goal: Package first playable build.

Checklist:

- [ ] 10–15 levels
- [ ] Main menu
- [ ] Level select
- [ ] Save progression
- [ ] Basic sound effects
- [ ] Placeholder visual polish
- [ ] App icon placeholder
- [ ] iOS build succeeds
- [ ] Runs on physical iPhone
- [ ] Basic performance profiling

Exit criteria:

- A player can install the game, play through multiple levels, and understand the core loop without developer help

---

## 13. Technical Risks and Mitigations

### Risk 1 — Free-form building becomes too complex

Mitigation:

- Keep anchors simple
- Start with a small set of compatible anchor types
- Allow janky overlap visually but keep anchor rules deterministic

### Risk 2 — Physics feels unpredictable or unfair

Mitigation:

- Use arcade-friendly physics values
- Limit part count early
- Keep levels short
- Use generous goal zones
- Make failures funny and quick to retry

### Risk 3 — AI-generated levels are not solvable

Mitigation:

- Use schema validation
- Use rule validation
- Use template solver later
- Human-review all shipped MVP levels

### Risk 4 — Gears and pulleys become too hard

Mitigation:

- Exclude from MVP
- Build simplified versions later
- Implement gears as connection-graph torque modifiers first
- Implement pulleys as simple distance-joint rope systems first

### Risk 5 — Solo scope grows too large

Mitigation:

- MVP has one world only
- Use placeholder art first
- Avoid live AI generation in app
- Avoid monetization in MVP
- Avoid cloud saves in MVP

---

## 14. Recommended Immediate Next Step

Start with **Milestone 1: Data-Driven Level Loader**.

The first technical target should be:

> Load `junkyard_escape_001.json`, generate terrain/start/goal, and display the level in Unity.

Do not start with rockets, gears, pulleys, or AI generation yet. The level spec loader is the foundation that makes every later system possible.

