using DataAccess.Entities;

namespace ElectricityDataManager.Tests.Common
{
    public class TestData
    {
        public static List<ESOEntity> GetESOEntities()
        {
            return new List<ESOEntity>
            {
                new ESOEntity
                {
                    Id = 1,
                    Network = "Network 1",
                    ObtName = "Obj 1",
                    ObjGvType = "Obj type 1",
                    ObjNumber = 1,
                    PPlus = 1.1m,
                    PlT = new DateTime(2020, 1, 1),
                    PMinus = 1.1m
                },
                new ESOEntity
                {
                    Id = 2,
                    Network = "Network 2",
                    ObtName = "Obj 2",
                    ObjGvType = "Obj type 2",
                    ObjNumber = 2,
                    PPlus = 2.2m,
                    PlT = new DateTime(2020, 1, 2),
                    PMinus = 2.2m
                },
                new ESOEntity
                {
                    Id = 3,
                    Network = "Network 3",
                    ObtName = "Obj 3",
                    ObjGvType = "Obj type 3",
                    ObjNumber = 3,
                    PPlus = 3.3m,
                    PlT = new DateTime(2020, 1, 3),
                    PMinus = 3.3m
                }
            };
        }
    }
}
