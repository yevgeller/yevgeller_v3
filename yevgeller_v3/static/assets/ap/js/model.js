'use strict';

var myProjList = angular.module('myProjList', []); //ng-app directive
//myProjList contains ProjCtrl controller (a function that contains a $scope parameter)
//myProjList.controller('ProjCtrl', function ($scope, $http) {//<body ... ng-controller="ProjCtrl">
myProjList.controller('ProjCtrl', ['$scope', '$http',
  function ($scope, $http) {
    //$http is a built-in service to angular
    //to use a service in angular, you declare the names of the dependencies
    //as args to the controller ctor
    //the names are significant, angular looks up dependencies by names
    $http.get('/Static/assets/ap/data/projects.js').success(function (data) {
      $scope.projects = data;

      var lookup = {};
      var result = [];
      for (var item, i = 0; item = data[i++];) {
        var name = item.category;
        if (!(name in lookup)) {
          lookup[name] = 1;
          result.push(name);
        }
      }
      result.sort();
      $scope.cats = result;
      //console.log(data[0]);
    });
    //console.log('got data');
  }]);

myProjList.controller('ProjectDetailCtrl', ['$scope', '$http', '$routeParams',
  function ($scope, $http, $routeParams) {
    $http.get('/Static/assets/ap/data/projects.js').success(function (data) {
      for (var i = 0; i < data.length; i++) {
        if (data[i]['id'] == $routeParams.projId) {
          $scope.unit = data[i];
          break;
        }
      }
      if ($scope.unit.scr != undefined)
        $scope.screenShots = $scope.unit.scr.split(";");

      if ($scope.unit == undefined) {
        $scope.unit = { name: "No such project", linkwording: "Back to projects", url: "/index.html" };
      } else {
        //console.log("link: " + $scope.unit.url);
        if ($scope.unit.url.toLowerCase().indexOf('project/') > -1) {//no link if the project only has screenshots
          $scope.unit.linkwording = null;
          $scope.unit.url = null;
        }
      }
    });
    //$scope.projId = $routeParams.projId;
  }]);