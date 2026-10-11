/**
 * Defines the allowed priority levels for a support ticket.
 */
export type Priority = 'Low' | 'Medium' | 'High';

/**
 * Defines the allowed lifecycle statuses for a support ticket.
 */
export type Status = 'Open' | 'InProgress' | 'Closed' | 'Resolved';

/**
 * Represents a support ticket entity within the frontend application.
 */
export interface Ticket {
    id: number;
    title: string;
    description: string;
    status: Status;
    priority: Priority;
    createdBy: string;
    assignedTo: string;
    createdAt: string;
    resolvedAt?: string | null;
}

/**
 * Data Transfer Object for creating a new support ticket.
 */
export interface CreateTicketDto {
    title: string;
    description: string;
    priority: Priority;
}