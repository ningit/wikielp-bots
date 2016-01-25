Bots para la wiki de ELP
========================

Los bots escritos en C# requieren la infraestructura de [DotNetWikiBot](http://dotnetwikibot.sourceforge.net/). Los bots escritos en Python necesitan [Pywikibot](https://www.mediawiki.org/wiki/PWB).

Para ejecutar los bots escritos en Python se necesita que el archivo *user-config.py*, con la información de la wiki y el usuario, esté accesible en el directorio donde se ejecute. El nombre de usuario se puede omitir si no se van a realizar cambios, en caso contrario la contraseña se solicitará por consola. En el repositorio hay un ejemplo de [user-config.py](user-config.py).

El código de los bots está liberado licencia [GPLv3](http://www.gnu.org/licenses/gpl-3.0.txt).

Más información sobre la función y los cambios realizados por los bots en la [wiki de ELP](http://wikis.fdi.ucm.es/ELP/Trabajo:Mejora_de_la_wiki_de_ELP/Bots).


Compilación y ejecución
-----------------------

Para compilar **Categorizador.cs** descarga *DotNetWikiBot* desde su [página de descargas](http://sourceforge.net/projects/dotnetwikibot/files/latest/download). Suponiendo que lo has descomprimido en la raíz del repositorio ejecuta `mcs -reference:DotNetWikiBot.dll Categorizador.cs`{.bash} si usas Mono o `csc /reference:DotNetWikiBot.dll Categorizador.cs`{.bat} si usas el compilador de VisualStudio.

El bot admite dos opciones de línea de comandos `-v` para mostrar más información sobre las operaciones realizadas y `-d` para operar en modo simulación, de forma que los cambios sólo se indican y no se escriben sobre la wiki.

Para ejecutar **enlaces-rotos.py** descarga la versión *core* de *Pywikibot* de su [página de descargas](http://tools.wmflabs.org/pywikibot/). Supuesto que lo has descomprimido sobre la raíz del directorio basta introducir el comando:

```bash
PYTHONPATH=core python3 enlaces-rotos.py
```
