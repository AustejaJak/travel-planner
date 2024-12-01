export async function fetchAPI(urlString, payload) {
    try {
        const url = new URL(urlString);
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(payload),
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        console.log("Request successful, no response body required.");
        return { success: true };

    } catch (err) {
        console.error("Error in fetch request:", err);
        return { error: err.message };
    }
}
