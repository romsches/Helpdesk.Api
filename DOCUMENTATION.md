# Technische Projektdokumentation – Mini Helpdesk

Diese Dokumentation beschreibt die Architektur, die technischen Komponenten, das Datenmodell, die REST-API sowie die Containerisierung des Mini-Helpdesk-Systems. Sie dient als technische Referenz für Entwicklung, Wartung und mögliche Erweiterungen.

## 1. Projektübersicht

**Mini Helpdesk** ist eine containerisierte Web-API zur Verwaltung von Support-Tickets. Das System ermöglicht das Erstellen, Abrufen, Aktualisieren und Löschen von Tickets sowie die Verwaltung ihres Bearbeitungsstatus und ihrer Priorität.

### Technologie-Stack

| Bereich | Technologie |
| :--- | :--- |
| Programmiersprache | C# |
| Backend-Framework | ASP.NET Core 10 Web API |
| Datenzugriff | Entity Framework Core |
| Datenbank | PostgreSQL |
| Eingabevalidierung | FluentValidation |
| Logging | Serilog |
| API-Dokumentation | Swagger / OpenAPI |
| Containerisierung | Docker, Docker Compose |

## 2. Architektur und Design

Die Anwendung folgt einer **schichtenorientierten Architektur (Layered Architecture)** innerhalb des Backend-Projekts. Ziel ist es, HTTP-Verarbeitung, Validierung, Datenmodellierung und Datenbankzugriff möglichst klar voneinander zu trennen.

### 2.1 Präsentationsschicht – API

Die Präsentationsschicht bildet die Schnittstelle zwischen Client und Backend.

- **Controller:** Verarbeiten eingehende HTTP-Anfragen und stellen die API-Endpunkte bereit.
- **Routing:** Ordnet Anfragen den entsprechenden Controller-Aktionen zu.
- **HTTP-Statuscodes:** Kommunizieren das Ergebnis einer Anfrage.
- **Swagger / OpenAPI:** Stellt die verfügbaren Endpunkte und deren Parameter interaktiv dar.

Controller sollten sich auf die Verarbeitung von HTTP-Anfragen konzentrieren und unnötig komplexe Logik vermeiden.

### 2.2 Validierungsschicht

FluentValidation wird zur Prüfung eingehender Daten verwendet.

Beispiele für mögliche Validierungsregeln:

- Der Ticket-Titel darf nicht leer sein.
- Der Titel darf die definierte maximale Länge nicht überschreiten.
- Erforderliche Felder müssen vorhanden sein.
- Prioritätswerte müssen den unterstützten Werten entsprechen.

Ungültige Eingaben sollen kontrolliert zurückgewiesen werden, üblicherweise mit `400 Bad Request`.

### 2.3 Datenmodell und Anwendungslogik

Die Ticket-Entität bildet das zentrale Datenmodell der Anwendung.

Sie enthält die fachlichen Informationen eines Support-Tickets, beispielsweise Titel, Beschreibung, Status, Priorität, Ersteller und zuständigen Bearbeiter.

Die Anwendungslogik verwaltet unter anderem:

- die Erstellung neuer Tickets,
- die Änderung des Ticket-Status,
- die Zuweisung eines Bearbeiters,
- die Erfassung des Abschlusszeitpunkts,
- die Bereitstellung gefilterter Ticketlisten.

Fehlende optionale Angaben wie Ersteller oder Bearbeiter können mit definierten Standardwerten ergänzt werden, sofern dies im jeweiligen Anwendungsfall vorgesehen ist.

### 2.4 Datenzugriffsschicht

Entity Framework Core übernimmt den objektrelationalen Datenzugriff zwischen C# und PostgreSQL.

Der `AppDbContext` repräsentiert die Datenbankverbindung und stellt die verwalteten Entitäten für Abfragen und Änderungen bereit.

Wesentliche Aufgaben:

- Abfragen und Speichern von Ticket-Datensätzen,
- asynchrone Datenbankoperationen,
- Nachverfolgung von Änderungen an Entitäten,
- Verwaltung des Datenbankschemas über EF-Core-Migrations.

## 3. Datenbank und Datenmodell

**Datenbanksystem:** PostgreSQL

Die Datenbank speichert Tickets dauerhaft. In der Docker-Umgebung wird dafür ein persistentes Volume verwendet, damit die Daten beim Stoppen oder Neuerstellen des Containers nicht automatisch verloren gehen.

### 3.1 Ticket-Entität

Das zentrale Datenmodell umfasst folgende Felder, entsprechend der beschriebenen Implementierung:

| Feld | Datentyp | Bedeutung |
| :--- | :--- | :--- |
| `Id` | `int` | Eindeutige Ticket-ID und Primärschlüssel |
| `Title` | `string` | Titel des Tickets |
| `Description` | `string` | Beschreibung des Problems |
| `Status` | `string` | Aktueller Bearbeitungsstatus |
| `Priority` | `string` | Priorität des Tickets |
| `CreatedBy` | `string?` | Optionaler Ersteller |
| `AssignedTo` | `string?` | Optional zugewiesener Bearbeiter |
| `CreatedAt` | `DateTime` | Zeitpunkt der Erstellung |
| `ResolvedAt` | `DateTime?` | Optionaler Zeitpunkt des Abschlusses |

Die konkreten Pflichtfelder, Standardwerte, Längenbegrenzungen und Datenbank-Constraints werden durch die Implementierung und die Datenbankkonfiguration bestimmt.

### 3.2 Status und Prioritäten

Beispielhafte Statuswerte:

- `Open` – Ticket wurde erstellt und ist noch offen.
- `In Progress` – Ticket wird bearbeitet.
- `Closed` – Ticket wurde geschlossen.

Prioritätswerte:

- `Low` – niedrige Priorität
- `Medium` – mittlere Priorität
- `High` – hohe Priorität

Die API sollte nur die Werte akzeptieren, die von der Implementierung tatsächlich unterstützt werden.

### 3.3 Zeitstempel und Abschluss

`CreatedAt` dokumentiert den Erstellungszeitpunkt eines Tickets. `ResolvedAt` kann den Zeitpunkt der Lösung oder Schließung erfassen.

Für eine konsistente Verarbeitung über verschiedene Zeitzonen hinweg empfiehlt sich die Speicherung von Zeitpunkten in UTC. Ob dies bereits vollständig umgesetzt ist, richtet sich nach der konkreten Implementierung.

## 4. REST-API und HTTP-Kommunikation

Die REST-API ermöglicht den Zugriff auf Tickets über HTTP. Daten werden üblicherweise im JSON-Format übertragen.

### 4.1 Endpunkte

Die folgende Tabelle beschreibt die vorgesehenen Operationen. Die tatsächlichen Routen und HTTP-Methoden müssen mit `TicketsController.cs` übereinstimmen.

| Methode | Endpunkt | Funktion |
| :--- | :--- | :--- |
| `GET` | `/api/tickets` | Tickets abrufen |
| `GET` | `/api/tickets/{id}` | Einzelnes Ticket abrufen |
| `POST` | `/api/tickets` | Neues Ticket erstellen |
| `PUT` oder `PATCH` | `/api/tickets/{id}` | Ticket oder einzelne Felder aktualisieren |
| `DELETE` | `/api/tickets/{id}` | Ticket löschen |

### 4.2 HTTP-Statuscodes

| Statuscode | Bedeutung |
| :--- | :--- |
| `200 OK` | Anfrage erfolgreich verarbeitet |
| `201 Created` | Ressource erfolgreich erstellt |
| `204 No Content` | Operation erfolgreich, keine Antwortdaten erforderlich |
| `400 Bad Request` | Ungültige Anfrage oder Validierungsfehler |
| `404 Not Found` | Angefordertes Ticket nicht gefunden |
| `500 Internal Server Error` | Unerwarteter serverseitiger Fehler |

Der konkrete Statuscode hängt von der jeweiligen Controller-Aktion und der implementierten Fehlerbehandlung ab.

### 4.3 API-Dokumentation mit Swagger

Swagger / OpenAPI ermöglicht es, die API-Endpunkte direkt im Browser zu untersuchen und Anfragen auszuführen.

Dadurch können beispielsweise Ticket-Erstellung, Abfragen und Statusänderungen getestet werden, ohne dafür zunächst ein eigenes Frontend entwickeln zu müssen.

## 5. Validierung und Fehlerbehandlung

Die Anwendung soll fehlerhafte Eingaben und unerwartete Laufzeitfehler kontrolliert behandeln.

### 5.1 Eingabevalidierung

FluentValidation ermöglicht die Definition zentraler und wiederverwendbarer Validierungsregeln. Dadurch können ungültige Eingaben frühzeitig erkannt werden, bevor sie zu unerwünschten Datenbankoperationen führen.

### 5.2 Globale Fehlerbehandlung

Eine zentrale Exception-Handling-Middleware kann unbehandelte Ausnahmen abfangen und standardisierte HTTP-Fehlerantworten zurückgeben.

Dabei gelten folgende Grundsätze:

- Keine internen Stacktraces oder vertraulichen Informationen an Clients ausgeben.
- Fehler mit geeigneten Details serverseitig protokollieren.
- Erwartbare Fehler, etwa ein nicht vorhandenes Ticket, mit passenden Statuscodes beantworten.
- Unerwartete Fehler kontrolliert behandeln.

Die tatsächliche Antwortstruktur und die erfassten Ausnahmen richten sich nach der implementierten Middleware.

## 6. Logging und Diagnose

Serilog dient der strukturierten Protokollierung von Anwendungsereignissen.

Das Logging kann dabei helfen, Fehler und unerwartetes Verhalten nachzuvollziehen, beispielsweise bei:

- Anwendungsstart und -beendigung,
- fehlgeschlagenen Datenbankoperationen,
- unerwarteten Ausnahmen,
- der Verarbeitung von HTTP-Anfragen.

Für eine aussagekräftige Diagnose sind insbesondere HTTP-Methode, Request-Pfad, Statuscode und