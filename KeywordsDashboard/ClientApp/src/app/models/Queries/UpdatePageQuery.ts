export interface UpdatePageQuery {
    PageAttributes: PageAttributes[];
}

export interface PageAttributes {
    Title: string,
    Description: string,
    LanguageId: number,
    Keywords: Object[],
}
