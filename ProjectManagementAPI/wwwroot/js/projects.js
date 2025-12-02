async function fetchProjects() {
    const response = await fetch('https://localhost:7239/projects', {
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

    
    var table = document.getElementById('project-table');
    var tbody = table.querySelector('tbody');
    responseBody.body.forEach(function (project) {
        var projectRow = document.createElement('tr');
        
        var dataId = document.createElement('td');
        dataId.textContent = project.id;
        projectRow.appendChild(dataId);

        var dataTitle = document.createElement('td');
        dataTitle.textContent = project.title;
        if (role === "ADMIN" || role === "MANAGER")
            dataTitle.setAttribute('contenteditable', true);
        projectRow.appendChild(dataTitle);

        var dataDescription = document.createElement('td');
        dataDescription.textContent = project.description;
        if (role === "ADMIN" || role === "MANAGER")
            dataDescription.setAttribute('contenteditable', true);
        projectRow.appendChild(dataDescription);

        var dataStartDate = document.createElement('td');
        var startDateSelect = document.createElement('input');
        startDateSelect.setAttribute('type', 'date');
        if (role !== "ADMIN" && role !== "MANAGER")
            startDateSelect.setAttribute('disabled', '');
        startDateSelect.value = new Date(project.startDate).toISOString().split('T')[0];
        dataStartDate.appendChild(startDateSelect);
        projectRow.appendChild(dataStartDate);

        var dataDeadline = document.createElement('td');
        var deadlineSelect = document.createElement('input');
        if (role !== "ADMIN" && role !== "MANAGER")
            deadlineSelect.setAttribute('disabled', '');
        deadlineSelect.setAttribute('type', 'date');
        deadlineSelect.value = new Date(project.deadline).toISOString().split('T')[0];
        dataDeadline.appendChild(deadlineSelect);
        projectRow.appendChild(dataDeadline);

        var dataEndDate = document.createElement('td');
        var endDateSelect = document.createElement('input');
        endDateSelect.setAttribute('type', 'date');
        if (role !== "ADMIN" && role !== "MANAGER")
            endDateSelect.setAttribute('disabled', '');
        if (project.endDate != null)
            endDateSelect.value = new Date(project.endDate).toISOString().split('T')[0];
        dataEndDate.appendChild(endDateSelect);
        projectRow.appendChild(dataEndDate);

        const dataStatus = document.createElement('td');
        const selectStatus = document.createElement('select');

        const option0 = document.createElement('option');
        option0.value = '0';
        option0.textContent = 'In progress';
        selectStatus.appendChild(option0);

        const option1 = document.createElement('option');
        option1.value = '1';
        option1.textContent = 'Finished';
        selectStatus.appendChild(option1);

        const option2 = document.createElement('option');
        option2.value = '2';
        option2.textContent = 'Pending';
        selectStatus.appendChild(option2);

        selectStatus.value = project.status;
        if (role !== "ADMIN" && role !== "MANAGER")
            selectStatus.setAttribute('disabled', '');
        dataStatus.appendChild(selectStatus);
        projectRow.appendChild(dataStatus);

        if (role === "ADMIN" || role === "MANAGER") {
            var saveBtn = document.createElement('button');
            saveBtn.setAttribute('href', 'tasks.html?id=' + dataId.textContent);
            saveBtn.textContent = 'Save'
            saveBtn.setAttribute('class', 'btn m-1');
            saveBtn.setAttribute('id', 'save-project-btn');
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
                            field: 'StartDate',
                            value: new Date(startDateSelect.value).toISOString().split('T')[0] + 'T12:00:00'
                        },
                        {
                            field: 'Deadline',
                            value: new Date(deadlineSelect.value).toISOString().split('T')[0] + 'T12:00:00'
                        },
                        {
                            field: 'Status',
                            value: Number(selectStatus.value)
                        }
                    ]
                };

                if (endDateSelect.value !== "")
                    patch['patches'].push({
                        field: 'EndDate',
                        value: new Date(endDateSelect.value).toISOString().split('T')[0] + 'T12:00:00'
                    });

                const response = await fetch('https://localhost:7239/projects/' + dataId.textContent, {
                    method: 'PATCH',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': 'Bearer ' + sessionStorage['JWT']
                    },
                    body: JSON.stringify(patch)
                });
                window.location.reload();
            });

            projectRow.appendChild(saveBtn);
        }

        var taskBtn = document.createElement('button');
        taskBtn.setAttribute('href', 'tasks.html?id=' + dataId.textContent);
        taskBtn.textContent = 'Tasks'
        taskBtn.setAttribute('class', 'btn m-1');
        taskBtn.setAttribute('style', 'background-color: lightblue');
        taskBtn.setAttribute('project-id', project.id);
        taskBtn.addEventListener('click', function (event) {
            window.location.href = "/tasks.html?projectId=" + event.target.getAttribute('project-id');
        })

        projectRow.appendChild(taskBtn);

        if (role === "ADMIN") {
            var deleteBtn = document.createElement('button');
            deleteBtn.setAttribute('href', 'tasks.html?id=' + dataId.textContent);
            deleteBtn.textContent = 'Delete'
            deleteBtn.setAttribute('class', 'btn m-1');
            deleteBtn.setAttribute('style', 'background-color: darkred; color: white');
            deleteBtn.addEventListener('click', async function (event) {
                const response = await fetch('https://localhost:7239/projects/' + dataId.textContent, {
                    method: 'DELETE',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': 'Bearer ' + sessionStorage['JWT']
                    }
                });
                window.location.reload();
            });
            deleteBtn.setAttribute('id', 'delete-project-btn');
            projectRow.appendChild(deleteBtn);
        }
        tbody.appendChild(projectRow);

    });
}

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

fetchProjects();
