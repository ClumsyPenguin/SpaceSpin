using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SpaceSpin.Shared.Game;

public record Wave
{
    public List<Vector2> SpawnPositions;
    public double BaseEnemyHealthModifier;
}