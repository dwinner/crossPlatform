using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using SixLabors.ImageSharp.Formats.Png;
using Image = SixLabors.ImageSharp.Image;

namespace HotdogOrNot.ImageClassifier;

internal class MlNetClassifier : IClassifier
{
   private readonly string _inputName;
   private readonly int _inputSize;
   private readonly bool _isBgr;
   private readonly bool _isRange255;
   private readonly InferenceSession _session;

   public MlNetClassifier(byte[] model)
   {
      _session = new InferenceSession(model);
      _isBgr = _session.ModelMetadata.CustomMetadataMap["Image.BitmapPixelFormat"] == "Bgr8";
      _isRange255 = _session.ModelMetadata.CustomMetadataMap["Image.NominalPixelRange"] == "NominalRange_0_255";
      _inputName = _session.InputMetadata.Keys.First();
      _inputSize = _session.InputMetadata[_inputName].Dimensions[2];
   }

   public ClassifierOutput Classify(byte[] imageBytes)
   {
      var (tensor, resizedImage) = LoadInputTensor(imageBytes, _inputSize, _isBgr, _isRange255);
      var resultsCollection = _session.Run(new List<NamedOnnxValue>
      {
         NamedOnnxValue.CreateFromTensor(_inputName, tensor)
      });

      var topLabel = resultsCollection
         ?.FirstOrDefault(i => i.Name == "classLabel")
         ?.AsTensor<string>()
         ?.First();

      var labelScores = resultsCollection
         ?.FirstOrDefault(i => i.Name == "loss")
         ?.AsEnumerable<NamedOnnxValue>()
         ?.First()
         ?.AsDictionary<string, float>();

      return ClassifierOutput.Create(topLabel, labelScores, resizedImage);
   }

   private static (Tensor<float>, byte[] resizedImage) LoadInputTensor(
      byte[] imageBytes,
      int imageSize,
      bool isBgr,
      bool isRange255)
   {
      var input = new DenseTensor<float>(new[] { 1, 3, imageSize, imageSize });
      byte[] pixelBytes;

      using (var image = Image.Load<Rgba32>(imageBytes))
      {
         image.Mutate(x => x.Resize(imageSize, imageSize));
         var height = image.Height;
         var width = image.Width;

         image.ProcessPixelRows(source =>
         {
            for (var y = 0; y < height; y++)
            {
               var pixelSpan = source.GetRowSpan(y);
               for (var x = 0; x < width; x++)
               {
                  if (isBgr)
                  {
                     input[0, 0, y, x] = pixelSpan[x].B;
                     input[0, 1, y, x] = pixelSpan[x].G;
                     input[0, 2, y, x] = pixelSpan[x].R;
                  }
                  else
                  {
                     input[0, 0, y, x] = pixelSpan[x].R;
                     input[0, 1, y, x] = pixelSpan[x].G;
                     input[0, 2, y, x] = pixelSpan[x].B;
                  }

                  if (!isRange255)
                  {
                     input[0, 0, y, x] /= 255;
                     input[0, 1, y, x] /= 255;
                     input[0, 2, y, x] /= 255;
                  }
               }
            }
         });

         var outStream = new MemoryStream();
         image.Save(outStream, new PngEncoder());
         pixelBytes = outStream.ToArray();
      }

      return (input, pixelBytes);
   }
}