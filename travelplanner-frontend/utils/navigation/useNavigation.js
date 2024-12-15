import { ref, watchEffect } from 'vue';
import { useRoute } from 'vue-router';
import {jwtDecode} from "jwt-decode";
import {getRolesFromToken} from "~/utils/rolesFromTokenHelper.js";
import {checkIfAdmin} from "~/utils/checkIfAdmin.js";

export function useNavigation() {
    const route = useRoute();
    const isLoggedIn = ref(false);
    const isAdmin = ref(false);

    if (typeof window !== "undefined"){
        const accessToken = localStorage.getItem('AccessToken');
        const roles = getRolesFromToken();
        isAdmin.value = checkIfAdmin(roles);
        if (accessToken) {
            isLoggedIn.value = true;
        }
    }
    const navigation = computed(() => {
        return [
            { name: 'Home', href: '/', current: false, requiresAuth: false },
            ...(!isAdmin.value ? [{ name: 'All Trips', href: '/trips', current: false, requiresAuth: false }] : []),
            { name: isAdmin.value ? 'Edit trips' : 'Create a trip', href: isAdmin.value ? '/adminTrips' : '/createTrips', current: false, requiresAuth: true },
        ];
    });

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
