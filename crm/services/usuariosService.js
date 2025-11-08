import { API_URL } from '../../config.js';

const usuarioService = (() => {
  const axios = require('axios');

  const instance = axios.create({
    baseURL: API_URL,
    withCredentials: true, // Include cookies in requests
  });

  instance.interceptors.request.use(
    config => {
      const token = localStorage.getItem('jwt-token');
      if (token) {
        config.headers['Authorization'] = 'Bearer ' + token;
      }
      return config;
    },
    error => {
      return Promise.reject(error);
    }
  );

  const addUsuario = async (usuario) => {
    try {
      const response = await instance.post('/api/Usuarios', usuario);
      return response.data;
    } catch (error) {
      console.log(error);
      return null;
    }
  };
   
  const getAllUsuarios = async () => {
    try {
      const response = await instance.get('/api/Usuarios');
      return response.data;
    } catch (error) {
      console.log(error);
      return null;
    }
  };

  const getById = async (id) => {
    try {
      const response = await instance.get(`/api/Usuarios/${id}`);
      return response.data;
    } catch (error) {
      console.log(error);
      return null;
    }
  };

  const editUsuario = async (id, usuario) => {
    try {
      const response = await instance.put(`/api/Usuarios/${id}`, usuario);
      return response.data;
    } catch (error) {
      console.log(error);
      return null;
    }
  };

  const removeUsuario = async (id) => {
    try {
      const response = await instance.delete(`/api/Usuarios/${id}`);
      return response.data;
    } catch (error) {
      console.log(error);
      return null;
    }
  };

  return {
    addUsuario,
    getAllUsuarios,
    getById,
    editUsuario,
    removeUsuario
  };
})();

export default usuarioService;
