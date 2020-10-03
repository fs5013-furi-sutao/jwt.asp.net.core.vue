import axios from 'axios';
import authHeader from './auth-header';

const API_URL = 'http://localhost:4000/api/v1/';

class UserService {
  getPublicContent() {
    return axios.get(API_URL + 'users/public');
  }

  getUserBoard() {
    return axios.get(API_URL + 'users/', { headers: authHeader() });
  }

  getModeratorBoard() {
    return axios.get(API_URL + 'mod', { headers: authHeader() });
  }

  getAdminBoard() {
    return axios.get(API_URL + 'admin', { headers: authHeader() });
  }
}

export default new UserService();
