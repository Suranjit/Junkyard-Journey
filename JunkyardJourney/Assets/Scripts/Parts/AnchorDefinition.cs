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
