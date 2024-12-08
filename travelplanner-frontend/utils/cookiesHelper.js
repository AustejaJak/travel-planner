export function getCookies(name) {
    const cookies = document.cookie.split('; ');
    const cookie = cookies.find(row => row.startsWith(`${name}=`));
    return cookie ? cookie.split('=')[1] : null;
}

export function setCookies(name, value, minutes) {
    const date = new Date();
    date.setTime(date.getTime() + minutes * 60 * 1000);
    const expires = `expires=${date.toUTCString()}`;
    document.cookie = `${name}=${value}; ${expires}; secure; samesite=strict`;
}

export function removeCookies(name) {
    document.cookie = `${name}=; Max-Age=0; secure; samesite=strict`;
}