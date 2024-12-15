export const checkIfAdmin = (roles) => {
    for (let role of roles) {
        if (typeof window !== 'undefined') {
            if (role === 'Admin') {
                localStorage.setItem("Role", role);
                return true;
            }
        }
    }
    return false;
}