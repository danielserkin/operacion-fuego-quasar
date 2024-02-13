using System.ComponentModel.DataAnnotations;

namespace OperacionFuegoQuasar.Domain.Entities;

public class SatelliteDataReceveid
{
    public string Name { get; set; }
    public float Distance { get; set; }
    public string[] Message { get; set; }
   
}
