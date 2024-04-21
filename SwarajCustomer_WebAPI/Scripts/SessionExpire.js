$(document).ajaxError(
       function (e, xhr, settings) {
           //alert('ajaxError: ' + xhr.status);
           if (xhr.status == 403)
               window.location.href = gbl_login_url;
           else if (xhr.status == 401)
               window.location.href = gbl_session_expire_url;
           else if (xhr.status == 402)
               window.location.href = gbl_unauthorized_url;
       }
   );
$(document).ajaxComplete(
    function (event, xhr, settings) {
        //alert('ajaxComplete: ' + xhr.status);
        if (xhr.status == 403)
            window.location.href = gbl_login_url;
        else if (xhr.status == 401)
            window.location.href = gbl_session_expire_url;
        else if (xhr.status == 402)
            window.location.href = gbl_unauthorized_url;
    }
);