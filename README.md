# DiscountCodesGenerator

> A modular, layered .NET 8 application for generating and managing discount codes, containerized with Docker.

---

## 🚀 How to Run

### 🔧 Prerequisites

-   [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
-   [Docker Desktop](https://www.docker.com/products/docker-desktop) (optional for container execution)
-   [Postman](https://www.postman.com/downloads/)
-   **Make sure you're not using the ports 5000, 5001, 6000, 6001**

---

### ✅ Option 1: Run with Docker

> The easiest way to spin up the application locally.

#### 1. Clone the repository

```bash
git clone https://github.com/GlennMateus/DiscountCodesGenerator.git
cd DiscountCodesGenerator
```

#### 2. Run with Docker Compose

```bash
docker-compose up --build
```

#### 3. Access the Web UI

Open your browser at https://localhost:6001

### ✅ Option 2: Run Manually VS2022

#### 1. Clone the repository

```bash
git clone https://github.com/GlennMateus/DiscountCodesGenerator.git
```

#### 2. Open the code in the VS2022

#### 3. Set docker-compose as the Startup

    1. Option 1: Set it at the top menu in your IDE

<img src="https://raw.githubusercontent.com/GlennMateus/DiscountCodesGenerator/refs/heads/master/docs/images/docker-compose-opt-1.png"/>

    2. Option 2: Right click in the docker-compose file > set startup prokect

<img src="https://raw.githubusercontent.com/GlennMateus/DiscountCodesGenerator/refs/heads/master/docs/images/docker-compose-opt-2.png"/>

#### 4. Compose up

Right click in the docker-compose > Compose Up, and wait until the end of the process
<img src="https://raw.githubusercontent.com/GlennMateus/DiscountCodesGenerator/refs/heads/master/docs/images/docker-compose-up.png"/>

If everything work as expected, you should be able to watch the list of the images in your Docker desktop.
<img src="https://raw.githubusercontent.com/GlennMateus/DiscountCodesGenerator/refs/heads/master/docs/images/docker-desktop-listing.png"/>

#### 5. Access your Web UI

Open your browser at https://localhost:6001

### 🚀 Postman Setup

#### 1. Open your Postman

#### 2. Configure a new collection

#### 3. Add a new gRPC request

<img src="https://raw.githubusercontent.com/GlennMateus/DiscountCodesGenerator/refs/heads/master/docs/images/postman-new-grpc-request.png"/>

#### 4. Generate Codes request

> This step is the same for all the services: - GenerateCodesService/GenerateCodes - ConsumeCodeService/ConsumeCode - GetCodesService/GetCodes

    1. Add the URL grpc://localhost:5001
    2. Go to Service definition and click on the 🔄️ button

<img src="https://raw.githubusercontent.com/GlennMateus/DiscountCodesGenerator/refs/heads/master/docs/images/postman-request-server-reflection-config.png"/>

    3. At the right side of the URL, choose the service to run
    4. Click the "Use Example Message" button to auto-fill the message

<img src="https://raw.githubusercontent.com/GlennMateus/DiscountCodesGenerator/refs/heads/master/docs/images/postman-request-message-example.png"/>

    5. Make sure to configure Postman to not verify the server certificates

<img src="https://raw.githubusercontent.com/GlennMateus/DiscountCodesGenerator/refs/heads/master/docs/images/postman-config-SSL-verification.png" />

---

## 📦 Solution Overview

This solution consists of the following projects:

| Project                       | Description                                                       |
| ----------------------------- | ----------------------------------------------------------------- |
| `Common`                      | Shared utilities used across all layers.                          |
| `DiscountCodesGenerator`      | Core business logic for generating and validating discount codes. |
| `Web.DiscountCodesGenerator`  | Razor Pages-based UI for interacting with the code generator.     |
| `Test.DiscountCodesGenerator` | Unit tests for validating business logic.                         |

## 🧩 Project Communication Diagram

<img src="https://github.com/GlennMateus/DiscountCodesGenerator/blob/master/docs/images/project-diagram.png"/>
API invokes MediatR commands/queries in Application.

Application uses Domain interfaces and Models DTOs.

Infrastructure implements Domain repositories/services and persists data.

Dependency Direction:
API → Application → Domain ← Infrastructure

## 🧱 Architecture

This application follows a **Layered Architecture**:

-   **Presentation Layer**: `Web.DiscountCodesGenerator`
-   **Business Layer**: `DiscountCodesGenerator`
-   **Shared Models**: `Common`

### 🧠 Design Patterns Used

-   **Dependency Injection** – services are injected via constructors.
-   **Repository Pattern** – data access logic is abstracted.
-   **Strategy Pattern** – code generation algorithms are pluggable.
-   **Factory Pattern** – creation logic centralized for extensibility.
-   **CQRS Pattern** - it separates the read and write operations into different models
-   **MVC/Razor Pages** – in the web interface.

## 🐳 Containerization

The solution is containerized with:

-   Individual `Dockerfile`s for each service.
-   A `docker-compose.yml` that orchestrates the services.
