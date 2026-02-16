# 📊 F1 Race Engineer Platform - Progress Tracker

Track daily progress, learning milestones, and technical achievements throughout the 12-week project.

---

## 🎯 Project Overview

**Start Date:** February 16, 2025  
**Target Completion:** May 11, 2025 (12 weeks)  
**Daily Commitment:** 1 hour  
**Philosophy:** Consistency over intensity

---

## 📅 Session Log

### **Session 1: February 16, 2025** ✅

**Duration:** ~1 hour  
**Focus:** Clean Architecture Setup & CRUD Foundation

#### What I Built
- ✅ Created solution structure with Clean Architecture (3 projects)
  - `F1Sim.Domain` - Entities and base classes
  - `F1Sim.Infrastructure` - EF Core and DbContext
  - `F1Sim.Api` - Controllers and DTOs
- ✅ Implemented `Driver` and `Team` entities with relationships
- ✅ Configured EF Core with in-memory database
- ✅ Built complete CRUD controllers for Drivers and Teams
  - GET (all), GET (by id), POST, PUT, DELETE
- ✅ Created DTOs for request/response separation
- ✅ Added validation and error handling
- ✅ Configured structured logging
- ✅ Set up Swagger for API documentation
- ✅ Created comprehensive `.http` test file

#### What I Learned
1. **Clean Architecture Principles**
   - Dependency inversion (Domain has no dependencies)
   - Separation of concerns across projects
   - When to use Clean Architecture vs simpler patterns
   - Tradeoffs: complexity vs maintainability

2. **EF Core Relationships**
   - One-to-many relationships (`Team` → `Drivers`)
   - Foreign key configuration
   - Delete behaviors:
     - `Cascade` - Delete children automatically
     - `SetNull` - Nullify foreign keys
     - `Restrict` - Block delete if children exist
   - Navigation properties

3. **REST API Best Practices**
   - HTTP status codes:
     - `200 OK` - Successful GET/PUT
     - `201 Created` - Successful POST
     - `204 No Content` - Successful DELETE
     - `400 Bad Request` - Validation errors
     - `404 Not Found` - Entity not found
     - `500 Internal Server Error` - Unexpected errors
   - `CreatedAtAction` for POST responses (includes Location header)

4. **Deletion Strategies Spectrum**
   - **Cascade Delete** (Aggressive): Auto-delete children
   - **Soft Delete** (Safest): Mark as deleted, keep in DB
   - **Nullify + Warn** (Current): Prevent delete if relationships exist
   - **Strict Block** (Paranoid): Always block if any related data
   - **Archive & Status** (Sophisticated): Status-based lifecycle
   - Industry usage: ~60% use soft delete in enterprise apps

5. **DTOs vs Entities**
   - Entities = Domain models (what data *is*)
   - DTOs = API contracts (what data *travels*)
   - Prevents exposing internal domain structure
   - Allows API versioning without changing domain

6. **Validation Approaches**
   - Manual validation in controllers (current approach)
   - Data Annotations (`[Required]`, `[Range]`, etc.)
   - FluentValidation (Phase 2 material)

7. **Frontend vs Backend Mindset**
   - Frontend: "User clicks button → API call → Update UI"
   - Backend: "Request → Check 10 things that could go wrong → Execute safely"
   - Backend focuses on data integrity, business rules, side effects

#### Architectural Decisions Made
- **Clean Architecture** over Vertical Slice or Simple Layered
  - Rationale: Learning goals, scalability, portfolio value
- **3 projects** instead of 4 (defer Application layer to Phase 2)
- **Nullify + Warn** deletion strategy for Phase 1
  - Plan to migrate to Soft Delete in Phase 2
- **In-memory database** for rapid iteration
  - Plan to migrate to SQL Server LocalDB in Session 2

#### Challenges Faced
- Understanding dependency flow in Clean Architecture
- Deciding when to use `200 OK` vs `204 No Content`
- Choosing the right deletion strategy for the domain
- Balancing simplicity (Phase 1) with best practices (future phases)

#### Files Created/Modified
```
Created:
- src/F1Sim.Domain/Entities/BaseEntity.cs
- src/F1Sim.Domain/Entities/Driver.cs
- src/F1Sim.Domain/Entities/Team.cs
- src/F1Sim.Infrastructure/Data/F1SimDbContext.cs
- src/F1Sim.Api/Controllers/DriversController.cs
- src/F1Sim.Api/Controllers/TeamsController.cs
- src/F1Sim.Api/DTOs/DriverDtos.cs
- src/F1Sim.Api/DTOs/TeamDtos.cs
- src/F1Sim.Api/F1Sim.Api.http
- docs/learning_notes.md
- docs/progress_tracker.md (this file)

Modified:
- src/F1Sim.Api/Program.cs (DI configuration)
- README.md (added .http file documentation)
```

#### Quiz Results
- **Score:** 2/3 (67%)
- ✅ Question 2: HTTP status codes (200 OK for PUT)
- ✅ Question 3: EF Core delete behaviors (SetNull)
- ❌ Question 1: Clean Architecture dependencies (incorrectly thought Domain could reference Infrastructure)
- **Key Learning:** Dependencies point inward - Domain knows nothing about outer layers

#### Next Session Goals
- [ ] Migrate from in-memory database to SQL Server LocalDB
- [ ] Create first EF Core migration
- [ ] Add seed data for testing
- [ ] Test with persistent database

---

## 📈 Phase Progress

### **Phase 1: Core Backend (Weeks 1-2)** 🟢 In Progress

| Task | Status | Session | Notes |
|------|--------|---------|-------|
| ✅ Solution structure | Done | 1 | 3 projects: Domain, Infrastructure, Api |
| ✅ Driver entity | Done | 1 | With Team relationship |
| ✅ Team entity | Done | 1 | With Drivers collection |
| ✅ DbContext configuration | Done | 1 | In-memory DB for now |
| ✅ Drivers CRUD controller | Done | 1 | Full CRUD with validation |
| ✅ Teams CRUD controller | Done | 1 | Full CRUD with validation |
| ✅ DTOs | Done | 1 | Request/response separation |
| ✅ Validation | Done | 1 | Manual in controllers |
| ✅ Error handling | Done | 1 | Try-catch with logging |
| ✅ Logging | Done | 1 | ILogger with structured logging |
| ✅ Swagger setup | Done | 1 | API documentation |
| ✅ Test .http file | Done | 1 | Manual testing scenarios |
| ⏳ SQL Server LocalDB | Next | 2 | Persistent database |
| ⏳ EF Migrations | Next | 2 | Database versioning |
| ⏳ Seed data | Next | 2 | Test data |
| ⏳ Race entity | Pending | - | More complex domain model |
| ⏳ Strategy entity | Pending | - | Relationships with Race |

**Phase 1 Completion:** 60% (12/20 tasks)

---

### **Phase 2: Production Backend (Weeks 3-4)** ⏳ Not Started

| Task | Status | Notes |
|------|--------|-------|
| ⏳ Application layer | Pending | Services, repositories, interfaces |
| ⏳ Repository pattern | Pending | Abstraction over DbContext |
| ⏳ Soft delete | Pending | IsDeleted, global query filters |
| ⏳ FluentValidation | Pending | Move validation out of controllers |
| ⏳ Unit tests | Pending | xUnit, Moq |
| ⏳ Integration tests | Pending | WebApplicationFactory |
| ⏳ Pagination | Pending | PagedResult<T> |
| ⏳ Filtering | Pending | Query string parameters |
| ⏳ Sorting | Pending | OrderBy, ThenBy |
| ⏳ JWT authentication | Pending | Phase 2 goal |

---

### **Phase 3: Full-Stack Integration (Weeks 5-6)** ⏳ Not Started

| Task | Status | Notes |
|------|--------|-------|
| ⏳ React setup | Pending | Vite + TypeScript |
| ⏳ React Query | Pending | Data fetching |
| ⏳ Dashboard UI | Pending | Driver/Team lists |
| ⏳ Forms | Pending | Create/edit entities |
| ⏳ Validation | Pending | Client-side validation |

---

### **Phase 4: System Design (Weeks 7-8)** ⏳ Not Started

| Task | Status | Notes |
|------|--------|-------|
| ⏳ Background jobs | Pending | Hangfire or similar |
| ⏳ Redis caching | Pending | Cache race data |
| ⏳ Docker | Pending | Containerization |
| ⏳ PostgreSQL migration | Pending | Final production DB |

---

### **Phase 5: AI Agent (Weeks 9-10)** ⏳ Not Started

| Task | Status | Notes |
|------|--------|-------|
| ⏳ Agent framework | Pending | Tool-calling orchestration |
| ⏳ Race simulation | Pending | Monte Carlo simulation |
| ⏳ Strategy comparison | Pending | AI-powered analysis |

---

### **Phase 6: Production Hardening (Weeks 11-12)** ⏳ Not Started

| Task | Status | Notes |
|------|--------|-------|
| ⏳ Evaluation scenarios | Pending | Agent performance metrics |
| ⏳ Distributed tracing | Pending | Observability |
| ⏳ Demo prep | Pending | Presentation materials |

---

## 📚 Cumulative Learning Log

### **Concepts Mastered**
- [x] Clean Architecture principles
- [x] EF Core relationships (one-to-many)
- [x] REST API design patterns
- [x] HTTP status code semantics
- [x] DTO pattern
- [x] Deletion strategies in enterprise apps
- [x] Frontend vs backend design thinking
- [ ] EF Core migrations (next session)
- [ ] Repository pattern
- [ ] Unit testing
- [ ] Integration testing

### **Technologies Used**
- [x] .NET 8 / ASP.NET Core
- [x] Entity Framework Core (in-memory)
- [x] Swagger / OpenAPI
- [x] ILogger structured logging
- [ ] SQL Server
- [ ] xUnit
- [ ] Moq
- [ ] FluentValidation
- [ ] React
- [ ] Docker

### **Design Patterns Encountered**
- [x] Clean Architecture
- [x] DTO (Data Transfer Object)
- [x] Dependency Injection
- [ ] Repository Pattern (Phase 2)
- [ ] Unit of Work (Phase 2)
- [ ] CQRS (Phase 2+)
- [ ] Mediator (Phase 2+)

---

## 🎯 Learning Milestones

### **Week 1**
- ✅ Understand Clean Architecture dependency flow
- ✅ Build first production-style CRUD API
- ✅ Learn EF Core relationship configuration
- ✅ Understand deletion strategy tradeoffs
- ⏳ Master EF Core migrations (Session 2)

### **Week 2**
- [ ] Complete all Phase 1 entities (Race, Strategy)
- [ ] Understand database normalization
- [ ] Practice writing migrations
- [ ] Learn seed data strategies

### **Weeks 3-4 (Phase 2)**
- [ ] Implement repository pattern
- [ ] Write first unit tests
- [ ] Write first integration tests
- [ ] Understand mocking with Moq

---

## 💪 Confidence Levels

Rate your confidence (1-5) in each area:

### **Backend Development**
- Clean Architecture: ⭐⭐⭐⭐☆ (4/5) - Understand concepts, need more practice
- EF Core Basics: ⭐⭐⭐⭐☆ (4/5) - Relationships solid, migrations next
- REST API Design: ⭐⭐⭐⭐☆ (4/5) - Status codes clear, need more complex scenarios
- Validation: ⭐⭐⭐☆☆ (3/5) - Manual works, need FluentValidation experience
- Error Handling: ⭐⭐⭐☆☆ (3/5) - Try-catch works, need middleware approach

### **Database**
- Relational Modeling: ⭐⭐⭐⭐☆ (4/5) - One-to-many clear, many-to-many next
- EF Core Migrations: ⭐☆☆☆☆ (1/5) - Not yet attempted
- Indexing: ⭐⭐☆☆☆ (2/5) - Understand concept, need practice

### **Testing**
- Manual Testing: ⭐⭐⭐⭐☆ (4/5) - .http files comfortable
- Unit Testing: ⭐☆☆☆☆ (1/5) - Not yet attempted
- Integration Testing: ⭐☆☆☆☆ (1/5) - Not yet attempted

### **Architecture**
- SOLID Principles: ⭐⭐⭐☆☆ (3/5) - Understand SRP, DIP; need OCP, LSP, ISP
- Design Patterns: ⭐⭐☆☆☆ (2/5) - Know DTO, DI; need Repository, CQRS
- System Design: ⭐⭐☆☆☆ (2/5) - Aware of caching, queues; no hands-on yet

---

## 🔥 Wins & Breakthroughs

### **Session 1 Wins**
- ✅ **First Clean Architecture project** - Successfully structured solution with proper dependencies
- ✅ **Full CRUD in one session** - Built complete API for two entities with validation
- ✅ **Understanding deletion strategies** - Grasped enterprise patterns (soft delete, cascade, etc.)
- ✅ **Quiz insight** - Realized I need to strengthen understanding of dependency inversion

---

## 🤔 Questions & Curiosities

### **Unanswered Questions**
- How does EF Core track changes internally?
- When should I use `IQueryable` vs `IEnumerable`?
- What's the performance impact of soft delete with global query filters?
- How do I handle optimistic concurrency conflicts?

### **Want to Explore**
- Specification pattern for complex queries
- Domain events for decoupled business logic
- CQRS with MediatR
- Background job processing with Hangfire

---

## 📝 Notes to Self

### **Things That Clicked**
- "Dependencies point inward" - Domain never references outer layers
- DELETE should return 204 No Content (not 200 OK)
- Soft delete is the default in most enterprise apps (~60%)
- Backend thinking = "what could go wrong?" vs frontend = "what should happen?"

### **Things to Remember**
- Always use `async/await` for database operations
- DTOs protect domain from API contract changes
- Log at appropriate levels (Information for actions, Error for exceptions)
- Test error paths, not just happy paths

### **Mistakes Made & Lessons**
- Initially confused about which layer should depend on which (thought Domain could reference Infrastructure)
- Realized frontend mindset needs to shift to backend defensive programming

---

## 🎯 30-Day, 60-Day, 90-Day Goals

### **By Day 30 (End of Phase 2)**
- [ ] Complete backend with tests, auth, and production patterns
- [ ] Comfortable writing unit and integration tests
- [ ] Understand repository pattern deeply
- [ ] Can explain SOLID principles with examples

### **By Day 60 (End of Phase 4)**
- [ ] Full-stack app running with React frontend
- [ ] Background jobs processing simulations
- [ ] Redis caching implemented
- [ ] Dockerized application

### **By Day 90 (Project Complete)**
- [ ] AI agent integrated and working
- [ ] Production-grade codebase with observability
- [ ] Portfolio-ready demo
- [ ] Able to explain every architectural decision

---

## 🏆 Badges Earned

- 🎖️ **Clean Architect** - Successfully implemented Clean Architecture
- 🎖️ **API Builder** - Built first production-style REST API
- 🎖️ **CRUD Master** - Complete CRUD for multiple entities
- 🎖️ **Relationship Manager** - Configured EF Core relationships
- ⏳ **Migration Expert** - (To be earned in Session 2)
- ⏳ **Test Champion** - (To be earned in Phase 2)

---

## 📊 Weekly Retrospectives

### **Week 1 Retrospective** (In Progress)

#### What Went Well
- Clean Architecture setup smoother than expected
- Validation and error handling intuitive
- Learning notes system helping retention
- Quiz helped identify knowledge gaps

#### What Could Be Better
- Need to commit code more frequently
- Should run tests before ending session
- Want to explore more EF Core features

#### Action Items for Next Week
- [ ] Set up SQL Server LocalDB
- [ ] Learn EF Core migrations
- [ ] Add more comprehensive validation
- [ ] Explore middleware for global error handling

---

## 📅 Session Template (Copy for Next Session)

```markdown
### **Session X: [Date]** ⏳

**Duration:** ~X hour  
**Focus:** [Main topic]

#### What I Built
- [ ] Task 1
- [ ] Task 2

#### What I Learned
1. **Concept 1**
   - Details

#### Architectural Decisions Made
- Decision and rationale

#### Challenges Faced
- Challenge and how resolved

#### Files Created/Modified
- List of files

#### Next Session Goals
- [ ] Goal 1
```

---

**Last Updated:** Session 1 - February 16, 2025  
**Next Session:** February 17, 2025 (Target)
