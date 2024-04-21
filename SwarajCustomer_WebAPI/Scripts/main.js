jQuery(document).ready(function( $ ) {
   
   $("#menuToggle").on("click" ,function(){
      $("body").toggleClass("menuToggle"), 500, 'swing';
         
         $(".rightbox").focus(function(){
          $(this).removeClass("menuToggle");
        });
    }); 

});