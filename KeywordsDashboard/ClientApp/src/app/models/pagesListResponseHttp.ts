export interface PagesListResponseHttp {
    pages: ResponsePageData[],
    pagesCount: number,
}

interface ResponsePageData {
    id: number,
    link: string,
    title: string,
    description: string,
    languages: LanguagesInfo[],
    createdAt: Date,
    currentLanguageId: number | null
}

interface LanguagesInfo {
    id: number;
    code: string;
}
