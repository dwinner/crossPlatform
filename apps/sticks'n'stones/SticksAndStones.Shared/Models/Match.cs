using System;
using System.Collections.Generic;

namespace SticksAndStones.Models;

public class Match
{
   public Guid Id { get; set; } = Guid.Empty;

   public Guid PlayerOneId { get; set; }

   public int PlayerOneScore { get; set; }

   public Guid PlayerTwoId { get; set; }

   public int PlayerTwoScore { get; set; }

   public Guid NextPlayerId { get; set; }

   public List<int> Sticks { get; set; } = [..new int[24]];

   public List<int> Stones { get; set; } = [..new int[9]];

   // public List<int> Score = [..new int[2]];

   public bool Completed { get; set; }

   public Guid WinnerId { get; set; } = Guid.Empty;

   public static Match New(Guid challenger, Guid opponent) =>
      new()
      {
         Id = Guid.NewGuid(),
         PlayerOneId = opponent,
         PlayerTwoId = challenger,
         NextPlayerId = opponent
      };
}