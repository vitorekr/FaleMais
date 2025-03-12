namespace FaleMais.Application.DTOs;

public class CalculateCallCostRequest
{
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public int Duration { get; set; }
    public string Plan { get; set; } = string.Empty;
}