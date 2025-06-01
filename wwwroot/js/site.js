// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var principal = new Principal();


var user = new Usuario();
var imageUser = (evt) => {
    user.archivo(evt, "imageUser");
}
var imagePerfil = (evt) => {
    user.archivo(evt, "imagePerfil");
}
$().ready(() => {
    let URLactual = window.location.pathname;
    principal.userLink(URLactual);
});