import axios from 'axios';

const axiosInstance = axios.create({
  baseURL: 'http://localhost:5167',
  headers: {
    'Content-Type': 'application/json',
  },
});

export default axiosInstance;