using EcommerceAPI.Application.DTOs;
using EcommerceAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _service;

    public ProductController(IProductService service)
    {
        _service = service;
    }

    /// <summary>
    /// Récupère tous les produits
    /// </summary>
    /// <returns>Liste des produits</returns>
    /// <response code="200">Succès</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<ProductDto> productDtos = await _service.GetAllProductsAsync();
        return Ok(productDtos);
    }

    /// <summary>
    /// Récupère un produit par son Id
    /// </summary>
    /// <param name="id">Id du produit</param>
    /// <returns>Détails du produit</returns>
    /// <response code="200">Produit trouvé</response>
    /// <response code="404">Produit introuvable</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        ProductDto? productDto = await _service.GetProductByIdAsync(id);
        return productDto is null ? NotFound() : Ok(productDto);
    }


    /// <summary>
    /// Créé un produit
    /// </summary>
    /// <param name="createProductDto"></param>
    /// <returns>Le produit créé</returns>
    /// <response code="200">Produit créé</response>
    [HttpPost]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateProductDto createProductDto)
    {
        ProductDto productDto = await _service.CreateProductAsync(createProductDto);
        return CreatedAtAction(nameof(GetById), new { id = productDto.Id }, productDto);
    }

    /// <summary>
    /// Supprime un prduit
    /// </summary>
    /// <param name="id"></param>
    /// <response code="404">Produit non trouvé</response>
    /// <response code="204">Produit supprimé</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        ProductDto? productDto = await _service.GetProductByIdAsync(id);
        if (productDto is null)
        {
            return NotFound();
        }

        await _service.DeleteProductAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Met à jour un produit
    /// </summary>
    /// <param name="id">Id du produit</param>
    /// <param name="updateProductDto">Produit modifié</param>
    /// <returns>Le produit modifié</returns>
    /// <response code="404">Produit non trouvé</response>
    /// <response code="200">Produit trouvé et modifié</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateProductDto updateProductDto)
    {
        ProductDto? productDto = await _service.GetProductByIdAsync(id);
        if (productDto is null)
        {
            return NotFound();
        }

        ProductDto updatedProductDto = await _service.UpdateProductAsync(id, updateProductDto);
        return Ok(updatedProductDto);
    }
}
