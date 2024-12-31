using AddressPrishtina.Models;

namespace AddressPrishtina.Interfaces;

public interface IAddressService
{
    Task Create(AddressRequest updatedAddress, string id, CancellationToken cancellationToken);
    Task<PaginatedList<Address>> GetAll(CancellationToken cancellationToken, int pageIndex, int pageSize, string searchQuery);
    Task<PaginatedList<Address>> GetAllUnapproved(CancellationToken cancellationToken, int pageIndex, int pageSize);
    Task<Address> Get(int id, CancellationToken cancellationToken);
    Task Update(int id, AddressRequest updatedAddress, string userId, CancellationToken cancellationToken);
    Task Delete(int id, string userId, CancellationToken cancellationToken);
    Task ApproveAddress(int id, CancellationToken cancellationToken);
}