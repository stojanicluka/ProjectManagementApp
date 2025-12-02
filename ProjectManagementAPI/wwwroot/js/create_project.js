document.getElementById('project-form').addEventListener('submit', async function (event) {
    event.preventDefault();

    var title = document.getElementById('title').value;
    var description = document.getElementById('description').value;
    var startDate = document.getElementById('start-date').value;
    var deadline = document.getElementById('deadline').value;
    var status = document.getElementById('status').value;

    const data = {
        title: title,
        description: description,
        startDate: new Date(startDate).toISOString(),
        deadline: new Date(deadline).toISOString(),
        status: Number(status)
    }

    const response = await fetch('https://localhost:7239/projects', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + sessionStorage['JWT']
        },
        body: JSON.stringify(data)
    });

    if (response.status === 401) {
        alert("Unauthorized");
        return;
    }

    const responseBody = await response.json();
    console.log(responseBody);

    if (!responseBody.success) {
        const errorMsg = 'An error has occured: \n';
        responseBody.errors.forEach(function (error) {
            errorMsg += error.message + '\n';
        });
        alert(errorMsg);
        return;
    }

    window.location.href = "/projects.html";

});