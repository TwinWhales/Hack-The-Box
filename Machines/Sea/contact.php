<!DOCTYPE html>
<html lang="en">
<head>
<meta charset="UTF-8">
<meta name="viewport" content="width=device-width, initial-scale=1.0">
<title>Contact Form</title>
<style>
    body, html {
        margin: 0;
        padding: 0;
        height: 100%;
        overflow: hidden;
    }
    #background {
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: #00274c;
        z-index: -1;
    }
    #stars {
        width: 1px;
        height: 1px;
        background: #fff;
        position: absolute;
        animation: twinkle 2s infinite;
    }
    @keyframes twinkle {
        0% {
            opacity: 1;
        }
        100% {
            opacity: 0;
        }
    }
    #container {
        position: relative;
        z-index: 1;
        max-width: 600px;
        margin: 50px auto;
        padding: 20px;
        background-color: rgba(255, 255, 255, 0.9);
        border-radius: 10px;
        box-shadow: 0 0 20px rgba(0, 0, 0, 0.2);
    }
    h1 {
        text-align: center;
        margin-bottom: 30px;
        color: #00274c;
    }
    label {
        display: block;
        margin-bottom: 5px;
        color: #00274c;
    }
    input[type="text"],
    input[type="email"],
    input[type="number"] {
        width: calc(100% - 12px);
        padding: 8px;
        margin-bottom: 20px;
        border: 1px solid #ccc;
        border-radius: 5px;
    }
    input[type="text"]:focus,
    input[type="email"]:focus,
    input[type="number"]:focus {
        outline: none;
        border-color: #6fb3e0;
    }
    input[type="submit"] {
        background-color: #6fb3e0;
        color: #fff;
        border: none;
        padding: 10px 20px;
        border-radius: 5px;
        cursor: pointer;
    }
    input[type="submit"]:hover {
        background-color: #4a90c6;
    }
</style>
</head>
<body>


 <?php
    if ($_SERVER["REQUEST_METHOD"] == "POST") {
        $name = $_POST["name"];
        $email = $_POST["email"];
        $age = $_POST["age"];
        $country = $_POST["country"];
        $website = $_POST["website"];
        $message = "";
        $content = "Name: $name\nEmail: $email\nAge: $age\nCountry: $country\nWebsite: $website\n";

        $file_path = "/var/www/sea/messages/" . date("Y-m-d") . ".txt";

        if (file_put_contents($file_path, $content, FILE_APPEND) !== false) {
            $message = "<p style='color: green;'>Form submitted successfully!</p>";
        } else {
            $message = "<p style='color: red;'>Failed to submit form. Please try again later.</p>";
        }
    }
    ?>


    <div id="background">
        <div id="stars"></div>
    </div>
    <div id="container">
        <h1>Competition registration - Sea</h1>
        <?php echo $message; ?>

        <form action="<?php echo htmlspecialchars($_SERVER["PHP_SELF"]); ?>" method="post">
            <label for="name">Name:</label>
            <input type="text" id="name" name="name" required>

            <label for="email">Email:</label>
            <input type="email" id="email" name="email" required>

            <label for="age">Age:</label>
            <input type="number" id="age" name="age" required>

            <label for="country">Country:</label>
            <input type="text" id="country" name="country" required>

            <label for="website">Website:</label>
            <input type="text" id="website" name="website">

            <input type="submit" value="Submit">
        </form>
    </div>
</body>
</html>

