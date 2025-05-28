using MeTracker.Models;

namespace MeTracker.Services;

public interface ILocationRepository
{
   Task<List<Location>> GetAllAsync();

   Task SaveAsync(LocationEntry location);
}