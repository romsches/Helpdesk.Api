import axios from 'axios';
import type { Ticket, CreateTicketDto } from '../types/ticket';

const API_URL = 'http://localhost:5208/api/tickets';

export const getTickets = async () => {
    const response = await axios.get<Ticket[]>(API_URL);
    return response.data;
};

export const createTicket = async (dto: CreateTicketDto) => {
    const response = await axios.post<Ticket>(API_URL, dto);
    return response.data;
};

export const updateTicketStatus = async (id: number, status: string) => {
    const response = await axios.patch(`${API_URL}/${id}/status`, { status }, {
        headers: { 'Content-Type': 'application/json' }
    });
    return response.data;
};

export const updateTicketAssignee = async (id: number, assignedTo: string) => {
    const response = await axios.patch(`${API_URL}/${id}/assign`, { assignedTo }, {
        headers: { 'Content-Type': 'application/json' }
    });
    return response.data;
};

export const deleteTicket = async (id: number) => {
    await axios.delete(`${API_URL}/${id}`);
};