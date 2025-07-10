# DiscountCodesGenerator

This a solution designed to generate random discount codes, and enable the user to consume each of them.

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

## Open your browser at https://localhost:6001

### 🚀 Postman Setup

#### 1. Open your Postman

#### 2. Configure a new collection

#### 3. Add a new gRPC request

<img src="https://raw.githubusercontent.com/GlennMateus/DiscountCodesGenerator/refs/heads/master/docs/images/postman-new-grpc-request.png"/>

#### 4. Generate Codes request

    1. Add the URL grpc://localhost:5001
    2.
