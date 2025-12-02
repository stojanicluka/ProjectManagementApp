async function fetchTasks(status, username) {
   
    var projectId = new URLSearchParams(window.location.search).get('projectId');
    if (projectId == null)
        return;

    var uri = 'https://localhost:7239/tasks?projectId=' + projectId;

    if (status != null)
        uri += '&status=' + status;

    if (username != null)
        uri += '&username=' + username;

    const response = await fetch(uri, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + sessionStorage['JWT']
        }
    });

    if (response.status === 401) {
        alert("Unauthorized");
        return;
    }

    const responseBody = await response.json();


    if (!responseBody.success) {
        const errorMsg = 'An error has occured: \n';
        responseBody.errors.forEach(function (error) {
            errorMsg += error.message + '\n';
        });
        alert(errorMsg);
        return
    }

    const role = await fetchRole();

    var table = document.getElementById('task-table');
    var tbody = table.querySelector('tbody');
    while (tbody.firstChild) {
        tbody.removeChild(tbody.firstChild);
    }
    responseBody.body.forEach(function (task) {
        var taskRow = document.createElement('tr');

        var dataId = document.createElement('td');
        dataId.textContent = task.id;
        taskRow.appendChild(dataId);

        var dataTitle = document.createElement('td');
        dataTitle.textContent = task.title;
        dataTitle.setAttribute('contenteditable', true);
        taskRow.appendChild(dataTitle);

        var dataDescription = document.createElement('td');
        dataDescription.textContent = task.description;
        dataDescription.setAttribute('contenteditable', true);
        taskRow.appendChild(dataDescription);

        var dataDeadline = document.createElement('td');
        var deadlineSelect = document.createElement('input');
        deadlineSelect.setAttribute('type', 'date');
        deadlineSelect.value = new Date(task.deadline).toISOString().split('T')[0];
        dataDeadline.appendChild(deadlineSelect);
        taskRow.appendChild(dataDeadline);

        const dataStatus = document.createElement('td');
        const selectStatus = document.createElement('select');

        const option00 = document.createElement('option');
        option00.value = '0';
        option00.textContent = 'In progress';
        selectStatus.appendChild(option00);

        const option01 = document.createElement('option');
        option01.value = '1';
        option01.textContent = 'Finished';
        selectStatus.appendChild(option01);

        const option02 = document.createElement('option');
        option02.value = '2';
        option02.textContent = 'Pending';
        selectStatus.appendChild(option02);

        selectStatus.value = task.status;

        dataStatus.appendChild(selectStatus);
        taskRow.appendChild(dataStatus);

        const dataPriority = document.createElement('td');
        const selectPriority = document.createElement('select');

        const option10 = document.createElement('option');
        option10.value = '0';
        option10.textContent = 'Low';
        selectPriority.appendChild(option10);

        const option11 = document.createElement('option');
        option11.value = '1';
        option11.textContent = 'Medium';
        selectPriority.appendChild(option11);

        const option12 = document.createElement('option');
        option12.value = '2';
        option12.textContent = 'High';
        selectPriority.appendChild(option12);

        selectPriority.value = task.priority;

        dataPriority.appendChild(selectPriority);
        taskRow.appendChild(dataPriority);

        var dataAssignedTo = document.createElement('td');
        dataAssignedTo.textContent = task.username;
        dataAssignedTo.setAttribute('contenteditable', true);
        taskRow.appendChild(dataAssignedTo);

        var saveBtn = document.createElement('button');
        saveBtn.setAttribute('href', 'tasks.html?id=' + dataId.textContent);
        saveBtn.textContent = 'Save'
        saveBtn.setAttribute('class', 'btn m-1');
        saveBtn.setAttribute('style', 'background-color: lightgray');
        saveBtn.addEventListener('click', async function (event) {
            var patch = {
                patches: [
                    {
                        field: 'Title',
                        value: dataTitle.textContent
                    },
                    {
                        field: 'Description',
                        value: dataDescription.textContent
                    },
                    {
                        field: 'Deadline',
                        value: new Date(deadlineSelect.value).toISOString().split('T')[0] + 'T12:00:00'
                    },
                    {
                        field: 'Priority',
                        value: Number(selectPriority.value)
                    },
                    {
                        field: 'Status',
                        value: Number(selectStatus.value)
                    },
                    {
                        field: 'UserId',
                        value: dataAssignedTo.textContent
                    }
                ]
            };

            const response = await fetch('https://localhost:7239/tasks/' + dataId.textContent, {
                method: 'PATCH',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + sessionStorage['JWT']
                },
                body: JSON.stringify(patch)
            });
            window.location.reload();
        });

        taskRow.appendChild(saveBtn);

        if (role === "ADMIN" || role === "MANAGER") {
            var deleteBtn = document.createElement('button');
            deleteBtn.setAttribute('href', 'tasks.html?id=' + dataId.textContent);
            deleteBtn.textContent = 'Delete'
            deleteBtn.setAttribute('class', 'btn m-1');
            deleteBtn.setAttribute('style', 'background-color: darkred; color: white');
            deleteBtn.addEventListener('click', async function (event) {
                const response = await fetch('https://localhost:7239/tasks/' + dataId.textContent, {
                    method: 'DELETE',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': 'Bearer ' + sessionStorage['JWT']
                    }
                });
                window.location.reload();
            });

            taskRow.appendChild(deleteBtn);
        }
        tbody.appendChild(taskRow);

    });
}

document.getElementById('filterForm').addEventListener('submit', async function (event) {
    event.preventDefault();

    var statusInput = document.getElementById('statusInput');
    var usernameInput = document.getElementById('usernameInput');

    var statusParam = Number(statusInput.value);
    if (statusInput.value === '')
        statusParam = null;

    var usernameParam = usernameInput.value;
    if (usernameParam.value === '')
        usernameParam = null;

    fetchTasks(statusParam, usernameParam);
});

fetchTasks(null, null);
document.getElementById('add-task-btn').addEventListener('click', function () {
    var projectId = new URLSearchParams(window.location.search).get('projectId');
    window.location.href = "/create_task.html?projectId=" + projectId;
})

async function fetchRole() {
    const response = await fetch('https://localhost:7239/users/role/myrole', {
        method: 'GET',
        headers: {
            'Content-Type': 'None',
            'Authorization': "Bearer " + sessionStorage['JWT']
        },
    });

    const resp = await response.json();
    return resp.body.value;
}
