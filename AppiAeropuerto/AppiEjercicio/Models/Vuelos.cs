using System.ComponentModel.DataAnnotations;

namespace AppiEjercicio.Models
{
    public class Vuelos
    {
        public int IdVuelo { get; set; }

        [Required]
        [MaxLength(50)]
        public string CiudadOrigen { get; set; }

        [Required]
        [MaxLength(50)]
        public string CiudadDestino { get; set; }

        [Required]
        public DateTime FechaVuelo { get; set; }

        [Required]
        [MaxLength(50)]
        public string Aerolinea { get; set; }

        [Required]
        [MaxLength(50)]
        public string NumeroVuelo { get; set; }
    }
}
