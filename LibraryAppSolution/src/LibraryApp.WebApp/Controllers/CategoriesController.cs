using LibraryApp.Core.DTOs;
using LibraryApp.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.WebApp.Controllers;

/// <summary>
/// Controller for category-related operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;

    /// <summary>
    /// Initializes a new instance of the CategoriesController.
    /// </summary>
    /// <param name="categoryRepository">The category repository.</param>
    public CategoriesController(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
    }

    /// <summary>
    /// Gets all categories.
    /// </summary>
    /// <returns>List of categories.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CategoryDto>), 200)]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _categoryRepository.GetAllAsync();

        var categoryDtos = categories.Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description
        });

        return Ok(categoryDtos);
    }
}