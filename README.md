# SinavOlusturma

Project details:
1. C# ASP.NET Core 3.1
2. DB: SQLite (in the folder API/App_Data)
3. N-Tier Architecture

Notes:
1. Be sure that the sql connection string in appsettings.json in the API project, be sure that is correct on your machine.
2. Be sure that the API project and the UI project are startup projects.
3. Login info: 
  Normal user: 
     username: demo
     password: 123
 Admin:
    username: admin
    password: 321
4. Home page has the list of all exams and you can show, create, edit or delete any one.
5. If you login with the normal user info, you will be redirected to Student/Exams page. This page shows the user attended exams.
6. If you want some user to attened an exam you can open the Home page with admin info, and pressing the grey button with heman icon, ait will open the student exam page. And the just share the link with the student,
7. Student cant attened the exam if he not logged in to the application.
