using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using DatingApp.API.Data;

namespace DatingApp.API.Controllers
{
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    private readonly DatingApp.API.Data.DataContext _context;
    public ValuesController(DatingApp.API.Data.DataContext context )
    {
        _context=context;
        
    }
    // GET api/values
    [HttpGet]
    public IActionResult GetValues()
    {
        var values = _context.Values.ToList();
        return Ok(values);
    }

     [AllowAnonymous]
    // GET api/values/5
    [HttpGet("{id}")]
    public IActionResult GetValue(int id)
    {
        var value = _context.Values.FirstOrDefault(x=>x.ID==id);
        return Ok(value);
    }

    // POST api/values
    [HttpPost]
    public void Post([FromBody] string value)
    { 
    }

    // PUT api/values/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {   
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {  
    }
}
}