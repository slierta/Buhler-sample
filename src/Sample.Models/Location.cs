using System.Text.Json.Serialization;

namespace Sample.Models;

public class Location
{
    [JsonPropertyName("latitude")]
    public string Latitude { get; set; }
    [JsonPropertyName("longitude")]
    public string Longitude { get; set; }
    [JsonPropertyName("human_address")]
    public string Address { get; set; }
}