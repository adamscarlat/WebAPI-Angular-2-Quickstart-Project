
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ViewModels
{
    public class HeroViewModel
    {
        [JsonProperty("id")]
        public int HeroId { get; set; }
        
        [JsonProperty("name")]
        [Required(ErrorMessage = "Hero name is required")] 
        public string HeroName { get; set; }
    }
}