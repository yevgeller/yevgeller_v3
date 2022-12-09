'use strict';

describe('projects list', function() {
  beforeEach(function() {
    browser().navigateTo('index.html');
  });

  it('Categories displayed, more than one', function() {
    expect(repeater('.projectInfo').count()).toBeGreaterThan(0);
  });

  it('Search works', function () {
    expect(repeater('.projectInfo').count()).toBeGreaterThan(5);
    input('search').enter('xna');
    expect(repeater('.projectInfo').count()).toBe(2);
  });

  it('Displays search parameter properly', function() {
    input('search').enter('');
    expect(element('#searchQuery').text()).toMatch(/Current filter: \s*$/);
    input('search').enter('xna')
    expect(element('#searchQuery').text()).toMatch(/Current filter: xna\s*$/);
    using('#searchQuery').expect(binding('search')).toBe('xna');//same as the line above
  });

  it('should render phone specific links', function () {
    input('search').enter('hir');
    element('.projectInfo a').click();
    expect(browser().window().href()).toBe('http://localhost:9877/index.html#/project/6');
  });

  it('should redirect index.html to index.html#/projects', function () {
    browser().navigateTo('index.html');
    expect(browser().window().href()).toBe('http://localhost:9877/index.html#/projects');
  });
});

describe('Project detail view', function () {
  beforeEach(function () {
    browser().navigateTo('index.html#/project/5');
  });

  it('should display placeholder page with projectId', function () {
    expect(binding('projId')).toBe('5');
  });

  it('should have info about a project', function () {
    console.log(binding('unit.name'));
    expect(binding('unit')).not().toBe('US Citizenship Test');
  });
});