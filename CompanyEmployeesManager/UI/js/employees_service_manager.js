
$(function () {
    const serviceUrl = "http://localhost:53104/api/employees/",
        itemsPerPage = 3;
    var selectors = {
        nextButtonSelector: "#next",
        prevButtonSelector: "#prev",
        paginatorSelector: ".paginator",
        editEmployeeWindowSelector: ".edit-employee-window",
        createEmployeeWindowSelector: ".create-employee-window"
    },
    viewModel = new EmployeesViewModel(serviceUrl, itemsPerPage, selectors);
    ko.applyBindings(viewModel);
    viewModel.init();
});

function EmployeesViewModel(url, itemsPerPage, selectors) {
    var self = this;
    self.serviceUrl = url;
    self.nextButtonSelector = selectors.nextButtonSelector;
    self.previousButtonSelector = selectors.prevButtonSelector;
    self.paginatorSelector = selectors.paginatorSelector;
    self.$createModalWindow = $(selectors.createEmployeeWindowSelector);
    self.$editModalWindow = $(selectors.editEmployeeWindowSelector);
    self.itemsPerPage = itemsPerPage;
    self.currentPage = ko.observable(1);
    self.totalPages = ko.observable(1);
    self.employeesPageSearchString = ko.observable("");
    self.employeesArray = ko.observableArray();
    self.createModel = {Name: ko.observable(""), Age: ko.observable(""), Position: ko.observable(""), StartDate: ko.observable("")};
    self.editModel = { Id: ko.observable(""), Age: ko.observable(""), Name: ko.observable(""), Position: ko.observable(""), StartDate: ko.observable("") };
    self.searchString = ko.observable("");
    self.employeesItemsStyle = ko.pureComputed(function () {
        var lenght = self.employeesArray().length;
        if (lenght === 0) {
            return "not-found";
        }
        $("#employees").show();
        return  "employees-items";
    });
    self.init = function () {
        self.getEmployees(false, self.currentPage());
        var closeButtonSelector = ".close",
            cancelButtonSelector = ".cancel-button";
        $("body").on("click", cancelButtonSelector, hideModalDialogs);
        $("body").on("click", closeButtonSelector, hideModalDialogs);

        //hide dialogs and clear models
        function hideModalDialogs() {
            self.$createModalWindow.hide();
            self.$editModalWindow.hide();
            $.each([self.$createModalWindow.find("input"), self.$editModalWindow.find("input")],
                function(index, item) {
                    $.each(item,
                        function(index, input) {
                            $(input).removeClass("error");
                        });
                });
            self.createModel.Age("");
            self.createModel.Name("");
            self.createModel.Position("");
            self.createModel.StartDate("");
        }

        $("#employees-menu-button").on("click", function () {
            self.employeesPageSearchString("");
            self.getEmployees(false, 1);
        });
    }
    self.onNextPageButtonClick = function () {
        self.getEmployees(null, self.currentPage()+1);
    }
    self.onPreviousPageButtonClick = function () {
        self.getEmployees(null, self.currentPage() - 1);
    }
    self.onSearchButtonClick = function () {
        self.getEmployees(true, 1);
    }
    self.onCreateEmployeeButtonClick = function () {
        //set current date as default value
        self.createModel.StartDate(moment().format("YYYY-MM-DD"));
        self.$createModalWindow.show();
    }
    self.onRemoveEmployeeButtonClick = function (model) {
        if (confirm("Are you sure? You want to delete " + model.Name + " ?")) {
            self.removeEmployee(model);
        }
    }
    self.onEditEmployeeButtonClick = function (model) {
        //set editModel values
        self.editModel.Id(model.Id);
        self.editModel.Age(model.Age);
        self.editModel.Position(model.Position);
        self.editModel.Name(model.Name);
        self.editModel.StartDate(model.StartDate);
        self.$editModalWindow.show();
    }
    self.refresh = function () {
        //if employees array is search result(refresh array according to search string)
        if (self.employeesPageSearchString()=="") {
            self.getEmployees(false, self.currentPage());
        }
        else {
            self.getEmployees(true, self.currentPage());
        }
    }
    self.addEmployee = function () {
        $.ajax({
            url: self.serviceUrl + "create",
            data: ko.toJS(self.createModel),
            async: true,
            cashe: false,
            type: 'POST',
            contentType: 'application/x-www-form-urlencoded',
            success: function (data) {
                if (data.Success) {
                    //clear create model, close modal dialog
                    self.refresh();
                    self.$createModalWindow.hide();
                    self.createModel.Age("");
                    self.createModel.Name("");
                    self.createModel.Position("");
                    self.createModel.StartDate("");
                } else {
                    //adding red border to invalid fields
                    $.each(self.$createModalWindow.find("input[name]"),
                        function (index, item) {
                            var $input = $(item);
                            if (data.Data.includes($input.attr("name"))) {
                                $input.addClass("error");
                                $input.on("click", function() {
                                     $(this).removeClass("error");
                                });
                            }
                        });
                }
            },
            error: function () {
                alert("Something went wrong!");
            }
        });
    }
    self.removeEmployee = function (model) {
        $.ajax({
            cache: false,
            type: 'POST',
            async: true,
            data: model,
            contentType: 'application/x-www-form-urlencoded',
            url: self.serviceUrl + "delete",
            success: function (data) {
                if (data.Success) {
                    //checking when we can go to previous page(set prev page as current)
                    if (self.employeesArray().length == 1 & self.totalPages() > 1) {
                        self.currentPage(self.currentPage() - 1);
                    }
                    self.refresh();
                } else {
                    alert(data.Message);
                }
            },
            error: function () {
                alert("Something went wrong!");
            }
        });
    }
    self.editEmployee = function () {
        $.ajax({
            url: self.serviceUrl + "edit",
            type: 'POST',
            contentType: 'application/x-www-form-urlencoded',
            cache: false,
            data: ko.toJS(self.editModel),
            async: true,
            success: function (data) {
                if (data.Success) {
                    self.$editModalWindow.hide();
                    self.employeesPageSearchString("");
                    self.refresh();
                }
                //adding red border to invalid fields
                $.each(self.$editModalWindow.find("input[name]"),
                    function (index, item) {
                        var $input = $(item);
                        if (data.Data.includes($input.attr("name"))) {
                            $input.addClass("error");
                            $input.on("click", function () {
                                $(this).removeClass("error");
                            });
                        }
                    });
            },
            error: function () {
                alert("Something went wrong!");
            }
        });
    }
    self.getEmployees = function (isSearchRequest, page) {
        $(".loader").show();
        if (self.employeesArray.lenght == 0) {
            $("#not-found-info").hide();
        } else {
            self.employeesArray.removeAll();
        }
        var url = self.serviceUrl;
        if (isSearchRequest) {
            if (self.searchString()=="") {
                return;
            }
            url += "search/" + self.searchString() + "/" + self.itemsPerPage + "/" + page;
        } else {
            //checking when employees array is search result(go to next or prev page according to search string)
            url += self.employeesPageSearchString() ? "search/" + self.employeesPageSearchString() +
            + "/" + self.itemsPerPage + "/" + page
            : "getPage/" + self.itemsPerPage + "/" + page;
        }
        $.ajax({
            url: url,
            type: 'GET',
            cache: false,
            async: true,
            success: function (data) {
                if (data.Success) {
                    //take only date from DateTime property
                    $.each(data.Data.Employees, function (index, item) {
                        item.StartDate = moment(item.StartDate).format("YYYY-MM-DD");
                    });
                    //hide paginator when totalPages:1
                    if (data.Data.TotalPages == 1) {
                        $(self.paginatorSelector).hide();
                    }
                    else {
                        $(self.paginatorSelector).show();
                        if (data.Data.CurrentPage == data.Data.TotalPages) {
                            $(self.nextButtonSelector).prop("disabled", true);
                        } else {
                            $(self.nextButtonSelector).prop("disabled", false);
                        }
                        if (data.Data.CurrentPage == 1) {
                            $(self.previousButtonSelector).prop("disabled", true);
                        } else {
                            $(self.previousButtonSelector).prop("disabled", false);
                        }
                        self.totalPages(data.Data.TotalPages);
                        self.currentPage(data.Data.CurrentPage);
                    }
                    self.employeesPageSearchString(data.Data.SearchString);
                    $(".loader").hide();
                    //when array is empty show 'Not found!'
                    if (data.Data.Employees.length == 0) {
                        $("#not-found-info").show();
                    }
                    self.employeesArray(data.Data.Employees);
                }
                else {
                    alert(data.Message);
                }
            },
            error: function () {
                alert("Something went wrong!");
            }
        });
    }
}
