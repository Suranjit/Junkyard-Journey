# Ticket 002 — Part Catalog and Placeholder Prefabs

**Branch:** `feat/part-catalog-and-prefabs`  
**Status:** Completed  
**Merged:** 2026-06-05

## Summary

Implemented a ScriptableObject-based part catalog system with data-driven `PartDefinition` assets and placeholder prefabs for all 6 MVP parts. Catalog supports O(1) lookup, lazy initialization, and duplicate/null detection.

## Acceptance Criteria

- [x] `PartType` enum defined: Core, Frame, Wheel, Engine, Rocket, Balloon, Glider, Gear, Pulley
- [x] `ConnectionType` enum defined: FixedBolt, AxleBolt, RopeBolt, GearMesh
- [x] `PartAnchorType` enum defined: Generic, Mount, Axle, EngineOutput, Rope, GearCenter
- [x] `AnchorDefinition` serializable class with anchorId, anchorType, localPosition, allowedConnections
- [x] `PartDefinition` ScriptableObject with all required fields (partId, displayName, partType, buildPrefab, simulationPrefab, mass, size, breakForce, canRotate, allowOverlap, anchors)
- [x] `PartCatalog` ScriptableObject with `Initialize()`, `GetPart(string)`, `HasPart(string)`, dictionary-backed O(1) lookup
- [x] `PartCatalog.Initialize()` logs warning on duplicate partIds (keeps first)
- [x] `PartCatalog.Initialize()` logs warning and skips null PartDefinitions
- [x] `PartCatalog.GetPart()` logs error and returns null for missing partId
- [x] Lazy initialization: `GetPart()` auto-initializes if `Initialize()` not called
- [x] 6 placeholder build prefabs created: CoreCockpit_Build, FrameWood_Build, WheelSmall_Build, EngineBasic_Build, RocketSmall_Build, BalloonSmall_Build
- [x] 6 placeholder simulation prefabs created: CoreCockpit_Sim, FrameWood_Sim, WheelSmall_Sim, EngineBasic_Sim, RocketSmall_Sim, BalloonSmall_Sim
- [x] 6 `PartDefinition` assets created in `Assets/Data/Parts/`: core_cockpit, frame_wood, wheel_small, engine_basic, rocket_small, balloon_small
- [x] `PartCatalog.asset` created in `Assets/Data/Parts/` referencing all 6 parts
- [x] `level_001.json` updated with `availableParts` array (core_cockpit ×1, frame_wood ×8, wheel_small ×4, engine_basic ×1)
- [x] `LevelDefinition` updated with `AvailablePartDefinition` class and `availableParts` list

## Tests (45 total, all passing)

### PartCatalogTests (6 tests)
- `GetPart_ReturnsCorrectPart_WhenPartIdExists`
- `HasPart_ReturnsTrue_WhenPartExists`
- `HasPart_ReturnsFalse_WhenPartDoesNotExist`
- `GetPart_ReturnsNull_WhenPartDoesNotExist`
- `Initialize_LogsWarning_WhenDuplicatePartIdExists`
- `Initialize_SkipsNullParts_WithWarning`
- `GetPart_InitializesLookup_WhenNotYetInitialized`

### PartDefinitionValidationTests (3 tests — real assets)
- `AllPartDefinitions_HaveValidRequiredFields`
- `PartIds_AreUnique`
- `RequiredMvpParts_Exist`

### LevelAvailablePartsValidationTests (1 test — real assets)
- `Level001_AvailableParts_ExistInPartCatalog`

### Carried over from Ticket 1 (35 tests)
- LevelValidationTests (14), LevelSerializationTests (20), LevelLoaderTests (1)
