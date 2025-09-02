class AuthService {
  private token: string | null = null;

  constructor() {
    if (typeof window !== "undefined") {
      this.token = localStorage.getItem("user");
    }
  }

  getToken() {
    return this.token;
  }

  setToken(token: string) {
    this.token = token;
    localStorage.setItem("user", token);
  }

  clearToken() {
    this.token = null;
    localStorage.removeItem("user");
  }

  isAuthenticated() {
    return !!this.token;
  }
}

export const authService = new AuthService();