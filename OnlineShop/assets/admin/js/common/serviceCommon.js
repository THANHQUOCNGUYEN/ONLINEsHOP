(function (app) {
    app.controller('ProductController', ProductController);
    ProductController.$inject = ['$scope'];
    function ProductController($scope) {
        $scope.MoreImages = [];
        $scope.getParent = [];

        $scope.Product = {};
        $scope.ChooseMoreImage = function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                if ($scope.MoreImages.indexOf(fileUrl) == -1) {
                    $scope.$apply(function () {
                        $scope.MoreImages.push(fileUrl);
                    })
                } else {
                    alert('Hình ảnh đã tồn tại !');
                }
            }

            finder.popup();
        }

        function getParent() {
            $.ajax({
                url: '/Admin/Product/getProductCategories',
                dataType: 'json',
                type: 'post',
                success: function (res) {
                    if (res.status) {
                        $scope.getParent = res.data;
                    }
                }
            });
        }
        getParent();
    }
})(angular.module('shop.product'));