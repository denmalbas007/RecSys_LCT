import axios from "axios";

const API_URL = "http://37.230.196.148:1001/v1/";

const getHeaders = () => {
  return {
    Authorization: `Bearer ${localStorage.getItem("jwt")}`,
  };
};

export const doFetchItemsRoot = async () => {
  const url = API_URL + "filters/item-types/root";
  const response = await axios.get(url);

  return response.data;
};

export const doFetchItemsById = async (id) => {
  const url = API_URL + "filters/item-types/" + id;
  const response = await axios.get(url);

  return response.data.itemTypes;
};

export const doSignIn = async (username, password) => {
  const url = API_URL + "auth/authenticate";

  const response = await axios.post(url, {
    Login: username,
    Password: password,
  });

  if (response.status === 200) {
    localStorage.setItem("jwt", response.data.jwtToken);
    // localStorage.setItem("refreshToken", response.data.refreshToken);
    return {
      success: true,
      errorMessage: "",
    };
  }
  return {
    success: false,
    errorMessage: "Неверный логин или пароль",
  };
};

export const doCheckAuth = async () => {
  const url = API_URL + "users/profile";
  const token = localStorage.getItem("jwt");
  if (token == "undefined" || !token) {
    return false;
  }

  try {
    console.log(getHeaders());
    const response = await axios.get(url, {
      headers: getHeaders(),
    });

    if (response.status === 200) {
      return true;
    } else {
      locaclStorage.removeItem("jwt");
      return false;
    }
  } catch (e) {
    localStorage.removeItem("jwt");
    return false;
  }
};

export const doGetUserInfo = async () => {
  const url = API_URL + "users/profile";
  const response = await axios.get(url, {
    headers: getHeaders(),
  });

  return response.data;
};

//   await new Promise((resolve) => setTimeout(resolve, 1500));
//   if (localStorage.getItem("jwtToken")) {
//     return {
//       success: true,
//       user: {
//         id: 0,
//         username: "usernametest",
//         firstName: "Joe",
//         middleName: "Real",
//         lastName: "Mama",
//         email: "test@test.com",
//         profilePicUrl: "string",
//         reportIds: [0],
//         layoutIds: [0],
//       },
//     };
//   } else {
//     return {
//       success: false,
//     };
//   }
// };

export const doLogout = () => {
  localStorage.removeItem("jwtToken");
  localStorage.removeItem("refreshToken");
};

export const doGetProjects = async () => {
  await new Promise((resolve) => setTimeout(resolve, 1500));

  const fetchedData = {
    layouts: [
      {
        id: 0,
        name: "Проект 1",
        filters: [
          {
            name: "Направление",
            values: ["Экспорт"],
          },
          {
            name: "Регионы",
            values: ["Москва", "Тверская обл."],
          },
          {
            name: "Страны",
            values: ["США", "Китай", "Япония"],
          },
          {
            name: "Продукты",
            values: ["Мясо", "Рыба", "Овощи", "Фрукты"],
          },
        ],
        lastUpdatedAt: "2022-11-01T14:32:53.448Z",
        createdAt: "2022-11-01T14:32:53.448Z",
      },
      {
        id: 1,
        name: "Проект 2",
        filters: [
          {
            name: "Направление",
            values: ["Импорт"],
          },
          {
            name: "Регионы",
            values: ["Екатеринбург", "Санкт-Петербург"],
          },
          {
            name: "Страны",
            values: ["Мексика", "Швеция", "Германия"],
          },
          {
            name: "Продукты",
            values: ["Овощи", "Рыба", "Мясо", "Молочные продукты"],
          },
        ],
        lastUpdatedAt: "2022-11-01T14:32:53.448Z",
        createdAt: "2022-11-01T14:32:53.448Z",
      },
    ],
  };

  const reformatDate = (date) =>
    new Date(date).toLocaleString("ru", {
      day: "numeric",
      month: "long",
      hour: "numeric",
      minute: "numeric",
    });

  return {
    success: true,
    projects: fetchedData.layouts.map((layout) => ({
      ...layout,
      lastUpdatedAt: reformatDate(layout.lastUpdatedAt),
      createdAt: reformatDate(layout.createdAt),
    })),
  };
};

export const doGetReports = async () => {
  await new Promise((resolve) => setTimeout(resolve, 1500));

  const fetchedData = {
    reports: [
      {
        id: 0,
        name: "Отчет 1",
        excelUrl: "google.com",
        pdfUrl: "google.com",
        createdAt: "2022-11-01T14:32:53.448Z",
        status: "loading",
      },
      {
        id: 1,
        name: "Отчет 1",
        excelUrl: "google.com",
        pdfUrl: "google.com",
        createdAt: "2022-11-01T14:32:53.448Z",
        status: "ready",
      },
    ],
  };

  const reformatDate = (date) =>
    new Date(date).toLocaleString("ru", {
      day: "numeric",
      month: "long",
      hour: "numeric",
      minute: "numeric",
    });

  return {
    success: true,
    reports: fetchedData.reports.map((report) => ({
      ...report,
    })),
  };
};
