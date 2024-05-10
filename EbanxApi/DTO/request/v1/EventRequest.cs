public class EventRequest
{
    public string Type { get; set; }
    public string? Destination { get; set; }
    public string? Origin { get; set; }
    public int Amount { get; set; }
}