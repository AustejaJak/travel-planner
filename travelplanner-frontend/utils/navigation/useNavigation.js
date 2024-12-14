import { ref, watchEffect } from 'vue';
import { useRoute } from 'vue-router';

export function useNavigation() {
    const route = useRoute();
    const isLoggedIn = ref(false);

    if (typeof window !== "undefined"){
        const accessToken = localStorage.getItem('AccessToken');
        if (accessToken) {
            isLoggedIn.value = true;
        }
    }

    const navigation = ref([
        { name: 'Home', href: '/', current: false, requiresAuth: false },
        { name: 'All Trips', href: '/trips', current: false, requiresAuth: false },
        { name: 'Create a trip', href: '/createTrips', current: false, requiresAuth: true },
    ]);

    watchEffect(() => {
        const currentPath = route.path;
        navigation.value.forEach(item => {
            item.current = item.href === currentPath;
        });
    });

    return {
        navigation,
    };
}
