using Sample.Models;
using Sample.Models.Requests;

namespace Sample.Contracts;

/// <summary>
/// Declares the methods for search food trucks
/// </summary>
public interface ITruckFinder
{
    /// <summary>
    /// Search the trucks in the SF area matching the specified criteria
    /// </summary>
    /// <param name="latitude">Latitude to find the information</param>
    /// <param name="longitude">Longitude to find the information</param>
    /// <param name="food">Food to search</param>
    /// <param name="maxResults">Maximum number of results</param>
    /// <returns></returns>
    Task<IEnumerable<FoodTruck>> Search(double latitude, double longitude, string food, int maxResults);
    /// <summary>
    /// Search the trucks in the SF area matching the specified criteria
    /// </summary>
    /// <param name="request">Contains all the criteria to search</param>
    /// <returns></returns>
    Task<IEnumerable<FoodTruck>> Search(TruckFindRequest request);
}