import axios from 'axios';
import type { Ticket, CreateTicketDto } from '../types/ticket';

// Use environment variable if defined, otherwise fallback to local development URL
const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5208';
const TICKETS_ENDPOINT = `${API_BASE_URL}/api/tickets`;

/**
 * Retrieves all support tickets from the backend API.
 * @returns A promise resolving to an array of tickets.
 */
export const getTickets = async (): Promise<Ticket[]> => {
    const response = await axios.get<Ticket[]>(TICKETS_ENDPOINT);
    return response.data;
};

/**
 * Creates a new support ticket.
 * @param dto The ticket creation payload.
 * @returns A promise resolving to the created ticket.
 */
export const createTicket = async (dto: CreateTicketDto): Promise<Ticket> => {
    const response = await axios.post<Ticket>(TICKETS_ENDPOINT, dto);
    return response.data;
};

/**
 * Updates the status of an existing ticket.
 * @param id The unique identifier of the ticket.
 * @param status The new status value.
 * @returns A promise resolving upon successful update.
 */
export const updateTicketStatus = async (id: number, status: string): Promise<void> => {
    await axios.patch(`${TICKETS_ENDPOINT}/${id}/status`, { status }, {
        headers: { 'Content-Type': 'application/json' }
    });
};

/**
 * Updates the assigned support technician of an existing ticket.
 * @param id The unique identifier of the ticket.
 * @param assignedTo The name of the new assignee.
 * @returns A promise resolving upon successful update.
 */
export const updateTicketAssignee = async (id: number, assignedTo: string): Promise<void> => {
    await axios.patch(`${TICKETS_ENDPOINT}/${id}/assign`, { assignedTo }, {
        headers: { 'Content-Type': 'application/json' }
    });
};

/**
 * Permanently deletes a support ticket by its ID.
 * @param id The unique identifier of the ticket.
 * @returns A promise resolving upon successful deletion.
 */
export const deleteTicket = async (id: number): Promise<void> => {
    await axios.delete(`${TICKETS_ENDPOINT}/${id}`);
};