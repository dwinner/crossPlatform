using System.Collections.Generic;
using SticksAndStones.Models;

namespace SticksAndStones.Messages;

public record struct GetAllPlayersResponse(List<Player> Players);