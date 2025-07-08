flowchart TD
    %% Actors and Client
    Admin["Admin User"]:::actor
    Student["Student User"]:::actor
    Browser["Web Browser"]:::browser

    Admin --> Browser
    Student --> Browser

    %% UI Layer
    subgraph "UI (SinavOlusturma.UI)"
        UIApp["UI App"]:::frontend
        UIProj["UI.csproj"]:::frontend
        UIStartup["Startup.cs"]:::frontend
        UIProgram["Program.cs"]:::frontend
        AuthController["AuthController.cs"]:::frontend
        HomeController["HomeController.cs"]:::frontend
        Views["Views/"]:::frontend
        JwtMW_UI["JwtMiddleware.cs"]:::frontend
        WWWRoot["wwwroot/"]:::frontend
    end

    Browser -->|"HTTPS, Razor Pages"| UIApp

    %% API Layer
    subgraph "API (SinavOlusturma.API)"
        APIApp["API Service"]:::api
        APIProj["API.csproj"]:::api
        APIStartup["Startup.cs"]:::api
        APIProgram["Program.cs"]:::api
        AppSettingsAPI["appsettings.json"]:::api
        ControllersAPI["Controllers/"]:::api
        JwtMW_API["JwtMiddleware.cs"]:::api
        AllowAnon["AllowAnonymousAttribute.cs"]:::api
        AuthorizeAttr["AuthorizeAttribute.cs"]:::api
        DBFile["DB.db"]:::db
    end

    UIApp -->|"HTTPS/REST, JWT"| APIApp

    %% BLL Layer
    subgraph "Business Logic Layer (BLL)"
        BLLApp["Business Logic"]:::bll
        BLLProj["BLL.csproj"]:::bll
        Businesses["Businesses/"]:::bll
        Models["Models/"]:::bll
    end

    APIApp -->|"DI Calls"| BLLApp

    %% DAL Layer
    subgraph "Data Access Layer (DAL)"
        DALApp["Data Access"]:::dal
        DALProj["DAL.csproj"]:::dal
        Entities["Entities/"]:::dal
        Repositories["Repositories/"]:::dal
    end

    BLLApp -->|"Repository Calls"| DALApp
    DALApp -->|"SQL Commands"| DBFile

    %% Common Library
    CommonLib["Common Library"]:::common
    CommonProj["Common.csproj"]:::common
    Extensions["Extensions/"]:::common
    Helpers["Helpers/"]:::common

    CommonLib --> APIApp
    CommonLib --> BLLApp
    CommonLib --> DALApp

    %% Click Events
    click UIProj "https://github.com/ahmadmdabit/sinavolusturma/blob/master/UI/UI.csproj"
    click UIStartup "https://github.com/ahmadmdabit/sinavolusturma/blob/master/UI/Startup.cs"
    click UIProgram "https://github.com/ahmadmdabit/sinavolusturma/blob/master/UI/Program.cs"
    click AuthController "https://github.com/ahmadmdabit/sinavolusturma/blob/master/UI/Controllers/AuthController.cs"
    click HomeController "https://github.com/ahmadmdabit/sinavolusturma/blob/master/UI/Controllers/HomeController.cs"
    click Views "https://github.com/ahmadmdabit/sinavolusturma/tree/master/UI/Views/"
    click JwtMW_UI "https://github.com/ahmadmdabit/sinavolusturma/blob/master/UI/Helpers/JwtMiddleware.cs"
    click WWWRoot "https://github.com/ahmadmdabit/sinavolusturma/tree/master/UI/wwwroot/"

    click APIProj "https://github.com/ahmadmdabit/sinavolusturma/blob/master/API/API.csproj"
    click APIStartup "https://github.com/ahmadmdabit/sinavolusturma/blob/master/API/Startup.cs"
    click APIProgram "https://github.com/ahmadmdabit/sinavolusturma/blob/master/API/Program.cs"
    click AppSettingsAPI "https://github.com/ahmadmdabit/sinavolusturma/blob/master/API/appsettings.json"
    click ControllersAPI "https://github.com/ahmadmdabit/sinavolusturma/tree/master/API/Controllers/"
    click JwtMW_API "https://github.com/ahmadmdabit/sinavolusturma/blob/master/API/Helpers/JwtMiddleware.cs"
    click AllowAnon "https://github.com/ahmadmdabit/sinavolusturma/blob/master/API/Helpers/AllowAnonymousAttribute.cs"
    click AuthorizeAttr "https://github.com/ahmadmdabit/sinavolusturma/blob/master/API/Helpers/AuthorizeAttribute.cs"
    click DBFile "https://github.com/ahmadmdabit/sinavolusturma/blob/master/API/App_Data/DB.db"

    click BLLProj "https://github.com/ahmadmdabit/sinavolusturma/blob/master/BLL/BLL.csproj"
    click Businesses "https://github.com/ahmadmdabit/sinavolusturma/tree/master/BLL/Businesses/"
    click Models "https://github.com/ahmadmdabit/sinavolusturma/tree/master/BLL/Models/"

    click DALProj "https://github.com/ahmadmdabit/sinavolusturma/blob/master/DAL/DAL.csproj"
    click Entities "https://github.com/ahmadmdabit/sinavolusturma/tree/master/DAL/Entities/"
    click Repositories "https://github.com/ahmadmdabit/sinavolusturma/tree/master/DAL/Repositories/"

    click CommonProj "https://github.com/ahmadmdabit/sinavolusturma/blob/master/Common/Common.csproj"
    click Extensions "https://github.com/ahmadmdabit/sinavolusturma/tree/master/Common/Extensions/"
    click Helpers "https://github.com/ahmadmdabit/sinavolusturma/tree/master/Common/Helpers/"

    %% Styles
    classDef actor fill:#fff2cc,stroke:#333,shape:circle
    classDef browser fill:#f0f0f0,stroke:#333,shape:rect
    classDef frontend fill:#a2fca6,stroke:#333
    classDef api fill:#a6c8fc,stroke:#333
    classDef bll fill:#fca66a,stroke:#333
    classDef dal fill:#dddddd,stroke:#333
    classDef common fill:#e0a6fc,stroke:#333
    classDef db fill:#c69c6d,stroke:#333,shape:cylinder