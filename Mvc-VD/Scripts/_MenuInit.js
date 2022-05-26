$(document).ready(function () {
    var MenuLevel_1 = [];
    var MenuLevel_2 = [];
    var MenuLevel_3 = [];
    var MenuLevel_4 = [];
    var SessionDataMenu = "ghghj";
    var IconSideMenu = ['fa-tags', 'fa-wrench', 'fa-anchor', 'fa-cogs', 'fa-th', 'fa-database', 'fa-retweet', 'fa-shield', 'fa-suitcase', 'fa-recycle'];
    var IconNavMenu = ['fa-cubes', 'fa-university', 'fa-pie-chart', 'fa-cube', 'fa-paper-plane', 'fa-shopping-basket', 'fa-sitemap', 'fa-industry', 'fa-snowflake-o', 'fa-window-restore'];

    $.ajax({
        url: '/Home/GetSessionMenuData',
        type: 'GET',
        datatype: 'json',
        data: {},
        success: function (SessionData) {
            SessionDataMenu = SessionData;
            document.cookie = SessionDataMenu;
            DisplayMenu(SessionData.authList);
            $('#username').html(SessionData.authName + "@" + SessionData.userid);
        },
        error: function () { },
    });
    function DisplayMenu(SessionData) {
        var MaxMenuLevel_1 = 0;
        SessionDataMenu = SessionData;
        if (SessionDataMenu.length > 0) {
            SessionDataMenu.forEach(function (item) {
                IntMenuCodeLevel_1 = (item.mn_cd.substr(0, 3) == "000") ? (IntMenuCodeLevel_1 = 0) : (IntMenuCodeLevel_1 = parseInt(item.mn_cd.substr(0, 3)));
                if (IntMenuCodeLevel_1 > MaxMenuLevel_1) {
                    MaxMenuLevel_1 = IntMenuCodeLevel_1;
                    var DataMenuCodeLevel_1 = item.mn_cd.substr(0, 3);
                    MenuLevel_1.push(DataMenuCodeLevel_1);
                    $('.nav-menu-item').append('<li class="nav-item  menu-detail" data-levelmenu_1="' + DataMenuCodeLevel_1 + '">' +
                        '<h4 class="menu-detail-b"><a class="nav-link" href="#""><i class="nav-icon fa ' +
                        IconNavMenu[MaxMenuLevel_1 - 1] +
                        ' text-white pr-2"></i><span class="text-white">' +
                        item.mn_nm.toUpperCase() + '</span></a></h4>' +
                        '</li>');
                }
            });
        }
        else {
            alert("mất menu");
        }
        var DisplayMenuLevel_2 = function (RootMenuLevel) {
            $('.side-menu-item').empty();
            var MaxMenuLevel_2 = 0;
            MenuLevel_2 = [];
            SessionDataMenu.forEach(function (item) {
                IntMenuCodeLevel_2 = (item.mn_cd.substr(3, 3) == "000") ? (IntMenuCodeLevel_2 = 0) : (IntMenuCodeLevel_2 = parseInt(item.mn_cd.substr(3, 3)));
                if ((item.mn_cd.substr(0, 3) == RootMenuLevel) && (IntMenuCodeLevel_2 > MaxMenuLevel_2)) {
                    MaxMenuLevel_2 = IntMenuCodeLevel_2;
                    var DataMenuCodeLevel_2 = item.mn_cd.substr(3, 3);
                    MenuLevel_2.push(DataMenuCodeLevel_2);
                    $('.side-menu-item').append('<li class="nav-item has-treeview submenu-detail" data-levelmenu_2="' + DataMenuCodeLevel_2 + '">' +
                                                '<a href="#" class="nav-link">' +
                                                    '<i class="fa ' + IconSideMenu[MaxMenuLevel_2 - 1] + ' text-success"></i>' +
                                                    '<p class="text-success">' +
                                                        item.mn_nm +
                                                    '<i class="right fa fa-angle-left"></i>' +
                                                    '<small class="label pull-right bg-green"></small>' +
                                                    '</p>' +
                                                '</a>' +
                                            '<li');
                }

            });

        }

        var DisplayMenuLevel_3 = function (RootMenuLevel) {
            LengthMenuLevel_2 = MenuLevel_2.length;
            for (i = 0; i <= LengthMenuLevel_2 - 1; i++) {
                var MaxMenuLevel_3 = 0;
                MenuLevel_3 = [];
                $('[data-levelmenu_2="' + MenuLevel_2[i] + '"]').append('<ul class="nav nav-treeview"></ul>');
                SessionDataMenu.forEach(function (item) {
                    IntMenuCodeLevel_3 = (item.mn_cd.substr(6, 3) == "000") ? (IntMenuCodeLevel_3 = 0) : (IntMenuCodeLevel_3 = parseInt(item.mn_cd.substr(6, 3)));

                    if ((item.mn_cd.substr(0, 3) == RootMenuLevel) && (item.mn_cd.substr(3, 3) == MenuLevel_2[i]) && (IntMenuCodeLevel_3 > MaxMenuLevel_3)) {
                        MaxMenuLevel_3 = IntMenuCodeLevel_3;
                        var DataMenuCodeLevel_3 = item.mn_cd.substr(6, 3);
                        MenuLevel_3.push(DataMenuCodeLevel_3);
                        $('[data-levelmenu_2="' + MenuLevel_2[i] + '"] > ul').append('<li class="nav-item">' +
                                                                                            '<a href="' + item.url_link + '" class="nav-link">' +
                                                                                                '<i class="fa fa-circle-o nav-icon"></i>' +
                                                                                                '<p>' + item.mn_nm + '</p>' +
                                                                                            '</a>' +
                                                                                        '</li> ');
                    }
                });
            }
        }


        DisplayMenuLevel_2("001");
        DisplayMenuLevel_3("001");

        $('.menu-detail').click(function () {
            var RootMenuLevel = $(this).data("levelmenu_1");
            DisplayMenuLevel_2(RootMenuLevel);
            DisplayMenuLevel_3(RootMenuLevel);


        });
        $('.menu-detail-b:eq(0)').addClass('font-weight-bold');
        $('.menu-detail-b').click(function () {
            $('.menu-detail-b').removeClass('font-weight-bold');
            $(this).addClass('font-weight-bold');
        });

        var menu_level_1 = $('#current_menu').data('level_1');
        var menu_level_2 = $('#current_menu').data('level_2');
        var menu_level_3 = $('#current_menu').data('level_3');
        $('#current_menu').html('<h4 class="m-0 text-dark"><b>' + menu_level_3 + '</b></h4>' +
                                    '<small style="font-size: 14px;"><i class="fa fa-dashboard"></i>' +
                                     menu_level_1 + ' > ' + menu_level_2 + ' > ' + menu_level_3 +
                                    '</small>'
                                );
        $('div>nav>ul>li').has('p:contains("' + menu_level_2 + '")').addClass('menu-open');
        $('div>nav>ul>li>ul>li>a').has('p:contains("' + menu_level_3 + '")').addClass('text-warning');

        switch (menu_level_1) {
            case "MMS": {
                DisplayMenuLevel_2("001");
                DisplayMenuLevel_3("001");
                $('div>nav>ul>li').has('p:contains("' + menu_level_2 + '")').addClass('menu-open');
                $('div>nav>ul>li>ul>li>a').has('p:contains("' + menu_level_3 + '")').addClass('text-warning');
                $('div>ul>li>h4').has('span:contains("MMS")').addClass('font-weight-bold');
                break;
            }
            case "WMS": {
                DisplayMenuLevel_2("002");
                DisplayMenuLevel_3("002");
                $('div>nav>ul>li').has('p:contains("' + menu_level_2 + '")').addClass('menu-open');
                $('div>nav>ul>li>ul>li>a').has('p:contains("' + menu_level_3 + '")').addClass('text-warning');
                $('div>ul>li>h4').has('span:contains("WMS")').addClass('font-weight-bold');
                break;
            }
            case "PURCHASE": {
                DisplayMenuLevel_2("003");
                DisplayMenuLevel_3("003");
                $('div>nav>ul>li').has('p:contains("' + menu_level_2 + '")').addClass('menu-open');
                $('div>nav>ul>li>ul>li>a').has('p:contains("' + menu_level_3 + '")').addClass('text-warning');
                $('div>ul>li>h4').removeClass('font-weight-bold');
                $('div>ul>li>h4').has('span:contains("PURCHASE")').addClass('font-weight-bold');
                break;
            }
            case "SALES": {
                DisplayMenuLevel_2("004");
                DisplayMenuLevel_3("004");
                $('div>nav>ul>li').has('p:contains("' + menu_level_2 + '")').addClass('menu-open');
                $('div>nav>ul>li>ul>li>a').has('p:contains("' + menu_level_3 + '")').addClass('text-warning');
                $('div>ul>li>h4').removeClass('font-weight-bold');
                $('div>ul>li>h4').has('span:contains("SALES")').addClass('font-weight-bold');
                break;
            }
            case "DEVELOPMENT": {
                DisplayMenuLevel_2("005");
                DisplayMenuLevel_3("005");
                $('div>nav>ul>li').has('p:contains("' + menu_level_2 + '")').addClass('menu-open');
                $('div>nav>ul>li>ul>li>a').has('p:contains("' + menu_level_3 + '")').addClass('text-warning');
                $('div>ul>li>h4').removeClass('font-weight-bold');
                $('div>ul>li>h4').has('span:contains("DEVELOPMENT")').addClass('font-weight-bold');
                break;
            }
            case "STANDARD": {
                DisplayMenuLevel_2("006");
                DisplayMenuLevel_3("006");
                $('div>nav>ul>li').has('p:contains("' + menu_level_2 + '")').addClass('menu-open');
                $('div>nav>ul>li>ul>li>a').has('p:contains("' + menu_level_3 + '")').addClass('text-warning');
                $('div>ul>li>h4').removeClass('font-weight-bold');
                $('div>ul>li>h4').has('span:contains("STANDARD")').addClass('font-weight-bold');
                break;
            }

        }
    };

    $('#menu_left_right').click(function () {
        if ($('#menu_left_right > i').hasClass('fa-angle-double-left')) {
            $('#menu_left_right > i').removeClass('fa-angle-double-left');
            $('#menu_left_right > i').addClass('fa-angle-double-right');
        }
        else {
            $('#menu_left_right > i').removeClass('fa-angle-double-right');
            $('#menu_left_right > i').addClass('fa-angle-double-left');
        }
    });

});