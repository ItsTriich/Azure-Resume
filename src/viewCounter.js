const apiUrl = 'https://functionapp-232545.azurewebsites.net/api/ViewCounter?'; 

try {
    const response = await fetch(apiUrl, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        }
    });

    if (!response.ok) {
        throw new Error('Network response was not ok');
    }