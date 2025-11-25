using System.Reflection;
using System.Runtime.Serialization;
using AutoMapper;
using Microsoft.Extensions.Logging;
using StockTake.App.Common.Mappings;
using StockTake.Domain.Entities;
using StockTake.Shared.Products;

namespace StockTake.App.UnitTests.Common.Mappings;

public class MappingTests
{
   private readonly IConfigurationProvider _configuration;
   private readonly IMapper _mapper;

   public MappingTests()
   {
      _configuration = new MapperConfiguration(config =>
      {
         Assembly mapperAssembly = typeof(MappingProfile).Assembly;
         config.AddMaps(mapperAssembly);
      }, LoggerFactory.Create(builder => { }));

      _mapper = _configuration.CreateMapper();
   }

   [Test]
   public void ShouldHaveValidConfiguration()
   {
      _configuration.AssertConfigurationIsValid();
   }

   [Test]
   [TestCase(typeof(Product), typeof(ProductDto))]
   public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
   {
      object instance = GetInstanceOf(source);
      _mapper.Map(instance, source, destination);
   }

   private object GetInstanceOf(Type type)
   {
      if (type.GetConstructor(Type.EmptyTypes) != null)
      {
         return Activator.CreateInstance(type)!;
      }

      // Type without parameterless constructor
      return FormatterServices.GetUninitializedObject(type);
   }
}