using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecureSoftware.Entities
{
    [Table("MapPoint")]
    public class MapPoint
    {
        [Key]
        public int IdMapPoint { get; set; }

        public int IdUser { get; set; }

        [StringLength(100)]
        public string PointName { get; set; } = null!;

        [StringLength(500)]
        public string PointDesc { get; set; } = null!;

        public double Lat { get; set; }

        public double Lng { get; set; }
    }
}
