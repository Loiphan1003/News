// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// tinymce.init({
//     selector: 'textarea#editor',
//     skin: 'bootstrap',
//     plugins: 'lists, link, image, media',
//     toolbar: 'h1 h2 bold italic strikethrough blockquote bullist numlist backcolor | link image media | removeformat help',
//     menubar: false,
// });

ClassicEditor
    .create(document.querySelector('#editor'))
    .catch(error => {
        console.error(error);
    });

$('.owl-carousel').owlCarousel({
    loop: true,
    margin: 24,
    nav: false,
    responsive: {
        0: {
            items: 1
        },
        1100: {
            items: 1
        },
        1200: {
            items: 4
        }
    }
})

var owl = $('.owl-carousel');
owl.owlCarousel();
// Go to the next item
$('.customNextBtn').click(function () {
    owl.trigger('next.owl.carousel');
})
// Go to the previous item
$('.customPrevBtn').click(function () {
    owl.trigger('prev.owl.carousel', [300]);
})