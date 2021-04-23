// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function checkTime(i) {
    if (i < 10) {
        i = "0" + i;
    }
    return i;
}
function startTime() {
    var today = new Date();
    var h = today.getHours() % 12;
    if(h == 0)
        h=12;
    var m = today.getMinutes();
    var s = today.getSeconds();
    var y = today.getFullYear();
    var mo  = today.getMonth() + 1;
    var d = today.getDate().toFixed();
    // add a zero in front of numbers<10
    m = checkTime(m);
    s = checkTime(s);
    document.getElementById('time').innerHTML = h + ":" + m + ":" + s;
    document.getElementById('date').innerHTML = `${mo}/${d}/${y}`;
    t = setTimeout(function () {
        startTime()
    }, 500);
}
startTime();
