using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration.Attributes;
using CsvHelper.Configuration;

namespace DataAccess.Entities
{
    [Table("ESOEntities")]
    public class ESOEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Network { get; set; }
        public string? Obt_name { get; set; }
        public string? Obj_gv_type { get; set; }
        public int? Obj_number { get; set; }
        public decimal? P_plus { get; set; }
        public DateTime PL_T { get; set; }
        public decimal? P_minus { get; set; }
    }

    public sealed class ESOEntityMap : ClassMap<ESOEntity>
    {
        public ESOEntityMap()
        {
            Map(m => m.Network).Name("TINKLAS");
            Map(m => m.Obt_name).Name("OBT_PAVADINIMAS");
            Map(m => m.Obj_gv_type).Name("OBJ_GV_TIPAS");
            Map(m => m.Obj_number).Name("OBJ_NUMERIS");
            Map(m => m.P_plus).Name("P+");
            Map(m => m.PL_T).Name("PL_T");
            Map(m => m.P_minus).Name("P-");
        }
    }
}
