# MyWPFApplication for administraition
Проект приложения для администрирования машин подключающийхся по сети

<b>ClientHandler.cs</b> - класс контролллер выполняющий действия отдельно для каждого TCP клиента 

<b>ExeConfig.txt</b> - конфигурация содержащая в каждой линии приложение которое клиент запускает у себя на машине

<b>ServerHandler.cs </b>- класс контроллер реализующий работу с TCP сервером 

<b>IPAdressConfig.txt</b> - конфигурация содержащая в каждой линии  клиента для которого разрешено подключение 

для реализации многопоточности использовался класс Thread
TODO: переписать и добавить логгер через DI Container

![image](https://user-images.githubusercontent.com/61508770/193871187-d9fdae38-7809-4834-b34e-dafc36ec121d.png)

![image](https://user-images.githubusercontent.com/61508770/193871398-48dd3699-f915-4560-81d1-3376bab04568.png)
