﻿
class Principal {
    userLink(URLactual) {
        let url = "";
        let cadena = URLactual.split("/");
        for (var i = 0; i < cadena.length; i++) {
            if (cadena[i] != "Index") {
                url += cadena[i];
            }
        }
        switch (url) {
            case "UsuarioAddUsuario":
                document.getElementById('files').addEventListener('change', imageUser, false);
                break;
            case "PerfilPerfil":
                document.getElementById('files').addEventListener('change', imagePerfil, false);
                break;
        }
    }
}
