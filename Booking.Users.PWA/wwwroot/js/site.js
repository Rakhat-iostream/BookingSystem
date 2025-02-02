﻿$(document).ready(function () {
    if ("serviceWorker" in navigator) {
        window.addEventListener("load", function () {
            navigator.serviceWorker
                .register("js/serviceWorker.js")
                .then(res => console.log("service worker registered"))
                .catch(err => console.log("service worker not registered", err))
        })
    }
    var trigger = $('.hamburger'),
        overlay = $('.overlay'),
        isClosed = false;
    trigger.click(function () {
        hamburger_cross();
    });

    function hamburger_cross() {

        if (isClosed == true) {
            $('#sidebar-wrapper').css('left', '0px');
            overlay.hide();
            trigger.removeClass('is-open');
            trigger.addClass('is-closed');
            isClosed = false;
        } else {
            $('#sidebar-wrapper').css('left', '220px');
            overlay.show();
            trigger.removeClass('is-closed');
            trigger.addClass('is-open');
            isClosed = true;
        }
    }

    $('[data-toggle="offcanvas"]').click(function () {
        $('#wrapper').toggleClass('toggled');
    });
});