# Task Management System

Task Management System is a web application built with ASP.NET that allows users to efficiently manage and track tasks, user stories, and project progress.

## Table of Contents

- [Features](#features)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
- [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)

## Features

- **Task Management:** Create, update, and delete tasks.
- **User Stories:** Manage user stories and associated tasks.
- **Project Progress:** Track project progress with a user-friendly interface.
- **User Authentication:** Secure login and registration system for users.
- **Role-Based Access Control:** Assign roles (e.g., manager, employee) to control access.

## Getting Started

### Prerequisites

- [Visual Studio](https://visualstudio.microsoft.com/) with ASP.NET support.
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) for database storage.

### Installation

1. Clone the repository:

    ```bash
    git clone https://github.com/yourusername/task-management-system.git
    ```

2. Open the solution in Visual Studio.

3. Configure the database connection string in `appsettings.json`:

    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=TaskManagementDb;Trusted_Connection=True;MultipleActiveResultSets=true"
      }
    }
    ```

4. Run the application.

## Usage

1. Navigate to the application in your web browser (default: `https://localhost:5001`).
2. Register a new account or log in if you already have one.
3. Explore the dashboard and start managing tasks, user stories, and projects.

## Contributing

We welcome contributions! If you have improvements or new features to suggest, feel free to open an issue or create a pull request. Please follow our [contributing guidelines](CONTRIBUTING.md).

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for.
