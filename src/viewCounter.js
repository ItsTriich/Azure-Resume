async function updateViewCount() {
    const apiUrl = 'https://functionapp-5476.azurewebsites.net/api/ViewCounter?'; 

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

        const data = await response.json();
        document.getElementById('viewCount').innerText = `View count: ${data.count}`;
    } catch (error) {
        console.error('There was a problem with the fetch operation:', error);
    }
}

// Call the function to update the view count
updateViewCount();
