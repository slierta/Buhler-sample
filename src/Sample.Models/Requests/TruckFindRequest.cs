namespace Sample.Models.Requests;

/// <summary>
/// Provides the request to search data
/// </summary>
public class TruckFindRequest
{
    /// <summary>
    /// The latitude used to find the nearly trucks 
    /// </summary>
    public double Latitude { get; set; }
    /// <summary>
    /// The longitude used to find the nearly trucks
    /// </summary>
    public double Longitude { get; set; }
    /// <summary>
    /// The food that must be served in the food trucks
    /// </summary>
    public string Food { get; set; }

    /// <summary>
    /// Maximum number of results
    /// </summary>
    public int MaxResults { get; set; } = 10;
    /// <summary>
    /// Check if the request is valid
    /// In order to be valid it needs to include some food details and at least 1 result
    /// </summary>
    /// <returns></returns>
    public bool IsValid() => !string.IsNullOrEmpty(Food) && MaxResults >= 1;
}