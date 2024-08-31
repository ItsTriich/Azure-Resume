async function updateViewCount() {
    const response = await fetch('http://localhost:7071/api/ViewCounter');
    const data = await response.json();
    document.getElementById('view-count').textContent = data.count;
}

window.onload = updateViewCount;
