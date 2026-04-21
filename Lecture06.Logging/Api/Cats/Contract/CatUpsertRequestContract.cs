using System.ComponentModel.DataAnnotations;

namespace Lecture06.Logging.Api.Cats.Contract
{
    public class CatUpsertRequestContract
    {
        [MaxLength(40)]
        [MinLength(2)]
        [RegularExpression(@"^([A-ZА-Я][a-zа-я]*[ ]*)+$")]
        public required string Name { get; init; }

        [Range(0, 30)]
        public required int AgeYears { get; init; }

        [Range(typeof(double), "0.5", "20.0")]
        public required double WeightKg { get; init; }
    }
}
