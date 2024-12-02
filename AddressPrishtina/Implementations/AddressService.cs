using AddressPrishtina.Data;
using AddressPrishtina.Interfaces;
using AddressPrishtina.Models;
using Microsoft.EntityFrameworkCore;

namespace AddressPrishtina.Implementations;

public class AddressService : IAddressService
{
    private readonly DataContext _dataContext;

    public AddressService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task Create(AddressRequest updatedAddress, CancellationToken cancellationToken)
    {
        var address = new Address
        {
            Numri = updatedAddress.Numri,
            Rruga = updatedAddress.Rruga
        };
        await _dataContext.Addresses.AddAsync(address, cancellationToken);
        await _dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Address>> GetAll(CancellationToken cancellationToken)
    {
        return await _dataContext.Addresses.ToListAsync(cancellationToken);
    }

    public async Task<Address> Get(int id, CancellationToken cancellationToken)
    {
        return await _dataContext.Addresses.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task Update(int id, AddressRequest updatedAddress, CancellationToken cancellationToken)
    {
        var address = await _dataContext.Addresses.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        if (address is not null)
        {
            address.Numri = updatedAddress.Numri;
            address.Rruga = updatedAddress.Rruga;
            await _dataContext.SaveChangesAsync(cancellationToken);
        }
        else
        {
            throw new Exception("Address doesn't exist!");
        }
    }

    public async Task Delete(int id, CancellationToken cancellationToken)
    {
        var address = await _dataContext.Addresses.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        if (address is not null)
        {
            _dataContext.Addresses.Remove(address);
            await _dataContext.SaveChangesAsync(cancellationToken);
        }
        else
        {
            throw new Exception("Address doesn't exist!");
        }
    }

    public async Task<List<Address>> Search(string searchItem, CancellationToken cancellationToken)
    {
        return await _dataContext.Addresses.Where(a => a.Rruga.ToLower().Contains(searchItem)).ToListAsync(cancellationToken);
    }
}