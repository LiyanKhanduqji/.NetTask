// Defines the structure for pagination metadata
export interface Pagination {
  currentPage: number;
  itemsPerPage: number;
  totalItems: number;
  totalPages: number;
}

// A generic class that holds paginated data (items) and the pagination metadata
export class PaginatedResult<T> {
  items?: T;
  pagination?: Pagination;
}
