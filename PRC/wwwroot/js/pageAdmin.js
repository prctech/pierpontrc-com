const NOMENUID = "00000000-0000-0000-0000-000000000000";
var menuId = document.getElementById("MenuId");
var menuList = document.getElementsByName("Menus");
var menuTick = document.getElementsByName("SubPages");
var menusGroup = document.querySelector(".menusGroup");

menuList[0].addEventListener("change", setMenuId);
menuTick[0].addEventListener("change", toggleMenuList);

function setMenuId() {
    menuId.value = menuList[0].value;
}

function toggleMenuList() {
    if (menuTick[0].checked) {
        menusGroup.classList.remove('invisible');
        if(menuId != NOMENUID)
            menuList[0].value = menuId.value;

    } else {
        menusGroup.classList.add('invisible');
        menuId.value = NOMENUID;
    }
}

toggleMenuList();