// composables/useAuth.js
import { useState } from '#app';
import { environmentAuth } from '~/utils/environments/environmentAuth.js';
import { apiAuthPaths } from '~/utils/paths/apiAuthPaths.js';
import { jwtDecode } from 'jwt-decode';
import { setCookies } from "~/utils/cookiesHelper.js";

export const useAuth = () => {
    const accessToken = useState('accessToken', () => null);
    const refreshToken = useState('refreshToken', () => null);
    const refreshTask = useState('refreshTask', () => null);

    const isTokenExpired = (token) => {
        try {
            const { exp } = jwtDecode(token);
            const now = Date.now() / 1000;
            return exp <= now;
        } catch (error) {
            console.error('Invalid token:', error);
            return true;
        }
    };

    // Registration function
    const register = async (userDetails) => {
        const baseUrl = environmentAuth.baseUrl;
        const urlRegister = `${baseUrl}/${apiAuthPaths.register}`;

        try {
            const response = await fetch(urlRegister, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(userDetails),
            });

            if (response.ok) {
                console.log('Registration successful.');
                return { status: response.status, message: 'Registration successful.' };
            } else {
                const errorData = await response.json();
                console.error('Registration failed:', errorData);
                throw new Error(errorData.message || 'Registration failed');
            }
        } catch (error) {
            console.error('Error during registration:', error);
            throw error;
        }
    };

    // Login function
    const login = async (credentials) => {
        const baseUrl = environmentAuth.baseUrl;
        const urlLogin = `${baseUrl}/${apiAuthPaths.login}`;

        try {
            const response = await fetch(urlLogin, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(credentials),
            });

            if (response.ok) {
                const data = await response.json();
                accessToken.value = data.accessToken;
                console.log(accessToken.value);
                refreshToken.value = data.refreshToken;
                setCookies(accessToken.value, 20);
                autoRefresh(); // Set up auto-refresh
                return { status: response.status, message: 'Login successful.' };
            } else if (response.status === 422) {
                return { status: response.status, message: 'Incorrect username or password.' };
            } else {
                const errorData = await response.json();
                console.error('Login failed:', errorData);
                throw new Error(errorData.message || 'Login failed');
            }
        } catch (error) {
            console.error('Error during login:', error);
            throw error;
        }
    };

    // Logout function
    const logout = async () => {
        const baseUrl = environmentAuth.baseUrl;
        const urlLogout = `${baseUrl}/${apiAuthPaths.logout}`;

        try {
            await fetch(urlLogout, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${accessToken.value}`,
                },
                body: JSON.stringify({ refreshToken: refreshToken.value }),
            });
            clearTokens();
        } catch (error) {
            console.error('Error during logout:', error);
            clearTokens();
        }
    };

    // Function to set up auto-refresh for tokens
    const autoRefresh = () => {
        if (!accessToken.value) return;
        try {
            const { exp } = jwtDecode(accessToken.value);
            const now = Date.now() / 1000;
            let timeUntilRefresh = exp - now;
            timeUntilRefresh -= 15 * 60; // Refresh 15 minutes before expiration

            if (timeUntilRefresh > 0) {
                refreshTask.value = setTimeout(() => {
                    refreshTokens();
                }, timeUntilRefresh * 1000);
            } else {
                refreshTokens();
            }
        } catch (error) {
            console.error('Error setting up auto-refresh:', error);
            clearTokens();
        }
    };

    // Function to refresh tokens
    const refreshTokens = async () => {
        const baseUrl = environmentAuth.baseUrl;
        const urlRefresh = `${baseUrl}/${apiAuthPaths.refreshToken}`;

        try {
            const response = await fetch(urlRefresh, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ refreshToken: refreshToken.value }),
            });

            if (response.ok) {
                const data = await response.json();
                accessToken.value = data.accessToken;
                refreshToken.value = data.refreshToken;
                setCookies(accessToken.value, 20);
                autoRefresh(); // Reset auto-refresh
            } else {
                console.error('Failed to refresh tokens:', await response.json());
                clearTokens();
            }
        } catch (error) {
            console.error('Error refreshing tokens:', error);
            clearTokens();
        }
    };

    // Function to clear tokens
    const clearTokens = () => {
        accessToken.value = null;
        refreshToken.value = null;
        if (refreshTask.value) {
            clearTimeout(refreshTask.value);
            refreshTask.value = null;
        }
    };

    return {
        accessToken,
        refreshToken,
        register,
        login,
        logout,
        autoRefresh,
        refreshTokens,
        clearTokens,
        isTokenExpired,
    };
};
