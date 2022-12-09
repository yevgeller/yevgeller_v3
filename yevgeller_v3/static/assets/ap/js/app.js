'use strict';

var projectsApp = angular.module('projectsApp', [
  'ngRoute',
  'myProjList',
  'ngResource'
]);

var homePage = '/Static/angularProjects.html';

projectsApp.config(['$routeProvider',
  function ($routeProvider) {
    $routeProvider.
    when('/projects', {
      title: 'some projects',
      templateUrl: '/Static/assets/ap/partials/projectList.html',
      controller: 'ProjCtrl'
    }).
    when('/project/:projId', {
      title: 'this project',
      templateUrl: '/Static/assets/ap/partials/projectDetail.html',
      controller: 'ProjectDetailCtrl'
    }).
    otherwise({ redirectTo: '/projects' });
  }]);

projectsApp.run(['$location', '$rootScope', function ($location, $rootScope) {
  $rootScope.$on('$routeChangeSuccess', function (event, current, previous) {
    $rootScope.title = current.$$route.title;
  });
}]);