import { ref, watchEffect } from 'vue';
import { useRoute } from 'vue-router';

export function useNavigation() {
    const route = useRoute();
    const navigation = ref([
        { name: 'Home', href: '/', current: false },
        { name: 'All Trips', href: '/all-trips', current: false },
        { name: 'Calendar', href: '/calendar', current: false },
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
