async function fetchAPI(urlString){
    try {
        var url = new URL(urlString);
        const response = await fetch(url);

        if (!response.ok){
            throw new Error(response.statusText);
        }

        const json = await response.json();
        console.log(json);
    } catch (err){
        console.error(err);
    }
}