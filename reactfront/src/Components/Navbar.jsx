import React, { useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useToken } from '../TokenContext';
import axiosInstance from '../Helpers/AxiosInstance';

const Navbar = () => {
    const navigate = useNavigate();
    const { token, setToken } = useToken();
    const [role, setRole] = useState('');

    useEffect(() => {
        setToken(localStorage.getItem("jwt"));
    }, []);

    useEffect(() => {
        if(token) {
            axiosInstance.get('/v1/User/role', {
                headers: {
                    "Authorization": `Bearer ${token}`
                }
            })
            .then(res => setRole(res.data));
        } else {
            setRole('');
        }
    }, [token])

    const handleLogout = () => {
        setToken(null);
        setRole('');
        localStorage.removeItem("userId");
        localStorage.removeItem("jwt");
        navigate("/login");
    }

    return (
    <nav className="bg-gray-800 text-white px-4 py-3">
        <div className="container mx-auto flex items-center justify-between">

        <Link to="/" className="text-2xl font-bold">
            AddressPrishtina
        </Link>

        <div className="flex items-center space-x-4 flex-grow justify-end">
            <Link to="/addresses">
                <button className="px-4 py-2 bg-transparent font-semibold text-white rounded-md focus:outline-none">
                    Addresses
                </button>
            </Link>

            {
                role == 'Admin' && 
                <Link to="/address/approve">
                    <button className="px-4 py-2 bg-transparent font-semibold text-white rounded-md focus:outline-none">
                        Addresses to approve
                    </button>
                </Link>
            }

            {
                !token &&
                <>
                    <Link to="/login">
                    <button className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500">
                        Login
                    </button>
                    </Link>
                    <Link to="/register">
                    <button className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500">
                        Register
                    </button>
                    </Link>
                </>
            }

            {
                token &&
                <button className="px-4 py-2 bg-red-600 text-white rounded-md hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-blue-500" onClick={handleLogout}>
                    Log out
                </button>
            }
        </div>
        </div>
    </nav>
    );
};

export default Navbar;
