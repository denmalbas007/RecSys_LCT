# Заготовка
## 
### Платформа с элементами геймификации для вовлечения сотрудников

#### FAQ: Как запустить
0. Необходим установленный docker-compose

1. В папке репозитория прописать `docker-compose up -d`
> http://localhost:2002/ - front </br>
> http://localhost:1001/swagger - api swagger docs

Чтобы фронт приложение начало ходить в локальный сервис необходимо в Auth.js заменить
const API_URL = "http://37.230.196.148:1001/v1/"; на const API_URL = "http://localhost:1001/v1/";
и сбилдить npm run build и переместить папку файлы из папки dist в папку web на уровне выше

#### Стек
*Backend* - ASP.NET Core + PostgresSQL </br>
*Frontend* - React

#### Прототип: http://37.230.196.148:2002/



