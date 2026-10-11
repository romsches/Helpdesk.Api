import React from 'react';
import type { Ticket, Priority } from '../types/ticket';
import { X, Calendar, User, ShieldAlert, Tag } from 'lucide-react';

interface TicketModalProps {
    ticket: Ticket | null;
    onClose: () => void;
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

export const TicketModal: React.FC<TicketModalProps> = ({ ticket, onClose }) => {
    if (!ticket) return null;

    return (
        <div className= "fixed inset-0 z-50 flex items-center justify-center bg-black/50 backdrop-blur-sm p-4" >
        <div className="bg-white rounded-xl shadow-xl max-w-lg w-full overflow-hidden border border-gray-200 animate-in fade-in zoom-in duration-200" >
        {/* Шапка модального окна */ }
            < div className = "flex justify-between items-center px-6 py-4 border-b border-gray-100 bg-gray-50" >
                <div className="flex items-center gap-2" >
                    <Tag size={ 18 } className = "text-blue-600" />
                        <span className="text-xs font-bold text-gray-500 uppercase tracking-wider" > Ticket #{ ticket.id } </span>
                            </div>
                            < button
    onClick = { onClose }
    className = "text-gray-400 hover:text-gray-600 p-1 rounded-lg hover:bg-gray-200 transition"
        >
        <X size={ 20 } />
            </button>
            </div>

    {/* Тело модального окна */ }
    <div className="p-6 space-y-5" >
        <div>
        <div className="flex items-center justify-between gap-2 mb-2" >
            <h2 className="text-xl font-bold text-gray-900" > { ticket.title } </h2>
                < span className = {`text-xs px-2.5 py-1 rounded-full font-semibold border ${getPriorityBadgeClass(ticket.priority)}`
}>
{ ticket.priority }
    </span>
    </div>
    </div>

    < div >
    <h4 className="text-xs font-semibold text-gray-400 uppercase tracking-wider mb-1.5" > Beschreibung </h4>
        < div className = "bg-gray-50 p-4 rounded-lg border border-gray-200 text-sm text-gray-800 whitespace-pre-wrap leading-relaxed max-h-60 overflow-y-auto" >
        { ticket.description }
            </div>
            </div>

            < div className = "grid grid-cols-2 gap-4 pt-2 border-t border-gray-100 text-xs text-gray-600" >
                <div className="flex items-center gap-2" >
                    <User size={ 16 } className = "text-gray-400" />
                        <div>
                        <span className="block text-gray-400 font-medium" > Erstellt von </span>
                            < span className = "font-semibold text-gray-800" > { ticket.createdBy || 'Anonymous' } </span>
                                </div>
                                </div>

                                < div className = "flex items-center gap-2" >
                                    <ShieldAlert size={ 16 } className = "text-gray-400" />
                                        <div>
                                        <span className="block text-gray-400 font-medium" > Bearbeiter </span>
                                            < span className = "font-semibold text-gray-800" > { ticket.assignedTo || 'Unassigned' } </span>
                                                </div>
                                                </div>

                                                < div className = "flex items-center gap-2 col-span-2" >
                                                    <Calendar size={ 16 } className = "text-gray-400" />
                                                        <div>
                                                        <span className="block text-gray-400 font-medium" > Erstellt am </span>
                                                            < span className = "font-semibold text-gray-800" >
                                                            { ticket.createdAt ? new Date(ticket.createdAt).toLocaleString() : 'Unbekannt' }
                                                                </span>
                                                                </div>
                                                                </div>
                                                                </div>
                                                                </div>

{/* Футер */ }
<div className="px-6 py-3 bg-gray-50 border-t border-gray-100 flex justify-end" >
    <button
            onClick={ onClose }
className = "bg-gray-800 text-white px-4 py-2 rounded-lg text-xs font-semibold hover:bg-gray-900 transition"
    >
    Schließen
    </button>
    </div>
    </div>
    </div>
  );
};