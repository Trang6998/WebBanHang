$(document).ready(function ($) {
	awe_backtotop();
	awe_owl();
	awe_category();
	awe_menumobile();
	owl_thumb_image();
	hover_thumb_image();
	count_product();
	setTimeout(function(){
		$('.owl_product_previews').removeClass('timeout');
	},4000);
});
$(window).on("load resize",function(e){	
	setTimeout(function(){					 
		awe_resizeimage();
	},200);
	setTimeout(function(){	
		awe_resizeimage();
	},1000);
});
$(document).on('click','.overlay, .close-popup, .btn-continue, .fancybox-close', function() {   
	awe_hidePopup('.awe-popup'); 	
	setTimeout(function(){
		$('.loading').removeClass('loaded-content');
	},500);
	return false;
})

function awe_resizeimage() { 
	$('.product-box .product-thumbnail a img').each(function(){
		var t1 = (this.naturalHeight/this.naturalWidth);
		var t2 = ($(this).parent().height()/$(this).parent().width());
		if(t1<= t2){
			$(this).addClass('bethua');
		}
		var m1 = $(this).height();
		var m2 = $(this).parent().height();
		if(m1 <= m2){
			$(this).css('padding-top',(m2-m1)/2 + 'px');
		}
	})	
} window.awe_resizeimage=awe_resizeimage;

function callbackW() {
	iWishCheck();				  
	iWishCheckInCollection();
	$(".iWishAdd").click(function () {			
		var iWishvId = iWish$(this).parents('form').find("[name='id']").val();
		if (typeof iWishvId === 'undefined') {
			iWishvId = iWish$(this).parents('form').find("[name='variantId']").val();
		};
		var iWishpId = iWish$(this).attr('data-product');
		if (Bizweb.template == 'collection' || Bizweb.template == 'index') {
			iWishvId = iWish$(this).attr('data-variant');
		}
		if (typeof iWishvId === 'undefined' || typeof iWishpId === 'undefined') {
			return false;
		}
		if (iwish_cid == 0) {
			iWishGotoStoreLogin();
		} else {
			var postObj = {
				actionx : 'add',
				cust: iwish_cid,
				pid: iWishpId,
				vid: iWishvId
			};
			iWish$.post(iWishLink, postObj, function (data) {
				if (iWishFindAndGetVal('#iwish_post_result', data) == undefined) return;
				var result = (iWishFindAndGetVal('#iwish_post_result', data).toString().toLowerCase() === 'true');
				var redirect = parseInt(iWishFindAndGetVal('#iwish_post_redirect', data), 10);
				if (result) {
					if (Bizweb.template == "product") {
						iWish$('.iWishAdd').addClass('iWishHidden'), iWish$('.iWishAdded').removeClass('iWishHidden');
						if (redirect == 2) {
							iWishSubmit(iWishLink, { cust: iwish_cid });
						}
					}
					else if (Bizweb.template == 'collection' || Bizweb.template == 'index') {
						iWish$.each(iWish$('.iWishAdd'), function () {
							var _item = $(this);
							if (_item.attr('data-variant') == iWishvId) {
								_item.addClass('iWishHidden'), _item.parent().find('.iWishAdded').removeClass('iWishHidden');
							}
						});
					}
				}
			}, 'html');
		}
		return false;
	});
	$(".iWishAdded").click(function () {
		var iWishvId = iWish$(this).parents('form').find("[name='id']").val();
		if (typeof iWishvId === 'undefined') {
			iWishvId = iWish$(this).parents('form').find("[name='variantId']").val();
		};
		var iWishpId = iWish$(this).attr('data-product');
		if (Bizweb.template == 'collection' || Bizweb.template == 'index') {
			iWishvId = iWish$(this).attr('data-variant');
		}
		if (typeof iWishvId === 'undefined' || typeof iWishpId === 'undefined') {
			return false;
		}
		if (iwish_cid == 0) {
			iWishGotoStoreLogin();
		} else {
			var postObj = {
				actionx: 'remove',
				cust: iwish_cid,
				pid: iWishpId,
				vid: iWishvId
			};
			iWish$.post(iWishLink, postObj, function (data) {
				if (iWishFindAndGetVal('#iwish_post_result', data) == undefined) return;
				var result = (iWishFindAndGetVal('#iwish_post_result', data).toString().toLowerCase() === 'true');
				var redirect = parseInt(iWishFindAndGetVal('#iwish_post_redirect', data), 10);
				if (result) {
					if (Bizweb.template == "product") {
						iWish$('.iWishAdd').removeClass('iWishHidden'), iWish$('.iWishAdded').addClass('iWishHidden');
					}
					else if (Bizweb.template == 'collection' || Bizweb.template == 'index') {
						iWish$.each(iWish$('.iWishAdd'), function () {
							var _item = $(this);
							if (_item.attr('data-variant') == iWishvId) {
								_item.removeClass('iWishHidden'), _item.parent().find('.iWishAdded').addClass('iWishHidden');
							}
						});
					}
				}
			}, 'html');
		}
		return false;
	});

}  window.callbackW=callbackW;
function awe_currency(str){	
	str = str.replace('$','');
	str = str.replace('.00','');			
	str = str.replace('.','xxx');
	str = str.replace(',','.');
	str = str.replace('xxx',',');									
	return str;
}window.awe_currency=awe_currency;



/*UPdate OfficeWorld*/
$(window).bind('load resize scroll', function () {
	var wDH = $(window).height();
	var wDW = $(window).width();
	var heightSetmenu = $('.list_menu').height();
	$('.ul_content_right_1, .ul_content_right_2').css('min-height', heightSetmenu);
	
});
/*Check so nho hon 1*/

function maxLengthCheck(object) {
	if (object.value.length > object.maxLength)
		object.value = object.value.slice(0, object.maxLength)
		}
function isNumeric (evt) {
	var theEvent = evt || window.event;
	var key = theEvent.keyCode || theEvent.which;
	key = String.fromCharCode (key);
	var regex = /[0-9]|\./;
	if ( !regex.test(key) ) {
		theEvent.returnValue = false;
		if(theEvent.preventDefault) theEvent.preventDefault();
	}
}


$(window).on("load resize",function(e){	
	setTimeout(function(){					 
		var contactheight = $('.info_width .page_cotact').height();
		$('#contact_map').css('min-height', contactheight + 140);
		
	},200);
});




 $(document).ready(function(){


	var wDW = $(window).width();
	/*Footer*/
	if(wDW > 767){
		$('.toggle-mn').show();

	}else {
		$('.footer-widget > .cliked').click(function(){
			$(this).toggleClass('open_');
			$(this).next('ul').slideToggle("fast");
			$(this).next('div').slideToggle("fast");
		});
	}
	if (wDW < 991) {
		$(".filter-group li span label").click(function(){
			$('.dqdt-sidebar').removeClass('openf');
			$('.open-filters').removeClass('openf');
			$('.opacity_filter').removeClass('opacity_filter_true');
		});
		$('.opacity_filter').click(function(e){
			$('.dqdt-sidebar').removeClass('openf');
			$('.open-filters').removeClass('openf');
			$('.opacity_filter').removeClass('opacity_filter_true');
		});
	}
	if (wDW > 992) {
		$(".button_clicked").click(function(){ 
			$('.search_pc').slideToggle('fast');
		})
	}

	/*Click tab danh muc*/
	var $this = $('.tab_link_module');
	$this.find('.head-tabs').first().addClass('active');
	$this.find('.content-tab').first().show();
	$this.find('.head-tabs').on('click',function(){
		if(!$(this).hasClass('active')){
			$this.find('.head-tabs').removeClass('active');
			var $src_tab = $(this).attr("data-src");
			$this.find($src_tab).addClass("active");
			$this.find(".content-tab").hide();
			var $selected_tab = $(this).attr("href");
			$this.find($selected_tab).show();
		}
		return false;
	})
	$(".tab_link_module .button_show_tab").click(function(){ 
		$('.link_tab_check_click').slideToggle('down');
	});
	if (wDW < 992) {
		var title_first = $('.link_tab_check_click li:first-child >a').text();
		$('.title_check_tabs').text(title_first);
		$this.find('.head-tabs').on('click',function(){
			$('.link_tab_check_click').slideToggle('up');
			var title_tabs = $(this).text();
			$('.title_check_tabs').text(title_tabs);
		})
	}

});
/*Show hide Recoverpass*/
$('.recv-text #rcv-pass').click(function(){
	$('.recover').slideToggle('500');
});


/*Show searchbar*/
$('.header_search').on('hover, mouseover', function() {
	$('.st-default-search-input').focus();
});
$('.showsearchfromtop').click(function(event){
	$('.searchfromtop').slideToggle("fast");
	$('.wrap_login_').hide();
});
$('.hidesearchfromtop').click(function(event){
	$('.searchfromtop').slideToggle("up");
});

var wDH = $(window).height();
if (wDH < 1199) {
	$('.use_ico_register').click(function(){
		$('.wrap_login_').slideToggle("fast");
		$('.searchfromtop').hide();
	});
}

/*Repcomment*/

$(".reply").click(function () {
	$(this).closest('div').find('.form-comment-input').focus();
});


/*End*/



/*Hover cart addd class opacity for body*/
// $(window).on("load resize",function(){
// 	if ($(window).width() >= 1200) {

// 		$(".cart_hover")
// 			.mouseover(function() { 
// 			$('.body_opactiy').addClass('opacity');
// 		})
// 			.mouseout(function() {
// 			$('.body_opactiy').removeClass('opacity');
// 		});

// 		$(".user_hover")
// 			.mouseover(function() { 
// 			$('.body_opactiy').addClass('opacity');
// 		})
// 			.mouseout(function() {
// 			$('.body_opactiy').removeClass('opacity');
// 		});

// 		$(".menu_hover")
// 			.mouseover(function() { 
// 			$('.body_opactiy').addClass('opacity');
// 		})
// 			.mouseout(function() {
// 			$('.body_opactiy').removeClass('opacity');
// 		});

// 	};
// });


function owl_thumb_image() { 
	$('.product_image_list').owlCarousel({
		loop:false,
		margin:5,
		responsiveClass:true,
		items: 5,
		dots:false,
		nav:true,
		responsive:{
			0:{
				items:1,
				nav:true
			},
			600:{
				items:3,
				nav:false
			},
			992: {
				items:4,
			},
			1200:{
				items:5,
				nav:true,
				loop:false
			}
		}
	})
} window.owl_thumb_image=owl_thumb_image;

/*dem so san pham hien co tren trang*/
function count_product() {
	var size_col = $('.products-view-grid .row .product-col').size();
	var number_page = $('.paginate_position .pagination .page-item').size();
	if (size_col >= 25 || number_page > 1) {
		$('.sortPagiBar .wr_sort .sortpage').addClass('page_avalible');
	} else {
		$('.sortPagiBar .wr_sort .sortpage').removeClass('page_avalible');
	}
}window.count_product=count_product;




/*hover get image thumb product item grid*/
$(function() {
	$(".owl_product_item_content .saler_item").each(function() {
		var _this = $(this).find('.owl_item_product .product-box');
		var this_thumb = $(_this).find('.product-thumbnail .image_link img');
		var default_this_thumb = $(_this).find('.product-thumbnail .image_link').attr('data-images');
		var this_mini_thumb = $(_this).find('.action_image .owl_image_thumb_item .product_image_list .item_image');
		$(this_mini_thumb)
			.mouseover(function() { 
			var this_s = $(this).attr('data-image');
			this_thumb.attr('src', this_s);
		})
			.mouseout(function() {
			this_thumb.attr('src', default_this_thumb);
		});
	});
});
function hover_thumb_image() {
	$(function() {
		$(".product-col").each(function() {
			var _this = $(this).find('.product-box');
			var this_thumb = $(_this).find('.product-thumbnail .image_link img');
			var default_this_thumb = $(_this).find('.product-thumbnail .image_link').attr('data-images');
			var this_mini_thumb = $(_this).find('.action_image .owl_image_thumb_item .product_image_list .item_image');
			$(this_mini_thumb)
				.mouseover(function() { 
				var this_s = $(this).attr('data-image');
				this_thumb.attr('src', this_s);
			})
				.mouseout(function() {
				this_thumb.attr('src', default_this_thumb);
			});
		});
	});
} window.hover_thumb_image=hover_thumb_image;
/*End*/





/*Open filter*/
$('.open-filters').click(function(e){
	e.stopPropagation();
	$(this).toggleClass('openf');
	$('.opacity_filter').toggleClass('opacity_filter_true');
	$('.dqdt-sidebar').toggleClass('openf');
});

/*Menu mobile*/
$('.menu-bar').click(function(e){
	e.stopPropagation();
	$('.menu_mobile').toggleClass('open_sidebar_menu');
	$('.opacity_menu').toggleClass('open_opacity');
});
$('.opacity_menu').click(function(e){
	$('.menu_mobile').removeClass('open_sidebar_menu');
	$('.opacity_menu').removeClass('open_opacity');
});
$('.ct-mobile li .ti-plus').click(function() {
	$(this).closest('li').find('> .sub-menu').slideToggle("fast");
	$(this).closest('i').toggleClass('show_open hide_close');
	return false;              
}); 

function validate(evt) {
	var theEvent = evt || window.event;
	var key = theEvent.keyCode || theEvent.which;
	key = String.fromCharCode( key );
	var regex = /[0-9]|\./;
	if( !regex.test(key) ) {
		theEvent.returnValue = false;
		if(theEvent.preventDefault) theEvent.preventDefault();
	}
}

/*Double click go to link menu*/

$( '.ul_menu li:has(ul)' ).doubleTapToGo();


/*San pham da xem*/

window.onload = function(e){ 
	
		$("#recent-product").owlCarousel({

			items : 6,
			responsiveClass:true,
			responsive:{
				0:{
					items:1,
					navigationText : false,
					nav:true
				},
				600:{
					items:2,
					navigationText : false,
					nav:true
				},
				768:{
					items:3,
					nav:true,
					navigationText : false,
					loop:false
				},
				992:{
					items:4,
					nav:true,
					navigationText : false,
					loop:false
				},
				1200:{
					items:4,
					nav:true,
					navigationText : false,
					loop:false
				},
				1440:{
					items:6,
					nav:true,
					navigationText : false,
					loop:false
				}
			},
			navigation : true,
			navigationText : false,
			nav: true,
			slideSpeed : 500,
			pagination : false,
			dots: false,
			margin: 0,
		});
		$('#recent-product').addClass('owl-carousel');
}

// OWL slide index
$(document).ready(function() {

  var sync1 = $("#sync1");
  var sync2 = $("#sync2");
  var slidesPerPage = 4; //globaly define number of elements per page
  var syncedSecondary = true;

  sync1.owlCarousel({
    items : 1,
    slideSpeed : 2000,
    nav: true,
    autoplay: true,
    dots: false,
    loop: true,
    responsiveRefreshRate : 200,
  }).on('changed.owl.carousel', syncPosition);

  sync2
    .on('initialized.owl.carousel', function () {
      sync2.find(".owl-item").eq(0).addClass("current");
    })
    .owlCarousel({
    items : slidesPerPage,
    dots: false,
    nav: false,
    smartSpeed: 200,
    slideSpeed : 500,
    slideBy: slidesPerPage, //alternatively you can slide by 1, this way the active slide will stick to the first item in the second carousel
    responsiveRefreshRate : 100,
    responsive:{
        0:{
            items:3
        },
        600:{
            items:5
        },
        1000:{
            items:5
        }
    }
  }).on('changed.owl.carousel', syncPosition2);

  function syncPosition(el) {
    //if you set loop to false, you have to restore this next line
    //var current = el.item.index;
    
    //if you disable loop you have to comment this block
    var count = el.item.count-1;
    var current = Math.round(el.item.index - (el.item.count/2) - .5);
    
    if(current < 0) {
      current = count;
    }
    if(current > count) {
      current = 0;
    }
    
    //end block

    sync2
      .find(".owl-item")
      .removeClass("current")
      .eq(current)
      .addClass("current");
    var onscreen = sync2.find('.owl-item.active').length - 1;
    var start = sync2.find('.owl-item.active').first().index();
    var end = sync2.find('.owl-item.active').last().index();
    
    if (current > end) {
      sync2.data('owl.carousel').to(current, 100, true);
    }
    if (current < start) {
      sync2.data('owl.carousel').to(current - onscreen, 100, true);
    }
  }
  
  function syncPosition2(el) {
    if(syncedSecondary) {
      var number = el.item.index;
      sync1.data('owl.carousel').to(number, 100, true);
    }
  }
  
  sync2.on("click", ".owl-item", function(e){
    e.preventDefault();
    var number = $(this).index();
    sync1.data('owl.carousel').to(number, 300, true);
  });
});


/********************************************************
# SHOW NOITICE
********************************************************/
function awe_showNoitice(selector) {
	$(selector).animate({right: '0'}, 500);
	setTimeout(function() {
		$(selector).animate({right: '-300px'}, 500);
	}, 3500);
}  window.awe_showNoitice=awe_showNoitice;

/********************************************************
# SHOW LOADING
********************************************************/
function awe_showLoading(selector) {
	var loading = $('.loader').html();
	$(selector).addClass("loading").append(loading); 
}  window.awe_showLoading=awe_showLoading;

/********************************************************
# HIDE LOADING
********************************************************/
function awe_hideLoading(selector) {
	$(selector).removeClass("loading"); 
	$(selector + ' .loading-icon').remove();
}  window.awe_hideLoading=awe_hideLoading;

/********************************************************
# SHOW POPUP
********************************************************/
function awe_showPopup(selector) {
	$(selector).addClass('active');
}  window.awe_showPopup=awe_showPopup;

/********************************************************
# HIDE POPUP
********************************************************/
function awe_hidePopup(selector) {
	$(selector).removeClass('active');
}  window.awe_hidePopup=awe_hidePopup;

/********************************************************
# CONVERT VIETNAMESE
********************************************************/
function awe_convertVietnamese(str) { 
	str= str.toLowerCase();
	str= str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g,"a"); 
	str= str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g,"e"); 
	str= str.replace(/ì|í|ị|ỉ|ĩ/g,"i"); 
	str= str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g,"o"); 
	str= str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g,"u"); 
	str= str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g,"y"); 
	str= str.replace(/đ/g,"d"); 
	str= str.replace(/!|@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'| |\"|\&|\#|\[|\]|~|$|_/g,"-");
	str= str.replace(/-+-/g,"-");
	str= str.replace(/^\-+|\-+$/g,""); 
	return str; 
} window.awe_convertVietnamese=awe_convertVietnamese;

/********************************************************
# RESIDE IMAGE PRODUCT BOX
********************************************************/
function awe_resizeimage() { 

} window.awe_resizeimage=awe_resizeimage;

/********************************************************
# SIDEBAR CATEOGRY
********************************************************/
function awe_category(){
	$('.nav-category .fa-angle-down').click(function(e){
		$(this).parent().toggleClass('active');
	});
} window.awe_category=awe_category;

/********************************************************
# MENU MOBILE
********************************************************/
function awe_menumobile(){
	$('.menu-bar').click(function(e){
		e.preventDefault();
		$('#nav').toggleClass('open');
	});
	$('#nav .fa').click(function(e){		
		e.preventDefault();
		$(this).parent().parent().toggleClass('open');
	});
} window.awe_menumobile=awe_menumobile;

/********************************************************
# ACCORDION
********************************************************/
function awe_accordion(){
	$('.accordion .nav-link').click(function(e){
		e.preventDefault;
		$(this).parent().toggleClass('active');
	})
} window.awe_accordion=awe_accordion;

/********************************************************
# OWL CAROUSEL
********************************************************/
function awe_owl() { 
	$('.owl-carousel:not(.not-dqowl)').each( function(){
		var xs_item = $(this).attr('data-xs-items');
		var md_item = $(this).attr('data-md-items');
		var lg_item = $(this).attr('data-lg-items');
		var sm_item = $(this).attr('data-sm-items');	
		var margin=$(this).attr('data-margin');
		var dot=$(this).attr('data-dot');
		var nav=$(this).attr('data-nav');
		var height=$(this).attr('data-height');
		var play=$(this).attr('data-play');
		var loop=$(this).attr('data-loop');
		if (typeof margin !== typeof undefined && margin !== false) {    
		} else{
			margin = 30;
		}
		if (typeof xs_item !== typeof undefined && xs_item !== false) {    
		} else{
			xs_item = 1;
		}
		if (typeof sm_item !== typeof undefined && sm_item !== false) {    

		} else{
			sm_item = 3;
		}	
		if (typeof md_item !== typeof undefined && md_item !== false) {    
		} else{
			md_item = 3;
		}
		if (typeof lg_item !== typeof undefined && lg_item !== false) {    
		} else{
			lg_item = 3;
		}
		if (typeof dot !== typeof undefined && dot !== true) {   
			dot= true;
		} else{
			dot = false;
		}
		$(this).owlCarousel({
			loop:loop,
			margin:Number(margin),
			responsiveClass:true,
			dots:dot,
			nav:nav,
			autoplay:false,
			autoplayTimeout:3000,
			autoplayHoverPause:true,
			autoHeight:height,
			responsive:{
				0:{
					items:Number(xs_item)				
				},
				600:{
					items:Number(sm_item)				
				},
				1000:{
					items:Number(md_item)				
				},
				1200:{
					items:Number(lg_item)				
				}
			}
		})
	})
} window.awe_owl=awe_owl;





/**************************************************
Silick Slider
**************************************************/
/*
$(document).ready(function(){



	setTimeout(function(){
		$('#sale_todays').slick({
			dots: true,
			infinite: false,
			speed: 300,
			slidesPerRow: 3,
			rows: 2,
			adaptiveHeight: true,
			responsive: [
				{
					breakpoint: 1200,
					settings: {
						slidesPerRow: 3,
						rows: 2,

						infinite: true,
						dots: true
					}
				},
				{
					breakpoint: 1024,
					settings: {
						slidesPerRow: 2,
						rows: 3,

						infinite: true,
						dots: true
					}
				},
				{
					breakpoint: 992,
					settings: {
						slidesPerRow: 2,
						rows: 3,

						infinite: true,
						dots: true
					}
				},
				{
					breakpoint: 768,
					settings: {
						slidesPerRow: 2,
						rows: 3,

						infinite: true,
						dots: true
					}
				},
				{
					breakpoint: 767,
					settings: {
						slidesPerRow: 1,
						rows: 1,
					}
				}
			]
		});
		$('#product_gift_box').slick({
			dots: true,
			infinite: false,
			speed: 300,
			slidesPerRow: 2,
			rows: 4,
			adaptiveHeight: true,
			responsive: [
				{
					breakpoint: 1200,
					settings: {
						slidesPerRow: 2,
						rows: 4,

						infinite: true,
						dots: true
					}
				},
				{
					breakpoint: 1024,
					settings: {
						slidesPerRow: 2,
						rows: 4,

						infinite: true,
						dots: true
					}
				},
				{
					breakpoint: 992,
					settings: {
						slidesPerRow: 2,
						rows: 4,

						infinite: true,
						dots: true
					}
				},
				{
					breakpoint: 768,
					settings: {
						slidesPerRow: 2,
						rows: 4,

						infinite: true,
						dots: true
					}
				},
				{
					breakpoint: 767,
					settings: {
						slidesPerRow: 1,
						rows: 4,
					}
				}
			]
		});


		$('#popular_product_slick').slick({
			dots: true,
			infinite: false,
			speed: 300,
			slidesPerRow: 1,
			rows: 4,
			adaptiveHeight: true,
			responsive: [
				{
					breakpoint: 1200,
					settings: {
						slidesPerRow: 1,
						rows: 4,

						infinite: true,
						dots: true
					}
				},
				{
					breakpoint: 1024,
					settings: {
						slidesPerRow: 1,
						rows: 4,

						infinite: true,
						dots: true
					}
				},
				{
					breakpoint: 992,
					settings: {
						slidesPerRow: 1,
						rows: 4,

						infinite: true,
						dots: true
					}
				},
				{
					breakpoint: 768,
					settings: {
						slidesPerRow: 1,
						rows: 4,

						infinite: true,
						dots: true
					}
				},
				{
					breakpoint: 767,
					settings: {
						slidesPerRow: 1,
						rows: 4,
					}
				}
			]
		});

	},500);



	var galleryTop = [];
	var galleryThumbs = [];
	$(".testimonial-thumbs").each(function(index, element){
		var $this = $(this);
		galleryThumbs.push(new Swiper(this, {
			slidesPerView: 5,
			touchRatio: 0.2,
			spaceBetween: 5,
			loop:true,
			centeredSlides: true,
			paginationClickable: true,
			loopedSlides: 5, //looped slides should be the same
			slideToClickedSlide: true,
			autoHeight: true
		}));
	});
	$(".testimonial-content-gallery").each(function(index, element){
		var $this = $(this);
		galleryTop.push(new Swiper(this, {
			loop:true,
			slidesPerView: 1,
			centeredSlides: true,
			loopedSlides: 5,
			pagination: '.swiper-pagination',
			paginationClickable: false,
			autoHeight: true
		}));
	});



	for (var i = 0; i < galleryTop.length; i++) {
		galleryTop[i].params.control = galleryThumbs[i];
		galleryThumbs[i].params.control = galleryTop[i];
	}


});


*/



/********************************************************
# BACKTOTOP
********************************************************/
function awe_backtotop() { 
	/* Back to top */
	if ($('#back-to-top').length) {
		var scrollTrigger = 200, // px
			backToTop = function () {
				var scrollTop = $(window).scrollTop();
				if (scrollTop > scrollTrigger) {
					$('#back-to-top').addClass('show');
				} else {
					$('#back-to-top').removeClass('show');
				}
			};
		backToTop();
		$(window).on('scroll', function () {
			backToTop();
		});
		$('#back-to-top').on('click', function (e) {
			e.preventDefault();
			$('html,body').animate({
				scrollTop: 0
			}, 700);
		});
	}
} window.awe_backtotop=awe_backtotop;

/********************************************************
# TAB
********************************************************/
/********************************************************
# Tab
********************************************************/
$(".e-tabs:not(.not-dqtab)").each( function(){
	$(this).find('.tabs-title li:first-child').addClass('current');
	$(this).find('.tab-content').first().addClass('current');

	$(this).find('.tabs-title li').click(function(){
		var tab_id = $(this).attr('data-tab');

		var url = $(this).attr('data-url');
		$(this).closest('.e-tabs').find('.tab-viewall').attr('href',url);

		$(this).closest('.e-tabs').find('.tabs-title li').removeClass('current');
		$(this).closest('.e-tabs').find('.tab-content').removeClass('current');

		$(this).addClass('current');
		$(this).closest('.e-tabs').find("#"+tab_id).addClass('current');
	});    
});
/*Check so nho hon 1*/
function maxLengthCheck(object) {
	if (object.value.length > object.maxLength)
		object.value = object.value.slice(0, object.maxLength)
		}
function isNumeric (evt) {
	var theEvent = evt || window.event;
	var key = theEvent.keyCode || theEvent.which;
	key = String.fromCharCode (key);
	var regex = /[0-9]|\./;
	if ( !regex.test(key) ) {
		theEvent.returnValue = false;
		if(theEvent.preventDefault) theEvent.preventDefault();
	}
}

/********************************************************
# DROPDOWN
********************************************************/
$('.dropdown-toggle').click(function() {
	$(this).parent().toggleClass('open'); 	
}); 
$('.btn-close').click(function() {
	$(this).parents('.dropdown').toggleClass('open');
}); 
$('body').click(function(event) {
	if (!$(event.target).closest('.dropdown').length) {
		$('.dropdown').removeClass('open');
	};
});

/*JS CHECK SÐT NHẬP TEXT*/
function preventNonNumericalInput(e) {
	e = e || window.event;
	var charCode = (typeof e.which == "undefined") ? e.keyCode : e.which;
	var charStr = String.fromCharCode(charCode);

	if (!charStr.match(/^[0-9]+$/))
		e.preventDefault();
}