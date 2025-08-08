using System.Collections.Generic;
using SticksAndStones.Models;

namespace SticksAndStones.Messages;

public record struct ConnectRequest(Player Player);

public record struct ConnectResponse(Player Player, List<Player> Players, ConnectionInfo ConnectionInfo);