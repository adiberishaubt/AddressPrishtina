using AddressPrishtina.Interfaces;
using AddressPrishtina.Models;
using Microsoft.AspNetCore.Mvc;

namespace AddressPrishtina.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class AddressController : ControllerBase
{
    private readonly IAddressService _addressService;

    public AddressController(IAddressService addressService)
    {
        _addressService = addressService;
    }
    
    [HttpPost()]
    public async Task<ActionResult> Create([FromBody] AddressRequest addressRequest, CancellationToken cancellationToken)
    {
        await _addressService.Create(addressRequest, cancellationToken);
        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<List<Address>>> GetAll(CancellationToken cancellationToken)
    {
        return await _addressService.GetAll(cancellationToken);
    }

    [HttpGet("id")]
    public async Task<ActionResult<Address>> Get(int id, CancellationToken cancellationToken)
    {
        return await _addressService.Get(id, cancellationToken);
    }

    [HttpPut("id")]
    public async Task<ActionResult> Update(int id, [FromBody] AddressRequest updatedAddress, CancellationToken cancellationToken)
    {
        await _addressService.Update(id, updatedAddress, cancellationToken);
        return Ok();
    }
    
    [HttpDelete("id")]
    public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _addressService.Delete(id, cancellationToken);
        return Ok();
    }

    [HttpGet("searchItem")]
    public async Task<ActionResult<List<Address>>> Search(string searchItem, CancellationToken cancellationToken)
    {
        return await _addressService.Search(searchItem, cancellationToken);
    }
}