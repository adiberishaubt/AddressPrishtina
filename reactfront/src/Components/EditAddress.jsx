import React, { useEffect, useState } from 'react';
import axiosInstance from '../Helpers/AxiosInstance';
import { useToken } from '../TokenContext';
import { useNavigate, useParams } from 'react-router-dom';

const EditAddress = () => {
    const { token, setToken } = useToken();
    const [address, setAddress] = useState();
    const [rruga, setRruga] = useState('');
    const [numri, setNumri] = useState('');
    const [error, setError] = useState('');
    const navigate = useNavigate();
    const { id } = useParams();
    const [success, setSuccess] = useState('');

    useEffect(() => {
        if(token) {
            axiosInstance.get(`/v1/Address/${id}`, {
                headers: {
                    "Authorization": `Bearer ${token}`
                }
            })
            .then(res => {
                setAddress(res.data);
                setRruga(res.data.rruga);
                setNumri(res.data.numri);
            })
            .catch(e => setError(e.response.data));
        }
    }, []);
  
    useEffect(() => {
        if(!token) {
            navigate("/");
        }
    }, [token]);
  
    const handleSubmit = async (e) => {
      e.preventDefault();
  
      setError('');
      setSuccess('');
  
      if (!rruga || !numri) {
        setError('Both Rruga and Numri are required.');
        return;
      }
  
      if (isNaN(numri)) {
        setError('Numri must be a valid number.');
        return;
      }
  
      if (numri <= 0) {
          setError('Numri must be greater than 0.');
          return;
        }
  
      try {
        await axiosInstance.put(`/v1/Address/${id}`, {
          rruga,
          numri: parseInt(numri),
        }, {
          headers: {
              "Authorization": `Bearer ${token}`
          }
        });
  
        setSuccess('Address added successfully!');
        setRruga('');
        setNumri('');
        navigate("/addresses");
      } catch (e) {
        setError('Failed to add address. Please try again.');
        console.error(e);
      }
    };
  
    return (
      <div className="flex items-center justify-center min-h-screen bg-gray-200">
        <div className="bg-white p-8 rounded-lg shadow-md w-96">
          <h2 className="text-2xl font-bold mb-6 text-center">Edit Address with Id {id}</h2>
          
          <form onSubmit={handleSubmit} className="space-y-4">
            <div>
              <label htmlFor="rruga" className="block text-sm font-medium text-gray-700">Rruga</label>
              <input
                type="text"
                id="rruga"
                value={rruga}
                onChange={(e) => setRruga(e.target.value)}
                required
                className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
              />
            </div>
  
            <div>
              <label htmlFor="numri" className="block text-sm font-medium text-gray-700">Numri</label>
              <input
                type="number"
                id="numri"
                value={numri}
                min={1}
                onChange={(e) => setNumri(e.target.value)}
                required
                className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
              />
            </div>
  
            {error && <p className="text-red-500 text-sm">{error}</p>}
            {success && <p className="text-green-500 text-sm">{success}</p>}
  
            <button
              type="submit"
              className="w-full py-2 px-4 bg-blue-600 text-white font-semibold rounded-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-opacity-50"
            >
              Edit Address
            </button>
          </form>
        </div>
      </div>
    );
  };

export default EditAddress