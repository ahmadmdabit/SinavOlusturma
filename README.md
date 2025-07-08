# SinavOlusturma 

[![Ask DeepWiki](https://deepwiki.com/badge.svg)](https://deepwiki.com/ahmadmdabit/SinavOlusturma)

## Diagram 

[![Interactive Diagram](https://raster.shields.io/badge/Interactive_Diagram-lightgreen.png?logoColor=eeeeee&logo=data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAMAAAAoLQ9TAAAAzFBMVEUAAACTM+qTM+mTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+qTM+pYr7W1AAAAQ3RSTlMAAAAlZGhpWxQEBajeV3QHCsHcYO6ABgm/3V75/oTtnJ7TVIqWjivDzJWXcs8cy8CHbPvrwqIIXQHKJyiZJinO0P3jWa9vVAAAAKRJREFUGNNVz+kSgiAYhWFI2zRTWtCKtJ2sLG3fM7n/ewqBpun9+czwzQEA+AuIClDTi6VypfoVaJiMsZpVt5VAB3FoNFttLAW6HodOt0f6jgLkB4PhaDyZzhRQatvePAwXS6xgFUVr/oxRR8KGxTHJwXMVkCTZ7oLARwr2B3w8WWdMqQJ0ud7M++P5UsCHpSl7ZxlB8qiYLjINAfnnRLomh/0FPrSFFcj8a3ouAAAAAElFTkSuQmCC)](https://gitdiagram.com/ahmadmdabit/SinavOlusturma)

![The project's diagram](ahmadmdabit-SinavOlusturma-diagram.png)

## Project Info 

1. N-Tier Architecture
2. C# ASP.NET Core 3.1 (Jwt, Swagger, Web Scraping)
3. SQLite DB for data storage - in the folder API/App_Data - (Dapper)
4. ASP.NET Core MVC (Razor) for front end (MVC, Razor)

## Notes:
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

## License

Licensed under the [MIT license](LICENSE.md).
