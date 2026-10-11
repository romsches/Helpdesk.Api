import React, { useState } from 'react';
import type { CreateTicketDto, Priority } from '../types/ticket';
import { PlusCircle } from 'lucide-react';

interface TicketFormProps {
    onCreate: (dto: CreateTicketDto) => void;
}

/**
 * Component for rendering the ticket creation form with validation inputs.
 */
export const TicketForm: React.FC<TicketFormProps> = ({ onCreate }) => {
    const [title, setTitle] = useState<string>('');
    const [description, setDescription] = useState<string>('');
    const [priority, setPriority] = useState<Priority>('Medium');

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        if (!title.trim() || !description.trim()) return;

        onCreate({ title, description, priority });
        setTitle('');
        setDescription('');
        setPriority('Medium');
    };

    return (
        <form onSubmit= { handleSubmit } className = "bg-white p-6 rounded-lg shadow border border-gray-200 mb-8" >
            <h2 className="text-xl font-bold text-gray-800 mb-4 flex items-center gap-2" >
                <PlusCircle className="text-blue-600" /> Create New Ticket
                    </h2>
                    < div className = "grid gap-4 md:grid-cols-2" >
                        <div>
                        <label className="block text-sm font-medium text-gray-700 mb-1" > Title </label>
                            < input
    type = "text"
    value = { title }
    onChange = {(e) => setTitle(e.target.value)}
required
className = "w-full bg-white text-gray-900 border border-gray-300 rounded px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
placeholder = "e.g., Printer is not working"
    />
    </div>
    < div >
    <label className="block text-sm font-medium text-gray-700 mb-1" > Priority </label>
        < select
value = { priority }
onChange = {(e) => setPriority(e.target.value as Priority)}
className = "w-full bg-white text-gray-900 border border-gray-300 rounded px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
    >
    <option value="Low" > Low </option>
        < option value = "Medium" > Medium </option>
            < option value = "High" > High </option>
                </select>
                </div>
                < div className = "md:col-span-2" >
                    <label className="block text-sm font-medium text-gray-700 mb-1" > Description </label>
                        < textarea
value = { description }
onChange = {(e) => setDescription(e.target.value)}
required
rows = { 2}
className = "w-full bg-white text-gray-900 border border-gray-300 rounded px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
placeholder = "Detailed description of the issue..."
    />
    </div>
    </div>
    < button
type = "submit"
className = "mt-4 bg-blue-600 text-white px-4 py-2 rounded text-sm font-medium hover:bg-blue-700 transition"
    >
    Submit Ticket
        </button>
        </form>
    );
};