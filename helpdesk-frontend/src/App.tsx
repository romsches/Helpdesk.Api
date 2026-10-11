import { useEffect, useState } from 'react';
import { getTickets, createTicket, updateTicketStatus, deleteTicket } from './services/api';
import type { Ticket, CreateTicketDto } from './types/ticket';
import { TicketList } from './components/TicketList';
import { TicketForm } from './components/TicketForm';
import { LifeBuoy } from 'lucide-react';

export default function App() {
    const [tickets, setTickets] = useState<Ticket[]>([]);
    const [loading, setLoading] = useState(true);

    const fetchTickets = async () => {
        try {
            const data = await getTickets();
            setTickets(data);
        } catch (error) {
            console.error('Fehler beim Laden der Tickets:', error);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchTickets();
    }, []);

    const handleCreate = async (dto: CreateTicketDto) => {
        try {
            await createTicket(dto);
            fetchTickets();
        } catch (error) {
            console.error('Fehler beim Erstellen:', error);
        }
    };

    const handleStatusChange = async (id: number, newStatus: string) => {
        try {
            await updateTicketStatus(id, newStatus);
            fetchTickets();
        } catch (error) {
            console.error('Fehler beim Aktualisieren des Status:', error);
        }
    };

    const handleDelete = async (id: number) => {
        try {
            await deleteTicket(id);
            fetchTickets();
        } catch (error) {
            console.error('Fehler beim Löschen:', error);
        }
    };

    return (
        <div className= "min-h-screen bg-gray-100 text-gray-900 pb-12" >
        <header className="bg-white shadow border-b border-gray-200 mb-8" >
            <div className="max-w-7xl mx-auto px-4 py-4 flex items-center gap-3" >
                <LifeBuoy className="text-blue-600" size = { 32} />
                    <h1 className="text-2xl font-bold text-gray-800" > IT Helpdesk & Ticket System </h1>
                        </div>
                        </header>

                        < main className = "max-w-7xl mx-auto px-4" >
                            <TicketForm onCreate={ handleCreate } />

                                < h2 className = "text-xl font-bold text-gray-800 mb-4" > Aktuelle Tickets </h2>
    {
        loading ? (
            <p className= "text-gray-500" > Lade Tickets...</p>
        ) : (
            <TicketList tickets= { tickets } onStatusChange = { handleStatusChange } onDelete = { handleDelete } />
        )
    }
    </main>
        </div>
  );
}