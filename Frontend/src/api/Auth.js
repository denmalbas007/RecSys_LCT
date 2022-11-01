import axios from "axios";

export const doSignIn = async (username, password) => {
  await new Promise((resolve) => setTimeout(resolve, 1500));

  localStorage.setItem("jwtToken", "98765");
  localStorage.setItem("refreshToken", "472189");

  return {
    success: true,
    errorMessage: "",
  };
};

export const doCheckAuth = async () => {
  await new Promise((resolve) => setTimeout(resolve, 1500));
  if (localStorage.getItem("jwtToken")) {
    return {
      success: true,
      user: {
        id: 0,
        username: "usernametest",
        firstName: "Joe",
        middleName: "Real",
        lastName: "Mama",
        email: "test@test.com",
        profilePicUrl: "string",
        reportIds: [0],
        layoutIds: [0],
      },
    };
  } else {
    return {
      success: false,
    };
  }
};

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
            name: "Регионы",
            values: ["Москва", "Санкт-Петербург"],
          },
          {
            name: "Продукты",
            values: ["Мясо", "Рыба", "Овощи", "Фрукты"],
          },
          {
            name: "Страны",
            values: ["США", "Китай", "Япония"],
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
            name: "Регионы",
            values: ["Екатеринбург", "Санкт-Петербург"],
          },
          {
            name: "Продукты",
            values: ["Овощи", "Рыба", "Мясо", "Молочные продукты"],
          },
          {
            name: "Страны",
            values: ["Мексика", "Швеция", "Германия"],
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
