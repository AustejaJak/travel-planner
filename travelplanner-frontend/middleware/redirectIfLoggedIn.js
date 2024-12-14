export default defineNuxtRouteMiddleware(() => {
    if (typeof window !== "undefined") {
        const accessToken = localStorage.getItem('AccessToken');
        if (accessToken) {
            return navigateTo('/');
        }
    }
});
