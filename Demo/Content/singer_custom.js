function initStarRating() {
    if ($('.user_star_rating li').length) {
        var stars = $('.user_star_rating li');
        stars.each(function () {
            var star = $(this);
            star.on('click', function () {
                var i = star.index();
                stars.find('i').each(function () {
                    $(this).removeClass('fa-star');
                    $(this).addClass('fa-star-o');
                });
                for (var x = 0; x <= i; x++) {
                    $(stars[x]).find('i').removeClass('fa-star-o');
                    $(stars[x]).find('i').addClass('fa-star');
                }
                $('#txtRate').val(i + 1); // Setting the rating value to the clicked star index + 1
            });
        });
    }
}

$(document).ready(function () {
    initStarRating();
});
