import React, { useEffect, useState } from 'react';
import axiosInstance from '../Helpers/AxiosInstance';
import AddressTable from './AddressTable';
import { useToken } from '../TokenContext';
import { Link } from 'react-router-dom';

const Address = () => {
    const { token } = useToken();
    const [addresses, setAddresses] = useState([]);
    const [pageIndex, setPageIndex] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [searchQuery, setSearchQuery] = useState('');

    const fetchAddresses = (page = 1, query = '') => {
        axiosInstance.get(`/v1/Address?pageIndex=${page}&pageSize=5&searchQuery=${query}`)
            .then(res => {
                setAddresses(res.data.items);
                setPageIndex(res.data.pageIndex);
                setTotalPages(res.data.totalPages);
            })
            .catch(err => console.error("Failed to fetch addresses", err));
    };

    useEffect(() => {
        fetchAddresses(pageIndex, searchQuery);
    }, [pageIndex, searchQuery]);

    const handleDelete = (id) => {
        axiosInstance.delete(`/v1/Address/${id}`, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        })
        .then(() => setAddresses(prev => prev.filter(a => a.id !== id)))
        .catch(err => console.error("Failed to delete address", err));
    };

    const handleSearchChange = (e) => {
        setSearchQuery(e.target.value);
        setPageIndex(1);
    };

    return (
        <div className="p-4">
            <div className='flex justify-between px-4 py-4'>
                <div className='flex gap-6'>
                  <h1 className='text-2xl font-bold'>Addresses</h1>
                  <input
                      type="text"
                      placeholder="Search..."
                      value={searchQuery}
                      onChange={handleSearchChange}
                      className="px-3 py-2 text-black rounded-md border border-gray-300 focus:outline-none focus:ring-2 focus:ring-blue-500"
                  />
                </div>
                {token && 
                    <Link to="/address/add">
                        <button className="px-4 py-2 bg-green-600 text-white rounded-md hover:bg-green-700 focus:outline-none focus:ring-2 focus:ring-green-500">
                            Add Address
                        </button>
                    </Link>
                }
            </div>
            <AddressTable 
                addresses={addresses} 
                action={"Delete"} 
                handleAction={handleDelete} 
            />
            <div className="flex justify-between items-center mt-4">
                <button 
                    className="px-4 py-2 bg-gray-300 rounded-md disabled:opacity-50" 
                    onClick={() => setPageIndex(prev => prev - 1)} 
                    disabled={pageIndex === 1}
                >
                    Previous
                </button>
                <span>
                    Page {pageIndex} of {totalPages}
                </span>
                <button 
                    className="px-4 py-2 bg-gray-300 rounded-md disabled:opacity-50" 
                    onClick={() => setPageIndex(prev => prev + 1)} 
                    disabled={pageIndex === totalPages}
                >
                    Next
                </button>
            </div>
        </div>
    );
};

export default Address;
