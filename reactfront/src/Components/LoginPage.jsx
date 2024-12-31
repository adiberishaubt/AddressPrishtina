import React, { useEffect, useState } from 'react';
import axiosInstance from '../Helpers/AxiosInstance';
import { useNavigate } from 'react-router-dom';
import { useToken } from '../TokenContext';

const LoginPage = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const navigate = useNavigate();
  const { token, setToken } = useToken();

    useEffect(() => {
      if(token) {
        navigate('/');
      }
    }, [token])
  

  const handleSubmit = (e) => {
    e.preventDefault();

    setError('');

    if (password.length < 8) {
      setError('Password must be at least 8 characters long');
      return;
    }

    axiosInstance.post("/v1/User/login", {
        "email": email,
        "password": password,
    })
    .then(res => {
      localStorage.setItem("userId", res.data.userId);
      localStorage.setItem("jwt", res.data.jwt);
      setToken(res.data.jwt);
      navigate('/');
    })
    .catch(e => {
        if(e?.response?.data?.errors) {
            const errorMessages = Object.values(e.response.data.errors).join('\n');
            setError(errorMessages);
        } else {
            setError(e.response.data);
        }
    });
  };

  return (
    <div className="flex items-center justify-center min-h-screen bg-gray-100">
      <div className="bg-white p-8 rounded-lg shadow-md w-80">
        <h2 className="text-2xl font-bold mb-6 text-center">Login</h2>
        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label htmlFor="email" className="block text-sm font-medium text-gray-700">Email</label>
            <input
              type="email"
              id="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
              className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
            />
          </div>

          <div>
            <label htmlFor="password" className="block text-sm font-medium text-gray-700">Password</label>
            <input
              type="password"
              id="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
              className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
            />
          </div>

          {error && <p className="text-red-500 text-sm">{error}</p>}

          <div>
            <button
              type="submit"
              className="w-full py-2 px-4 bg-blue-600 text-white font-semibold rounded-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-opacity-50"
            >
              Login
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default LoginPage;
