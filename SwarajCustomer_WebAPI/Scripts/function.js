/*Login Page Jquery*/

$(document).ready(function () {

    if ($('.date-picker-textbox').length) {
        $(".date-picker-textbox").datepicker();
    }
    if ($('.isCalender').length) {
        $(".isCalender").datepicker();
    }
    if ($('.isCalender2').length) {
        $(".isCalender2").datepicker({
            changeMonth: true,
            changeYear: true,
            yearRange: "-60:+0"
        });
    }
    var windowWidth = $(window).width();

    /*function for geern icon in textbox*/
    $('.text-box').keypress(function () {
        $(this).addClass('filled');
    });
    /*Login Form Validation*/
    function replaceValidationUI(form) {
        form.addEventListener("invalid", function (event) {
            event.preventDefault();
        }, true);
        form.addEventListener("submit", function (event) {
            if (!this.checkValidity()) {
                event.preventDefault();
            }
        });

        //var submitButton = form.querySelector( "button:not([type=button]), input[type=submit]" );
        //submitButton.addEventListener( "click", function( event ) {
        //	var invalidFields = form.querySelectorAll( ":invalid" ),
        //		errorMessages = form.querySelectorAll( ".error-message" ),
        //		parent;
        //	for ( var i = 0; i < errorMessages.length; i++ ) {
        //		errorMessages[ i ].parentNode.removeChild( errorMessages[ i ] );
        //	}

        //	for ( var i = 0; i < invalidFields.length; i++ ) {
        //		parent = invalidFields[ i ].parentNode;
        //		parent.insertAdjacentHTML( "beforeend", "<div class='error-message'>" + 
        //			invalidFields[ i ].validationMessage +
        //			"</div>" );
        //	}
        //	if ( invalidFields.length > 0 ) {
        //		invalidFields[ 0 ].focus();
        //	}
        //});
    }

    var forms = document.querySelectorAll("form");
    for (var i = 0; i < forms.length; i++) {
        replaceValidationUI(forms[i]);
    }

    /*Dashboard Nav*/
    $(document).on('click', '.toggle-nav', function () {
        //alert('hi');
        $('.side-nav').toggleClass('show-nav');
        if ($('.side-nav').hasClass('show-nav')) {
            $('.side-nav').addClass('show-nav');
            $('#content-area').addClass('push-right');
            setTimeout(function () { $('.menu-text').css('display', 'block'); }, 460);
            setTimeout(function () { $('.plus').css('display', 'block'); }, 460);

        } else {
            $('.side-nav').removeClass('show-nav');
            $('.menu-text').css('display', 'none');
            $('.dropdown-menu').css('display', 'none');
            $('.plus').css('display', 'none');
            $('#content-area').removeClass('push-right', 1000);
        }
    });

    if (windowWidth >= 992) {
        $('#quick-link-toggle-icon').click(function () {
            $('.right-menu').toggleClass('collapseNav');
            $('.slider').toggleClass('expand-content');

            if ($('.right-menu').hasClass('collapseNav')) {
                $('.collapseNav .quick-links-text').css('display', 'none');
                $('.collapseNav h2 span').css('display', 'none');
                $('.inner-left-content .alert-text').css('display', 'none');
            }
            else {
                setTimeout(function () { $('.quick-links-text').css('display', 'block'); }, 460);
                setTimeout(function () { $('.right-menu h2 span').css('display', 'inline-block'); }, 460);
                setTimeout(function () { $('.inner-left-content .alert-text').css('display', 'inline-block'); }, 460);

            }
        });

    }


    if (windowWidth > 320) {
        $('.dropdown-submenu a').click(function (e) {
            e.stopPropagation();
            $(this).next('.dropdown-menu').slideToggle();
            $(this).parent('.dropdown-submenu').toggleClass('active-menu');

        });
        /*$('.show-password').on('click',function(){
		   var getAttr = $('#password').attr('type');
		   if(getAttr == 'password') {
			$('#password').attr('type', 'text');
		   }else{
			$('#password').attr('type', 'password');
		   }
	  });*/

    }

    $('.show-password').mousedown(function () {
        //alert('mousedown');
        $('#password').attr('type', 'text');
        $('.hideImg').css('display', 'none');
        $('.showImg').css('display', 'block');

    });
    $('.show-password').mouseup(function () {
        // alert('mouseup');
        $('#password').attr('type', 'password');
        $('.hideImg').css('display', 'block');
        $('.showImg').css('display', 'none');
    });

    $('.first-level-dd').click(function () {

        $(this).find('.dropdown-menu.first-level').slideToggle();

        $(this).toggleClass('active');
        if ($(this).hasClass('active')) {
            $(this).find('.plus').text('-');
        } else {
            $(this).find('.plus').text('+');
        }
    });

    //Detecting IE browser
    var myNav = navigator.userAgent.toLowerCase();
    var msie = (myNav.indexOf('msie') != -1) ? parseInt(myNav.split('msie')[1]) : false;
    var ua = navigator.userAgent.toString().toLowerCase();
    var match = /(trident)(?:.*rv:([\w.]+))?/.exec(ua) || /(msie) ([\w.]+)/.exec(ua) || ['', null, -1];
    var rv = match[2];
    if (typeof msie == 'number' || rv == '12.0' || rv == '11.0' || rv == '10.0' || rv == '9.0' || rv == '8.0') {
        $('html').addClass('ieView');
    }

    /*Search textbox animation*/
    $('.search-icon').click(function () {
        $(this).prev('.inner-text-box').toggleClass('expand-text-box');
    });

    /*check all*/
    $("#checkAll").click(function () {
        $('.checkbox').not(this).prop('checked', this.checked);
    });





    //Wizard
    $('a[data-toggle="tab"]').on('show.bs.tab', function (e) {

        var $target = $(e.target);

        if ($target.parent().hasClass('disabled')) {
            return false;
        }


    });

    //$(".next-step").click(function (e) {
    //    var $active = $('.wizard .nav-tabs li.active');
    //    $active.next().removeClass('disabled');
    //    $active.next().addClass('done');
    //    nextTab($active);

    //});
    //$(".prev-step").click(function (e) {
    //    var $active = $('.wizard .nav-tabs li.active');
    //    prevTab($active);

    //});



    /*accordion*/
    $('.accordion').click(function () {
        $(this).toggleClass('minus');
        $('.accordion-content').slideToggle();
    });
    AlphaOnly('.alphaOnly');
    NumberOnly('.numberOnly');
    DecimalOnly('.decimalOnly');
    AlphaNumericOnly('.alphaNumericOnly');
    AlphaPlusMinus('.alphaPlusMinus');
    PreventKey('.preventkey');
    SevenDigitWithTwoDeci('.SevenWithTwoDeci');
});
function AlphaNumericOnly(cntxt) {
    $(cntxt).bind('keypress', function (e) {
        var k = e.which;
        //var ok = (k >= 65 && k <= 90) || // A-Z
        //    (k >= 96 && k <= 105) || // a-z
        //    (k >= 35 && k <= 40) || // arrows
        //    k == 8 || // Backspaces
        //    k == 46 || // Delete
        //    k == 32 || // space
        //    (k >= 48 && k <= 57); // 0-9
        //if (!ok) {
        //    e.preventDefault();
        if (k == 8 || k == 0)
            return;
        else {
            var regex = /^[a-zA-Z0-9\-\s]+$/
            var str = String.fromCharCode(!e.charCode ? k : e.charCode);
            if (!regex.test(str))
                e.preventDefault();
        }
    });

    $(cntxt).bind("cut copy paste", function (e) {
        e.preventDefault();
    });
}

function SevenDigitWithTwoDeci(cntxt) {
    $(cntxt).bind('keypress', function (e) {
        var k = e.which;
        if (k == 8 || k == 0)
            return;
        else {
            var regex = /^\d{0,7}(\.\d{0,2})?$/
            var str = String.fromCharCode(!e.charCode ? k : e.charCode);
            if (!regex.test(str))
                e.preventDefault();
        }
    });
}

function AlphaPlusMinus(cntxt) {
    $(cntxt).bind('keypress', function (e) {
        var k = e.which;
        if (k == 8 || k == 0)
            return;
        else {
            var regex = /^[a-zA-Z\+\-\s]+$/
            var str = String.fromCharCode(!e.charCode ? k : e.charCode);
            if (!regex.test(str))
                e.preventDefault();
        }
    });
}
function AlphaOnly(cntxt) {
    $(cntxt).bind('keypress', function (event) {
        //var key = event.which;
        //if (key >= 48 && key <= 57) {
        //    event.preventDefault();
        //}
        var inputValue = event.charCode;
        if (!((inputValue >= 65 && inputValue <= 90) || (inputValue >= 97 && inputValue <= 122)) && (inputValue != 32 && inputValue != 0)) {
            event.preventDefault();
        }
    });
    $(cntxt).bind("cut copy paste", function (e) {
        e.preventDefault();
    });
}
function NumberOnly(cntxt) {
    $(cntxt).bind('keypress', function (event) {
        //if the letter is not digit then display error and don't type anything
        var key = event.which;
        if (key != 8 && key != 0 && key != 9 && (key < 48 || key > 57)) {
            return false;
        }
    });
    $(cntxt).bind("cut copy paste", function (e) {
        e.preventDefault();
    });
}
//function DecimalOnly(cntxt) {
//    $(cntxt).bind('keypress', function (event) {
//        var key = event.which;
//        var key_code = event.keyCode;
//        if ((key != 46 || $(this).val().indexOf('.') != -1) && (key < 48 || key > 57)) {
//            if (key_code !== 8 && key_code !== 46 && key_code !== 9) { //exception
//                event.preventDefault();
//            }
//        }
//        if (($(this).val().indexOf('.') != -1) && ($(this).val().substring($(this).val().indexOf('.'), $(this).val().indexOf('.').length).length > 2)) {
//            if (key_code !== 8 && key_code !== 46 && key_code !== 9) { //exception
//                event.preventDefault();
//            }
//        }
//    });
//    $(cntxt).bind("cut copy paste", function (e) {
//        e.preventDefault();
//    });
//}

function DecimalOnly(cntxt) {
    $(cntxt).bind('keypress', function (event) {
        if ((event.which != 46 || $(this).val().indexOf('.') != -1) &&
      ((event.which < 48 || event.which > 57) &&
        (event.which != 0 && event.which != 8))) {
            event.preventDefault();
        }
        var text = $(this).val();
        if ((text.indexOf('.') != -1) &&
          (text.substring(text.indexOf('.')).length > 2) &&
          (event.which != 0 && event.which != 8) &&
          ($(this)[0].selectionStart >= text.length - 2)) {
            event.preventDefault();
        }
    });
    $(cntxt).bind("cut copy paste", function (e) {
        e.preventDefault();
    });
}

function nextTab(elem) {
    $(elem).next().find('a[data-toggle="tab"]').click();
}
function prevTab(elem) {
    $(elem).prev().find('a[data-toggle="tab"]').click();
    $(elem).removeClass('done');
}

$.fn.regexMask = function (mask) {
    $(this).keypress(function (event) {
        if (!event.charCode) return true;
        var part1 = this.value.substring(0, this.selectionStart);
        var part2 = this.value.substring(this.selectionEnd, this.value.length);
        if (!mask.test(part1 + String.fromCharCode(event.charCode) + part2))
            return false;
    });
};

$(document).ready(function () {
    var mask = new RegExp('^[A-Za-z0-9 ]*$')
    $(".alphaNumericOnly2").regexMask(mask)

   
});

function PreventKey(cntxt) {
    $(cntxt).bind('keypress', function (event) {
        if (!(event.keyCode == 8                                // backspace
      || event.keyCode == 46 || event.keyCode == 9                            // delete & tab
      || (event.keyCode >= 35 && event.keyCode <= 40)     // arrow keys/home/end
      || (event.keyCode >= 48 && event.keyCode <= 57)     // numbers on keyboard
      || (event.keyCode >= 96 && event.keyCode <= 105))   // number on keypad
      ) {
            event.preventDefault();     // Prevent character input
        }
    });
}

function formatDate(date) {
    debugger;
    var d = date.split('-');
    var currentDate = [d[2], d[1], d[0]].join("-");
    return currentDate;
}
