document.getElementById('task-form').addEventListener('submit', async function (event) {
    event.preventDefault();

    const projectId = new URLSearchParams(window.location.search).get('projectId');
    if (projectId == null)
        return;

    var title = document.getElementById('title').value;
    var description = document.getElementById('description').value;
    var deadline = document.getElementById('deadline').value;
    var status = document.getElementById('status').value;
    var priority = document.getElementById('priority').value;
    var assignedTo = document.getElementById('assigned-to').value;

    const data = {
        title: title,
        description: description,
        deadline: new Date(deadline).toISOString(),
        status: Number(status),
        priority: Number(priority),
        userId: assignedTo,
        projectId: projectId
    }

    const response = await fetch('https://localhost:7239/tasks', {
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

    window.location.href = "/tasks.html?projectId=" + projectId;

});