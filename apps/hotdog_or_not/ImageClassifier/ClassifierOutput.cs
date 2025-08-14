namespace HotdogOrNot.ImageClassifier;

internal sealed class ClassifierOutput
{
   private ClassifierOutput()
   {
   }

   public string TopResultLabel { get; private set; }

   public float TopResultScore { get; private set; }

   public IDictionary<string, float> LabelScores { get; private set; }

   public byte[] Image { get; private set; }

   public static ClassifierOutput Create(string topLabel, IDictionary<string, float> labelScores, byte[] image)
   {
      ArgumentNullException.ThrowIfNull(topLabel, nameof(topLabel));
      ArgumentNullException.ThrowIfNull(labelScores, nameof(labelScores));

      return new ClassifierOutput
      {
         TopResultLabel = topLabel,
         TopResultScore = labelScores.First(i => i.Key == topLabel).Value,
         LabelScores = labelScores,
         Image = image
      };
   }
}