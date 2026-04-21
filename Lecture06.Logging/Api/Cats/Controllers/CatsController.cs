using Lecture06.Logging.Api.Cats.Contract;
using Microsoft.AspNetCore.Mvc;

namespace Lecture06.Logging.Api.Cats.Controllers
{
    [Route("api/cats")]
    [ApiController]
    public class CatsController(ILogger<CatsController> logger) : ControllerBase
    {
        [HttpPost]
        public ActionResult Create([FromBody] CatUpsertRequestContract contract)
        {
            logger.LogInformation("Create cat request received. Name: {Name}, AgeYears: {AgeYears}, WeightKg: {WeightKg}", contract.Name, contract.AgeYears, contract.WeightKg);

            /// Нельзя использовать интерполяцию строк в логах, правильно так, как выше ↑:
            /// logger.LogInformation($"Create cat request received. Name: {contract.Name}, AgeYears: {contract.AgeYears}, WeightKg: {contract.WeightKg}");

            var nextId = _cats.Count == 0 ? 1 : _cats.Max(x => x.Id) + 1;

            var cat = (Id: nextId, Name: contract.Name, Age: contract.AgeYears, WeightKg: contract.WeightKg);

            _cats.Add(cat);

            logger.LogInformation("Cat created successfully. Id: {Id}, TotalCatsCount: {TotalCatsCount}", cat.Id, _cats.Count);

            return Ok();
        }

        [HttpGet]
        [Route("all")]
        public ActionResult<CatResponseContract[]> GetAllCats()
        {
            logger.LogInformation("Get all cats request received. CurrentCatsCount: {CurrentCatsCount}", _cats.Count);

            var cats = _cats
                .Select(x => new CatResponseContract
                {
                    Id = x.Id,
                    AgeYears = x.Age,
                    Name = x.Name,
                    WeightKg = x.WeightKg
                })
                .ToArray();

            logger.LogInformation("Returning all cats. ReturnedCount: {ReturnedCount}", cats.Length);

            return Ok(cats);
        }

        [HttpGet]
        [Route("{id:int:min(1)}")]
        public ActionResult<CatResponseContract> GetCat(int id)
        {
            logger.LogInformation("Get cat by id request received. Id: {Id}", id);

            var index = _cats.FindIndex(x => x.Id == id);
            if (index < 0)
            {
                logger.LogWarning("Cat not found. Id: {Id}", id);
                return NotFound();
            }

            var cat = _cats[index];

            logger.LogInformation("Cat found. Id: {Id}, Name: {Name}", cat.Id, cat.Name);

            return Ok(new CatResponseContract
            {
                Id = cat.Id,
                AgeYears = cat.Age,
                Name = cat.Name,
                WeightKg = cat.WeightKg
            });
        }

        [HttpPut]
        [Route("{id:int:min(1)}")]
        public ActionResult Update(int id, [FromBody] CatUpsertRequestContract contract)
        {
            logger.LogInformation("Update cat request received. Id: {Id}, NewName: {Name}, NewAgeYears: {AgeYears}, NewWeightKg: {WeightKg}", id, contract.Name, contract.AgeYears, contract.WeightKg);

            var index = _cats.FindIndex(x => x.Id == id);
            if (index < 0)
            {
                logger.LogWarning("Cat for update not found. Id: {Id}", id);
                return NotFound();
            }

            var updated = (id, contract.Name, contract.AgeYears, contract.WeightKg);
            _cats[index] = updated;

            logger.LogInformation("Cat updated successfully. Id: {Id}", id);

            return Ok();
        }

        [HttpDelete]
        [Route("{id:int:min(1)}")]
        public ActionResult Delete(int id)
        {
            logger.LogInformation("Delete cat request received. Id: {Id}", id);

            var index = _cats.FindIndex(x => x.Id == id);
            if (index < 0)
            {
                logger.LogWarning("Cat for deletion not found. Id: {Id}", id);
                return NotFound();
            }

            _cats.RemoveAt(index);

            logger.LogInformation("Cat deleted successfully. Id: {Id}, RemainingCatsCount: {RemainingCatsCount}", id, _cats.Count);

            return Ok();
        }

        private static List<(int Id, string Name, int Age, double WeightKg)> _cats = new();
    }
}




