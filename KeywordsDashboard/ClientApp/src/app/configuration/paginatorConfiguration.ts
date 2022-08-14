import { MatPaginatorIntl } from '@angular/material/paginator';

export class PaginatorConfiguration {
  public static ConfigurePaginatorTranslation(): MatPaginatorIntl {
    const customPaginatorIntl: MatPaginatorIntl = new MatPaginatorIntl();
    customPaginatorIntl.itemsPerPageLabel = '';
    customPaginatorIntl.nextPageLabel = "Следующая страница";
    customPaginatorIntl.previousPageLabel = "Предыдущая страница";
    customPaginatorIntl.getRangeLabel = (
      page: number,
      pageSize: number,
      length: number,
    ) => {
      if (length === 0 || pageSize === 0) {
        return `0 из ${length}`;
      }
      length = Math.max(length, 0);
      const startIndex = page * pageSize;
      const endIndex =
        startIndex < length
          ? Math.min(startIndex + pageSize, length)
          : startIndex + pageSize;
      return `${startIndex + 1} - ${endIndex} из ${length}`;
    };
    return customPaginatorIntl;
  }
}
