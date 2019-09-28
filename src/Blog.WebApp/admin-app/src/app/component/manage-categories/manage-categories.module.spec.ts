import { ManageCategoriesModule } from './manage-categories.module';

describe('ManageCategoriesModule', () => {
  let manageCategoriesModule: ManageCategoriesModule;

  beforeEach(() => {
    manageCategoriesModule = new ManageCategoriesModule();
  });

  it('should create an instance', () => {
    expect(manageCategoriesModule).toBeTruthy();
  });
});
