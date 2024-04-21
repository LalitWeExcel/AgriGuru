

$(".list-group-item").click(function () {
    var listItems = $(".list-group-item");
    for (let i = 0; i < listItems.length; i++) {
        listItems[i].classList.remove("active");
    }
    this.classList.add("active");
});
$(".list-group-BookAstrologers").click(function () {
   
    var listItems = $(".list-group-BookAstrologers");
    for (let i = 0; i < listItems.length; i++) {
        listItems[i].classList.remove("active");
    }
    this.classList.add("active");
});
$(".list-group-package").click(function () {

    var listItems = $(".list-group-package");
    for (let i = 0; i < listItems.length; i++) {
        listItems[i].classList.remove("active");
    }
    this.classList.add("active");
});


