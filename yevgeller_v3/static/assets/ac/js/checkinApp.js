'use strict';

var checkinApp = angular.module('checkinApp', [])

checkinApp.controller('SignInCtrl', ['$scope', '$http', '$timeout',
    function ($scope, $http, $timeout) {
        $scope.debug = {};
        $scope.debug.messages = [];
        $scope.debug.messageStrings = [];

        $scope.sys = {};
        $http.get('/Static/assets/ac/data/checkinReasons.js').success(function (data) {
            $scope.sys.reasonsFromDb = data;
        });

        $scope.sys.techs = [//should come from database. Technicians that manage queue
            { name: "Tech One", pin: 2255 },
            { name: "Tech Two", pin: 3717 },
            { name: "Tech Three", pin: 4328 }
        ];

        $scope.sys.typedPin = "";//technician's pin
        $scope.sys.customerName = "";
        $scope.sys.selection = { "ids": {} };//holds selected reasons for visit
        $scope.sys.registrations = [];//open registrations
        $scope.sys.completedRegistrations = [];//closed registrations
        $scope.sys.other = "";//a field that holds the 'other' reasons
        $scope.sys.showDetailsForRecordId = ""; //shows/hides 'Close' button by a record on hover
        $scope.sys.welcomeTechMessage = ""; //a message that a tech sees once she logs in
        $scope.sys.showCustomerQueue = false; //shows/hides customer queue
        $scope.sys.clientCount = 0; //defines clientId, sort of an identity field
        $scope.sys.errors = []; //errors display during registration
        $scope.sys.welcomeCustomerMessage = "";

        $scope.sys.register = function () {
           //addDebugMessage('00: $scope.sys.register called');
            $scope.sys.errors = [];
            var registration = {};
            registration.reasons = [];

            if ($scope.sys.other.length > 0) {//other reasons
                registration.reasons.push($scope.sys.other);
            }

            if (Object.keys($scope.sys.selection.ids).length > 0) {
                for (var i in $scope.sys.selection.ids) {
                    if ($scope.sys.selection.ids.hasOwnProperty(i)) {
                        if ($scope.sys.selection.ids[i] == true) {
                            registration.reasons.push(i);
                        }
                    }
                }
            }//and now we know why this customer came

            if ($scope.sys.customerName == undefined || $scope.sys.customerName.length == 0) {
                $scope.sys.errors.push({ message: "Name is required" });
            }
            if (registration.reasons.length == 0) {
                $scope.sys.errors.push({ message: "Please select at least one reason or type it in \"Other\" box if it is not on the list" });
            }
            if ($scope.sys.errors.length == 0) {
                registration.fullName = $scope.sys.customerName;
                registration.clientId = $scope.sys.clientCount++;
                registration.date = Date.now();
                registration.friendlyArrivalDate = getFriendlyDateFromMillisec(registration.date);
                $scope.sys.registrations.push(registration);
                $scope.sys.welcomeCustomerMessage = calcApproxWaitTime();
                //reset for the next customer
                $scope.sys.customerName = "";
                $scope.sys.other = "";
                $scope.sys.selection = { "ids": {} };
                $scope.sys.errors = [];
                $scope.sys.reasonFilter = "";
                $timeout((function () { $scope.sys.welcomeCustomerMessage = ""; }), 5000);

            }
        }

        $scope.sys.acceptPinEntry = function (key, id) {
           //addDebugMessage('01: $scope.sys.acceptPinEntry called');
            if (event.keyCode == 13) {
                if (id != undefined) {
                    if (id == "1") {
                        var t = getTechByPin($scope.sys.typedPin);
                        if (t != null) {
                            $scope.sys.welcomeTechMessage = "Hello, " + t.name;
                            $scope.sys.showCustomerQueue = true;
                            $timeout($scope.sys.hideCustomerQueue, 30000);
                        }
                    }
                }
            }
        }

        $scope.sys.closeRecord = function (recordId) {
           //addDebugMessage('02: $scope.sys.closeRecord called');
            var index = getSelectedRegistration(recordId);
            var tech = getTechByPin($scope.sys.typedPin);

            if (index > -1 && tech != null) { //have such reg and pin is correct
                var completed = $scope.sys.registrations[index];
                if (completed != undefined) {
                    completed.tech = tech;
                    completed.checkoutDate = Date.now();
                    completed.durationString = calcDurationString(completed.checkoutDate - completed.date);
                    $scope.sys.completedRegistrations.push(completed);
                    $scope.sys.registrations.splice(index, 1);
                }
            }
            $scope.sys.showDetailsForRecordId = "";
        }

        $scope.sys.logoutTech = function () {
           //addDebugMessage('03: $scope.sys.logoutTech called');
            $scope.sys.typedPin = "";
            $scope.sys.showCustomerQueue = false;
            $scope.sys.showDetailsForRecordId = "";
        }

        function getFriendlyDateFromMillisec(millisec) {
           //addDebugMessage('05: getFriendlyDateFromMillisec called');
            var d = new Date(millisec);

            return padWithZeroesToTwoChars(d.getHours(), 9) + ':' +
                padWithZeroesToTwoChars(d.getMinutes(), 9) + ' ' +
                d.getDate() + "." +
                padWithZeroesToTwoChars(d.getMonth() + 1, 9) + "." +
                d.getFullYear();
        }

        function padWithZeroesToTwoChars(incoming, inclusiveBorder) {
           //addDebugMessage('07: padWithZeroes... called');
            return (incoming <= inclusiveBorder ? "0" : "") + incoming;
        }

        function calcApproxWaitTime() {
           //addDebugMessage('08: calcApproxWaitTime called');
            var avg = 0;
            var count = 0;
            var today = new Date(); //today's date
            var todaysYr = today.getFullYear();
            var todaysMo = today.getMonth();
            var todaysDa = today.getDate();

            //getting all today's registrations:
            for (var i = 0; i < $scope.sys.completedRegistrations.length; i++) {
                var dt = new Date($scope.sys.completedRegistrations[i].date);
                if (dt.getFullYear() == todaysYr && dt.getMonth() == todaysMo && dt.getDate() == todaysDa) {
                    avg += $scope.sys.completedRegistrations[i].checkoutDate -
                        $scope.sys.completedRegistrations[i].date;
                    count++;
                }
            } //got all today's completed registrations

            //will start calculation with 5 or more registrations
            if (count < 5 || avg < 1000 * 60) //don't care if it is < 1 min
                return "Thank you. Someone will be with you shortly";

            return "Thank you. Your approximate wait time is " + getDiffInTime(avg / count);
        }

        function getTechByPin(pin) {
           //addDebugMessage('09: getTechByPin called');
            for (var i = 0; i < $scope.sys.techs.length; i++) {
                if ($scope.sys.techs[i].pin == pin)
                    return $scope.sys.techs[i];
            }

            return null;
        }

        //displays the button to close customer record when mouse is hovered over it
        $scope.sys.showVisitReasonsForCustomer = function (divId) {
           //addDebugMessage('10: $scope.sys.showVisitReasonsForCustomer called');
            var index = getSelectedRegistration(divId);
            if (index > -1) {
                $scope.sys.showDetailsForRecordId = divId;
            }
        }

        //hides the close button for customer record when mouse left the area
        $scope.sys.hideVisitReasons = function () {
           //addDebugMessage('11: $scope.sys.hideVisitReasons called');
            $scope.sys.showDetailsForRecordId = "";
        }

        $scope.sys.hideCustomerQueue = function () {
           //addDebugMessage('13: $scope.sys.hideCustomerQueue called');
            $scope.sys.typedPin = "";
            $scope.sys.showCustomerQueue = false;
            $scope.sys.welcomeCustomerMessage = "";
            $scope.sys.errors = [];
        }

        function calcDurationString(millisec) {
           //addDebugMessage('14: calcDurationString called');
            var fullHours = Math.floor(millisec / (1000 * 60 * 60));
            var fullMinutes = Math.floor(millisec / (1000 * 60)) - fullHours * 60;
            var fullSeconds = Math.floor(millisec / 1000) - fullMinutes * 60 * 60;
            if (fullSeconds > 30)
                fullMinutes++;

            if (fullHours == 0 && fullMinutes == 0)
                return fullSeconds + "sec";

            var ret = (fullHours > 0) ? fullHours + "h " : "";
            ret += (fullMinutes > 0) ? fullMinutes + "min " : "";
            return ret;
        }

        function getSelectedRegistration(regId) {
           //addDebugMessage('15: getSelectedRegistration called');
            for (var i = 0; i < $scope.sys.registrations.length; i++) {
                if ($scope.sys.registrations[i].date == regId) {
                    return i;
                }
            }
            return -1;
        }

        function addMinutes(tm2, m) {
           //addDebugMessage('16: addMinutes called');
            var secMod = 1000;
            var minMod = 1000 * 60;
            var houMod = 1000 * 60 * 60;
            return new Date(tm2 + m * minMod);
        }

        function addDays(tm2, d) {
           //addDebugMessage('17: addDays called');
            var secMod = 1000;
            var minMod = secMod * 60;
            var houMod = minMod * 60;
            var dayMod = houMod * 24;
            return new Date(tm2 + d * dayMod);
        }

        function getDiffInTime(diff) {
           //addDebugMessage('18: getDiffInTime called');
            var secMod = 1000;
            var minMod = secMod * 60;
            var houMod = minMod * 60;
            var dayMod = houMod * 24;
            var ret = "";

            var dayDiff = Math.floor(diff / dayMod);
            if (dayDiff > 0)
                return "more than 1 day";
            var houDiff = Math.floor(diff / houMod);
            if (houDiff > 0) {
                ret += houDiff + "hr ";
                diff -= houMod * houDiff;
            }
            var minDiff = Math.floor(diff / minMod);
            if (minDiff > 0) {
                ret += minDiff + "min ";
                diff -= minMod * minDiff;
            }

            var secDiff = Math.floor(diff / secMod);
            if (secDiff > 0)
                ret += secDiff + "sec";

            return ret;
        }

        function getRandomNumber(from, upTo) {
           //addDebugMessage('19: getRandomNumber called');
            return Math.floor((Math.random() * (upTo - from) + from) + 1);
        }

        //queue simulator
        $scope.sys.fillQueue = function () {
           //addDebugMessage('20: $scope.sys.fillQueue called');
            var now = new Date();
            var sourceDate = new Date();

            //closed registrations
            for (var i = 0; i < 35; i++) {
                var registration = {};
                registration.fullName = "Closed, Before " + $scope.sys.clientCount;
                registration.clientId = $scope.sys.clientCount++;
                var a = addMinutes(sourceDate.getTime(), -i * getRandomNumber(10, 20));
                registration.date = addDays(a.getTime(), ((-i % 5) - 2)).getTime();
                registration.friendlyArrivalDate = getFriendlyDateFromMillisec(registration.date);
                registration.checkoutDate = (addMinutes(registration.date, getRandomNumber(10, 20))).getTime();
                registration.d = getDiffInTime(registration.checkoutDate - registration.date);
                registration.reasons = getRandomReasonsReturnArray(3);//["reason " + $scope.sys.clientCount, "closed " + ($scope.sys.clientCount + 1)];
                $scope.sys.completedRegistrations.push(registration);
            }

            for (var i = 0; i < 10; i++) {
                var registration = {};
                registration.fullName = "Smith, Today " + $scope.sys.clientCount;
                registration.clientId = $scope.sys.clientCount++;
                registration.date = addMinutes(sourceDate.getTime(), -i * getRandomNumber(10, 20)).getTime();
                registration.friendlyArrivalDate = getFriendlyDateFromMillisec(registration.date);
                registration.checkoutDate = (addMinutes(registration.date, getRandomNumber(10, 20))).getTime();
                registration.reasons = getRandomReasonsReturnArray(3);//["reason " + $scope.sys.clientCount, "closed " + ($scope.sys.clientCount + 1)];
                registration.d = getDiffInTime(registration.checkoutDate - registration.date);
                $scope.sys.completedRegistrations.push(registration);
            }

            for (var i = 0; i < 10; i++) {
                var registration = {};
                registration.fullName = "Smith, Open Today " + $scope.sys.clientCount;
                registration.clientId = $scope.sys.clientCount++;
                registration.date = addMinutes(sourceDate.getTime(), -i * getRandomNumber(15, 30)).getTime();
                registration.friendlyArrivalDate = getFriendlyDateFromMillisec(registration.date);
                registration.reasons = getRandomReasonsReturnArray(getRandomNumber(1, 4));
                $scope.sys.registrations.push(registration);
            }
        }

        function getRandomReasonsReturnArray(howMany) {
            //addDebugMessage('21: getRandomReasonsReturnArray called');
            if (howMany > $scope.sys.reasonsFromDb.length) { //if need more reasons than there are available,
                return $scope.sys.reasonsFromDb; //just get them all
            }
            var ret = [];
            var usedReasonsSoFar = [];
            for (var i = 0; i < howMany; i++) {
                var j;
                do {
                    j = getRandomNumber(0, $scope.sys.reasonsFromDb.length - 1);
                } while (usedReasonsSoFar.indexOf(j) >= 0);
                usedReasonsSoFar.push(j);
                ret.push($scope.sys.reasonsFromDb[j].name);
            }
            return ret;
        }

        //function addDebugMessage(message) {
        //    if ($scope.debug.messageStrings.indexOf(message) < 0) {
        //        $scope.debug.messages.push({ msg: message });
        //        $scope.debug.messageStrings.push(message);
        //    }
        //}
    }]);