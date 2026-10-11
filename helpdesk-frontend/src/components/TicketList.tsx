import React, { useState } from 'react';
import type { Ticket, Priority } from '../types/ticket';
import { Trash2, UserCheck, Eye } from 'lucide-react';
import { TicketModal } from './TicketModal';

interface TicketListProps {
    tickets: Ticket[];
    onStatusChange: (id: number, newStatus: string) => void;
    onAssigneeChange: (id: number, assignee: string) => void;
    onDelete: (id: number) => void;
}

const getPriorityBadgeClass = (priority: Priority) => {
    switch (priority) {
        case 'High':
            return 'bg-red-100 text-red-700 border-red-200';
        case 'Medium':
            return 'bg-yellow-100 text-yellow-700 border-yellow-200';
        case 'Low':
            return 'bg-green-100 text-green-700 border-green-200';
        default:
            return 'bg-gray-100 text-gray-700 border-gray-200';
    }
};

export const TicketList: React.FC<TicketListProps> = ({
    tickets,
    onStatusChange,
    onAssigneeChange,
    onDelete
}) => {
    const [selectedTicket, setSelectedTicket] = useState<Ticket | null>(null);

    if (tickets.length === 0) {
        return (
            <div className= "bg-white p-8 rounded-lg shadow-sm border border-gray-200 text-center text-gray-500" >
            Keine Tickets vorhanden.
      </div>
    );
  }

return (
    <>
    <div className= "grid gap-4 md:grid-cols-2 lg:grid-cols-3" >
    {
        tickets.map((ticket) => (
            <TicketCard 
            key= { ticket.id } 
            ticket = { ticket } 
            onStatusChange = { onStatusChange }
            onAssigneeChange = { onAssigneeChange }
            onDelete = { onDelete }
            onViewDetails = {() => setSelectedTicket(ticket)}
    />
        ))}
</div>

    < TicketModal
ticket = { selectedTicket }
onClose = {() => setSelectedTicket(null)} 
      />
    </>
  );
};

interface TicketCardProps {
    ticket: Ticket;
    onStatusChange: (id: number, newStatus: string) => void;
    onAssigneeChange: (id: number, assignee: string) => void;
    onDelete: (id: number) => void;
    onViewDetails: () => void;
}

const TicketCard: React.FC<TicketCardProps> = ({ ticket, onStatusChange, onAssigneeChange, onDelete, onViewDetails }) => {
    const [assigneeInput, setAssigneeInput] = useState(ticket.assignedTo || '');

    const handleAssigneeBlur = () => {
        if (assigneeInput !== (ticket.assignedTo || '')) {
            onAssigneeChange(ticket.id, assigneeInput);
        }
    };

    return (
        <div className= "bg-white p-5 rounded-lg shadow-sm border border-gray-200 flex flex-col justify-between hover:shadow-md transition" >
        <div>
        <div className="flex justify-between items-start mb-2 gap-2" >
            <h3 
            onClick={ onViewDetails }
    className = "font-bold text-gray-800 break-words flex-1 cursor-pointer hover:text-blue-600 transition"
    title = "Details anzeigen"
        >
    { ticket.title }
        </h3>
        < span className = {`text-xs px-2.5 py-1 rounded-full font-semibold border ${getPriorityBadgeClass(ticket.priority)}`
}>
{ ticket.priority }
    </span>
    </div>

    < p className = "text-gray-600 text-sm mb-4 line-clamp-3" > { ticket.description } </p>
        </div>

        < div className = "space-y-3 pt-3 border-t border-gray-100" >
            <div className="flex items-center gap-2" >
                <UserCheck size={ 16 } className = "text-gray-400" />
                    <input
            type="text"
placeholder = "Zugewiesen an..."
value = { assigneeInput }
onChange = {(e) => setAssigneeInput(e.target.value)}
onBlur = { handleAssigneeBlur }
className = "w-full bg-gray-50 border border-gray-200 rounded px-2 py-1 text-xs text-gray-800 focus:outline-none focus:bg-white focus:border-blue-500"
    />
    </div>

    < div className = "flex items-center justify-between gap-2" >
        <select
            value={ ticket.status }
onChange = {(e) => onStatusChange(ticket.id, e.target.value)}
className = "bg-gray-50 border border-gray-300 rounded px-2.5 py-1.5 text-xs font-medium text-gray-800 focus:outline-none focus:ring-2 focus:ring-blue-500"
    >
    <option value="Open" > Offen </option>
        < option value = "InProgress" > In Bearbeitung </option>
            < option value = "Closed" > Geschlossen </option>
                </select>

                < div className = "flex items-center gap-1" >
                    <button
              onClick={ onViewDetails }
className = "p-1.5 text-gray-400 hover:text-blue-600 rounded transition"
title = "Details anzeigen"
    >
    <Eye size={ 16 } />
        </button>
        < button
onClick = {() => onDelete(ticket.id)}
className = "p-1.5 text-gray-400 hover:text-red-600 rounded transition"
title = "Ticket löschen"
    >
    <Trash2 size={ 16 } />
        </button>
        </div>
        </div>
        </div>
        </div>
  );
};