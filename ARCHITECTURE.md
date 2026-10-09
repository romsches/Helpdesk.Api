# Enterprise Architecture & System Design Document
## Mini-Helpdesk Core Backend Service (.NET 10)

| Dokumenten-Metadatum | Details |
| :--- | :--- |
| **System / Service** | Helpdesk Core API (`Helpdesk.Api`) |
| **Technologie-Stack** | .NET 10 (ASP.NET Core), EF Core, PostgreSQL, Serilog, FluentValidation |
| **Architektur-Paradigma** | Layered Monolith (Clean Architecture Principles) |
| **Autor / Lead Architect** | Backend Engineering Team |
| **Status** | Production-Ready / Enterprise Grade |

---

## 1. Executive Summary & Architektonische Zielsetzung

Das **Mini-Helpdesk API** ist als hochrobuster, fehlertoleranter und erweiterbarer Backend-Service für ein professionelles Service-Management-Umfeld konzipiert. Ziel dieses Dokuments ist es, die strukturellen Entscheidungen, Design-Muster, Sicherheitsaspekte und Skalierungsstrategien des Systems offenzulegen. 

Im Einklang mit modernen Software-Engineering-Standards (*Clean Code*, *SOLID*, *Defensive Programming*) legt die Architektur besonderen Wert auf:
* **Wartbarkeit & Lesbarkeit:** Striktes *Separation of Concerns*-Prinzip, um Geschäftslogik, Datenhaltung und Routing voneinander zu entkoppeln.
* **Resilienz & Defensive Data Handling:** Abfangen von Laufzeitfehlern und Schutz vor invaliden Datenbankzuständen auf Infrastruktur- und Anwendungsebene.
* **Observability (Beobachtbarkeit):** Durchgängiges, strukturiertes Logging mit präzisem Request-Tracking.

---

## 2. Systemarchitektur & Schichtenmodell (Layered Architecture)

Die Anwendung bricht bewusst mit dem Anti-Muster eines "Fat Controllers" und teilt Verantwortlichkeiten in klar definierte, aufeinanderfolgende Schichten auf:

1. **Presentation Layer (Präsentationsschicht):**
   * **Komponenten:** Controllers, Routing, HTTP Status Codes.
   * **Aufgabe:** Nimmt HTTP-Anfragen entgegen, delegiert diese und mappt Entitäten in standardisierte HTTP-Antworten. Es findet hier *keine* komplexe Validierungs- oder Geschäftslogik statt.

2. **Validation Layer (Validierungsschicht):**
   * **Komponenten:** FluentValidation Pipeline (Fail-Fast Policy).
   * **Aufgabe:** Eingehende Payloads werden vor dem Erreichen der Controller-Logik strikt validiert. Ungültige Anfragen werden sofort mit einem HTTP `400 Bad Request` abgefangen.

3. **Application Layer (Geschäftslogik & Fallback):**
   * **Komponenten:** Business Logic, Fallback Injection, Queries.
   * **Aufgabe:** Verarbeitet die Kernlogik und reichert unvollständige Client-Daten mit sicheren Standardwerten an (Fallback-Injektion).

4. **Data Access Layer (Persistenzschicht):**
   * **Komponenten:** AppDbContext, PostgreSQL (via Docker), Entity Framework Core Migrations.
   * **Aufgabe:** Verwaltet die relationale Datenhaltung über optimierte, asynchrone LINQ-Abfragen.

---

## 3. Resilienz, Fehlertoleranz & Transaktionssicherheit

In einer verteilten Microservice- oder Client-Server-Landschaft ist ein deterministisches Verhalten bei Ausfällen unabdingbar. Das System implementiert ein dreistufiges Schutzkonzept:

### 3.1. Global Exception Handling Middleware
* **Mechanismus:** Unerwartete Ausnahmen innerhalb des Request-Lifecycles werden zentral auf oberster Pipeline-Ebene abgefangen.
* **Nutzen:** Es wird verhindert, dass interne Server-Stacktraces an den Client durchgereicht werden. Stattdessen wird eine einheitliche, standardisierte JSON-Fehlerstruktur zurückgegeben, begleitet von einem detaillierten Fehler-Log über Serilog.

### 3.2. Defensive Fallback-Injektion (Database Constraints)
* **Problem:** PostgreSQL erzwingt bei Feldern mit `NOT NULL`-Restriktionen strenge Integritätsregeln. Unvollständige Payloads von Clients können zu unkontrollierten SQL-Exceptions führen.
* **Lösung:** Der Controller injiziert deterministische Fallback-Werte auf Anwendungsebene, falls optionale Metadaten fehlen:
  * `CreatedBy` $\rightarrow$ Automatisches Fallback auf `"Anonymous"`.
  * `AssignedTo` $\rightarrow$ Automatisches Fallback auf `"Unassigned"`.

### 3.3. Concurrency Control (Optimistic Locking via EF Core)
* Beim Aktualisieren von Datensätzen über `PUT /api/tickets/{id}` verwendet das System den Entity State Tracker. Tritt eine `DbUpdateConcurrencyException` auf (z. B. bei konkurrierenden Schreibvorgängen), prüft das System die Existenz der Entität und reagiert entsprechend mit einem präzisen Statuscode (`404` oder erneuter Exception-Propagation).

---

## 4. Observability & Logging-Architektur

Das standardmäßige .NET-Logging wurde durch **Serilog** ersetzt, um den Anforderungen moderner Cloud-Native-Umgebungen gerecht zu werden:
* **Strukturiertes Konsolen-Logging:** Logs werden als strukturierte Datenstrom-Ereignisse ausgegeben.
* **Performance-Metriken:** Jede HTTP-Transaktion wird automatisch mit exakter Ausführungsdauer (`ms`), HTTP-Methode, Pfad und Statuscode versehen.
* **Lifecycle-Logging:** Der Anwendungsstart und kritische Systemabstürze werden explizit abgefangen (`Log.Fatal`).

---

## 5. Datenbankschema & Technische Details (`Ticket` Entität)

Das Datenmodell ist schlank, performant und auf Skalierbarkeit ausgelegt:

| Feldname | Datentyp | Restriktionen / Eigenschaften | Architektonische Begründung |
| :--- | :--- | :--- | :--- |
| `Id` | `int` | Primary Key, Identity (Auto-Increment) | Optimaler Index-Lookup, geringer Speicher-Footprint. |
| `Title` | `string` | `Required`, Max. 100 Zeichen | Kurzer, prägnanter Betreff; durch Validierung begrenzt. |
| `Description` | `string` | `Required` | Ausführliche Problembeschreibung ohne harte Längenbegrenzung. |
| `Status` | `string` | Default: `"Open"` | Zustandsvariable (`Open`, `In Progress`, `Closed`). |
| `Priority` | `string` | `Required`, Default: `"Medium"`| Steuerungsmerkmal (`Low`, `Medium`, `High`). |
| `CreatedBy` | `string?` | Nullable mit Fallback | Audit-Feld für den Ersteller. |
| `AssignedTo` | `string?` | Nullable mit Fallback | Zuweisungsfeld für den Bearbeiter. |
| `CreatedAt` | `DateTime` | UTC Timestamp (`DateTime.UtcNow`) | Vermeidung von Zeitzoneninkonsistenzen in verteilten Systemen. |
| `ResolvedAt`| `DateTime?`| Nullable Timestamp | Ermöglicht KPI-Auswertungen zur Bearbeitungszeit. |

---

## 6. REST-API Endpunkte & Design-Prinzipien

Die Schnittstellen folgen strikt den REST-Konventionen (Resource-Oriented Architecture):

| Methode | Endpoint | Beschreibung | Erfolgs-Status | Fehler-Status |
| :--- | :--- | :--- | :--- | :--- |
| **GET** | `/api/tickets` | Abruf aller Tickets. Unterstützt Status-Filter per Query-Parameter (`?status=Open`). | `200 OK` | `500 Internal Error` |
| **GET** | `/api/tickets/{id}` | Abruf eines einzelnen Tickets über die Primärschlüssel-ID. | `200 OK` | `404 Not Found` |
| **POST** | `/api/tickets` | Neuanlage eines Tickets inkl. FluentValidation und Fallback-Injektion. | `201 Created` | `400 Bad Request` |
| **PUT** | `/api/tickets/{id}` | Vollständige Aktualisierung eines bestehenden Tickets. | `204 No Content`| `400`, `404`, `409` |
| **DELETE**| `/api/tickets/{id}` | Unwiderrufliches Löschen eines Tickets aus der Persistenzschicht. | `204 No Content`| `404 Not Found` |

---

## 7. Non-Functional Requirements (NFRs) & Zukunftsfähigkeit

* **Skalierbarkeit:** Die Stateless-Natur der Web API erlaubt ein horizontales Skalieren (Load Balancing über mehrere Instanzen hinter einem Reverse Proxy).
* **Testbarkeit:** Durch die konsequente Entkopplung von Services und den Einsatz von Interfaces ist die Anwendung vollständig Unit- und Integrationstest-fähig.
* **Dockerisierung:** Das System ist für den Betrieb in Containern optimiert (PostgreSQL läuft im Docker-Container, die API nutzt Umgebungsvariablen).