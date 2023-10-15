using Microsoft.AspNetCore.Mvc;
using ContosoPizza.Models;
using WebApplication1.Services;

namespace ContosoPizza.Controller;

[ApiController]
[Route("[controller]")]
public class PizzaController : ControllerBase
{
    public PizzaController()
    {
    }

    //HTTP GET request
    [HttpGet]
    public ActionResult<List<Pizza>> GetAll() => PizzaService.GetAll();

    //HTTP get request by id
    [HttpGet("{Id}")]
    public ActionResult<Pizza?> Get(int id) => PizzaService.Get(id);

    //HTTP post request
    [HttpPost]
    public IActionResult Create(Pizza pizza)
    {
        PizzaService.Add(pizza);
        return CreatedAtAction(nameof(Get), new { Id = pizza.Id },  pizza);
    }

    //HTTP PUT request
    [HttpPut("{id}")]
    public IActionResult Update(int id, Pizza pizza)
    {
        if (id != pizza.Id)
            return BadRequest();
        var updatePizza = PizzaService.Get(id);
        if (updatePizza is null)
            return NotFound();
        PizzaService.Update(pizza);
        return NoContent();
    }

    //HTTP DELETE request
    [HttpDelete("{id}")]
    public IActionResult Remove(int id)
    {
        var pizza = PizzaService.Get(id); 
        if (pizza is null) 
            return NotFound();
        PizzaService.Remove(id);
        return NoContent();
    }
}
