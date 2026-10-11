# Mini Helpdesk / Ticket-System – Backend & API

Eine containerisierte Helpdesk- und Ticket-Management-Anwendung mit C#, ASP.NET Core 10 Web API, Entity Framework Core und PostgreSQL. Das Projekt demonstriert die Entwicklung einer REST-API, die Speicherung relationaler Daten sowie typische IT-Support-Prozesse – von der Erstellung und Priorisierung von Tickets bis hin zur Statusverwaltung und Zuweisung an Support-Mitarbeiter.

## Inhaltsverzeichnis

- [Projektübersicht](#projektübersicht)
- [Technologie-Stack](#technologie-stack)
- [Architektur und Projektstruktur](#architektur-und-projektstruktur)
- [Funktionalitäten](#funktionalitäten)
- [Installation und Start](#installation-und-start)
- [REST-API und Swagger](#rest-api-und-swagger)
- [Mögliche Erweiterungen](#mögliche-erweiterungen)
- [Repository](#repository)

## Projektübersicht

Die Anwendung stellt eine REST-API für die Verwaltung von IT-Support-Anfragen bereit. Tickets können mit einem Titel, einer Beschreibung und einer Priorität erstellt werden. Darüber hinaus unterstützt das Backend die Abfrage, Filterung und Bearbeitung von Tickets sowie die Verwaltung ihres Status und ihrer Zuweisung.

Die Anwendung bildet grundlegende Abläufe eines IT-Helpdesks ab und ermöglicht die Kommunikation zwischen Clients und Backend über HTTP und JSON.

Die Komponenten werden mithilfe von Docker Compose gemeinsam gestartet.

**Projektziele:**

- Entwicklung einer REST-API mit C# und ASP.NET Core 10
- Strukturierung des Backends nach den Prinzipien einer geschichteten Architektur
- Datenpersistenz mit Entity Framework Core und PostgreSQL
- Umsetzung typischer IT-Support- und Ticket-Management-Prozesse
- Validierung von Eingabedaten mit FluentValidation
- Implementierung einer zentralen Fehlerbehandlung
- Containerisierung mit Docker und Docker Compose
- Dokumentation und Testen der API mit Swagger UI

## Technologie-Stack

### Backend

- C#
- ASP.NET Core 10 Web API
- Entity Framework Core
- FluentValidation
- Serilog
- REST-API
- Swagger / OpenAPI

### Datenbank und Infrastruktur

- PostgreSQL
- Docker
- Docker Compose

### Entwicklung und Dokumentation

- Git
- GitHub
- Swagger UI

## Architektur und Projektstruktur

Das Backend folgt einer geschichteten Struktur mit klar getrennten Verantwortlichkeiten. Die API verarbeitet eingehende HTTP-Anfragen, die Datenmodelle bilden die Support-Tickets ab und Entity Framework Core übernimmt den Zugriff auf die Datenbank.

FluentValidation dient der Prüfung eingehender Daten. Eine zentrale Middleware ermöglicht eine einheitliche Behandlung von Fehlern. Serilog unterstützt die Protokollierung der Anwendung.

Die Projektstruktur orientiert sich an folgender Organisation:

```text
mini-helpdesk/
├── Helpdesk.Api/
│   ├── Controllers/
│   │   └── TicketsController.cs
│   ├── Data/
│   │   └── AppDbContext.cs
│   ├── Models/
│   │   └── Ticket.cs
│   ├── Validators/
│   │   └── TicketValidator.cs
│   ├── Middleware/
│   │   └── ExceptionHandlingMiddleware.cs
│   ├── Dockerfile
│   └── ...
├── docker-compose.yml
└── README.md
```

Die genaue Verzeichnisstruktur kann je nach aktuellem Entwicklungsstand abweichen.

## Funktionalitäten

### Ticket-Verwaltung

- Tickets mit Titel, Beschreibung und Priorität erstellen
- Standardwerte für nicht angegebene Angaben wie Ersteller und Bearbeiter verwenden
- Alle Tickets abrufen
- Einzelne Tickets anhand ihrer ID abrufen
- Tickets nach Status filtern
- Status eines Tickets aktualisieren
- Tickets einem Bearbeiter zuweisen
- Tickets löschen

### Validierung und Fehlerbehandlung

- Eingabedaten vor der Verarbeitung validieren
- Titel, Beschreibung und Priorität auf zulässige Werte prüfen
- Fehler zentral über eine Middleware behandeln
- Anwendungsereignisse und Fehler protokollieren

### Backend und Datenbank

- REST-Endpunkte für die Kommunikation über HTTP und JSON
- Datenbankzugriff mit Entity Framework Core
- Speicherung von Tickets in PostgreSQL
- Erfassung des Lösungszeitpunkts (`ResolvedAt`) beim Abschließen eines Tickets
- Interaktive API-Dokumentation mit Swagger UI

### Containerisierung

- Backend und PostgreSQL über Docker Compose starten
- Dienste in einem isolierten Containernetzwerk betreiben
- PostgreSQL-Daten über ein persistentes Docker-Volume speichern

## Installation und Start

### Voraussetzungen

- Git
- Docker Desktop

### 1. Repository klonen

```bash
git clone https://github.com/romsches/mini-helpdesk.git
cd mini-helpdesk
```

### 2. Docker Desktop starten

Stelle sicher, dass Docker Desktop gestartet ist und Docker-Container ausgeführt werden können.

### 3. Anwendung starten

Führe im Projektverzeichnis folgenden Befehl aus:

```bash
docker compose up --build -d
```

Docker Compose erstellt die benötigten Images und startet die in der Compose-Datei definierten Dienste.

### 4. Anwendung öffnen

| Komponente | Adresse |
|---|---|
| Backend und Swagger UI | http://localhost:5208/swagger/index.html |

Die angegebene Adresse setzt voraus, dass der API-Port in der Docker-Konfiguration entsprechend veröffentlicht wird.

### Anwendung stoppen

```bash
docker compose down
```

Dieser Befehl stoppt und entfernt die zugehörigen Container und das Compose-Netzwerk. Das Datenbankvolume wird dabei nicht ausdrücklich gelöscht.

**Hinweis:** Verwende `docker compose down -v` nicht, wenn du die gespeicherten Daten behalten möchtest.

## REST-API und Swagger

Die REST-API ermöglicht die Verwaltung von Support-Tickets über HTTP-Anfragen. Swagger UI stellt die dokumentierten Endpunkte übersichtlich dar und erlaubt es, Requests direkt im Browser auszuführen und die API-Antworten zu überprüfen.

**Swagger UI:** http://localhost:5208/swagger/index.html

Die verfügbaren Endpunkte umfassen entsprechend der Implementierung unter anderem:

| HTTP-Methode | Funktion |
|---|---|
| `GET` | Tickets abrufen |
| `GET` | Einzelnes Ticket anhand der ID abrufen |
| `POST` | Neues Ticket erstellen |
| `PATCH` | Ticketstatus oder Bearbeiter aktualisieren |
| `DELETE` | Ticket löschen |

Die konkreten Routen und Parameter sind der Swagger-Dokumentation der laufenden Anwendung zu entnehmen.

## Mögliche Erweiterungen

- React- und TypeScript-Frontend für die Ticketverwaltung
- JWT-basierte Authentifizierung
- Rollenbasierte Autorisierung für Benutzer und Support-Mitarbeiter
- Erweiterte Such-, Filter- und Sortierfunktionen
- Unit- und Integrationstests
- CI/CD-Pipeline für automatisierte Builds und Tests
- Erweiterte Protokollierung und Monitoring
- Deployment in einer Cloud-Umgebung

Diese Punkte sind mögliche Erweiterungen und keine Aussage über bereits implementierte Funktionen.

## Repository

GitHub: https://github.com/romsches/mini-helpdesk