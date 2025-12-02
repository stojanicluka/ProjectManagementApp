async function fetchUsers() {
    const response = await fetch('https://localhost:7239/users', {
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

    console.log(responseBody);


    if (!responseBody.success) {
        const errorMsg = 'An error has occured: \n';
        responseBody.errors.forEach(function (error) {
            errorMsg += error.message + '\n';
        });
        alert(errorMsg);
        return
    }


    var table = document.getElementById('user-table');
    var tbody = table.querySelector('tbody');
    responseBody.body.forEach(function (user) {
        var userRow = document.createElement('tr');

        var dataId = document.createElement('td');
        dataId.textContent = user.id;
        userRow.appendChild(dataId);

        var dataUsername = document.createElement('td');
        dataUsername.textContent = user.username;
        dataUsername.setAttribute('contenteditable', true);
        userRow.appendChild(dataUsername);

        var dataFirstName = document.createElement('td');
        dataFirstName.setAttribute('contenteditable', true);
        dataFirstName.textContent = user.firstName;
        userRow.appendChild(dataFirstName);

        var dataLastName = document.createElement('td');
        dataLastName.textContent = user.lastName;
        dataLastName.setAttribute('contenteditable', true);
        userRow.appendChild(dataLastName);

        var dataEmail = document.createElement('td');
        dataEmail.textContent = user.email;
        dataEmail.setAttribute('contenteditable', true);
        userRow.appendChild(dataEmail);

        const dataRole = document.createElement('td');
        const selectRole = document.createElement('select');

        const option0 = document.createElement('option');
        option0.value = 'ADMIN';
        option0.textContent = 'ADMIN';
        selectRole.appendChild(option0);

        const option1 = document.createElement('option');
        option1.value = 'MANAGER';
        option1.textContent = 'MANAGER';
        selectRole.appendChild(option1);

        const option2 = document.createElement('option');
        option2.value = 'TEAM_MEMBER';
        option2.textContent = 'TEAM_MEMBER';
        selectRole.appendChild(option2);

        const option3 = document.createElement('option');
        option3.value = 'INACTIVE';
        option3.textContent = 'INACTIVE';
        selectRole.appendChild(option3);

        selectRole.value = user.role.name;
        if (user.username == sessionStorage['username'])
            selectRole.setAttribute('disabled', '');
        dataRole.appendChild(selectRole);
        userRow.appendChild(dataRole);

        var saveBtn = document.createElement('button');
        saveBtn.setAttribute('href', 'tasks.html?id=' + dataId.textContent);
        saveBtn.textContent = 'Save'
        saveBtn.setAttribute('class', 'btn m-1');
        saveBtn.setAttribute('style', 'background-color: lightgray');
        saveBtn.setAttribute('username', user.username);
        saveBtn.addEventListener('click', async function (event) {
            var patch = {
                patches: [
                    {
                        field: 'Username',
                        value: dataUsername.textContent
                    },
                    {
                        field: 'FirstName',
                        value: dataFirstName.textContent
                    },
                    {
                        field: 'LastName',
                        value: dataLastName.textContent
                    },
                    {
                        field: 'Email',
                        value: dataEmail.textContent
                    }
                ]
            };

            const responsePatch = await fetch('https://localhost:7239/users/' + dataId.textContent, {
                method: 'PATCH',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + sessionStorage['JWT']
                },
                body: JSON.stringify(patch)
            });

            const patchResult = await responsePatch.json();
            if (!patchResult.success)
                alert(patchResult.errors[0]);

            const responseRole = await fetch('https://localhost:7239/users/role/' + dataId.textContent, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + sessionStorage['JWT']
                },
                body: JSON.stringify({ roleName: selectRole.value })
            });

            const roleResult = await responseRole.json();
            if (!roleResult.success)
                alert(roleResult.errors[0].type);

            if (sessionStorage['username'] == event.target.getAttribute('username')) {
                alert('You must sign in again.');
                sessionStorage.removeItem('JWT');
                window.location.href = "/index.html";
            }

            window.location.reload();
        });
        userRow.appendChild(saveBtn);

        var deleteBtn = document.createElement('button');
        deleteBtn.setAttribute('href', 'tasks.html?id=' + dataId.textContent);
        deleteBtn.textContent = 'Delete'
        deleteBtn.setAttribute('class', 'btn m-1');
        deleteBtn.addEventListener('click', async function (event) {
            const response = await fetch('https://localhost:7239/users/' + dataId.textContent, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + sessionStorage['JWT']
                }
            });
            window.location.reload();
        });
        deleteBtn.setAttribute('style', 'background-color: darkred; color: white');

        userRow.appendChild(deleteBtn);

        tbody.appendChild(userRow);

    });
}

fetchUsers();
