using Lecture06.Configuration.Api.Cats.Contract;
using Lecture06.Configuration.Api.Cats.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Lecture06.Configuration.Api.Cats.Controllers
{
    [Route("api/cats")]
    [ApiController]
    public class CatsController(IOptions<CatApiOptions> options) : ControllerBase
    {
        [HttpPost]
        public ActionResult Create([FromBody] CatUpsertRequestContract contract)
        {
            if (_cats.Count >= options.Value.MaxAllowedCatCount)
                return BadRequest();

            var nextId = _cats.Count == 0 ? 1 : _cats.Max(x => x.Id) + 1;

            var cat = (Id: nextId, Name: contract.Name, Age: contract.AgeYears, WeightKg: contract.WeightKg);

            _cats.Add(cat);

            return Ok();
        }

        [HttpGet]
        [Route("all")]
        public ActionResult<CatResponseContract[]> GetAllCats()
        {
            var cats = _cats
                .Select(x => new CatResponseContract { Id = x.Id, AgeYears = x.Age, Name = x.Name, WeightKg = x.WeightKg })
                .ToArray();

            return Ok(cats);
        }

        [HttpGet]
        [Route("{id:int:min(1)}")]
        public ActionResult<CatResponseContract> GetCat(int id)
        {
            var index = _cats.FindIndex(x => x.Id == id);
            if (index < 0) return NotFound();

            var cat = _cats[index];

            return Ok(new CatResponseContract { Id = cat.Id, AgeYears = cat.Age, Name = cat.Name, WeightKg = cat.WeightKg });
        }

        [HttpPut]
        [Route("{id:int:min(1)}")]
        public ActionResult Update(int id, [FromBody] CatUpsertRequestContract contract)
        {
            if (!options.Value.AllowUpdate)
                return BadRequest();

            var index = _cats.FindIndex(x => x.Id == id);
            if (index < 0) return NotFound();

            var updated = (id, contract.Name, contract.AgeYears, contract.WeightKg);
            _cats[index] = updated;

            return Ok();
        }

        [HttpDelete]
        [Route("{id:int:min(1)}")]
        public ActionResult Delete(int id)
        {
            if (!options.Value.AllowDelete)
                return BadRequest();

            var index = _cats.FindIndex(x => x.Id == id);
            if (index < 0) return NotFound();

            _cats.RemoveAt(index);
            return Ok();
        }

        private static List<(int Id, string Name, int Age, double WeightKg)> _cats = new();
    }
}
