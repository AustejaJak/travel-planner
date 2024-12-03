export async function fetchAPI(urlString, payload, token) {
    try {
        const url = new URL(urlString);
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(payload),
        });

        if (token === true){
            return response;
        }

        return { success: true };

    } catch (err) {
        console.error("Error in fetch request:", err);
        return { error: err.message };
    }
}
