using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CsvHelper.Configuration;

namespace DataAccess.Entities
{
    [Table("ESOEntities")]
    public class ESOEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Network { get; set; }
        public string? ObtName { get; set; }
        public string? ObjGvType { get; set; }
        public int? ObjNumber { get; set; }
        public decimal? PPlus { get; set; }
        public DateTime PlT { get; set; }
        public decimal? PMinus { get; set; }
    }

    public sealed class ESOEntityMap : ClassMap<ESOEntity>
    {
        public ESOEntityMap()
        {
            Map(m => m.Network).Name("TINKLAS");
            Map(m => m.ObtName).Name("OBT_PAVADINIMAS");
            Map(m => m.ObjGvType).Name("OBJ_GV_TIPAS");
            Map(m => m.ObjNumber).Name("OBJ_NUMERIS");
            Map(m => m.PPlus).Name("P+");
            Map(m => m.PlT).Name("PL_T");
            Map(m => m.PMinus).Name("P-");
        }
    }
}
