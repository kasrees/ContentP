export interface PageIndexData {
    pages: PageIndex[];
    pagesCount: number;
}

export interface PageIndex {
    id: number
    title: string,
    link: string,
    description: string,
    languages: string,
    isEmpty: boolean
}
