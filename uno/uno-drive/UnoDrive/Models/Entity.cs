using System.Numerics;

namespace UnoDrive.Models;

public record Entity(string Name) : IEqualityOperators<Entity, Entity, bool>;
