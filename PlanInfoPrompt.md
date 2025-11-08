# PlanInfoPrompt

Implement this project specification as a .Net 9 Blazor Hybrid application using standard .Net 9 best practices and Blazor Bootstrap components.

List pages should use the Blazor Grid component with Server side filtering, paging and sorting.

The application should be sleek, modern,responsive and ready for mobile.

## The Ideal Project Structure

Here’s what the solution structure and its dependencies should look like:

1. **`YourApp.Core` (Standard .NET Class Library)**
    * **Contents:**
        * Services (e.g., `ProductService`, `UserService`, `WeatherService`).
        * Data Models/Entities (e.g., `Product`, `User`).
        * Interfaces (e.g., `IProductService`).
        * Logic that uses `HttpClient` to call external APIs.
    * **Dependencies:** None. This library has zero dependencies on Blazor, MAUI, or any UI framework. It's pure C# logic.

2. **`YourApp.UI` (Razor Class Library - RCL)**
    * **Contents:**
        * All your `.razor` components (pages, layouts, shared components).
        * Shared static assets like CSS and JavaScript.
    * **Dependencies:** It **references `YourApp.Core`** so that your components can inject and use the services.
    * **Responsibility:** To define the standard UI components and layouts for the app that can be shared between web and native.

3. **`YourApp.WebApp` (Blazor Web App Project)**
    * **Contents:**
        * The `Program.cs` file for web hosting.
        * Web-specific configurations (`appsettings.json`).
        * The `wwwroot` folder.
    * **Dependencies:** It **references `YourApp.UI`** (and transitively `YourApp.Core`).
    * **Responsibility:** To host the UI for the web.

4. **`YourApp.MauiApp` (.NET MAUI Blazor App Project)**
    * **Contents:**
        * The `Program.cs` file for MAUI app hosting.
        * Platform-specific resources (icons, splash screens).
        * The `BlazorWebView` control.
    * **Dependencies:** It also **references `YourApp.UI`** (and transitively `YourApp.Core`).
    * **Responsibility:** To host the UI for native desktop and mobile.

## The Ideal Project Folder Structure

Your file structure should look something like this:

```markdown
/YourAppSolution/
├── YourApp.sln
├── .gitignore
├── README.md
└── src/
    ├── YourApp.Core/
    │   └── YourApp.Core.csproj
    ├── YourApp.UI/
    │   └── YourApp.UI.csproj
    ├── YourApp.WebApp/
    │   └── YourApp.WebApp.csproj
    └── YourApp.MauiApp/
        └── YourApp.MauiApp.csproj
```
