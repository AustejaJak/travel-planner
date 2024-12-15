import {jwtDecode} from "jwt-decode";

export function getRolesFromToken() {
    const accessToken = localStorage.getItem('AccessToken');

    if (accessToken) {
        try {
            const decodedToken = jwtDecode(accessToken);
            const roles = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

            if (Array.isArray(roles)) {
                return roles;
            } else {
                return [];
            }
        } catch (error) {
            console.error('Error decoding token:', error);
            return [];
        }
    } else {
        return [];
    }
}
