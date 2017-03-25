
using Newtonsoft.Json;

[ValidateHero]
public class HeroViewModel
{
    [JsonProperty("id")]
    public int HeroId { get; set; }
    
    [JsonProperty("name")]
    public string HeroName { get; set; }
}