export type Priority = 'Low' | 'Medium' | 'High';
export type Status = 'Open' | 'InProgress' | 'Closed';

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

export interface CreateTicketDto {
    title: string;
    description: string;
    priority: Priority;
}