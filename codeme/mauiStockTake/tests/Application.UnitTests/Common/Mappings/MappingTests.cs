using System.Runtime.Serialization;
using AutoMapper;
using MauiStockTake.Application.Common.Mappings;
using MauiStockTake.Domain.Entities;
using MauiStockTake.Shared.Inventory.Queries;
using MauiStockTake.Shared.Products;
using NUnit.Framework;

namespace MauiStockTake.Application.UnitTests.Common.Mappings;

public class MappingTests
{
    private readonly IConfigurationProvider _configuration;
    private readonly IMapper _mapper;

    public MappingTests()
    {
        _configuration = new MapperConfiguration(config =>
        {
            var mapperAssembly = typeof(MappingProfile).Assembly;
            config.AddMaps(mapperAssembly);
        });

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
        var instance = GetInstanceOf(source);

        _mapper.Map(instance, source, destination);
    }

    private object GetInstanceOf(Type type)
    {
        if (type.GetConstructor(Type.EmptyTypes) != null)
            return Activator.CreateInstance(type)!;

        // Type without parameterless constructor
        return FormatterServices.GetUninitializedObject(type);
    }
}
