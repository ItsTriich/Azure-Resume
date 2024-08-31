async function updateViewCount() {
    const apiUrl = 'https://functionapp-232545.azurewebsites.net/api/ViewCounter?'; 

    try {
        const response = await fetch(apiUrl, {
            method: 'POST', // or 'GET' depending on your function
            headers: {
                'Content-Type': 'application/json'
            }
        });

        if (!response.ok) {
            throw new Error('Network response was not ok');
        }

        const count = await response.json();
        document.getElementById('viewCount').innerText = count;
    } catch (error) {
        console.error('There was a problem with the fetch operation:', error);
    }
}

// Call the function to update the view count
updateViewCount();
