using Microsoft.AspNetCore.Mvc;

namespace OperacionFuegoQuasar.Application.Requests;

public class TopSecret
{
    public IEnumerable<Satellite> Satellites { get; set; }
}

public class Satellite
{
    public string Name { get; set; }
    public float Distance { get; set; }
    public string[] Message { get; set; }
}


public class TopSecretSplit
{
    public float Distance { get; set; }
    public string[] Message { get; set; }
}
