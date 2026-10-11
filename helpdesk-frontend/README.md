# Helpdesk Frontend

The frontend application for the **IT Helpdesk & Ticket System**, built with React and TypeScript. It communicates with the ASP.NET Core Web API to manage support tickets.

## Tech Stack

- **React 19** — User interface
- **TypeScript** — Static typing and type safety
- **Vite** — Development server and build tool
- **Axios** — HTTP requests to the backend API
- **Tailwind CSS** — UI styling, currently loaded via CDN
- **Lucide React** — Interface icons

## Features

- View the list of support tickets.
- Create tickets with a title, description, and priority.
- Update ticket statuses.
- Delete tickets.
- Display ticket priorities and statuses with visual indicators.
- Responsive ticket card layout.
- Type-safe data models using TypeScript.

## Project Structure

```text
helpdesk-frontend/
├── public/                 # Static assets
├── src/
│   ├── components/
│   │   ├── TicketForm.tsx  # Ticket creation form
│   │   └── TicketList.tsx  # Ticket list and actions
│   ├── services/
│   │   └── api.ts          # Backend API integration
│   ├── types/
│   │   └── ticket.ts       # Ticket types and DTOs
│   ├── App.tsx             # Main application component
│   ├── main.tsx            # Application entry point
│   └── index.css           # Global styles
├── index.html              # HTML entry point
├── package.json            # Dependencies and scripts
├── tsconfig.json           # TypeScript configuration
└── vite.config.ts          # Vite configuration
```

## Prerequisites

- [Node.js](https://nodejs.org/) compatible with the installed Vite version
- npm
- A running instance of the Helpdesk backend API

## Getting Started

### 1. Install Dependencies

Open a terminal in the frontend directory and run:

```bash
npm install
```

### 2. Configure the Backend API

The API base URL is defined in `src/services/api.ts`.

```ts
const API_URL = 'http://localhost:5208/api/tickets';
```

Make sure the URL matches the address and port exposed by your running backend.

### 3. Start the Development Server

```bash
npm run dev
```

Open the local URL displayed in the terminal. By default, Vite uses:

`http://localhost:5173`

## Available Scripts

| Command | Description |
|---|---|
| `npm run dev` | Starts the Vite development server. |
| `npm run build` | Runs TypeScript checks and builds the application. |
| `npm run lint` | Runs Oxlint. |
| `npm run preview` | Previews the production build locally. |

## Production Build

To create an optimized production build, run:

```bash
npm run build
```

The generated files are placed in the `dist/` directory.

To preview the build locally:

```bash
npm run preview
```

## Docker

The current Docker Compose configuration defines the PostgreSQL database and ASP.NET Core backend.

The frontend can also be containerized, but this requires a frontend Dockerfile or another suitable frontend-serving configuration.

## Notes

- The backend API must be running for ticket operations to work.
- The frontend API URL must match the backend's exposed address and port.
- Tailwind CSS is currently loaded through its CDN script in `index.html`. For production deployments, use a supported build integration instead.

## License

No license has been specified for this project.