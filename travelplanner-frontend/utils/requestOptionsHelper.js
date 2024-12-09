export const requestOptionsHelper = {
    get: {
        method: 'GET',
        credentials: 'include',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${typeof window !== 'undefined' ? localStorage.getItem('AccessToken') : ''}`,
        },
    },

    post: (credentials) => ({
        method: 'POST',
        credentials: 'include',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${typeof window !== 'undefined' ? localStorage.getItem('AccessToken') : ''}`,
        },
        body: JSON.stringify(credentials),
    }),

    put: (credentials) => ({
        method: 'PUT',
        credentials: 'include',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${typeof window !== 'undefined' ? localStorage.getItem('AccessToken') : ''}`,
        },
        body: JSON.stringify(credentials),
    }),

    update: (credentials) => ({
        method: 'PUT',
        credentials: 'include',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${typeof window !== 'undefined' ? localStorage.getItem('AccessToken') : ''}`,
        },
        body: JSON.stringify(credentials),
    }),

    del: {
        method: 'DELETE',
        credentials: 'include',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${typeof window !== 'undefined' ? localStorage.getItem('AccessToken') : ''}`,
        },
    },
};
