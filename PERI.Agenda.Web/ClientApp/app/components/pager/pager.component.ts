export class Pager {
    totalItems: number;
    currentPage: number;
    pageSize: number;
    totalPages: number;
    startPage: number;
    endPage: number;

    // Added for special purpose
    pageIndeces: number[];
}