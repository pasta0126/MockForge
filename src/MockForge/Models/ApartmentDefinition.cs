namespace MockForge.Models;

public sealed class ApartmentDefinition
{
    public string Uid { get; init; } = string.Empty;
    public string Neighborhood { get; init; } = string.Empty;
    public string Zone { get; init; } = string.Empty; // "Alta" or "Economica"
    public int Size { get; init; }
    public string Category { get; init; } = string.Empty; // C, B, A, L, AT
    public int FloorsPerLanding { get; init; }
    public bool HasParking { get; init; }
    public int Bedrooms { get; init; }
    public int Bathrooms { get; init; }
    public string Description { get; init; } = string.Empty;
}
