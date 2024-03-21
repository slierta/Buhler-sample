using System.Globalization;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Sample.Contracts;
using Sample.Models;
using Sample.Models.Requests;

namespace Sample.Services;

/// <summary>
/// 
/// </summary>
public class TruckFinder(ILogger<TruckFinder> logger, IHttpClientFactory clientFactory)
    : ITruckFinder
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="latitude"></param>
    /// <param name="longitude"></param>
    /// <param name="food"></param>
    /// <param name="maxResults"></param>
    public async Task<IEnumerable<FoodTruck>> Search(double latitude, double longitude, string food, int maxResults)
    {
        return await Search(new TruckFindRequest()
        {
            Latitude = latitude,
            MaxResults = maxResults,
            Food = food,
            Longitude = longitude
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<IEnumerable<FoodTruck>> Search(TruckFindRequest request)
    {
        var client = clientFactory.CreateClient();
        var queryUri = new Uri($"{Constants.JsonUri}{ToQueryString(request)}");
        logger.LogInformation("Uri generated {uri}",queryUri);
        var req = new HttpRequestMessage(HttpMethod.Get, queryUri);
        logger.LogDebug("Starting request to service");
        // TODO: Here, I could use Polly to avoid issues if the service is not available at the moment
        var response = await client.SendAsync(req);
        logger.LogDebug("Finished request with status code {s}",response.StatusCode);
        return response is { IsSuccessStatusCode: true, Content: not null } ? JsonSerializer.Deserialize<FoodTruck[]>(await response.Content.ReadAsStringAsync())! : Enumerable.Empty<FoodTruck>();
    }
    /// <summary>
    /// Converts the request values into the querystring to be used
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    internal string ToQueryString(TruckFindRequest request)
    {
        var point = $"'POINT({System.Web.HttpUtility.UrlEncode(request.Longitude.ToString(CultureInfo.InvariantCulture))} {System.Web.HttpUtility.UrlEncode(request.Latitude.ToString(CultureInfo.InvariantCulture))})'";
        var distance = $"distance_in_meters(location, {point})";
        var sb = new StringBuilder("?");
        sb.Append($"$where=fooditems like '%{System.Web.HttpUtility.UrlEncode(request.Food)}%'");
        sb.Append($"&$limit={request.MaxResults}");
        sb.Append($"&$select=*," + distance + " as distance");          // This line is used to include the distance in meters in the results. Not really needed
        sb.Append($"&$order=distance_in_meters(location, {point})");
        return sb.ToString();
    }
}