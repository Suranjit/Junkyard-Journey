using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PartCatalogTests
{
    private PartDefinition CreatePart(string partId, string displayName, PartType type)
    {
        PartDefinition part = ScriptableObject.CreateInstance<PartDefinition>();
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
            allowedConnections = new List<ConnectionType> { ConnectionType.FixedBolt }
        });
        return part;
    }

    [Test]
    public void GetPart_ReturnsCorrectPart_WhenPartIdExists()
    {
        PartCatalog catalog = ScriptableObject.CreateInstance<PartCatalog>();
        catalog.parts.Add(CreatePart("frame_wood", "Wood Frame", PartType.Frame));
        catalog.parts.Add(CreatePart("wheel_small", "Small Wheel", PartType.Wheel));
        catalog.Initialize();

        PartDefinition result = catalog.GetPart("frame_wood");

        Assert.IsNotNull(result);
        Assert.AreEqual("frame_wood", result.partId);
        Assert.AreEqual("Wood Frame", result.displayName);
        Assert.AreEqual(PartType.Frame, result.partType);
    }

    [Test]
    public void HasPart_ReturnsTrue_WhenPartExists()
    {
        PartCatalog catalog = ScriptableObject.CreateInstance<PartCatalog>();
        catalog.parts.Add(CreatePart("wheel_small", "Small Wheel", PartType.Wheel));
        catalog.Initialize();

        Assert.IsTrue(catalog.HasPart("wheel_small"));
    }

    [Test]
    public void HasPart_ReturnsFalse_WhenPartDoesNotExist()
    {
        PartCatalog catalog = ScriptableObject.CreateInstance<PartCatalog>();
        catalog.parts.Add(CreatePart("wheel_small", "Small Wheel", PartType.Wheel));
        catalog.Initialize();

        Assert.IsFalse(catalog.HasPart("rocket_small"));
    }

    [Test]
    public void GetPart_ReturnsNull_WhenPartDoesNotExist()
    {
        PartCatalog catalog = ScriptableObject.CreateInstance<PartCatalog>();
        catalog.parts.Add(CreatePart("frame_wood", "Wood Frame", PartType.Frame));
        catalog.Initialize();

        LogAssert.Expect(LogType.Error, "Part not found in catalog: rocket_small");

        PartDefinition result = catalog.GetPart("rocket_small");

        Assert.IsNull(result);
    }

    [Test]
    public void Initialize_LogsWarning_WhenDuplicatePartIdExists()
    {
        PartCatalog catalog = ScriptableObject.CreateInstance<PartCatalog>();
        catalog.parts.Add(CreatePart("frame_wood", "Wood Frame A", PartType.Frame));
        catalog.parts.Add(CreatePart("frame_wood", "Wood Frame B", PartType.Frame));

        LogAssert.Expect(LogType.Warning, "Duplicate partId found: frame_wood");

        catalog.Initialize();

        PartDefinition result = catalog.GetPart("frame_wood");

        Assert.IsNotNull(result);
        Assert.AreEqual("Wood Frame A", result.displayName);
    }

    [Test]
    public void Initialize_SkipsNullParts_WithWarning()
    {
        PartCatalog catalog = ScriptableObject.CreateInstance<PartCatalog>();
        catalog.parts.Add(null);
        catalog.parts.Add(CreatePart("frame_wood", "Wood Frame", PartType.Frame));

        LogAssert.Expect(LogType.Warning, "Null PartDefinition found in PartCatalog.");

        catalog.Initialize();

        Assert.IsTrue(catalog.HasPart("frame_wood"));
    }

    [Test]
    public void GetPart_InitializesLookup_WhenNotYetInitialized()
    {
        PartCatalog catalog = ScriptableObject.CreateInstance<PartCatalog>();
        catalog.parts.Add(CreatePart("engine_basic", "Basic Engine", PartType.Engine));

        PartDefinition result = catalog.GetPart("engine_basic");

        Assert.IsNotNull(result);
        Assert.AreEqual("engine_basic", result.partId);
    }
}
