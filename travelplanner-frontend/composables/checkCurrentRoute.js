export function checkCurrentRoute(route) {
    const currentRouteName = computed(() => route.name);

    return {currentRouteName};
}