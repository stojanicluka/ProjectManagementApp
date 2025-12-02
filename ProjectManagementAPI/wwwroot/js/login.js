document.getElementById('login-form').addEventListener('submit', async function (event) {
    event.preventDefault();

    var user_name = document.getElementById('username').value;
    var password = document.getElementById('password').value;

    var data = {
        username: user_name,
        password: password
    };

    const response = await fetch('https://localhost:7239/users/login', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    });

    const result = await response.json();

    console.log(result);

    if (result.success) {
        sessionStorage.setItem('JWT', result.body);
        sessionStorage.setItem('username', user_name);
        console.log(sessionStorage.getItem('JWT'));
        document.getElementById('error-msg').textContent = 'Success!';
        window.location.href = "/index.html";
    } else {
        var errors = '';
        result.errors.forEach(function (error) {
            errors += error.message + '\n';
        });
        document.getElementById('error-msg').textContent = errors;
    }
   
    return;

});