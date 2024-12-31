using System.Security.Claims;
using AddressPrishtina.Interfaces;
using AddressPrishtina.Models;
using Microsoft.AspNetCore.Authorization;
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
    
    [Authorize]
    [HttpPost()]
    public async Task<ActionResult> Create([FromBody] AddressRequest addressRequest, CancellationToken cancellationToken)
    {
        await _addressService.Create(addressRequest, User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value, cancellationToken);
        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedList<AddressResponse>>> GetAll(CancellationToken cancellationToken, int pageIndex = 1, int pageSize = 5, string searchQuery = "")
    {
        var addresses = await _addressService.GetAll(cancellationToken, pageIndex, pageSize, searchQuery);
        var pagination = new PaginatedList<AddressResponse>(addresses.Items.Select(a => new AddressResponse
        {
            Id = a.Id,
            Rruga = a.Rruga,
            Numri = a.Numri,
            CreatorName = a.User.UserName,
            CreatorId = a.User.Id
        }).ToList() ,addresses.PageIndex, addresses.TotalPages);
        return pagination;
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet("unapproved")]
    public async Task<ActionResult<PaginatedList<AddressResponse>>> GetAllUnapproved(CancellationToken cancellationToken, int pageIndex = 1, int pageSize = 5)
    {
        var addresses = await _addressService.GetAllUnapproved(cancellationToken, pageIndex, pageSize);
        
        var pagination = new PaginatedList<AddressResponse>(addresses.Items.Select(a => new AddressResponse
        {
            Id = a.Id,
            Rruga = a.Rruga,
            Numri = a.Numri,
            CreatorName = a.User.UserName,
            CreatorId = a.User.Id
        }).ToList() ,addresses.PageIndex, addresses.TotalPages);
        
        return pagination;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("approve/{id}")]
    public async Task<ActionResult> ApproveAddress(int id, CancellationToken cancellationToken)
    {
        await _addressService.ApproveAddress(id, cancellationToken);
        return Ok();
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Address>> Get(int id, CancellationToken cancellationToken)
    {
        return await _addressService.Get(id, cancellationToken);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] AddressRequest updatedAddress, CancellationToken cancellationToken)
    {
        await _addressService.Update(id, updatedAddress, User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value, cancellationToken);
        return Ok();
    }
    
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _addressService.Delete(id, User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value, cancellationToken);
        return Ok();
    }
}