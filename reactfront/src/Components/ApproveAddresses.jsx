import React, { useEffect, useState } from 'react';
import axiosInstance from '../Helpers/AxiosInstance';
import AddressTable from './AddressTable';
import { useToken } from '../TokenContext';
import { useNavigate } from 'react-router-dom';

const ApproveAddresses = () => {
    const navigate = useNavigate();
    const { token } = useToken();
    const [addresses, setAddresses] = useState([]);
    const [role, setRole] = useState('');
    const [pageIndex, setPageIndex] = useState(1);
    const [totalPages, setTotalPages] = useState(1);

    const fetchUnapprovedAddresses = (page = 1) => {
        if (token) {
            axiosInstance.get(`/v1/Address/unapproved?pageIndex=${page}&pageSize=5`, {
                headers: {
                    "Authorization": `Bearer ${token}`
                }
            })
            .then(res => {
                setAddresses(res.data.items);
                setPageIndex(res.data.pageIndex);
                setTotalPages(res.data.totalPages);
            })
            .catch(err => console.error("Failed to fetch unapproved addresses", err));
        }
    };

    useEffect(() => {
        if (token) {
            axiosInstance.get('/v1/User/role', {
                headers: {
                    "Authorization": `Bearer ${token}`
                }
            })
            .then(res => setRole(res.data))
            .catch(err => console.error("Failed to fetch role", err));
        }
    }, [token]);

    useEffect(() => {
        if (role.length > 0 && role !== 'Admin') {
            navigate("/");
        } else {
            fetchUnapprovedAddresses(pageIndex);
        }
    }, [role, pageIndex, navigate]);

    const handleApprove = (id) => {
        axiosInstance.post(`/v1/Address/approve/${id}`, {}, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        })
        .then(() => {
            setAddresses(prev => prev.filter(a => a.id !== id));
        })
        .catch(err => console.error('Error approving address:', err));
    };

    return (
        <div className="p-4">
            <div className="flex justify-between px-4 py-4">
                <h1 className="text-2xl font-bold">Unapproved Addresses</h1>
            </div>
            <AddressTable
                addresses={addresses}
                action="Approve"
                handleAction={handleApprove}
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

export default ApproveAddresses;
