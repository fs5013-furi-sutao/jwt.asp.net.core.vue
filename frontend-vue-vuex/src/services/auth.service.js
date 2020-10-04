import axios from 'axios';

const API_URL = 'http://localhost:4000/api/v1/';

class AuthService {
  login(user) {
    return axios
      .post(API_URL + 'users/login', {
        username: user.username,
        password: user.password
      })
      .then(response => {
        if (response.data.accessToken) {
          localStorage.setItem('user', JSON.stringify(response.data));
        }

        return response.data;
      });
  }

  logout() {
    localStorage.removeItem('user');
  }

  register(user) {
    return axios.post(API_URL + 'users/register', {
      username: user.username,
      eMail: user.email,
      password: user.password
    });
  }
}

export default new AuthService();
