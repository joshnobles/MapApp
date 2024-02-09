using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecureSoftware.Entities
{
    [Table("MapPoint")]
    public class MapPoint
    {
        public int IdUser { get; set; }

        [StringLength(100)]
        public string PointName { get; set; } = null!;

        [StringLength(500)]
        public string PointDesc { get; set; } = null!;

        public float Lat { get; set; }

        public float Lng { get; set; }
    }
}
