function addHeaderButton(href, id, text) {
    var button = document.createElement('a');
    button.setAttribute('id', id);
    button.setAttribute('href', href);
    button.textContent = text;
    button.setAttribute('class', 'btn m-1');
    document.getElementById('navbar').appendChild(button);
}

async function adaptForLoggedUsers() {
    

    if (sessionStorage['JWT'] != null) {
        const role = await fetchRole();
        var loginBtn = document.getElementById('loginBtn');
        var registerBtn = document.getElementById('registerBtn');
        if (loginBtn != null)
            loginBtn.remove();
        if (registerBtn != null)
            registerBtn.remove();

        if (role == "ADMIN" || role == "MANAGER" || role == "TEAM_MEMBER")
            addHeaderButton('projects.html', 'projectBtn', 'Projects');
        if (role == "ADMIN")
            addHeaderButton('users.html', 'usersBtn', 'Users');
        addHeaderButton('index.html', 'logoutBtn', 'Sign out');

        if (role == "INACTIVE") {
            var inactiveNotice = document.getElementById("inactive-notice");
            if (inactiveNotice != null)
                inactiveNotice.style.display = "block";
        }

        document.getElementById('logoutBtn').addEventListener('click', function () {
            sessionStorage.removeItem('JWT');
        });

        if (role == "ADMIN" || role == "MANAGER") {
            var createProjectBtn = document.getElementById("add-project-btn");
            if (createProjectBtn != null)
                createProjectBtn.style.display = "block";

            var createTaskBtn = document.getElementById("add-task-btn");
            if (createTaskBtn != null)
                createTaskBtn.style.display = "block";
        }
        

        return;
    }

    var loginNotice = document.getElementById("login-notice");
    if (loginNotice != null)
        loginNotice.style.display = "block";
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

adaptForLoggedUsers();