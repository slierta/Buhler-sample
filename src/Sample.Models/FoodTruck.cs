using System.Globalization;
using System.Text.Json.Serialization;
using System.Transactions;

namespace Sample.Models;
/// <summary>
/// Contains information about the truck
/// TODO: Add the documentation to all the properties
/// </summary>
public class FoodTruck
{
    [JsonPropertyName("location")] public Location Location { get; set; }

    [JsonPropertyName("distance")] public string DistanceInMeters { get; set; }
    /// <summary>
    /// Returns the distance in meters to the source location
    /// </summary>
    [JsonIgnore] public double? Distance
    {
        get
        {
            var format = CultureInfo.InvariantCulture.NumberFormat;
            if (double.TryParse(DistanceInMeters, format, out var d) && d>0)
                return d/1000;
            return null;
        }
    }

    [JsonPropertyName("expirationdate")] public string? ExpirationDate { get; set; }
    [JsonPropertyName("priorpermit")] public string PriorPermit { get; set; }

    [JsonPropertyName("received")] public string Received { get; set; }

    [JsonPropertyName("approved")] public string? Approved { get; set; }

    [JsonPropertyName("schedule")] public string Schedule { get; set; }

    [JsonPropertyName("longitude")] public string Longitude { get; set; }

    [JsonPropertyName("latitude")] public string Latitude { get; set; }

    [JsonPropertyName("y")] public string Y { get; set; }

    [JsonPropertyName("x")] public string X { get; set; }

    [JsonPropertyName("fooditems")] public string FoodItems { get; set; }

    [JsonIgnore] public string[] FoodItemData => FoodItems?.Split(":");

    [JsonPropertyName("status")] public string Status { get; set; }

    [JsonPropertyName("permit")] public string Permit { get; set; }

    [JsonPropertyName("lot")] public string Lot { get; set; }

    [JsonPropertyName("objectid")] public string ObjectId { get; set; }

    [JsonPropertyName("applicant")] public string Applicant { get; set; }

    [JsonPropertyName("facilitytype")] public string FacilityType { get; set; }

    [JsonPropertyName("cnn")] public string CNN { get; set; }

    [JsonPropertyName("locationdescription")]
    public string LocDescription { get; set; }

    [JsonPropertyName("address")] public string Address { get; set; }

    [JsonPropertyName("block")] public string Block { get; set; }

    [JsonPropertyName("blocklot")] public string BlockLot { get; set; }
}