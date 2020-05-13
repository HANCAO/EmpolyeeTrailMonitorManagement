using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.ComponentModel.DataAnnotations;

namespace EmpolyeeTrailMonitor.Models
{
    public class EmpolyeeTrail
    {
        public int Id { get; set; }

        public float? GPSX { get; set; }
        public float? GPSY { get; set; }

        public float? BmapLap { get; set; }
        public float? BmapLng { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public bool? IsCar { get; set; }

        public int? Distance { get; set; }
        public int? DistanceSecond { get; set; }
    }
}
