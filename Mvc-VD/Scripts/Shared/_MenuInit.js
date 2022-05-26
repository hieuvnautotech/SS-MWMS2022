
var count = 0;
var IconSideMenu = ['fa-tags', 'fa-wrench', 'fa-anchor', 'fa-cogs', 'fa-th', 'fa-database', 'fa-retweet', 'fa-shield', 'fa-suitcase', 'fa-recycle', 'fa-bookmark-o', 'fa-bolt', 'fa-diamond', 'fa-flask'];
var IconNavMenu = [
    ' <img src="../Images/AccountImg/MMS.png" style="width:25px;height:25px" />',
    '<img src="../Images/AccountImg/WMS.png" style="width:25px;height:25px" />',
    '<img src="../Images/AccountImg/CMS.png" style="width:25px;height:25px" />',
    '<img src="../Images/AccountImg/QMS.png" style="width:25px;height:25px" />',
    '<img src="../Images/AccountImg/PMS.png" style="width:25px;height:25px" />',
    '<img src="../Images/AccountImg/SMS.png" style="width:25px;height:25px" />',
    '<img src="../Images/AccountImg/DMS.png" style="width:25px;height:25px" />',
    '<img src="../Images/AccountImg/STANDARD.png" style="width:25px;height:25px" />',
];
function getCookie(cname) {
    let name = cname + "=";
    let decodedCookie = decodeURIComponent(document.cookie);
    let ca = decodedCookie.split(';');
    for (let i = 0; i < ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}
function changeLanguage(language) {

    if (language == 'en') {
        document.cookie = "language=en; path=/";
    } else if (language == 'kr') {
        document.cookie = "language=kr; path=/";
    } else if (language == 'vi') {
        document.cookie = "language=vi; path=/";
    }
    var langActice = getCookie("language");
    //alert(langActice);

    LoadMenu();
    location.reload();

}
$(document).ready(function () {

    var sheet = document.createElement('style');
    var style = document.createElement('style');
    sheet.innerHTML = ".ui-jqgrid .ui-state-hover {border : black;background: rgb(170, 170, 170); color: black; }; ";
    document.body.appendChild(sheet);
    style.innerHTML = ".ui-jqgrid .ui-state-highlight { background: rgb(170, 170, 170); border-color: red; color: black; };";
    document.body.appendChild(style);
    MenuLevel_1 = [];
    MenuLevel_2 = [];
    MenuLevel_3 = [];
    MenuLevel_4 = [];
    SessionDataMenu = "ghghj";
    Session_authName = "";

    //var IconNavMenu = ['fa-cubes', 'fa-university', 'fa-pie-chart', 'fa-cube', 'fa-paper-plane', 'fa-shopping-basket', 'fa-sitemap', 'fa-industry', 'fa-snowflake-o', 'fa-window-restore'];

    var bien_wms = "";
    var url = window.location.pathname;
    LoadMenu(url);

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

function LoadMenu(url) {

    $.ajax({
        url: '/Home/GetSessionMenuData',
        type: 'GET',
        datatype: 'json',
        data: {
            url: url
        },
        success: function (SessionData) {
            //console.log("SessionData", SessionData);
            if (SessionData == null || SessionData == undefined) {
                if (window.location.port == null || window.location.port == "" || window.location.port == undefined) {
                    urlre = window.location.hostname;
                    //location.href = urlre;
                    window.location.assign("/");
                }
                else {
                    urlre = window.location.hostname + ":" + window.location.port;
                    //location.href = urlre;
                    window.location.assign("/");
                }
            }
            else {
                bien_wms = SessionData.wms;
                if (bien_wms == null || bien_wms == undefined) {
                    if (window.location.port == null || window.location.port == "" || window.location.port == undefined) {
                        urlre = window.location.hostname;
                        //location.href = urlre;
                        window.location.assign("/");
                    }
                    else {
                        urlre = window.location.hostname + ":" + window.location.port;
                        //location.href = urlre;
                        window.location.assign("/");
                    }
                } else {

                    //console.log(bien_wms);
                    SessionDataMenu = SessionData;
                    Session_authName = SessionData.authName;
                    document.cookie = SessionDataMenu;
                    DisplayMenu(SessionData.authList, SessionData.authList_cha);

                    $("#userid").val(SessionData.userid);
                    if (SessionData.userid == "root") {
                        $("#uname").val("root");
                    }
                    else {
                        $.get("/ShippingMgt/getadmin" + "?id=" + SessionData.userid, function (data) {
                            if (data != null && data != undefined) {
                                $("#uname").val(data[0].uname);
                            }
                        });
                    }

                    Session_authName = SessionData.userid;
                    $('#username').html(SessionData.authName + "@" + SessionData.userid);

                    if (SessionData.ds_role == 0) { } else {
                        $("#authoryty_readonly").val("read");
                        $.each(SessionData.ds_role, function (key, item) {
                            switch (item.type) {
                                case "column":
                                    var name = "" + item.name_table + "";
                                    var name_column = "" + item.id_button + "";
                                    $(name).jqGrid('hideCol', [name_column]);
                                    break;
                                case "class":
                                    var name_class = "." + item.id_button + "";
                                    $(name_class).addClass("hidden");
                                    break;
                                default:
                                    var element = document.getElementById(item.id_button);
                                    if (element != null && element != '') {
                                        document.getElementById(item.id_button).style.visibility = "hidden";
                                        break;
                                    }
                                    else {
                                        break;
                                    }
                            }
                        });
                    }
                }
            }

        },
        error: function () { },
    });
}

function DisplayMenu(SessionData, authList_cha) {
    //debugger
    var MaxMenuLevel_1 = 0;
    SessionDataMenu = SessionData;

    if (authList_cha == undefined || authList_cha == null) {
        var urlre = "";
        if (window.location.port == null || window.location.port == "" || window.location.port == undefined) {
            urlre = window.location.hostname;
            //location.href = urlre;
            window.location.assign("/");
        }
        else {
            urlre = window.location.hostname + ":" + window.location.port;
            //location.href = urlre;
            window.location.assign("/");

        }

        //location.replace(urlre);
        //window.location = urlre;
        //document.location.href = "/";
    }
    else if (authList_cha.length > 0) {
        authList_cha.forEach(function (item) {
            if (item.mn_cd == "002000000000" || item.mn_cd == "009000000000" || item.mn_cd == "012000000000" || item.mn_cd == "010000000000") {
                count++;
            }
            IntMenuCodeLevel_1 = (item.mn_cd.substr(0, 3) == "000") ? (IntMenuCodeLevel_1 = 0) : (IntMenuCodeLevel_1 = parseInt(item.mn_cd.substr(0, 3)));
            if ((IntMenuCodeLevel_1 > MaxMenuLevel_1) && (IntMenuCodeLevel_1 < 9) || count == 1) {
                MaxMenuLevel_1 = IntMenuCodeLevel_1;
                var DataMenuCodeLevel_1 = item.mn_cd.substr(0, 3);
                MenuLevel_1.push(DataMenuCodeLevel_1);
                var html = "";

                if (count == 1) {

                    html += '<li class="nav-item  menu-detail" data-levelmenu_1="002">';
                    html += '   <h4 class="menu-detail-b" data-focus="WMS">';
                    html += '       <a class="nav-link text-white iconmenu" href="#"">';
                    html += IconNavMenu[1];
                    html += '           <span class="text-white">' + item.mn_nm + '</span>';
                    html += '       </a>';
                    html += '   </h4>';
                    html += '   <div><a id=a_002 ' + ' data-id="002" onclick="showCheckboxes(this)"></a> <div class="popev_n" id="list_002"></div>   </div > ';

                    html += '   <div class="col-md-12 menubar menu-init" data-subfocus="WMS"></div>';
                    html += '</li>';
                    count++;
                } else {
                    html += '<li class="nav-item  menu-detail" data-levelmenu_1="' + DataMenuCodeLevel_1 + '">';
                    html += '   <h4 class="menu-detail-b" data-focus=' + item.mn_nm + '>';
                    html += '       <a class="nav-link text-white iconmenu" href="#"">';
                    html += IconNavMenu[MaxMenuLevel_1 - 1];
                    html += '           <span class="text-white">' + item.mn_nm.toUpperCase() + '</span>';
                    html += '       </a>';
                    html += '   </h4>';
                    html += '   <div><a id=a_' + DataMenuCodeLevel_1 + ' ' + ' data-id="' + DataMenuCodeLevel_1 + '" onclick="showCheckboxes(this)"></a> <div class="popev_n" id="list_' + DataMenuCodeLevel_1 + '"></div>   </div > ';

                    html += '   <div class="col-md-12 menubar menu-init" data-subfocus=' + item.mn_nm + '></div>';
                    html += '</li>';
                }

                $('.nav-menu-item').append(html);
            }
        });
    }
    else {
        var urlre = "";
        if (window.location.port == null || window.location.port == "" || window.location.port == undefined) {
            urlre = window.location.hostname;
            //location.href = urlre;
            window.location.assign("/");
        }
        else {
            urlre = window.location.hostname + ":" + window.location.port;
            //location.href = urlre;
            window.location.assign("/");

        }

        //location.replace(urlre);
        //window.location = urlre;
        //document.location.href = "/";
    }

    // Sub Menu


    $('.menubar').each(function () {
        switch ($(this).data('subfocus')) {

            case "WMS":
                var html = "";
                if (bien_wms == "true") {
                    html += '<ul class="sub-menu">';
                    html += '   <li id="M-WMS" data-levelmenu_1="013">M-WMS<a href="#" id="a_013"'  + ' data-id="013" onclick = "showCheckboxes(this)"  class="text-white"></a> <div class="popev_n" id="list_013" >  </div ></li>';
                    html += '   <li id="FG-WMS" data-levelmenu_1="009">FG-WMS<a href="#" id="a_009"' + ' data-id="009" onclick = "showCheckboxes(this)"  class="text-white"></a><div class="popev_n" id="list_009" >  </div ></li>';
                    html += '   <li id="WIP-WMS" data-levelmenu_1="010">WIP-WMS<a href="#" id="a_010"'  + ' data-id="010" onclick = "showCheckboxes(this)"  class="text-white"></a> <div class="popev_n" id="list_010">  </div ></li>';
                    html += '   <li id="TIMS" data-levelmenu_1="012">TIMS<a href="#" id="a_012"'  + ' data-id="012" onclick = "showCheckboxes(this)"  class="text-white"></a> <div class="popev_n" id="list_012">  </div ></li>';
                    html += '</ul>';
                } else {
                    html += '<ul class="sub-menu">';

                    if (bien_wms.indexOf("002000000000") != -1) {
                        html += '   <li id="M-WMS" data-levelmenu_1="013">M-WMS<a href="#" id="a_013"' + ' data-id="013" onclick = "showCheckboxes(this)"  class="text-white"></a> <div class="popev_n" id="list_013" >  </div ></li>';
                    }
                    if (bien_wms.indexOf("009000000000") != -1) {
                        html += '   <li id="FG-WMS" data-levelmenu_1="009">FG-WMS<a href="#" id="a_009"'  + ' data-id="009" onclick = "showCheckboxes(this)"  class="text-white"></a><div class="popev_n" id="list_009" >  </div ></li>';
                    }
                    if (bien_wms.indexOf("010000000000") != -1) {
                        html += '   <li id="WIP-WMS" data-levelmenu_1="010">WIP-WMS<a href="#" id="a_010"' + ' data-id="010" onclick = "showCheckboxes(this)"  class="text-white"></a> <div class="popev_n" id="list_010">  </div ></li>';
                    }
                    if (bien_wms.indexOf("012000000000") != -1) {
                        html += '   <li id="TIMS" data-levelmenu_1="012">TIMS<a href="#" id="a_012"' + ' data-id="012" onclick = "showCheckboxes(this)"  class="text-white"></a> <div class="popev_n" id="list_012">  </div ></li>';
                    }
                    html += '</ul>';
                }

                $(this).append(html);
                break;
        }
    });

    var DisplayMenuLevel_2 = function (RootMenuLevel) {
        $('.side-menu-item').empty();
        var MaxMenuLevel_2 = 0;
        MenuLevel_2 = [];
        //debugger;
        if (authList_cha == undefined || authList_cha == null) {
            var urlre = "";
            if (window.location.port == null || window.location.port == "" || window.location.port == undefined) {
                urlre = window.location.hostname;
                //location.href = urlre;
                window.location.assign("/");
            }
            else {
                urlre = window.location.hostname + ":" + window.location.port;
                //location.href = urlre;
                window.location.assign("/");

            }

            //location.replace(urlre);
            //window.location = urlre;
            //document.location.href = "/";
        }
        else if (authList_cha.length > 0) {
            authList_cha.forEach(function (item) {
                IntMenuCodeLevel_2 = (item.mn_cd.substr(3, 3) == "000") ? (IntMenuCodeLevel_2 = 0) : (IntMenuCodeLevel_2 = parseInt(item.mn_cd.substr(3, 3)));
                if ((item.mn_cd.substr(0, 3) == RootMenuLevel) && (IntMenuCodeLevel_2 > MaxMenuLevel_2)) {
                    MaxMenuLevel_2 = IntMenuCodeLevel_2;
                    var DataMenuCodeLevel_2 = item.mn_cd.substr(3, 3);
                    MenuLevel_2.push(DataMenuCodeLevel_2);
                    $('.side-menu-item').append('<li class="nav-item has-treeview submenu-detail" data-levelmenu_2="' + DataMenuCodeLevel_2 + '">' +
                        '<a href="#" class="nav-link">' +
                        '<i class="fa ' + IconSideMenu[MaxMenuLevel_2 - 1] + ' text-white"></i>' +
                        '<p class="text-white">' +
                        item.mn_nm +
                        '<i class="right fa fa-angle-left"></i>' +
                        '<small class="label pull-right bg-green"></small>' +
                        '</p>' +
                        '</a>' +
                        '<li');
                }
            });
        }
        else {
            var urlre = "";
            if (window.location.port == null || window.location.port == "" || window.location.port == undefined) {
                urlre = window.location.hostname;
                //location.href = urlre;
                window.location.assign("/");
            }
            else {
                urlre = window.location.hostname + ":" + window.location.port;
                //location.href = urlre;
                window.location.assign("/");

            }

            //location.replace(urlre);
            //window.location = urlre;
            //document.location.href = "/";
        }

    };

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
                    $('[data-levelmenu_2="' + MenuLevel_2[i] + '"] > ul').append('<li class="nav-item" data-mn_cd="' + item.mn_cd + '">' +
                        '<a href="' + item.url_link + '" class="nav-link" id="' + item.mn_cd + '">' +
                        '<i class="fa fa-circle-o nav-icon"></i>' +
                        '<p>' + item.mn_nm + '</p>' + /*'<i class="fa fa-bell" aria-hidden="true" style="color:red"></i>' +*/
                        '</a>' +
                        '</li> ');
                }
            });
        }
    };

    $('.menu-detail').click(function () {
        var RootMenuLevel = $(this).data("levelmenu_1");
        switch (RootMenuLevel) {
            case "002":
                return true;
                break;
            case "012":
                return true;
                break;
            case "009":
                return true;
                break;
            case "010":
                return true;
                break;
            default:
                DisplayMenuLevel_2(RootMenuLevel);
                DisplayMenuLevel_3(RootMenuLevel);
                break;
        }
    });
    $('#M-WMS').click(function () {
        DisplayMenuLevel_2("002");
        DisplayMenuLevel_3("002");
    });
    $('#FG-WMS').click(function () {
        DisplayMenuLevel_2("009");
        DisplayMenuLevel_3("009");
    });
    $('#WIP-WMS').click(function () {
        DisplayMenuLevel_2("010");
        DisplayMenuLevel_3("010");
    });
    $('#TIMS').click(function () {
        DisplayMenuLevel_2("012");
        DisplayMenuLevel_3("012");
    });
    $('.menu-detail-b:eq(0)').addClass('font-weight-bold');

    $('.menu-detail-b').click(function () {
        $('.menu-detail-b').removeClass('font-weight-bold');
        $(this).addClass('font-weight-bold');
        var focus = $(this).data('focus');
        if (focus == "WMS") {
            GetSumNote("010");
        }
        $('.menubar').each(function () {
            if ($(this).data('subfocus') == focus) {
                $('.menubar').removeClass('menu-focus');
                $(this).addClass('menu-focus');
            };
        });

        switch (focus) {
            case "WMS":
                if ($('.sub-menu').css('display') == 'none') {
                    $('.sub-menu').css('display', 'block');
                }
                else {
                    $('.sub-menu').css('display', 'none');
                }
                break;
            default:
                $('.sub-menu').css('display', 'none');
                break;
        }
    });

    DisplayMenuLevel_2("001");
    DisplayMenuLevel_3("001");
    var menu_level_1 = $('#current_menu').data('level_1');
    var menu_level_2 = $('#current_menu').data('level_2');
    var menu_level_3 = $('#current_menu').data('level_3');
    $('#current_menu').html('<h4 class="m-0 text-dark"><b>' + menu_level_3 + '</b></h4>' +
        '<small style="font-size: 14px;"><i class="fa fa-dashboard"></i>' +
        menu_level_1 + ' > ' + menu_level_2 + ' > ' + menu_level_3 +
        '</small>'
    );
    //$('div>nav>ul>li>ul>li>a').has('p:contains("' + menu_level_3 + '")').addClass('text-warning');

    switch (menu_level_1) {
        case "MMS":
            {
                DisplayMenuLevel_2("001");
                DisplayMenuLevel_3("001");
                $('div>nav>ul>li').has('p:contains("' + menu_level_2 + '")').addClass('menu-open');
                $('div>nav>ul>li>ul>li>a').has('p:contains("' + menu_level_3 + '")').each(function () { if (($(this).text() === menu_level_3)) { $(this).addClass('text-warning'); } });
                $('div>ul>li>h4').has('span:contains("MMS")').addClass('font-weight-bold');
                var focus = menu_level_1;
                $('.menubar').each(function () {
                    if ($(this).data('subfocus') == focus) {
                        $('.menubar').removeClass('menu-focus');
                        $(this).addClass('menu-focus');
                    }
                });

                break;
            }
        case "WMS":
            {
                DisplayMenuLevel_2("002");
                DisplayMenuLevel_3("002");

                $('div>nav>ul>li').has('p:contains("' + menu_level_2 + '")').addClass('menu-open');
                $('div>nav>ul>li>ul>li>a').has('p:contains("' + menu_level_3 + '")').each(function () { if ($(this).text() === menu_level_3) { $(this).addClass('text-warning'); } });
                //$('div>nav>ul>li>p').each(function () {
                //    console.log($('div>nav>ul>li>p').text());
                //});
                $('div>ul>li>h4').has('span:contains("WMS")').addClass('font-weight-bold');
                var focus = menu_level_1;
                $('.menubar').each(function () {
                    if ($(this).data('subfocus') == focus) {
                        $('.menubar').removeClass('menu-focus');
                        $(this).addClass('menu-focus');
                    }
                });

                break;
            }
        case "CMS": {
            DisplayMenuLevel_2("003");
            DisplayMenuLevel_3("003");
            $('div>nav>ul>li').has('p:contains("' + menu_level_2 + '")').addClass('menu-open');
            $('div>nav>ul>li>ul>li>a').has('p:contains("' + menu_level_3 + '")').each(function () { if ($(this).text() === menu_level_3) { $(this).addClass('text-warning'); } });
            $('div>ul>li>h4').has('span:contains("WMS")').addClass('font-weight-bold');
            var focus = menu_level_1;
            $('.menubar').each(function () {
                if ($(this).data('subfocus') == focus) {
                    $('.menubar').removeClass('menu-focus');
                    $(this).addClass('menu-focus');
                }
            });
            break;
        }
        case "QMS": {
            DisplayMenuLevel_2("004");
            DisplayMenuLevel_3("004");
            $('div>nav>ul>li').has('p:contains("' + menu_level_2 + '")').addClass('menu-open');
            $('div>nav>ul>li>ul>li>a').has('p:contains("' + menu_level_3 + '")').each(function () { if ($(this).text() === menu_level_3) { $(this).addClass('text-warning'); } });
            $('div>ul>li>h4').has('span:contains("WMS")').addClass('font-weight-bold');
            var focus = menu_level_1;
            $('.menubar').each(function () {
                if ($(this).data('subfocus') == focus) {
                    $('.menubar').removeClass('menu-focus');
                    $(this).addClass('menu-focus');
                }
            });
            break;
        }
        case "PMS":
        case "아웃소싱":
            {
                DisplayMenuLevel_2("005");
                DisplayMenuLevel_3("005");
                $('div>nav>ul>li').has('p:contains("' + menu_level_2 + '")').addClass('menu-open');
                $('div>nav>ul>li>ul>li>a').has('p:contains("' + menu_level_3 + '")').each(function () { if ($(this).text() === menu_level_3) { $(this).addClass('text-warning'); } });
                $('div>ul>li>h4').removeClass('font-weight-bold');
                $('div>ul>li>h4').has('span:contains("PURCHASE")').addClass('font-weight-bold');
                var focus = menu_level_1;
                $('.menubar').each(function () {
                    if ($(this).data('subfocus') == focus) {
                        $('.menubar').removeClass('menu-focus');
                        $(this).addClass('menu-focus');
                    }
                });
                break;
            }
        case "SMS": {
            DisplayMenuLevel_2("006");
            DisplayMenuLevel_3("006");
            $('div>nav>ul>li').has('p:contains("' + menu_level_2 + '")').addClass('menu-open');
            $('div>nav>ul>li>ul>li>a').has('p:contains("' + menu_level_3 + '")').each(function () { if ($(this).text() === menu_level_3) { $(this).addClass('text-warning'); } });
            $('div>ul>li>h4').removeClass('font-weight-bold');
            $('div>ul>li>h4').has('span:contains("SALES")').addClass('font-weight-bold');
            var focus = menu_level_1;
            $('.menubar').each(function () {
                if ($(this).data('subfocus') == focus) {
                    $('.menubar').removeClass('menu-focus');
                    $(this).addClass('menu-focus');
                }
            });
            break;
        }
        case "DMS":
        case "개발관리":
            {
                DisplayMenuLevel_2("007");
                DisplayMenuLevel_3("007");
                $('div>nav>ul>li').has('p:contains("' + menu_level_2 + '")').addClass('menu-open');
                $('div>nav>ul>li>ul>li>a').has('p:contains("' + menu_level_3 + '")').each(function () { if ($(this).text() === menu_level_3) { $(this).addClass('text-warning'); } });
                $('div>ul>li>h4').removeClass('font-weight-bold');
                $('div>ul>li>h4').has('span:contains("DEVELOPMENT")').addClass('font-weight-bold');
                var focus = menu_level_1;
                $('.menubar').each(function () {
                    if ($(this).data('subfocus') == focus) {
                        $('.menubar').removeClass('menu-focus');
                        $(this).addClass('menu-focus');
                    }
                });
                break;
            }
        case "STANDARD":
        case "표준":
            {
                DisplayMenuLevel_2("008");
                DisplayMenuLevel_3("008");
                $('div>nav>ul>li').has('p:contains("' + menu_level_2 + '")').addClass('menu-open');
                $('div>nav>ul>li>ul>li>a').has('p:contains("' + menu_level_3 + '")').each(function () { if ($(this).text() === menu_level_3) { $(this).addClass('text-warning'); } });
                $('div>ul>li>h4').removeClass('font-weight-bold');
                $('div>ul>li>h4').has('span:contains("STANDARD")').addClass('font-weight-bold');
                var focus = menu_level_1;
                $('.menubar').each(function () {
                    if ($(this).data('subfocus') == focus) {
                        $('.menubar').removeClass('menu-focus');
                        $(this).addClass('menu-focus');
                    }
                });
                break;
            }
        case "FG-WMS": {
            DisplayMenuLevel_2("009");
            DisplayMenuLevel_3("009");
            $('div>nav>ul>li').has('p:contains("' + menu_level_2 + '")').addClass('menu-open');
            $('div>nav>ul>li>ul>li>a').has('p:contains("' + menu_level_3 + '")').each(function () { if ($(this).text() === menu_level_3) { $(this).addClass('text-warning'); } });
            $('div>ul>li>h4').removeClass('font-weight-bold');
            $('div>ul>li>h4').has('span:contains("FG-WMS")').addClass('font-weight-bold');
            var focus = menu_level_1;
            $('.menubar').each(function () {
                if ($(this).data('subfocus') == focus) {
                    $('.menubar').removeClass('menu-focus');
                    $(this).addClass('menu-focus');
                }
            });
            break;
        }
        case "WIP-WMS": {
            DisplayMenuLevel_2("010");
            DisplayMenuLevel_3("010");
            $('div>nav>ul>li').has('p:contains("' + menu_level_2 + '")').addClass('menu-open');
            $('div>nav>ul>li>ul>li>a').has('p:contains("' + menu_level_3 + '")').each(function () { if ($(this).text() === menu_level_3) { $(this).addClass('text-warning'); } });
            $('div>ul>li>h4').removeClass('font-weight-bold');
            $('div>ul>li>h4').has('span:contains("WIP-WMS")').addClass('font-weight-bold');
            var focus = menu_level_1;
            $('.menubar').each(function () {
                if ($(this).data('subfocus') == focus) {
                    $('.menubar').removeClass('menu-focus');
                    $(this).addClass('menu-focus');
                }
            });
            break;
        }
        case "TIMS": {
            DisplayMenuLevel_2("012");
            DisplayMenuLevel_3("012");
            $('div>nav>ul>li').has('p:contains("' + menu_level_2 + '")').addClass('menu-open');
            $('div>nav>ul>li>ul>li>a').has('p:contains("' + menu_level_3 + '")').each(function () { if ($(this).text() === menu_level_3) { $(this).addClass('text-warning'); } });
            $('div>ul>li>h4').has('span:contains("TIMS")').addClass('font-weight-bold');
            var focus = menu_level_1;
            $('.menubar').each(function () {
                if ($(this).data('subfocus') == focus) {
                    $('.menubar').removeClass('menu-focus');
                    $(this).addClass('menu-focus');
                }
            });
            break;
        }
    }
};

$("#dialogAlert").dialog({
    width: 400,
    height: 100,
    maxWidth: 400,
    maxHeight: 200,
    minWidth: 350,
    minHeight: 200,
    resizable: false,
    fluid: true,
    modal: true,
    autoOpen: false,
    title: getTitleAlert,
    classes: {
        "ui-dialog": "ui-dialog",
        "ui-dialog-titlebar": "ui-dialog ui-dialog-titlebar-sm",
        "ui-dialog-titlebar-close": "visibility: hidden",
    },
    resize: function (event, ui) {
        $('.ui-dialog-content').addClass('m-0 p-0');
    },
    open: function (event, ui) {
    },
});

$("#closeAlert").click(function () {
    $('#dialogAlert').dialog('close');
});
function SuccessAlert(content) {
    toastr.success(content, 'Success');
    toastr.options = {
        "closeButton": false,
        "newestOnTop": true,
        "progressBar": true,
        "positionClass": "toast-bottom-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "1000",
        "hideDuration": "1000",
        "timeOut": "3000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };
}

function ErrorAlert(content) {
    toastr.error(content, 'Error');
    toastr.options = {
        "closeButton": false,
        "newestOnTop": true,
        "progressBar": true,
        "positionClass": "toast-bottom-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "1000",
        "hideDuration": "1000",
        "timeOut": "3000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };
}

function getTitleAlert() {
    var title = $('#titleAlert').val();
    return title;
}
function GetSumNote(e) {
    switch (e) {
        case "010":
            get_Link_note_010(e);
            break;
        //case "013":
        //    get_Link_note_008(e);
        //    break;
        //case "009":
        //    get_Link_note_009(e);
        //    break;
        //case "012":
        //    get_Link_note_012(e);
        //    break;
        default:
            return;
    }
}
//var GetSumNote = (e) => {
//switch (e) {
//    case "010":
//        get_Link_note_010(e);
//        break;
//    case "013":
//        get_Link_note_008(e);
//        break;
//    //case "009":
//    //    get_Link_note_009(e);
//    //    break;
//    //case "012":
//    //    get_Link_note_012(e);
//    //    break;
//    default:
//        return;
//}
//}
function get_Link_note_010(e) {
    //alert(e);
    $.get("/Home/GetNumNote?mn_cd=" + e, function (response) {
        if (response.result) {
            var html = '';
            html = response.data;
            $("#a_" + e).html(html);
            var element = document.getElementById("a_" + e);
            if (e == "010" && element != null) {
                element.classList.add("noti-number-wip");
            }
        }
        else {
            $("#a_" + e).html("");
            var element = document.getElementById("a_" + e);
            if (e == "010" && element != null) {
                element.classList.remove("noti-number-wip");
            }
        }
    });
    return "";
}

function get_Link_note_008(e) {
    //alert(e);
    $.get("/Home/GetNumNote?mn_cd=" + e, function (response) {
        if (response.result) {
            var html = '';
            html = response.data;
            $("#a_" + e).html(html);

            var element = document.getElementById("a_" + e);
            if (e == "013" && element != null) {
                element.classList.add("noti-number-wms");
            }
        }
        else {
            $("#a_" + e).html("");
            var element = document.getElementById("a_" + e);
            if (e == "013" && element != null) {
                element.classList.remove("noti-number-wms");
            }
        }
    });
    return "";
}
function get_Link_note_009(e) {
    //alert(e);
    $.get("/Home/GetNumNote?mn_cd=" + e, function (response) {
        if (response.result) {
            var html = '';
            html = response.data;
            $("#a_" + e).html(html);

            var element = document.getElementById("a_" + e);
            if (e == "009" && element != null) {
                element.classList.add("noti-number-wms");
            }
        }
        else {
            $("#a_" + e).html("");
            var element = document.getElementById("a_" + e);
            if (e == "009" && element != null) {
                element.classList.remove("noti-number-wms");
            }
        }
    });
    return "";
}
function get_Link_note_012(e) {
    //alert(e);
    $.get("/Home/GetNumNote?mn_cd=" + e, function (response) {
        if (response.result) {
            var html = '';
            html = response.data;
            $("#a_" + e).html(html);

            var element = document.getElementById("a_" + e);
            if (e == "012" && element != null) {
                element.classList.add("noti-number-wms");
            }
        }
        else {
            $("#a_" + e).html("");
            var element = document.getElementById("a_" + e);
            if (e == "012" && element != null) {
                element.classList.remove("noti-number-wms");
            }
        }
    });
    return "";
}

//function PrintNGBellRing()
//{
//    $.get(`/ActualWO/CountingMaterialNG`, function (response)
//    {
//        if (response > 0)
//        {
//            if ($(`#ngBell`).length)
//            {
//                $(`#ngBell`).remove();
//            }
//            $(`#001007002000`).append(`<i class="fa fa-bell" id="ngBell" aria-hidden="true" style="color:red"></i>`);
//        }
//        else
//        {
//            if ($(`#ngBell`).length)
//            {
//                $(`#ngBell`).remove();
//            }
//        }
//    });

//    $.get(`/TIMS/CountingTIMSMaterialNG`, function (response)
//    {
//        if (response > 0)
//        {
//            if ($(`#ngBellTIMS`).length)
//            {
//                $(`#ngBellTIMS`).remove();
//            }
//            $(`#012002003000`).append(`<i class="fa fa-bell" id="ngBellTIMS" aria-hidden="true" style="color:red"></i>`);
//        }
//        else
//        {
//            if ($(`#ngBellTIMS`).length)
//            {
//                $(`#ngBellTIMS`).remove();
//            }
//        }
//    });

//    $.get(`/fgwms/CountingReceivingFG`, function (response)
//    {
//        if (parseInt(response) > 0)
//        {
//            if ($(`#ngBellFG`).length)
//            {
//                $(`#ngBellFG`).remove();
//            }
//            $(`#009002001000`).append(`<i class="fa fa-bell" id="ngBellFG" aria-hidden="true" style="color:red"></i>`);
//        }
//        else
//        {
//            if ($(`#ngBellFG`).length)
//            {
//                $(`#ngBellFG`).remove();
//            }
//        }
//    });
//}

var expanded = false;
function showCheckboxes(e) {
    var id = $(e).data("id");

    $.get("/Home/GetNumNote_detail?mn_cd=" + id, function (data) {
        var html = '<div class="dropdow_pp fa fa-caret-down"  ">';
        if (data.result) {
            //for (var i = 0; i < data.data.length; i++)
            //{
            //    html += '<div class ="pop_con"><a href="' + data.data[i].link + '?code=' + data.data[i].id + '" class=""><span class="popup"> ' + data.data[i].code + '   </span></a><span class="popup"> ' + data.data[i].name + '  </span></div>';
            //}

            html += ' <table>';
            html += '   <tr>';
            html += '        <th class="popup" >Code</th>';
            html += '        <th class="popup1">Name</th>';
            html += '        </tr>';
            html += '      <tr>';

            for (var i = 0; i < data.data.length; i++) {
                html += '<tr class="tr_menu">';
                html += ' <td class="popup"><a href="' + data.data[i].link + '?code=' + data.data[i].id + '" class="">' + data.data[i].code + '</a></td>';
                html += ' <td class="popup1">' + data.data[i].name + '</td>';
                html += '</tr>';
            }

            html += '   </table>';

            html += '</div>';

            $("#list_" + id).html(html);

            var checkboxes = document.getElementById("list_" + id);
            if (!expanded) {
                checkboxes.style.display = "block";
                checkboxes.style.position = "absolute";

                expanded = true;
            } else {
                checkboxes.style.display = "none";
                expanded = false;
            }
        }
        else {
            $("#list_" + id).html(html);
        }
    });
}
$(document).on('click', function () {
    $('.popev_n').hide();
});

$('#a_002, .popev_n').on('click', function (e) {
    e.stopPropagation();
});
