async function updateViewCount() {
    const apiUrl = 'https://functionapp-232545.azurewebsites.net/api/ViewCounter'; // Replace with your Azure Function URL

    try {
        const response = await fetch(apiUrl, {
            method: 'GET', // Assuming your function is using GET; change to POST if needed
            headers: {
                'Content-Type': 'application/json'
            }
        });

        if (!response.ok) {
            throw new Error('Network response was not ok');
        }

        // Ensure the response is valid JSON
        const data = await response.json();

        // Assuming data has a 'count' field, update the view count
        if (data && typeof data.count === 'number') {
            document.getElementById('viewCount').innerText = `View count: ${data.count}`;
        } else {
            throw new Error('Invalid JSON format: Missing or incorrect "count" field');
        }
    } catch (error) {
        console.error('There was a problem with the fetch operation:', error);
        document.getElementById('viewCount').innerText = 'Error fetching view count';
    }
}

// Call the function to update the view count
updateViewCount();
