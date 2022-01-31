/**
 * @license Copyright (c) 2003-2015, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
    // config.uiColor = '#AADC6E';

    //Cau hinh 1 so cai ve ket noi voi Ckfinder
    config.syntaxhighlight_lang = 'csharp';
    config.syntaxhighlight_hideControls = true;
    config.language = 'vi';
    config.filebrowserBrowseUrl = '/assets/admin/js/plugins/ckfinder/ckfinder.html';
    config.filebrowserImageBrowseUrl = '/assets/admin/js/plugins/ckfinder.html?Type=Images';
    config.filebrowserFlashBrowseUrl = '/assets/admin/js/plugins/ckfinder.html?Type=Flash';
    config.filebrowserUploadUrl = '/assets/admin/js/plugins/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files';
    config.filebrowserImageUploadUrl = '/Data';
    config.filebrowserFlashUploadUrl = '/assets/admin/js/plugins/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash';


    //tro tu ckeditor sang ckfinder
    CKFinder.setupCKEditor(null, '/assets/admin/js/plugins/ckfinder/');
};
