document.getElementById('registration-form').addEventListener('submit', async function (event) {
    event.preventDefault();

    var first_name = document.getElementById('first-name').value;
    var last_name = document.getElementById('last-name').value;
    var user_name = document.getElementById('username').value;
    var email = document.getElementById('email').value;
    var password = document.getElementById('password').value;
    var confirm_password = document.getElementById('confirm-password').value;

    if (password != confirm_password)
        document.getElementById('error-msg').innerHTML = 'Passwords do not match!'

    var data = {
        firstName: first_name,
        lastName: last_name,
        email: email,
        username: user_name,
        password: password
    };

    const response = await fetch('https://localhost:7239/users/register', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    });

    const result = await response.json();

    console.log(result);

    if (result.success) {
        document.getElementById('error-msg').innerHTML = 'Success!';
    } else {
        var errors = '';
        result.errors.forEach(function (error) {
            errors += error.message + '\n';
        });
        document.getElementById('error-msg').innerHTML = errors;
    }

    return;

});