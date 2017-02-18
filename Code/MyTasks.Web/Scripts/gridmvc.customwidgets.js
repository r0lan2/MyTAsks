/***
* This script demonstrates how you can build you own custom filter widgets:
* 1. Specify widget type for column:
*       columns.Add(o => o.Customers.CompanyName)
*           .SetFilterWidgetType("CustomCompanyNameFilterWidget")
* 2. Register script with custom widget on the page:
*       <script src="@Url.Content("~/Scripts/gridmvc.customwidgets.js")" type="text/javascript"> </script>
* 3. Register your own widget in Grid.Mvc:
*       GridMvc.addFilterWidget(new CustomersFilterWidget());
*
* For more documentation see: http://gridmvc.codeplex.com/documentation
*/



function StatusListFilterWidget() {
    /***
    * This method must return type of registered widget type in 'SetFilterWidgetType' method
    */
    this.getAssociatedTypes = function () {
        return ["CustomStatusFilterWidget"];
    };
    /***
    * This method invokes when filter widget was shown on the page
    */
    this.onShow = function () {
        /* Place your on show logic here */
    };

    this.showClearFilterButton = function () {
        return true;
    };
    /***
    * This method will invoke when user was clicked on filter button.
    * container - html element, which must contain widget layout;
    * lang - current language settings;
    * typeName - current column type (if widget assign to multipile types, see: getAssociatedTypes);
    * values - current filter values. Array of objects [{filterValue: '', filterType:'1'}];
    * cb - callback function that must invoked when user want to filter this column. Widget must pass filter type and filter value.
    * data - widget data passed from the server
    */
    this.onRender = function (container, lang, typeName, values, cb, data) {
        //store parameters:
       
        this.cb = cb;
        this.container = container;
        this.lang = lang;

        //this filterwidget demo supports only 1 filter value for column column
        this.value = values.length > 0 ? values[0] : { filterType: 1, filterValue: "" };

        this.renderWidget(); //onRender filter widget
        this.loadStatus(); //load status's list from the server
        this.registerEvents(); //handle events
    };
    this.renderWidget = function () {
        var html = '<p>' + Resources.SelectStatusToFilter + ' </p>\
                    <select style="width:250px;" class="grid-filter-type statusList form-control">\
                    </select>';
        this.container.append(html);
    };
    

    this.loadStatus = function () {
        var $this = this;
        dataService.getStatusList(function (items) {
            var statusList = $(".statusList");
            for (var i = 0; i < items.length; i++) {
                statusList.append('<option ' + (items[i] == $this.value.filterValue ? 'selected="selected"' : '') + ' value="' + items[i] + '">' + items[i] + '</option>');
            }
            }
        );
    };
    

    /***
    * Internal method that register event handlers for 'apply' button.
    */
    this.registerEvents = function () {
       
        var statusList = this.container.find(".statusList");
        //save current context:
        var $context = this;
        //register onclick event handler
        statusList.change(function () {
            //invoke callback with selected filter values:
            var values = [{ filterValue: $(this).val(), filterType: 1 /* Equals */ }];
            $context.cb(values);
        });
    };

}

function PriorityListFilterWidget() {
    /***
    * This method must return type of registered widget type in 'SetFilterWidgetType' method
    */
    this.getAssociatedTypes = function () {
        return ["CustomPriorityFilterWidget"];
    };
    /***
    * This method invokes when filter widget was shown on the page
    */
    this.onShow = function () {
        /* Place your on show logic here */
    };

    this.showClearFilterButton = function () {
        return true;
    };
    /***
    * This method will invoke when user was clicked on filter button.
    * container - html element, which must contain widget layout;
    * lang - current language settings;
    * typeName - current column type (if widget assign to multipile types, see: getAssociatedTypes);
    * values - current filter values. Array of objects [{filterValue: '', filterType:'1'}];
    * cb - callback function that must invoked when user want to filter this column. Widget must pass filter type and filter value.
    * data - widget data passed from the server
    */
    this.onRender = function (container, lang, typeName, values, cb, data) {
        //store parameters:
        this.cb = cb;
        this.container = container;
        this.lang = lang;

        //this filterwidget demo supports only 1 filter value for column column
        this.value = values.length > 0 ? values[0] : { filterType: 1, filterValue: "" };

        this.renderWidget(); //onRender filter widget
        this.loadPriorities(); //load status's list from the server
        this.registerEvents(); //handle events
    };
    this.renderWidget = function () {
        var html = '<p>' + Resources.SelectPriorityToFilter + ' </p>\
                    <select style="width:250px;" class="grid-filter-type priorityList form-control">\
                    </select>';
        this.container.append(html);
    };


    this.loadPriorities = function () {
        var $this = this;
        dataService.getPriorityList(function (items) {
            var priorityList = $(".priorityList");
            for (var i = 0; i < items.length; i++) {
                priorityList.append('<option ' + (items[i] == $this.value.filterValue ? 'selected="selected"' : '') + ' value="' + items[i] + '">' + items[i] + '</option>');
            }
        }
        );
    };


    /***
    * Internal method that register event handlers for 'apply' button.
    */
    this.registerEvents = function () {

        var priorityList = this.container.find(".priorityList");
        //save current context:
        var $context = this;
        //register onclick event handler
        priorityList.change(function () {
            //invoke callback with selected filter values:
            var values = [{ filterValue: $(this).val(), filterType: 1 /* Equals */ }];
            $context.cb(values);
        });
    };

}

function CategoryListFilterWidget() {
    /***
    * This method must return type of registered widget type in 'SetFilterWidgetType' method
    */
    this.getAssociatedTypes = function () {
        return ["CustomCategoryFilterWidget"];
    };
    /***
    * This method invokes when filter widget was shown on the page
    */
    this.onShow = function () {
        /* Place your on show logic here */
    };

    this.showClearFilterButton = function () {
        return true;
    };
    /***
    * This method will invoke when user was clicked on filter button.
    * container - html element, which must contain widget layout;
    * lang - current language settings;
    * typeName - current column type (if widget assign to multipile types, see: getAssociatedTypes);
    * values - current filter values. Array of objects [{filterValue: '', filterType:'1'}];
    * cb - callback function that must invoked when user want to filter this column. Widget must pass filter type and filter value.
    * data - widget data passed from the server
    */
    this.onRender = function (container, lang, typeName, values, cb, data) {
        //store parameters:
        this.cb = cb;
        this.container = container;
        this.lang = lang;

        //this filterwidget demo supports only 1 filter value for column column
        this.value = values.length > 0 ? values[0] : { filterType: 1, filterValue: "" };

        this.renderWidget(); //onRender filter widget
        this.loadCategories(); //load status's list from the server
        this.registerEvents(); //handle events
    };
    this.renderWidget = function () {
        var html = '<p>' + Resources.SelectCategoryToFilter + ' </p>\
                    <select style="width:250px;" class="grid-filter-type categoryList form-control">\
                    </select>';
        this.container.append(html);
    };


    this.loadCategories = function () {
        var $this = this;
        dataService.getCategoryList(function (items) {
            var categoryList = $(".categoryList");
            for (var i = 0; i < items.length; i++) {
                categoryList.append('<option ' + (items[i] == $this.value.filterValue ? 'selected="selected"' : '') + ' value="' + items[i] + '">' + items[i] + '</option>');
            }
        }
        );
    };


    /***
    * Internal method that register event handlers for 'apply' button.
    */
    this.registerEvents = function () {

        var categoryList = this.container.find(".categoryList");
        //save current context:
        var $context = this;
        //register onclick event handler
        categoryList.change(function () {
            //invoke callback with selected filter values:
            var values = [{ filterValue: $(this).val(), filterType: 1 /* Equals */ }];
            $context.cb(values);
        });
    };

}

function ProjectListFilterWidget() {
    /***
    * This method must return type of registered widget type in 'SetFilterWidgetType' method
    */
    this.getAssociatedTypes = function () {
        return ["CustomProjectFilterWidget"];
    };
    /***
    * This method invokes when filter widget was shown on the page
    */
    this.onShow = function () {
        /* Place your on show logic here */
    };

    this.showClearFilterButton = function () {
        return true;
    };
   
    this.onRender = function (container, lang, typeName, values, cb, data) {
        this.cb = cb;
        this.container = container;
        this.lang = lang;
        
        this.value = values.length > 0 ? values[0] : { filterType: 1, filterValue: "" };

        this.renderWidget(); //onRender filter widget
        this.loadProjects(); //load status's list from the server
        this.registerEvents(); //handle events
    };
    this.renderWidget = function () {
        var html = '<p>' + Resources.SelectProjectToFilter + ' </p>\
                    <select style="width:250px;" class="grid-filter-type projectList form-control">\
                    </select>';
        this.container.append(html);
    };


    this.loadProjects = function () {
        var $this = this;
        dataService.getProjects(function (items) {
            var projectList = $(".projectList");
            for (var i = 0; i < items.length; i++) {
                projectList.append('<option ' + (items[i] == $this.value.filterValue ? 'selected="selected"' : '') + ' value="' + items[i] + '">' + items[i] + '</option>');
            }
        }
        );
    };


    /***
    * Internal method that register event handlers for 'apply' button.
    */
    this.registerEvents = function () {

        var projectList = this.container.find(".projectList");
        //save current context:
        var $context = this;
        //register onclick event handler
        projectList.change(function () {
            //invoke callback with selected filter values:
            var values = [{ filterValue: $(this).val(), filterType: 1 /* Equals */ }];
            $context.cb(values);
        });
    };

}