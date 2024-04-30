# Forum

## Overview
Forum is a versatile platform designed to facilitate online discussions and interactions. Users can register, create topics, write comments, and manage their personal information. The platform supports various roles, including administrators, authenticated users, and guests, each with different levels of access and privileges.

## Roles
- **Administrator**: Administrators have full control over the platform, including managing users and topics. They can ban/unban users, edit topics, and view user information.
- **User**: Authenticated users can create topics, write comments, and interact with other users' posts.
- **Guest**: Unauthenticated users can browse topics and comments without the ability to post or interact.

## Functionality Description
### Role: User
After authentication, users are directed to the news feed page, displaying various topics along with the number of comments posted. Clicking on a topic opens it, revealing the comments. Users can create new topics and write comments on existing topics.

### Role: Administrator
Administrators have access to a separate administration page where they can manage users and topics. They can view user information, ban/unban users, and edit existing topics.

## Implemented Features
- **Archiving**: Posts older than a certain period automatically move to an archive, preventing further comments but remaining accessible for viewing. This is managed by a background worker.
- **File Upload**: Users can upload images for their topics, enhancing the visual experience.
- **Pagination**: Implemented pagination for topics to improve performance and navigation.
- **Validation**: Client-side and server-side validation ensure data integrity and security.
- **Health Checks**: Implemented health checks to monitor system status and performance.
- **Unit Testing**: Implemented unit tests to cover critical functionality and ensure code quality. Unit testing primarily focuses on testing individual components and classes within the application layer.

## Technical Details
- **Database**: Utilized SQL Server for data storage.
- **ORM**: Employed Entity Framework Core Code First for data access, including seeding and migration.
- **ASP.NET Core MVC and Web API**: Built the web application using ASP.NET Core MVC for server-side rendering and user interaction. Developed a RESTful API using ASP.NET Core Web API to provide data access and integration capabilities.
- **Workers**: Implemented background workers for archiving old posts and managing user bans. The archiving worker moves old posts to an archive, while the ban management worker handles user bans and unbans.
- **API Versioning**: Ensured compatibility and smooth upgrades with API versioning.
- **Middlewares**: Implemented custom middlewares for logging, authentication, and exception handling.
- **Authorization**: Utilized ASP.NET Core authorization for role-based access control.
- **Swagger**: Generated API documentation using Swagger for easier testing and integration.
- **Fluent API**: Configured Entity Framework Core using Fluent API for more control over database mappings.
- **Mapping**: Used Mapster for object-to-object mapping.
- **JWT Authentication**: Implemented JWT authentication for secure user authentication.
- **Clean Architecture**: Followed clean architecture principles for a modular and maintainable codebase.
- **Asynchronous Methods**: Utilized asynchronous methods and cancellation tokens for improved performance and responsiveness.

## Getting Started
To run the project locally, follow these steps:
1. Clone this repository.
2. Open the solution in Visual Studio or your preferred IDE.
3. Build the solution.
4. Run the application.

## Usage
Once the application is running, users can interact with the API endpoints using tools like Postman or Swagger documentation.

## Contributing
Contributions are welcome! If you'd like to contribute to this project, please fork the repository and submit a pull request with your changes.

## License
This project is licensed under the MIT License. Feel free to use and modify it according to your needs.
