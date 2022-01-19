# MeetingManager

![MeetingManager Main](./.images/meeting_manager_main_view.PNG)

## Informacje Generealne:
Projekt realizowany w ramach zaliczenia przedmiotu, wykorzystujący środowisko ASP.NET w wersji 5.0. MeetingManager jest to aplikacja webowa umożliwiająca użytkownikowi 
rezerwację pobytu (oferty) w danej lokalizacji.

## Założenia Projekty:
- [x] Projekt umożliwia rejestrację użytkownika.
- [x] Użytkownik może tworzyć oferty (dodawać swoje lokalizacje na wynajem) jak i 
  przeglądać i rezerwować inne oferty.
- [x] Profil użytkownika przevhowuje wszystkie wymagane dane użytkownika które użytkownik może edytować.
- [x] Użytkownik posiada możliwość usunięcia swojego konta.
- [x] Gotowe rezerwacje można w systemie anulować.
- [x] Przed dokonaniem rezerwacji wybrana przez użytkownika przed zapłaceniem trafia do koszyka (karty).
- [x] Użytkownik informowany jest o rezerwacji ofert drogą meilową, jak również o rejestracji i usunięciu konta.
- [x] Zapytania bazodanowe realizowane są przy pomocy API.
- [x] API aplikacji zabezpieczone jest kluczem.
- [x] Hasła użytkowników nie są hashowane podczas zapisywania w bazie danych zwiększa to bezpieczeństwo użytkwonika.

![MeetingManager Main](./.images/meeting_-manager_user_account.PNG)

## Technologie:
* DOCKER
* ASP.NET 5.0
* JS
* BOOTSTRAP
* MSSQL

## Instrukacja uruchomienia:

* Pierwszym krokiem jest uruchomienie aplikacji Docker.
* W Visual studio w opcjach uruchamiania projekty należy wybrać:

  > docker-compose
  
* Gdy projekt zostanie zbudowany należy przejśc do przeglądarki i wybrać interesujący nas adres: 

  | API  | USER INTERFACE |
  | ------------- | ------------- |
  | localhost:8088/swagger/index.html  | localhost:8001  |
  
* Poniżej znajduje się lista przykładowo przygotowanych przez nas użytkowników:  

    | USERNAME  | PASSWORD |
  | ------------- | ------------- |
  | -  | - |
  
## Dane dostępowe:

* API Key: Pei1deingai
* Hasło bazy danych: Pa55w0rd2021, Login: SA
  
## Autorzy:
* Artur Garlacz
* Maksymilian Jachymczak
