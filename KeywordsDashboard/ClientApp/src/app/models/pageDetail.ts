import { Keyword } from './pageResponseHttp';

export interface PageDetail {
    pageId: number,
    link: string,
    heading: string,
    languageName: string,
    pageTranslationId: number | undefined,
    title: string,
    description: string,
    languageId: number,
    keywords: Keyword[]
}
