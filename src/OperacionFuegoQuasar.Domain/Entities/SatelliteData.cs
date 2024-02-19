using System.ComponentModel.DataAnnotations;

namespace OperacionFuegoQuasar.Domain.Entities;

public class SatelliteData
{
    public SatelliteData()
    {
    }
    public SatelliteData(string name, float distance, string message)
    {
        Name = name;
        Distance = distance;
        Message = message;
        Timestamp = DateTime.UtcNow;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public float Distance { get; set; }
    public string Message { get; set; }

    public DateTime Timestamp { get; set; }

}
