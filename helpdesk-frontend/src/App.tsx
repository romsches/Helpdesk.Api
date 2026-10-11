import { useEffect, useState } from 'react';
import { getTickets, createTicket, updateTicketStatus, updateTicketAssignee, deleteTicket } from './services/api';
import type { Ticket, CreateTicketDto } from './types/ticket';
import { TicketList } from './components/TicketList';
import { TicketForm } from './components/TicketForm';
import { LifeBuoy, Search, Filter } from 'lucide-react';

/**
 * Root application component managing the IT Helpdesk dashboard state, 
 * filtering, search capabilities, and CRUD operations.
 */
export default function App() {
    const [tickets, setTickets] = useState<Ticket[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [statusFilter, setStatusFilter] = useState<string>('All');
    const [priorityFilter, setPriorityFilter] = useState<string>('All');
    const [searchQuery, setSearchQuery] = useState<string>('');

    const fetchTickets = async () => {
        try {
            const data = await getTickets();
            setTickets(data);
        } catch (error) {
            console.error('Failed to fetch tickets:', error);
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
            await fetchTickets();
        } catch (error) {
            console.error('Failed to create ticket:', error);
        }
    };

    const handleStatusChange = async (id: number, newStatus: string) => {
        try {
            await updateTicketStatus(id, newStatus);
            await fetchTickets();
        } catch (error) {
            console.error('Failed to update ticket status:', error);
        }
    };

    const handleAssigneeChange = async (id: number, assignee: string) => {
        try {
            await updateTicketAssignee(id, assignee);
            await fetchTickets();
        } catch (error) {
            console.error('Failed to update ticket assignee:', error);
        }
    };

    const handleDelete = async (id: number) => {
        try {
            await deleteTicket(id);
            await fetchTickets();
        } catch (error) {
            console.error('Failed to delete ticket:', error);
        }
    };

    // Filter tickets by Status, Priority, and Search Query (Title, Description, Assignee)
    const filteredTickets = tickets.filter((ticket) => {
        const matchesStatus = statusFilter === 'All' || ticket.status === statusFilter;
        const matchesPriority = priorityFilter === 'All' || ticket.priority === priorityFilter;
        const matchesSearch =
            ticket.title.toLowerCase().includes(searchQuery.toLowerCase()) ||
            ticket.description.toLowerCase().includes(searchQuery.toLowerCase()) ||
            (ticket.assignedTo && ticket.assignedTo.toLowerCase().includes(searchQuery.toLowerCase()));

        return matchesStatus && matchesPriority && matchesSearch;
    });

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

                                < div className = "flex flex-col md:flex-row justify-between items-center gap-4 mb-6" >
                                    <h2 className="text-xl font-bold text-gray-800" >
                                        Current Tickets < span className = "text-sm font-normal text-gray-500" > ({ filteredTickets.length }) </span>
                                            </h2>

    {/* Search and Filters Panel */ }
    <div className="flex flex-wrap items-center gap-3 w-full md:w-auto" >
    {/* Search Input */ }
        < div className = "relative flex-1 md:w-64" >
            <Search className="absolute left-3 top-2.5 text-gray-400" size = { 16} />
                <input
                                type="text"
    placeholder = "Search by title, assignee..."
    value = { searchQuery }
    onChange = {(e) => setSearchQuery(e.target.value)
}
className = "w-full bg-white border border-gray-300 rounded-lg pl-9 pr-3 py-2 text-sm text-gray-900 focus:outline-none focus:ring-2 focus:ring-blue-500"
    />
    </div>

{/* Status Filter */ }
<div className="flex items-center gap-1.5 bg-white border border-gray-300 rounded-lg p-1 text-sm" >
    <Filter size={ 14 } className = "text-gray-400 ml-1.5" />
        <select
                                value={ statusFilter }
onChange = {(e) => setStatusFilter(e.target.value)}
className = "bg-transparent border-none text-gray-900 focus:outline-none pr-2 cursor-pointer text-xs font-medium"
    >
    <option value="All" > All Statuses </option>
        < option value = "Open" > Open </option>
            < option value = "InProgress" > In Progress </option>
                < option value = "Closed" > Closed </option>
                    < option value = "Resolved" > Resolved </option>
                        </select>
                        </div>

{/* Priority Filter */ }
<div className="flex items-center gap-1.5 bg-white border border-gray-300 rounded-lg p-1 text-sm" >
    <select
                                value={ priorityFilter }
onChange = {(e) => setPriorityFilter(e.target.value)}
className = "bg-transparent border-none text-gray-900 focus:outline-none pr-2 cursor-pointer text-xs font-medium"
    >
    <option value="All" > All Priorities </option>
        < option value = "Low" > Low </option>
            < option value = "Medium" > Medium </option>
                < option value = "High" > High </option>
                    </select>
                    </div>
                    </div>
                    </div>

{
    loading ? (
        <p className= "text-gray-500 text-center py-8" > Loading tickets...</p>
                ) : (
        <TicketList 
                        tickets= { filteredTickets }
    onStatusChange = { handleStatusChange }
    onAssigneeChange = { handleAssigneeChange }
    onDelete = { handleDelete }
        />
                )
}
</main>
    </div>
    );
}