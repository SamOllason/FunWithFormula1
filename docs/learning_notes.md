# 🎓 F1 Race Engineer Platform - Learning Notes

A knowledge base documenting key decisions, tradeoffs, and lessons learned throughout the project.

---

## 📐 Architecture Decisions

### Session 1: Choosing Clean Architecture

**Decision Date**: Week 1, Session 1  
**Context**: Starting fresh with Phase 1 backend development

#### Architecture Options Evaluated

##### Option A: Clean Architecture (4 Layers) ⭐ **SELECTED**

**Structure:**
```
F1Sim.Domain        - Entities, value objects, domain logic (no dependencies)
F1Sim.Application   - Use cases, DTOs, interfaces, business logic
F1Sim.Infrastructure - EF Core, DbContext, external services, repositories
F1Sim.Api           - Controllers, middleware, configuration, startup
```

**Dependency Flow:**
```
Api → Infrastructure → Application → Domain
                  ↓
            (implements interfaces from Application)
```

**Pros:**
- ✅ **Separation of Concerns**: Each layer has a single, well-defined responsibility
- ✅ **Testability**: Easy to mock Infrastructure and test business logic in isolation
- ✅ **Maintainability**: Changes in one layer rarely affect others
- ✅ **Industry Standard**: Most .NET enterprise teams use this or similar patterns
- ✅ **Scalability**: Easy to add new features without breaking existing structure
- ✅ **Portfolio Value**: Demonstrates understanding of SOLID principles and dependency inversion
- ✅ **Future-Proof**: When adding Redis, background jobs, AI agents, structure supports it naturally

**Cons:**
- ❌ **Initial Complexity**: More projects and files to set up
- ❌ **Navigation Overhead**: Feature code spread across multiple projects
- ❌ **Potential Over-Engineering**: Can feel heavy for simple CRUD operations
- ❌ **Learning Curve**: Requires understanding dependency inversion principle

**When to Use:**
- Enterprise applications expected to grow
- Projects requiring high testability
- Teams valuing maintainability over initial speed
- **This project**: Aligns with "production-grade" and "senior-level" goals

---

##### Option B: Vertical Slice Architecture

**Structure:**
```
F1Sim.Api/
├── Features/
│   ├── Drivers/
│   │   ├── CreateDriver.cs (entire feature in one file)
│   │   ├── GetDriver.cs
│   │   ├── UpdateDriver.cs
│   └── Teams/
└── Common/ (shared DbContext, services)
```

**Pros:**
- ✅ **Feature Locality**: All code for a feature in one place
- ✅ **Fast Development**: No jumping between layers
- ✅ **Modern Approach**: Popularized by Jimmy Bogard, gaining traction
- ✅ **Reduced Coupling**: Features don't share abstractions unless needed

**Cons:**
- ❌ **Less Familiar**: Many teams haven't adopted this pattern yet
- ❌ **Discipline Required**: Easy to create spaghetti code without strict conventions
- ❌ **Shared Logic**: Harder to enforce reuse of common business rules
- ❌ **Testing**: Can be harder to test if handlers do too much

**When to Use:**
- Modern startups prioritizing speed
- Projects with many independent features
- Teams experienced with MediatR and CQRS patterns

---

##### Option C: Simple Layered (Single Project)

**Structure:**
```
F1Sim.Api/
├── Controllers/
├── Models/
├── Data/
├── Services/
└── DTOs/
```

**Pros:**
- ✅ **Simplicity**: Easiest to understand and navigate
- ✅ **Rapid Prototyping**: Fastest initial development
- ✅ **Low Overhead**: No multi-project complexity

**Cons:**
- ❌ **Tight Coupling**: Everything depends on everything
- ❌ **Hard to Test**: DbContext and business logic mixed
- ❌ **Not Scalable**: Becomes messy as project grows
- ❌ **Portfolio Value**: Doesn't demonstrate architectural knowledge

**When to Use:**
- Learning exercises
- Proof of concepts
- Small internal tools with short lifespan

---

### Our Choice: Clean Architecture (Simplified Start)

**Initial Implementation (Phase 1):**
We're starting with **3 projects** instead of 4:

```
F1Sim.Domain        - Entities only
F1Sim.Infrastructure - DbContext, EF Core configuration
F1Sim.Api           - Controllers, DTOs, validation, dependency injection
```

**Why Skip Application Layer Initially?**
- Phase 1 is simple CRUD - no complex business logic yet
- Avoid premature abstraction
- Add `F1Sim.Application` in Phase 2 when we implement:
  - Repository pattern
  - Service layer
  - Complex validation logic
  - Business rules

**Progression Plan:**
```
Phase 1: Domain + Infrastructure + Api (simple CRUD)
Phase 2: Add Application layer (repositories, services, validation)
Phase 3: Frontend integration (Application becomes thin orchestration layer)
Phase 4: Add background jobs (separate F1Sim.Jobs project)
Phase 5: AI agents (F1Sim.Agent project, calls Application services)
```

---

## 🎯 Key Takeaways

### When to Use Clean Architecture
1. **Long-lived projects** (this is a 12-week+ project)
2. **Learning goals** include architecture and testing
3. **Portfolio projects** where you want to show best practices
4. Projects that will **grow in complexity** (we're adding AI, caching, jobs)

### When NOT to Use Clean Architecture
1. **Prototypes** or throwaway code
2. **Trivial applications** (simple CRUD with no business logic)
3. **Time-constrained hackathons**
4. When the team **lacks experience** with dependency inversion

### The Meta-Lesson: Architecture
> "Architecture is about tradeoffs, not right vs wrong. The 'best' architecture depends on your constraints: team size, timeline, growth expectations, and learning goals."

### The Meta-Lesson: Deletion Strategies
> "There's no 'right' deletion strategy - it depends on your domain, compliance requirements, and how much you value safety vs. convenience. Start simple, evolve as requirements become clear."

For this project, Clean Architecture aligns with:
- ✅ Demonstrating senior-level thinking
- ✅ Building a scalable, maintainable system
- ✅ Preparing for future complexity (AI, caching, jobs)
- ✅ Portfolio value

---

## 📚 Resources

### Clean Architecture
- [Microsoft Docs: Common Web App Architectures](https://docs.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures)
- [Jason Taylor's Clean Architecture Template](https://github.com/jasontaylordev/CleanArchitecture)
- Book: "Clean Architecture" by Robert C. Martin

### Vertical Slice Architecture
- [Jimmy Bogard - Vertical Slice Architecture](https://www.youtube.com/watch?v=SUiWfhAhgQw)
- [MediatR Library](https://github.com/jbogard/MediatR)

### General Architecture
- Book: "Fundamentals of Software Architecture" by Mark Richards & Neal Ford
- [Martin Fowler's Architecture Guide](https://martinfowler.com/architecture/)

### Entity Framework & Data Management
- [Microsoft Docs: Delete Behaviors](https://docs.microsoft.com/en-us/ef/core/saving/cascade-delete)
- [EF Core Query Filters (for soft delete)](https://docs.microsoft.com/en-us/ef/core/querying/filters)
- [EF Core Database Providers](https://docs.microsoft.com/en-us/ef/core/providers/)

---

## 🗄️ Entity Framework Core: Database Abstraction & Providers

### Session 1: Understanding DbContext & Database Portability

**Context**: Learning how EF Core works with different databases and when to use provider-specific features

#### What is DbContext?

**DbContext is a "Database Manager" or "Translation Layer":**

```
Your C# Code  ←→  DbContext  ←→  Database Provider  ←→  Actual Database
   (Objects)      (EF Core)     (Translator)          (SQL/NoSQL)
```

**DbContext Responsibilities:**
1. **Translation** - Converts C# LINQ to SQL
2. **Change Tracking** - Detects what you modified
3. **Connection Management** - Opens/closes database connections
4. **Query Execution** - Translates and executes queries

#### Database Provider Architecture

```csharp
// Your domain code - STAYS THE SAME regardless of database!
public class Driver : BaseEntity
{
    public string FirstName { get; set; }
    public int DriverNumber { get; set; }
}

// Your queries - STAY THE SAME!
var drivers = await _context.Drivers
    .Where(d => d.DriverNumber == 44)
    .ToListAsync();

// ONLY THIS CHANGES when switching databases:
builder.Services.AddDbContext<F1SimDbContext>(options =>
    options.UseInMemoryDatabase("F1SimDb"));      // Phase 1
    // options.UseSqlServer(connectionString);    // Session 2
    // options.UseNpgsql(connectionString);       // Phase 4
    // options.UseMySql(connectionString, ...);   // Alternative
);
```

#### Available Database Providers

| Database | NuGet Package | Provider Method | Project Phase |
|----------|---------------|-----------------|---------------|
| **In-Memory** | `Microsoft.EntityFrameworkCore.InMemory` | `UseInMemoryDatabase()` | Phase 1 ✅ |
| **SQL Server** | `Microsoft.EntityFrameworkCore.SqlServer` | `UseSqlServer()` | Session 2 ⏳ |
| **PostgreSQL** | `Npgsql.EntityFrameworkCore.PostgreSQL` | `UseNpgsql()` | Phase 4 🎯 |
| **MySQL** | `Pomelo.EntityFrameworkCore.MySql` | `UseMySql()` | Alternative |
| **SQLite** | `Microsoft.EntityFrameworkCore.Sqlite` | `UseSqlite()` | Alternative |

#### In-Memory Database: How It Works

**Conceptually:**
```csharp
// Behind the scenes, in-memory DB is like:
var InMemoryDatabase = new 
{
    Drivers = new List<Driver>(),  // "Drivers table" = List in RAM
    Teams = new List<Team>()       // "Teams table" = List in RAM
};
```

**Characteristics:**
- ⚡ **Super fast** - No disk I/O
- 💨 **Volatile** - Data lost when app stops
- ✅ **Zero setup** - Perfect for learning/testing
- ❌ **No persistence** - Not for production

**Our Progression:**
```
Phase 1: In-Memory    → Fast iteration, learn CRUD
Session 2: SQL Server → Learn migrations, persistent data
Phase 4: PostgreSQL   → Production-ready, Docker/cloud
```

---

## 🎯 Database-Specific Features & Provider Capabilities

### The Portability vs Power Tradeoff

```
← More Portable                                More Powerful →
┌─────────────────┬─────────────────┬──────────────────────┐
│  Generic LINQ   │ Provider APIs   │   Raw SQL            │
│  (works everywhere) │ (EF.Functions) │ (database-specific) │
└─────────────────┴─────────────────┴──────────────────────┘
   95% of use cases   Special features   Full database power
```

### PostgreSQL-Specific Features

#### 1. JSON/JSONB Support (PostgreSQL's Superpower)

**Use Case:** Store complex metadata without creating many tables

```csharp
// Domain entity
public class Driver : BaseEntity
{
    public string FirstName { get; set; }

    // Store complex data as JSON
    public DriverMetadata Metadata { get; set; }  // Stored as JSONB
}

public class DriverMetadata
{
    public List<string> Sponsors { get; set; }
    public Dictionary<string, int> SeasonStats { get; set; }
}

// Configure in DbContext
modelBuilder.Entity<Driver>(entity =>
{
    entity.Property(e => e.Metadata)
        .HasColumnType("jsonb");  // PostgreSQL-specific
});

// Query inside JSON!
var driversWithRedBull = await _context.Drivers
    .Where(d => d.Metadata.Sponsors.Contains("Red Bull"))
    .ToListAsync();
```

#### 2. Full-Text Search

```csharp
// PostgreSQL has powerful built-in text search
var drivers = await _context.Drivers
    .Where(d => EF.Functions.ToTsVector("english", d.FirstName + " " + d.LastName)
        .Matches(EF.Functions.PlainToTsQuery("english", "Lewis Hamilton")))
    .ToListAsync();
```

#### 3. Array Support

```csharp
public class Team : BaseEntity
{
    public int[] ChampionshipYears { get; set; }  // Native array support
}

var teams = await _context.Teams
    .Where(t => t.ChampionshipYears.Contains(2020))
    .ToListAsync();
```

### SQL Server-Specific Features

#### Temporal Tables (Time-Travel Queries)

```csharp
// Enable temporal table
modelBuilder.Entity<Driver>(entity =>
{
    entity.ToTable("Drivers", t => t.IsTemporal());
});

// Query historical data!
var driverLastYear = await _context.Drivers
    .TemporalAsOf(DateTime.UtcNow.AddYears(-1))  // Go back in time!
    .FirstOrDefaultAsync(d => d.Id == 1);
```

### Raw SQL for Advanced Features

```csharp
// When EF Core doesn't expose a feature, use raw SQL
var driverRankings = await _context.Drivers
    .FromSqlRaw(@"
        SELECT *, 
               RANK() OVER (PARTITION BY TeamId ORDER BY Points DESC) as TeamRank
        FROM Drivers
    ")
    .ToListAsync();
```

### Strategy: When to Use Database-Specific Features

**Decision Matrix:**

| Scenario | Approach | Portability | Our Phase |
|----------|----------|-------------|-----------|
| **Basic CRUD** | Generic LINQ | ✅ Portable | Phase 1 ✅ |
| **Simple queries** | Generic LINQ | ✅ Portable | Phase 1 ✅ |
| **JSON storage** | `HasColumnType("jsonb")` | ⚠️ Postgres/MySQL | Phase 4 |
| **Full-text search** | `EF.Functions.ToTsVector()` | ❌ Postgres-specific | Phase 4+ |
| **Advanced analytics** | Raw SQL | ❌ DB-specific | As needed |

**Best Practice:**
1. **Start generic** - 90% of code should be portable LINQ
2. **Use provider features strategically** - When they provide real value
3. **Encapsulate in repositories** - Hide database-specific code (Phase 2)
4. **Raw SQL when needed** - Don't fight EF Core for complex queries

---

## 🍃 NoSQL & Document Databases (MongoDB)

### MongoDB vs EF Core: Fundamental Differences

```
Relational DB (SQL)           Document DB (MongoDB)
┌─────────────┐              ┌─────────────────────┐
│   Tables    │              │   Collections       │
│   Rows      │              │   Documents (JSON)  │
│   Columns   │              │   Fields (flexible) │
│   JOINs     │              │   Embedded/Refs     │
│   Schema    │              │   Schema-less       │
└─────────────┘              └─────────────────────┘
```

### MongoDB with EF Core: NOT Recommended

**EF Core has a MongoDB provider**, but:
- ❌ Experimental/preview quality
- ❌ Missing many MongoDB features
- ❌ Forces relational patterns onto document DB
- ❌ Official `MongoDB.Driver` is much better

### If Using MongoDB: Use MongoDB.Driver

```csharp
// MongoDB replaces DbContext with different architecture
public class MongoDbService
{
    private readonly IMongoDatabase _database;

    public IMongoCollection<Driver> Drivers => _database.GetCollection<Driver>("Drivers");
}

// Query syntax is similar but different
var drivers = await _mongo.Drivers
    .Find(d => d.DriverNumber > 10)
    .SortBy(d => d.LastName)
    .ToListAsync();
```

**Architectural Impact:**
- ❌ **DbContext:** Complete replacement needed
- ❌ **Controllers:** Different query syntax
- ⚠️ **Domain entities:** Minimal changes
- 🔄 **Modeling:** Embed data vs normalize (no JOINs!)

### When to Choose SQL vs MongoDB

**Choose SQL (PostgreSQL/MySQL/SQL Server):**
- ✅ Complex relationships with JOINs
- ✅ ACID transactions across entities
- ✅ Reporting and analytics
- ✅ Mature ORM ecosystem (EF Core)
- ✅ **Your F1 app** - Drivers/Teams/Races have clear relationships

**Choose MongoDB:**
- ✅ Flexible/evolving schema
- ✅ Document-centric data (blogs, catalogs)
- ✅ Horizontal scaling (sharding)
- ✅ Hierarchical/nested data
- ✅ Rapid prototyping

**For F1 Race Engineer Platform:**
- 🎯 **Stick with SQL (PostgreSQL)** - Your domain has clear relationships, needs transactions, and benefits from referential integrity

---

## 🎓 Key Takeaways: Database Strategy

### DbContext & Providers

1. **DbContext is database-agnostic** - It's a translation layer, not tied to any database
2. **Providers = Translators** - Each provider knows how to talk to its database
3. **Your code is portable** - 99% works across databases with just provider swap
4. **Only change: provider + connection string** - Rest of code stays the same

### Using Database-Specific Features

1. **Start generic (90% of code)** - Use standard LINQ that works everywhere
2. **Use provider features strategically** - When they provide real value (JSON, full-text)
3. **Encapsulate in repositories** - Hide database-specific code behind interfaces
4. **Raw SQL when needed** - Don't fight the ORM for complex queries

### MongoDB vs SQL

1. **Document DBs are fundamentally different** - Not just a provider swap
2. **For MongoDB: Use MongoDB.Driver** - Not EF Core's MongoDB provider
3. **Trade JOINs for flexibility** - Embed data vs normalize
4. **For F1 app: SQL is the right choice** - Clear relationships, transactions, analytics

### Project Progression

```
Phase 1: In-Memory      → Learn CRUD, fast iteration
Session 2: SQL Server   → Learn migrations, persistent data  
Phase 4: PostgreSQL     → Production ready, leverage JSON features
```

**The Meta-Lesson:**
> "Choose databases based on your data model and query patterns, not hype. Relational databases excel at relationships; document databases excel at flexibility. EF Core's abstraction lets you start simple and optimize later."

---

## 🗑️ Deletion Strategies & Related Entity Management

### Session 1: Understanding Deletion Policies

**Context**: Designing how to handle deleting Teams that have related Drivers

#### The Deletion Strategy Spectrum

```
← More Permissive/Aggressive                           More Restrictive/Defensive →
┌──────────────┬──────────────┬──────────────┬──────────────┬──────────────┐
│   Cascade    │   Soft       │  Nullify +   │   Strict     │   Archive    │
│   Delete     │   Delete     │   Warn       │   Block      │   & Status   │
└──────────────┴──────────────┴──────────────┴──────────────┴──────────────┘
    Aggressive     Safest          Current        Paranoid      Sophisticated
```

---

### Strategy 1: Cascade Delete (Aggressive) ⚠️

**What Happens:**
```
DELETE Team (id=1) 
  → Automatically deletes all Drivers with TeamId=1
```

**EF Core Configuration:**
```csharp
entity.HasOne(d => d.Team)
    .WithMany(t => t.Drivers)
    .HasForeignKey(d => d.TeamId)
    .OnDelete(DeleteBehavior.Cascade);
```

**When to Use:**
- ✅ Parent-child where child has **no independent value**
- ✅ Data that "belongs to" the parent (e.g., `Order` → `OrderLineItems`)
- ✅ Logs, notifications, temporary data

**Real-World Examples:**
- GitHub: Deleting repository → deletes all issues, PRs
- Stripe: Deleting customer → deletes payment methods
- E-commerce: Deleting order → deletes line items

**For F1 App:**
- ❌ **BAD CHOICE** - Drivers exist independently of teams
- Deleting Mercedes shouldn't delete Lewis Hamilton!

---

### Strategy 2: Soft Delete (Most Common) ⭐

**What Happens:**
```
DELETE Team (id=1)
  → Sets IsDeleted=true, DeletedAt=now
  → Team still exists in DB but filtered from queries
```

**Implementation:**
```csharp
public class Team : BaseEntity
{
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}

// Global query filter (applied automatically)
modelBuilder.Entity<Team>()
    .HasQueryFilter(t => !t.IsDeleted);
```

**Pros:**
- ✅ Can restore deleted data ("undo" functionality)
- ✅ Full audit history maintained
- ✅ Safer for production (no accidental data loss)
- ✅ Foreign key relationships remain intact
- ✅ Meets compliance requirements (GDPR, SOX)

**Cons:**
- ❌ Database grows over time
- ❌ Must filter `IsDeleted` everywhere
- ❌ Unique constraints become complex

**When to Use:**
- ✅ **Most enterprise/SaaS applications** (~60%)
- ✅ Financial, healthcare, legal systems
- ✅ When "undo" or restoration is important

**Real-World Examples:**
- Salesforce: Recycle bin (30-day restoration)
- Gmail: Trash folder
- Slack: Deleted messages (compliance)

**For F1 App:**
- ✅ **RECOMMENDED FOR PHASE 2**

---

### Strategy 3: Nullify + Warn (Current Approach) ✅

**What Happens:**
```
DELETE Team (id=1)
  → Check: Does team have drivers?
  → If YES: Return 400 Bad Request
  → If NO: Delete team
```

**Implementation:**
```csharp
// EF Core Configuration
.OnDelete(DeleteBehavior.SetNull)

// Controller Logic
var hasDrivers = await _context.Drivers.AnyAsync(d => d.TeamId == id);
if (hasDrivers)
{
    return BadRequest("Cannot delete team with active drivers...");
}
```

**Pros:**
- ✅ Simple to understand and implement
- ✅ Prevents accidental data loss
- ✅ Forces conscious decision about drivers

**Cons:**
- ❌ Extra step for users
- ❌ Creates "orphaned" drivers if allowed

**When to Use:**
- ✅ Relationships that are important but not mandatory
- ✅ Phase 1 / MVP

**Real-World Examples:**
- Jira: Can't delete project with open tickets
- Azure: Can't delete resource group with resources

**For F1 App:**
- ✅ **CURRENT IMPLEMENTATION** - Good for Phase 1

---

### Strategy 4: Strict Block (Paranoid) 🚫

**What Happens:**
```
DELETE Team (id=1)
  → Check: ANY related data exists?
  → If YES: Always block delete
```

**When to Use:**
- ✅ Financial systems
- ✅ Medical records
- ✅ Absolutely critical referential integrity

---

### Strategy 5: Archive & Status (Sophisticated) 🎯

**What Happens:**
```
"DELETE" Team → Change Status to Archived
              → Reassign drivers
              → Keep historical data
```

**Implementation:**
```csharp
public enum TeamStatus { Active, Archived, Historical }

[HttpPost("api/teams/{id}/archive")]
public async Task<IActionResult> ArchiveTeam(int id, ArchiveTeamRequestDto dto)
{
    team.Status = TeamStatus.Archived;
    // Handle driver reassignment...
}
```

**When to Use:**
- ✅ Complex domains with lifecycle states

---

## 📊 Industry Usage Patterns

| Strategy | Usage % | Typical Domain |
|----------|---------|----------------|
| **Soft Delete** | ~60% | Enterprise, SaaS, Finance |
| **Cascade Delete** | ~20% | Simple CRUD, owned entities |
| **Strict Block** | ~15% | Legacy, critical data |
| **Archive/Status** | ~5% | Complex workflows |

---

## 💡 Frontend vs Backend Mindset

**Frontend Thinking (Component-Focused):**
```
User clicks delete → Modal → API call → Update UI → Done!
```
Focus: User experience, visual feedback

**Backend Thinking (System-Focused):**
```
User clicks delete → API checks:
  ✓ Does entity exist?
  ✓ User has permission?
  ✓ Related entities?
  ✓ Business rules satisfied?
  ✓ Side effects to trigger?
  ✓ What could go wrong?
→ Execute + audit + cache + events
```
Focus: Data integrity, business rules, **defensive programming**

---

## 🔄 Living Document

This file will be updated throughout the project with:
- Technical decisions and rationale
- Lessons learned from mistakes
- Performance optimizations
- Testing strategies
- Deployment notes

---

**Last Updated**: Session 1 - Architecture Decision
