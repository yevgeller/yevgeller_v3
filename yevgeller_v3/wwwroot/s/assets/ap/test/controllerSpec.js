'use strict';

describe('MyProj controllers', function () { //random name

  describe('ProjCtrl', function () { //myProjList.controller('ProjCtrl')
    var scope, ctrl, $httpBackend;
    beforeEach(module('myProjList'));
    beforeEach(inject(function (_$httpBackend_, $rootScope, $controller) {
      $httpBackend = _$httpBackend_;//step-5 explains
      $httpBackend.expectGET('data/projects.js').
        respond([{ name: "project1" }, { name: "project2" }]);

      scope = $rootScope.$new();
      ctrl = $controller('ProjCtrl', { $scope: scope });
    }));

    it('should create projects model with 2 projects fetched from xhr', function () {
      expect(scope.projects).toBeUndefined();
      $httpBackend.flush();

      expect(scope.projects).toEqual([{ name: "project1" }, { name: "project2" }]);
    });

    //it('should have categories', function () {
    //  expect(scope.cats).toBeUndefined();
    //  $httpBackend.flush();

    //  expect(scope.cats.length).toEqual(3);
    //  //expect(scope.cats().length).toBe(3);
    //});
    //beforeEach(module('myProjList'));

    //it('should create Projects controller with a bunch of projects',
    //  inject(function ($controller) {
    //    var scope = {},
    //      //$controller is a service that retrieves controllers by name
    //        ctrl = $controller('ProjCtrl', {$scope:scope});

    //    expect(scope.projects.length).toBeGreaterThan(0);
    //  }));
  });
});