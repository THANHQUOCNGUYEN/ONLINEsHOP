(function (app) {
    app.controller('createProductCategory', createProductCategory);
    createProductCategory.$inject = ['$scope'];

    function createProductCategory($scope) {
        $scope.productCategory = {};
        $scope.flatFolders = [];

        function getParent() {
            $.ajax({
                url: '/Admin/ProductCategory/getParent',
                dataType: 'json',
                type: 'post',
                success: function (result) {
                    if (result.status) {
                        console.log(result);
                        $scope.parentCategories = getTree(result.data,"ID","ParentID");

                        $scope.parentCategories.forEach(function (item) {
                            $scope.$apply(function () {
                                recur(item, 0, $scope.flatFolders);
                            });
                        });
                    }
                }
            });
        }
        //getTree
        function getTree(data, primaryIdName, parentIdName) {
            if (!data || data.length == 0 || !primaryIdName || !parentIdName)
                return [];

            var tree = [],
                rootIds = [],
                item = data[0],
                primaryKey = item[primaryIdName],
                treeObjs = {},
                parentId,
                parent,
                len = data.length,
                i = 0;

            while (i < len) {
                item = data[i++];
                primaryKey = item[primaryIdName];
                treeObjs[primaryKey] = item;
                parentId = item[parentIdName];

                if (parentId) {
                    parent = treeObjs[parentId];

                    if (parent.children) {
                        parent.children.push(item);
                    } else {
                        parent.children = [item];
                    }
                } else {
                    rootIds.push(primaryKey);
                }
            }

            for (var i = 0; i < rootIds.length; i++) {
                tree.push(treeObjs[rootIds[i]]);
            };

            return tree;
        }

        function times(n, str) {
            var result = '';
            for (var i = 0; i < n; i++) {
                result += str;
            }
            return result;
        };

        function recur(item, level, arr) {
            arr.push({
                Name: times(level, '–') + ' ' + item.Name,
                ID: item.ID,
                Level: level,
                Indent: times(level, '–')
            });
            if (item.children) {
                item.children.forEach(function (item) {
                    recur(item, level + 1, arr);
                });
            }
        };

        getParent();
    }
 
})(angular.module('shop.product'));