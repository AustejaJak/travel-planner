import { useState } from '#app';
import { jwtDecode } from 'jwt-decode';
import { environmentAuth } from '~/utils/environments/environmentAuth.js';
import { apiAuthPaths } from '~/utils/paths/apiAuthPaths.js';

export const useAuth = () => {
    const refreshTask = useState('refreshTask', () => null);

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
                return { status: response.status, message: 'Registration successful.' };
            } else {
                const errorData = await response.json();
                throw new Error(errorData.message || 'Registration failed');
            }
        } catch (error) {
            console.error('Error during registration:', error);
            throw error;
        }
    };

    const login = async (credentials) => {
        const baseUrl = environmentAuth.baseUrl;
        const urlLogin = `${baseUrl}/${apiAuthPaths.login}`;

        try {
            const response = await fetch(urlLogin, {
                method: 'POST',
                credentials: 'include',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(credentials),
            });

            if (response.ok) {
                const { accessToken: token } = await response.json();
                localStorage.setItem('AccessToken', token);
                setTimeout(() => {
                    autoRefresh();
                }, 500);
                return { status: response.status, message: 'Login successful.' };
            } else if (response.status === 422) {
                return { status: response.status, message: 'Incorrect username or password.' };
            } else {
                const errorData = await response.json();
                throw new Error(errorData.message || 'Login failed');
            }
        } catch (error) {
            console.error('Error during login:', error);
            throw error;
        }
    };

    const logout = async () => {
        const baseUrl = environmentAuth.baseUrl;
        const urlLogout = `${baseUrl}/${apiAuthPaths.logout}`;

        try {
            const response = await fetch(urlLogout, {
                method: 'POST',
                credentials: 'include',
                headers: {
                    'Content-Type': 'application/json',
                },
            });

            if (!response.ok && response.status !== 422) {
                console.error('Logout failed:', await response.json());
            }
        } catch (error) {
            console.error('Error during logout:', error);
        } finally {
            clearTokens();
        }
    };

    const autoRefresh = () => {
        return new Promise((resolve, reject) => {
            let token = localStorage.getItem('AccessToken');

            if (token === null) {
                reject("No access token available");
                return;
            }

            try {
                const { exp } = jwtDecode(token);
                const now = Date.now() / 1000;
                let timeUntilRefresh = exp - now - 15 * 60;

                if (timeUntilRefresh > 0) {
                    refreshTask.value = setTimeout(() => {
                        refreshAccessTokens()
                            .then(resolve)
                            .catch(reject);
                    }, timeUntilRefresh * 1000);
                } else {
                    refreshAccessTokens()
                        .then(resolve)
                        .catch(reject);
                }
            } catch (error) {
                console.error("Error setting up auto-refresh:", error);
                clearTokens();
                reject(error);
            }
        });
    };


    const refreshAccessTokens = async () => {
        const baseUrl = environmentAuth.baseUrl;
        const urlRefresh = `${baseUrl}/${apiAuthPaths.refreshToken}`;

        try {
            const response = await fetch(urlRefresh, {
                method: 'POST',
                credentials: 'include',
            });

            if (response.ok) {
                const { accessToken: token } = await response.json();
                localStorage.setItem('AccessToken', token);
                await autoRefresh();
            } else {
                console.error('Failed to refresh tokens:', await response.json());
                clearTokens();
            }
        } catch (error) {
            console.error('Error refreshing tokens:', error);
            clearTokens();
        }
    };

    const clearTokens = () => {
        localStorage.removeItem('AccessToken');
        if (refreshTask.value) {
            clearTimeout(refreshTask.value);
            refreshTask.value = null;
        }
    };

    return {
        register,
        login,
        logout,
        autoRefresh,
        refreshAccessTokens,
        clearTokens,
    };
};
