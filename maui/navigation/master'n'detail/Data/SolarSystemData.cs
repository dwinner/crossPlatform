namespace Astronomy.Data;

public static class SolarSystemData
{
   public static readonly AstronomicalBody Sun = new()
   {
      Name = "The Sun (Sol)",
      Mass = "1.9855*10^30 kg",
      Circumference = "4,379,000 km",
      Age = "4.57 billion years",
      EmojiIcon = "☀️"
   };

   public static readonly AstronomicalBody Earth = new()
   {
      Name = "Earth",
      Mass = "5.97237*10^24 kg",
      Circumference = "40,075 km",
      Age = "4.54 billion years",
      EmojiIcon = "🌎"
   };

   public static readonly AstronomicalBody Moon = new()
   {
      Name = "Moon",
      Mass = "7.342*10^22 kg",
      Circumference = "10,921 km",
      Age = "4.53 billion years",
      EmojiIcon = "🌕"
   };

   public static readonly AstronomicalBody HalleysComet = new()
   {
      Name = "Halley's Comet",
      Mass = "22 * 10^14 kg",
      Circumference = "11 km",
      Age = "4.6 billion years",
      EmojiIcon = "☄"
   };
}