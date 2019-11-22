import { VOUTemplatePage } from './app.po';

describe('VOU App', function() {
  let page: VOUTemplatePage;

  beforeEach(() => {
    page = new VOUTemplatePage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
