# 🏎️ F1 Race Engineer Platform

A full-stack platform for F1 race strategy simulation and AI-powered race engineering assistance. This project combines modern backend architecture, real-time data processing, and intelligent agent orchestration to provide comprehensive race strategy analysis and recommendations.

## 🎯 Project Vision

Build a production-grade system that demonstrates senior-level engineering capabilities across:

- **Backend Architecture**: Robust .NET Core API with clean architecture patterns
- **Data Engineering**: Complex relational modeling with EF Core and PostgreSQL
- **Full-Stack Development**: React-based dashboard with real-time updates
- **System Design**: Background jobs, caching, queues, and distributed processing
- **AI Integration**: Intelligent race engineer agent with tool-calling capabilities
- **DevOps**: Containerization, deployment, and observability

## 🏗️ Architecture

```
┌─────────────────────────────────────────────────────────┐
│                    React Dashboard                      │
│        (Strategy Editor, Race Data, Analytics)          │
└────────────────────────┬────────────────────────────────┘
                         │
┌────────────────────────▼────────────────────────────────┐
│                  ASP.NET Core API                       │
│     (CRUD, Auth, Validation, Real-time Events)          │
└─────┬──────────────────┬────────────────────┬───────────┘
      │                  │                    │
┌─────▼──────┐   ┌───────▼─────┐    ┌────────▼──────────┐
│ PostgreSQL │   │Redis Cache  │    │Background Jobs    │
│  (EF Core) │   │             │    │(Race Simulations) │
└────────────┘   └─────────────┘    └───────────────────┘
                                              │
                              ┌───────────────▼───────────┐
                              │  AI Race Engineer Agent   │
                              │  (Strategy Optimization)  │
                              └───────────────────────────┘
```

## 🚀 Tech Stack

### Backend
- **.NET 8.0** - Modern C# backend framework
- **ASP.NET Core** - RESTful API
- **Entity Framework Core** - ORM and migrations
- **PostgreSQL** - Primary database
- **Redis** - Caching layer

### Frontend
- **React** - UI framework
- **TypeScript** - Type-safe frontend development
- **React Query** - Data fetching and state management
- **Chart.js / Recharts** - Data visualization

### Infrastructure
- **Docker** - Containerization
- **Background Job Runner** - Async processing
- **Message Queue** - Event-driven architecture

### AI Layer
- **Agent Orchestration** - Custom tool-calling framework
- **LLM Integration** - Strategy analysis and recommendations
- **Evaluation Framework** - Agent performance tracking

## 📊 Core Features

### Phase 1: Core Backend (Weeks 1-2)
- ✅ Complete CRUD API for F1 entities
- ✅ Database schema and migrations
- ✅ Validation and error handling
- ✅ Logging and configuration management

### Phase 2: Production Backend (Weeks 3-4)
- ✅ Authentication and authorization
- ✅ Comprehensive test coverage
- ✅ Advanced querying (pagination, filtering, sorting)
- ✅ Repository pattern implementation

### Phase 3: Full-Stack Integration (Weeks 5-6)
- ✅ React dashboard with race data visualization
- ✅ Strategy editor and comparison tools
- ✅ Real-time API integration
- ✅ Form validation and error handling

### Phase 4: System Design (Weeks 7-8)
- ✅ Background job processing
- ✅ Race simulation engine
- ✅ Redis caching strategy
- ✅ Docker containerization
- ✅ Environment-based configuration

### Phase 5: AI Agent Layer (Weeks 9-10)
- ✅ Race Engineer AI Agent
- ✅ Tool framework (`getRaceData`, `runSimulation`, `compareStrategies`, `saveStrategy`)
- ✅ Prompt engineering and orchestration
- ✅ Agent logging and observability

### Phase 6: Production Hardening (Weeks 11-12)
- ✅ Evaluation scenarios and success metrics
- ✅ Cost and performance tracking
- ✅ Distributed tracing
- ✅ Architecture documentation
- ✅ Demo and presentation materials

## 🗂️ Domain Model

### Core Entities

**Driver**
- Personal information
- Current team affiliation
- Performance statistics
- Historical data

**Team**
- Constructor details
- Current lineup
- Performance metrics
- Resource allocation

**Race**
- Circuit information
- Weather conditions
- Session data (Practice, Qualifying, Race)
- Results and telemetry

**Strategy**
- Tire compounds
- Pit stop windows
- Fuel management
- Risk assessment

## 🛠️ Getting Started

### Prerequisites

- .NET 8.0 SDK
- Node.js 18+
- Docker Desktop
- PostgreSQL 15+
- Redis (optional for local dev)

### Installation

```bash
# Clone the repository
git clone <repository-url>
cd F1Sim

# Backend setup
cd src/F1Sim.Api
dotnet restore
dotnet ef database update

# Frontend setup
cd ../../client
npm install

# Run with Docker
docker-compose up
```

### Configuration

Create `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=f1sim;Username=postgres;Password=yourpassword"
  },
  "Redis": {
    "ConnectionString": "localhost:6379"
  },
  "JWT": {
    "SecretKey": "your-development-secret-key",
    "Issuer": "F1Sim",
    "Audience": "F1Sim.Api"
  }
}
```

### Running Locally

```bash
# Terminal 1 - Backend API
cd src/F1Sim.Api
dotnet run

# Terminal 2 - Frontend
cd client
npm start

# Terminal 3 - Background Jobs (if applicable)
cd src/F1Sim.Jobs
dotnet run
```

## 📚 API Documentation

Once running, access the API documentation at:
- Swagger UI: `http://localhost:5000/swagger`
- API Endpoints: `http://localhost:5000/api`

### Key Endpoints

```
GET    /api/drivers          - List all drivers
POST   /api/drivers          - Create driver
GET    /api/drivers/{id}     - Get driver details
PUT    /api/drivers/{id}     - Update driver
DELETE /api/drivers/{id}     - Delete driver

GET    /api/races            - List races
POST   /api/races            - Create race
GET    /api/races/{id}       - Get race details

GET    /api/strategies       - List strategies
POST   /api/strategies       - Create strategy
POST   /api/strategies/simulate - Run simulation
POST   /api/strategies/compare  - Compare strategies
```

## 🧪 Testing

### Unit & Integration Tests

```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test /p:CollectCoverage=true

# Frontend tests
cd client
npm test
```

### Manual API Testing with .http Files

Visual Studio and VS Code provide built-in support for `.http` files for testing REST APIs without external tools like Postman.

**Location**: `src/F1Sim.Api/F1Sim.Api.http`

#### How to Use

1. **Open the file**: `src/F1Sim.Api/F1Sim.Api.http` in Visual Studio or VS Code
2. **Start the API**: Run the project (`F5` or `dotnet run`)
3. **Execute requests**: Click the "Send Request" link above any request

#### Example Workflow: Creating a Complete F1 Team Setup

```http
### Step 1: Create Mercedes Team
POST http://localhost:5254/api/teams
Content-Type: application/json

{
  "name": "Mercedes-AMG Petronas F1 Team",
  "constructor": "Mercedes",
  "foundedYear": 2010,
  "baseLocation": "Brackley, United Kingdom",
  "championshipsWon": 8
}

### Step 2: Create Red Bull Team
POST http://localhost:5254/api/teams
Content-Type: application/json

{
  "name": "Oracle Red Bull Racing",
  "constructor": "Red Bull Racing",
  "foundedYear": 2005,
  "baseLocation": "Milton Keynes, United Kingdom",
  "championshipsWon": 6
}

### Step 3: Create Lewis Hamilton (for Team ID 1 - Mercedes)
POST http://localhost:5254/api/drivers
Content-Type: application/json

{
  "firstName": "Lewis",
  "lastName": "Hamilton",
  "nationality": "British",
  "dateOfBirth": "1985-01-07",
  "driverNumber": 44,
  "teamId": 1
}

### Step 4: Create Max Verstappen (for Team ID 2 - Red Bull)
POST http://localhost:5254/api/drivers
Content-Type: application/json

{
  "firstName": "Max",
  "lastName": "Verstappen",
  "nationality": "Dutch",
  "dateOfBirth": "1997-09-30",
  "driverNumber": 1,
  "teamId": 2
}

### Step 5: View all drivers with their teams
GET http://localhost:5254/api/drivers

### Step 6: Update a driver (change team)
PUT http://localhost:5254/api/drivers/1
Content-Type: application/json

{
  "firstName": "Lewis",
  "lastName": "Hamilton",
  "nationality": "British",
  "dateOfBirth": "1985-01-07",
  "driverNumber": 44,
  "teamId": 2
}
```

#### Testing Validation Rules

The `.http` file includes requests to test validation:

```http
### Test: Invalid Driver Number (must be 1-99)
POST http://localhost:5254/api/drivers
Content-Type: application/json

{
  "firstName": "Test",
  "lastName": "Driver",
  "nationality": "American",
  "dateOfBirth": "1990-01-01",
  "driverNumber": 100,
  "teamId": null
}
# Expected: 400 Bad Request

### Test: Duplicate Driver Number
POST http://localhost:5254/api/drivers
Content-Type: application/json

{
  "firstName": "Another",
  "lastName": "Driver",
  "nationality": "German",
  "dateOfBirth": "1995-05-15",
  "driverNumber": 44,
  "teamId": null
}
# Expected: 400 Bad Request - Driver number already exists

### Test: Duplicate Team Name
POST http://localhost:5254/api/teams
Content-Type: application/json

{
  "name": "Mercedes-AMG Petronas F1 Team",
  "constructor": "Mercedes Clone",
  "foundedYear": 2020,
  "baseLocation": "Somewhere",
  "championshipsWon": 0
}
# Expected: 400 Bad Request - Team name must be unique
```

#### Tips for .http Files

- **Use `###`** to separate requests
- **Variables**: Define at the top with `@variableName = value`
- **Response Inspection**: Results appear in a separate pane
- **Keyboard Shortcut**: `Ctrl+Alt+R` (VS) or `Ctrl+Alt+H` (VS Code) to send request
- **Environment Support**: Can switch between dev/staging/prod endpoints

#### Alternative: Swagger UI

If you prefer a GUI, Swagger is available at `http://localhost:5254/swagger` when running in Development mode.

## 🤖 AI Race Engineer

The AI Race Engineer agent provides intelligent race strategy recommendations through:

### Available Tools

1. **getRaceData** - Fetches current race conditions, weather, and telemetry
2. **runSimulation** - Executes Monte Carlo simulations for strategy outcomes
3. **compareStrategies** - Analyzes multiple strategies against each other
4. **saveStrategy** - Persists optimal strategy to database

### Example Usage

```
User: "What's the best strategy for Monaco if rain is expected?"

Agent:
1. Calls getRaceData(circuit="Monaco", conditions="current")
2. Calls runSimulation(strategies=["soft-soft", "medium-medium"], weather="rain")
3. Calls compareStrategies(results=[...])
4. Returns recommendation with confidence scores
5. Calls saveStrategy(optimal_strategy)
```

## 📈 Performance Considerations

- **Caching**: Race data cached in Redis with 5-minute TTL
- **Pagination**: All list endpoints support pagination (default: 20 items)
- **Background Jobs**: Heavy simulations run asynchronously
- **Database Indexing**: Optimized queries on frequently accessed fields

## 🚢 Deployment

### Docker Production Build

```bash
docker build -t f1sim-api ./src/F1Sim.Api
docker build -t f1sim-client ./client
docker-compose -f docker-compose.prod.yml up
```

### Environment Variables

```env
DATABASE_URL=postgresql://user:pass@host:5432/f1sim
REDIS_URL=redis://host:6379
JWT_SECRET=production-secret-key
ASPNETCORE_ENVIRONMENT=Production
LOG_LEVEL=Information
```

## 🗺️ Development Roadmap

### Current Phase: Week 1-2
Building core backend infrastructure

### Upcoming Milestones
- [ ] Complete CRUD operations
- [ ] Implement authentication
- [ ] Build React dashboard
- [ ] Add background job processing
- [ ] Integrate AI agent
- [ ] Production deployment

## 🤝 Contributing

This is a personal learning project, but suggestions and feedback are welcome!

### Development Workflow

1. Work in feature branches
2. Write tests for new features
3. Update documentation
4. Create pull request with clear description

### Commit Convention

```
feat: Add race simulation engine
fix: Correct tire compound validation
docs: Update API documentation
test: Add integration tests for strategies
refactor: Simplify repository pattern
```

## 📝 License

This project is for educational and portfolio purposes.

## 🎓 Learning Outcomes

By completing this project, I demonstrate:

- ✅ **Backend Architecture**: Design and implement production-grade APIs
- ✅ **Database Design**: Complex relational schemas with proper normalization
- ✅ **Full-Stack Development**: Own features from database to UI
- ✅ **System Design**: Understand caching, queues, and background processing
- ✅ **AI Engineering**: Build and deploy intelligent agents with tools
- ✅ **DevOps**: Container orchestration and deployment strategies
- ✅ **Testing**: Unit, integration, and end-to-end test coverage
- ✅ **Documentation**: Clear technical writing and architecture diagrams

## 🎯 Project Timeline

**Duration**: 12 weeks  
**Commitment**: 1 hour daily  
**Methodology**: Incremental progress, weekly milestones  
**Philosophy**: Consistency over intensity

---

**Status**: 🟢 Active Development  
**Current Phase**: Core Backend (Week 1-2)  
**Last Updated**: February 16, 2026

---

*"This project exists to make me a rock-solid full-stack senior engineer."*
