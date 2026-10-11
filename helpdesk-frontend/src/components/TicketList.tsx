import React from 'react';
import type { Ticket } from '../types/ticket';
import { Trash2, CheckCircle, Clock, AlertCircle } from 'lucide-react';

interface TicketListProps {
    tickets: Ticket[];
    onStatusChange: (id: number, newStatus: string) => void;
    onDelete: (id: number) => void;
}

export const TicketList: React.FC<TicketListProps> = ({ tickets, onStatusChange, onDelete }) => {
    if (tickets.length === 0) {
        return <div className="text-gray-500 text-center py-6" > Keine Tickets vorhanden.</div>;
    }

    const getPriorityBadge = (priority: string) => {
        switch (priority) {
            case 'High': return <span className="px-2 py-1 text-xs font-bold text-red-700 bg-red-100 rounded" > Hoch </span>;
            case 'Medium': return <span className="px-2 py-1 text-xs font-bold text-yellow-700 bg-yellow-100 rounded" > Mittel </span>;
            default: return <span className="px-2 py-1 text-xs font-bold text-green-700 bg-green-100 rounded" > Niedrig </span>;
        }
    };

    const getStatusBadge = (status: string) => {
        switch (status) {
            case 'Closed': return <span className="flex items-center gap-1 text-gray-600 bg-gray-100 px-2 py-1 rounded text-xs font-semibold" > <CheckCircle size={ 14 } /> Geschlossen</span >;
            case 'InProgress': return <span className="flex items-center gap-1 text-blue-600 bg-blue-100 px-2 py-1 rounded text-xs font-semibold" > <Clock size={ 14 } /> In Bearbeitung</span >;
            default: return <span className="flex items-center gap-1 text-orange-600 bg-orange-100 px-2 py-1 rounded text-xs font-semibold" > <AlertCircle size={ 14 } /> Offen</span >;
        }
    };

    return (
        <div className= "grid gap-4 md:grid-cols-2 lg:grid-cols-3" >
        {
            tickets.map((ticket) => (
                <div key= { ticket.id } className = "bg-white p-5 rounded-lg shadow border border-gray-200 flex flex-col justify-between" >
                <div>
                <div className="flex justify-between items-start mb-2" >
            <h3 className="font-bold text-lg text-gray-800" > { ticket.title } </h3>
              { getPriorityBadge(ticket.priority)
        }
        </div>
        < p className = "text-gray-600 text-sm mb-4 line-clamp-3" > { ticket.description } </p>
            </div>

            < div >
            <div className="flex items-center justify-between mb-4" >
            { getStatusBadge(ticket.status) }
                < span className = "text-xs text-gray-400" > ID: #{ ticket.id } </span>
                    </div>

                    < div className = "flex items-center justify-between pt-3 border-t border-gray-100" >
                        <select
                value={ ticket.status }
    onChange = {(e) => onStatusChange(ticket.id, e.target.value)}
className = "text-xs border border-gray-300 rounded px-2 py-1 bg-gray-50 focus:outline-none focus:ring-1 focus:ring-blue-500"
    >
    <option value="Open" > Offen </option>
        < option value = "InProgress" > In Bearbeitung </option>
            < option value = "Closed" > Geschlossen </option>
                </select>

                < button
onClick = {() => onDelete(ticket.id)}
className = "text-red-500 hover:text-red-700 p-1 rounded transition"
title = "Löschen"
    >
    <Trash2 size={ 16 } />
        </button>
        </div>
        </div>
        </div>
      ))}
</div>
  );
};