using AddressPrishtina.Models;

namespace AddressPrishtina.Interfaces;

public interface IAddressService
{
    Task Create(AddressRequest updatedAddress, CancellationToken cancellationToken);
    Task<List<Address>> GetAll(CancellationToken cancellationToken);
    Task<Address> Get(int id, CancellationToken cancellationToken);
    Task Update(int id, AddressRequest updatedAddress, CancellationToken cancellationToken);
    Task Delete(int id, CancellationToken cancellationToken);
    Task<List<Address>> Search(string searchItem, CancellationToken cancellationToken);
}