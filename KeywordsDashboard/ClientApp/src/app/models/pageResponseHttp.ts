export interface PagesResponseHttp {
    id: number,
    link: string,
    pageTranslations: pageTranslation[]
}

export interface pageTranslation {
    id: number,
    title: string,
    description: string,
    languageId: number,
    keywords: Keyword[]
}

export interface Keyword {
    id: number;
    phrase: string;
    order: number
}
