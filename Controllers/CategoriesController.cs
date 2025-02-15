﻿using FinalProject.Models;
using FinalProject.Repository.ProductRepository;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    /*[EnableCors("policy")]*/
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IProduct productRepo;

        public CategoriesController(IProduct productRepo)
        {
            this.productRepo = productRepo;
        }

       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
        {
            var result = await productRepo.GetAllCategories();
            if (result != null)
            {
                return Ok(result);
            }
            return NoContent();
        }

 
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            try
            {
                var category = await productRepo.GetCategoriesByCategoryId(id);

                if (category == null)
                {
                    throw new Exception("Category not found");
                }

                return category;
            }
            catch (Exception ex)
            {
                // Log the exception details here
                return StatusCode(500, "This Category is Not Present");
            }
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, [FromBody] Category category)
        {
           
            try
            {
                await productRepo.UpdateCategory(id, category);
            }
            catch (Exception)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

  
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory([FromBody] Category category)
        {
            await productRepo.AddCategory(category);

            return CreatedAtAction("GetCategory", new { id = category.CategoryId }, category);
        }

       
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await productRepo.DeleteCategory(id);
            if (category == null)
            {
                return NotFound("Not Found Category id");
            }


            return Ok("Deleted successfully");
        }

        private bool CategoryExists(int id)
        {
            return productRepo.CategoryExists(id);
        }
    }
}
