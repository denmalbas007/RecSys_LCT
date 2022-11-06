import axios from "axios";

const API_URL = "http://37.230.196.148:1001/v1/";

/**
 * Done
 */
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

export const doLogout = () => {
  localStorage.removeItem("jwt");
};

export const doCreateProject = async (title) => {
  const url = API_URL + "layouts";
  const response = await axios.post(
    url,
    {
      filter: {
        regions: [],
        itemTypes: [],
        countries: [],
      },
      name: title,
    },
    {
      headers: getHeaders(),
    }
  );
  if (response.status === 200) {
    return {
      success: true,
      errorMessage: "",
      id: response.data.id,
    };
  }
  return {
    success: false,
    errorMessage: "Ошибка при создании проекта",
  };
};
/**
 * In progress
 */
export const doGetProjectsByIds = async (projectIds) => {
  const url = API_URL + "layouts/by-ids";
  if (!projectIds) {
    return [];
  }
  const projects = await axios.post(
    url,
    { Ids: [...projectIds] },
    {
      headers: getHeaders(),
    }
  );

  return projects.data.layouts;
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
/**
 * To do
 */

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
