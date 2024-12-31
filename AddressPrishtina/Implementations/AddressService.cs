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

    public async Task Create(AddressRequest updatedAddress, string id, CancellationToken cancellationToken)
    {
        var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Id.Equals(id), cancellationToken);

        if (user is null)
        {
            throw new Exception("User doesn't exist");
        }

        var address = new Address
        {
            Numri = updatedAddress.Numri,
            Rruga = updatedAddress.Rruga,
            UserId = user.Id
        };
        await _dataContext.Addresses.AddAsync(address, cancellationToken);
        await _dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<PaginatedList<Address>> GetAll(CancellationToken cancellationToken, int pageIndex, int pageSize, string searchQuery)
    {
        var query = _dataContext.Addresses.Include(a => a.User).Where(a => a.Approved).AsQueryable();

        if (!string.IsNullOrEmpty(searchQuery))
        {
            query = query.Where(a => a.Rruga.Contains(searchQuery) || a.Numri.ToString().Contains(searchQuery) || a.User.UserName.Contains(searchQuery));
        }

        var totalItems = await query.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var addresses = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var paginatedList = new PaginatedList<Address>(addresses, pageIndex, totalPages);
        return paginatedList;
    }

    public async Task<PaginatedList<Address>> GetAllUnapproved(CancellationToken cancellationToken, int pageIndex, int pageSize)
    {
        var addresses = await _dataContext.Addresses
            .Include(a => a.User)
            .Where(a => !a.Approved)
            .OrderBy(b => b.Id)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var count = await _dataContext.Addresses.Where(a => !a.Approved).CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling(count / (double)pageSize);

        return new PaginatedList<Address>(addresses, pageIndex, totalPages);
    }

    public async Task<Address> Get(int id, CancellationToken cancellationToken)
    {
        return await _dataContext.Addresses.FirstOrDefaultAsync(a => a.Id == id && a.Approved, cancellationToken);
    }

    public async Task Update(int id, AddressRequest updatedAddress, string userId, CancellationToken cancellationToken)
    {
        var address = await _dataContext.Addresses.Include(a => a.User).FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        if (address is not null)
        {
            if (address.User.Id == userId)  
            {
                address.Numri = updatedAddress.Numri;
                address.Rruga = updatedAddress.Rruga;
                await _dataContext.SaveChangesAsync(cancellationToken);
            }
            else
            {
                throw new Exception("Address does not belong to you!");
            }
        }
        else
        {
            throw new Exception("Address doesn't exist!");
        }
    }

    public async Task Delete(int id, string userId, CancellationToken cancellationToken)
    {
        var address = await _dataContext.Addresses.Include(a => a.User).FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        if (address is not null)
        {
            if (address.User.Id == userId)
            {
                _dataContext.Addresses.Remove(address);
                await _dataContext.SaveChangesAsync(cancellationToken);
            }
            else
            {
                throw new Exception("Address does not belong to you!");
            }
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

    public async Task ApproveAddress(int id, CancellationToken cancellationToken)
    {
        var address = await _dataContext.Addresses.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

        if (address is null)
        {
            throw new Exception("Address doesn't exist!");
        }

        if (address.Approved)
        {
            throw new Exception("Address is already approved!");
        }

        address.Approved = true;
        await _dataContext.SaveChangesAsync(cancellationToken);
    }
}