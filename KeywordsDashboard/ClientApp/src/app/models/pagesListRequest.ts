export class PagesListRequest {
    constructor(
        public LanguageId: number,
        public PageNumber: number,
        public Limit: number,
        public SortField: string,
        public SortDirection: string
    ) { }
}
