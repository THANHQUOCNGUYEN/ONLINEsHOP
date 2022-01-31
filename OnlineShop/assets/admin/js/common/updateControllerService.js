(function (app) {
    app.controller('updateController', updateController);
    updateController.$inject = ['$scope'];
    function updateController($scope) {
        $scope.Product = {};
        $scope.MoreImages = [];

        $scope.getParent = [];

        $scope.ChooseMoreImage = function () {
            var finder = new CKFinder();

            finder.selectActionFunction = function (fileUrl) {
                //xem coi trong mảng có chứa phần tử đó không ? -1:no
                if ($scope.MoreImages.indexOf(fileUrl) == -1) {
                    $scope.$apply(function () {
                        $scope.MoreImages.push(fileUrl);
                    });
                }
                else {
                    alert('hình này đã tồn tại !')
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
        //xây dựng 1 hàm ajax để lấy danh sách
        function getMoreImage() {
            var ID = $('#ID').val();
            $.ajax({
                url: '/Admin/Product/getList',
                dataType: 'json',
                type: 'post',
                data: {
                    id: ID
                },
                success: function (res) {
                    if (res.status) {
                        $scope.$apply(function () {
                            $scope.Product.CategoryID = res.data.CategoryID;
                            if (res.data.MoreImages != null) {
                                $scope.MoreImages = JSON.parse(res.data.MoreImages);
                            }
                        })   
                    }
                }
            });
        }
        getParent();
        getMoreImage();
    }
})(angular.module('shop.product'));