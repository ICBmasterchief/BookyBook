
INSTRUCCIONES:

 1. Abrir la aplicación:
     - USANDO DOCKER 
         a. Instala Docker, ábrelo e inicia sesión.

         b. Descarga la imagen de Docker del repositorio con el siguiente comando en la terminal:
            docker pull icbmasterchief/bookybook-app:latest
        
         c. Crear y ejecutar el contenedor de la imagen que acabamos de descargar con el siguiente comando en la terminal:
            docker run -it -p 7790:7790 -e MACHINE_NAME=Docker icbmasterchief/bookybook-app
              
     - DEBUGEANDO LOCALMENTE DESDE EL PROPIO PROYECTO DESCARGADO
         a. Abre la terminal y ve a la carpeta "BookyBook.Presentation" del proyecto.
            ejemplo: cd "C:\Descargas\BookyBook\BookyBook.Presentation" 

         b. Ejecuta la aplicación con este comando:
            dotnet run

 2. Moverse por la aplicación:
     - Usa las flechas del teclado para navegar por los menús

     - Pulsa Enter para aceptar.

 3. Explicación de los menús:
     - "Log In": Iniciar sesión con un usuario existente.

     - "Sign Up": Crea un nuevo usuario.

     - "Search for books": Busca libros por título o autor.

     - "Show Library": Muestra todos los libros de la biblioteca.

     - "Borrow a book": Pide prestado un libro de la biblioteca.

     - "Return a book": Devuelve un libro que hayas cogido prestado de la biblioteca.

     - "Donate a book": Dona un nuevo libro a la biblioteca.

     - "My Account": Aquí puedes ver los datos de tu cuenta de usuario, como por ejemplo las multas 
       por no haber devuelto un lirbo a tiempo. Además aparecerá un nuevo menú con estas opciones:
         - "Pay penalty fee": Pagar tus multas.

         - "Current borrowed books": Los libros que tienes prestados actualmente.

     - "<- Back to menu": Esta opción aparecerá en varios apartados de la aplicación y sirve para
       regresar al menú principal. 
    
     - "Exit": Salir de la aplicación.
