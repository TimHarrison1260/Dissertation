(function () {
    //  create the namespace, if it's not already there and make it Global in scope.
    var Demo = window.Demo = window.Demo || {};

    Demo.Api = (function () {

        function click() {
            //  Build the ajax call and call the action method to get
            //  the partial page and show the results in the partPage div.
            //  Get the method to call from the title attriute of the anchor
            var thisLink = $(this);     //  only call jQuery once

            //  Get Url from <a>
            var url = getApiUrl(thisLink);

            //  Get the Http Verb
            var httpVerb = getHttpVerb(thisLink);

            //  Split URL into constituent parts
            var elements = getApiElements(url);
            var resource = (elements.resource != undefined) ? elements.resource : undefined;
            var id = (elements.id != null) ? elements.id : undefined;

            //  Get search criteria if there is any
            if (resource == 'windfarm' || resource == 'search') {
                var criteria = getCriteria(thisLink);
                if (criteria != undefined && criteria != "") {
                    //  Append data to the url
                    url += '/' + criteria;
                    id = criteria;
                }
            }

            //  Resolve callback function
            var resultHandler = resolveResultHandler(resource, id);

            //  GetData Formdata if any required, only for PUT, POST
            if (httpVerb == 'put' || httpVerb == 'post') {
                var inputData;
                var additionalHeaders;

                if (isInputRequired(resource, thisLink)) {
                    inputData = getInput(resource, thisLink);
                    if (inputData == undefined) {
                        alert("Please upload a KML file before calling this Import api.");
                        return;
                    }
                    additionalHeaders = { "Content-Disposition": "attachment; filename=" + inputData };
                }
            }

            //  Call the API with the URL
            switch (httpVerb) {
                case 'get':
                    ajaxGet(url, httpVerb, resultHandler);
                    break;
                case 'put':
                    ajaxPut(url, httpVerb, resultHandler, additionalHeaders, inputData);
                    break;
                default:
                    ajaxGet(url, httpVerb, resultHandler);
            }
        }


        function createUrlAndSubmit() {
            //  Get the values from the page, construct the URL and call click();
            //  1   construct the URL
            var url = constructUrl();
            var http = constructHttpVerb();

            //  2   crate the anchor element
            var link = constructAnchorTag(url, http);

            //  3   Add it to the DOM
            $('#constructedurl').html(link);

            //  4   Attach Handler
            $('#constructedurl a').click(Demo.Api.click);

            //  5   initiate the click event, using .trigger()
            $('#constructedurl a').trigger("click");

        }

        //  Expose interface
        return {
            click: click,
            createUrl: createUrlAndSubmit
        };

        /*
        *   Private methods
        */

        /************************************
         * Ajax calls and the General Callback function
         *
         ************************************/

        function ajaxGet(url, httpVerb, resultHandler) {
            $.ajax({
                url: url,
                type: httpVerb,
                dataType: 'json',
                success: function (results, status, xhr) {
                    apiCallback(results, status, xhr, resultHandler);
                }
            });
        }

        function ajaxPut(url, httpVerb, resultHandler, additionalHeaders, inputData) {
            $.ajax({
                url: url,
                type: httpVerb,
                headers: additionalHeaders,
                dataType: 'json',
                data: inputData,
                cache: false, //  No caching
                contentType: false, //  Allows the uploaded data to be correctly processed
                processData: false, //  No query strings to be used
                success: function (results, status, xhr) {
                    apiCallback(results, status, xhr, resultHandler);
                }
            });
        }


        function apiCallback(results, status, xhr, resultHandler) {
            if (status == 'success') {
                //  output the json returned
                addJsonResultToUi(results);
                //  process the resource and format the links
                //var html =resultHandler(results);
                addLinksToUi(resultHandler(results));
            } else {
                addReturnStatusToUi(status, xhr);
            }
            //  Make the results div visible.
            $('#returnvalues').show();
        }




        /************************************
         * functions used to construct the Api ajax call
         *
         ************************************/

        function getApiUrl(element) {
            var url = element.text();
            if (url == undefined) {
                var method = element.attr('title');
                url = '/api/' + method;
            }
            return url;
        }

        function getHttpVerb(element) {
            var httpVerb = element.attr('data-method');
            if (httpVerb == undefined || httpVerb == "") {
                httpVerb = 'get';
            }
            return httpVerb;
        }

        function getApiElements(url) {
            var resource = undefined;
            var id = undefined;
            if (url != undefined) {
                //  trim leading and trailing '/' then split into array
                var parts = url.replace(/^\/+|\/+$/gm, '').split('/');
                //var parts = url.split('/');
                switch (parts.length) {
                    case 2:
                        resource = parts[1];
                        break;
                    case 3:
                        resource = parts[1];
                        id = parts[2];
                        break;
                    case 4:
                        id = parts[2];
                        resource = parts[3];
                        break;
                    default:
                        resource = "";
                        id = "";
                }
            }
            var elements = { resource: resource.toLowerCase(), id: id };
            return elements;
        }

        function resolveResultHandler(resource, id) {
            if (resource == undefined) return undefined;

            var resultHandler = undefined;
            switch (resource) {
                case 'datasource':
                    if (id == undefined)        //  api/datasource
                        resultHandler = processDatasourceCollection;
                    else {                      //  api/datasource/5
                        resultHandler = processSingleDatasource;
                    }
                    break;
                case 'import':
                    resultHandler = processImport;
                    break;;
                case 'datatype':
                    resultHandler = processDataTypeCollection;
                    break;
                case 'windfarm':
                    if (id == undefined)        //  api/windfarm
                        resultHandler = processWindfarmCollection;
                    else {
                        if (isNaN(id)) {       //  api/windfarm/criteria
                            resultHandler = processWindfarmCollection;
                        } else {                //  api/windfarm/5
                            resultHandler = processSingleWindfarm;
                        }
                    }
                    break;
                case 'search':
                    resultHandler = processWindfarmCollection;
                    break;
                case 'status':
                    if (id == undefined)        //  api/windfarm/criteria
                        resultHandler = processWindfarmCollection;
                    else {                    //  api/windfarm/5
                        resultHandler = processSingleWindfarm;
                    }
                    break;
                case 'statistics':
                    if (id == undefined)        //  api/windfarm/criteria
                        resultHandler = processWindfarmCollection;
                    else {                    //  api/windfarm/5
                        resultHandler = processSingleWindfarm;
                    }
                    break;
                case 'footprint':
                    if (id == undefined)        //  api/windfarm/criteria
                        resultHandler = processWindfarmCollection;
                    else {                    //  api/windfarm/5
                        resultHandler = processSingleWindfarm;
                    }
                    break;
                case 'turbine':
                    if (id == undefined)        //  api/windfarm/criteria
                        resultHandler = processWindfarmCollection;
                    else {                    //  api/windfarm/5
                        resultHandler = processSingleWindfarm;
                    }
                    break;
                default:
                    resultHandler = processDefault;
            }
            return resultHandler;
        }

        function isInputRequired(resource, element) {
            //  GetData if any required
            if (resource === 'import') {
                if (element.attr("data-input") != 'none') {
                    return true;
                }
            }
            return false;
        }

        function getInput(resource, element) {
            //  GetData if any required
            if (resource === 'import') {
                if (element.attr("data-input") != 'none') {
                    //  Get the uploaded file.  The Id is in the data-input attribute
                    //  Snh: call api after ensuring source file uploaded
                    var formData = new FormData();      //  IE10+
                    var inputElement = '#' + element.attr('data-input');
                    var inputfile = $(inputElement)[0];
                    if (inputfile == undefined || inputfile.files.length == 0) {
                        return undefined;
                    }
                    formData.append("KmlFile", inputfile.files[0]);

                    //  return the formdata
                    return formData;
                }
            }
            return undefined;
        }

        function getCriteria(link) {
            var criteriaElementId = '#' + link.attr('data-input');
            var criteria = $(criteriaElementId).val();
            return criteria;
        }


        /************************************
         * functions used to build and submit
         * freehand url
         *
         ************************************/

        function constructUrl() {
            var resource = $('#inputresource').val();
            var id = $('#inputid').val();
            var url = '/api/';
            if (resource != undefined && resource != "")
                url += resource + '/' + id;
            return url;
        }

        function constructHttpVerb() {
            var http = $('#selectVerb').val();
            return http;
        }

        function constructAnchorTag(url, verb) {
            //  Fix for the import as data is needed for SNH and avoid breaking script
            if (url == '/api/import/1')
                var dataInput = 'kmlfile';
            if (url == '/api/import/2')
                dataInput = 'none';

            var anchor = '<a href="#" title="" data-method="' + verb;
            //  Include the data-input if an import
            if (dataInput != undefined)
                anchor += '" data-input="' + dataInput + '" ';
            else
                anchor += '"';

            anchor += '>' + url + '</a>';
            return anchor;
        }




        /************************************
         * functions used to update the UI
         *
         ************************************/

        function addJsonResultToUi(json) {
            //  Process a collection of results
            //  process the JSON to display the return in the results column
            $('#results').html('<pre>' + JSON.stringify(json, null, '  ') + '</pre>');
        }

        function addLinksToUi(html) {
            //  Add to the links results div
            $('#links').html(html);

            //  Add the handler for the links
            $('#links ul li a').click(Demo.Api.click);
        }

        function addReturnStatusToUi(status, xhr) {
            $('#results').html('<h3>' + status + '</h3><p>Request Status: ' + xhr.status + ': ' + xhr.statusText + '</p><p>Message: ' + xhr.responseText + '</p>');
        }



        /************************************
         * The api Response handlers   
         *
         ************************************/

        function processDatasourceCollection(datasources) {
            var html = '<ul>';
            for (var d in datasources) {
                html = html + '<li>';
                var links = datasources[d].Links;
                for (var l in links) {
                    html = html + toLinkTag(links[l]);
                }
                var name = datasources[d].Title;
                html = html + '<span class="linkname">' + name + '</span>';
                html = html + '</li>';
            }
            html = html + '</ul>';
            return html;
        }

        function processSingleDatasource(datasource) {
            var name = datasource.Title;
            var html = '<p>' + name + '</p>';
            html = html + '<ul>';
            var dlinks = datasource.Links;
            for (var l in dlinks) {
                html += '<li>';
                html += toLinkTag(dlinks[l]);
                html += '<span class="linkname">' + dlinks[l].Rel + '</span>';
                html += '</li>';
            }
            html += '</ul>';
            return html;
        }

        function processImport(importresult) {
            var name = importresult.Result;
            var html = '<p>' + name + '</p>';
            html = html + '<ul>';
            var dlinks = importresult.Links;
            for (var l in dlinks) {
                html += '<li>';
                html += toLinkTag(dlinks[l]);
                html += '<span class="linkname">' + dlinks[l].Rel + '</span>';
                html += '</li>';
            }
            html += '</ul>';
            return html;
            //$('#results').html("Ajax returned with: " + result);
            //$('#returnvalues').show();
        }

        function processDataTypeCollection(datatypes) {
            var html = '<ul>';
            for (var d in datatypes) {
                html = html + '<li>';
                var links = datatypes[d].Links;
                for (var l in links) {
                    html = html + toLinkTag(links[l]);
                }
                var name = datatypes[d].Type;         // the only difference
                html = html + '<span class="linkname">' + name + '</span>';
                html = html + '</li>';
            }
            html = html + '</ul>';
            return html;
        }

        function processWindfarmCollection(windfarms) {
            var html = '<ul>';
            for (var w in windfarms) {
                html = html + '<li>';
                var links = windfarms[w].Links;
                for (var l in links) {
                    html = html + toLinkTag(links[l]);
                }
                var name = windfarms[w].Name;
                html = html + '<span class="linkname">' + name + '</span>';
                html = html + '</li>';
            }
            html = html + '</ul>';
            return html;
        }

        function processSingleWindfarm(windfarm) {
            var name = windfarm.Name;
            var html = '<p>' + name + '</p>';
            html += '<ul>';
            var dlinks = windfarm.Links;
            for (var l in dlinks) {
                html += '<li>';
                html += toLinkTag(dlinks[l]);
                html += '<span class="linkname">' + dlinks[l].Rel + '</span>';
                html += '</li>';
            }
            html += '</ul>';
            return html;
        }

        function processDefault(result, status, xhr) {
            $('#loadingdata').hide();       //  hide if not already hidden
            $('#results').html('<h3>Return Code</h3><p>Request Status: ' + xhr.status + ': ' + xhr.statusText + '</p><p>Message: ' + xhr.responseText + '</p>');
            $('#returnvalues').show();      //  Show the return div so message is visible on the page
        }

        //  Generate an anchor tag from the link
        function toLinkTag(link) {
            var href = link.Href;
            var rel = link.Rel;
            var title = link.Title;
            var type = link.Type;

            var tag = '<a rel="' + rel + '" href="#" title="' + title + '">' + href + '</a>';

            return tag;
        }

    })();

})();
