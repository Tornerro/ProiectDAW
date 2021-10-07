# ProiectDAW
Site de tip Task Management(ex. Trello) facut cu ASP.NET folosind EntityFramework pe arhitectura MVC.

Features:

  -Tipuri de utilizatori: Admin, Organizator, Membru si Utilizator neinregistrat/nelogat.
  
  -Adminul are acces la toate informatiile si poate edita/sterge conturi, proiecte, taskuri si comentarii.
  
  -Un membru care creeaza un task devin Organizator si poate adauga membri la propriile proiecte, poate creea taskuri la propriile proiecte si asigna membrii echipei la orice task.

  -Membrii unui task pot lasa comentarii si au un buton pentru a indica ca acel task a fost finalizat. Dupa apasarea butonului data rezolvarii taskului va aparea in locul butonului.
  
  -Un organizator poate sterge proiectele proprii si poate edita/sterge orice informatii din proiectele sale.
  
  -Utilizatorii neinregistrati nu au acces decat la pagina principala(Front-End is WIP).
