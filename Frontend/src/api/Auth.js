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
  // const token = localStorage.getItem("refreshToken");

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
};

export const doLogout = () => {
  localStorage.removeItem("jwtToken");
  localStorage.removeItem("refreshToken");
};

export const getDashboardData = async () => {
  await new Promise((resolve) => setTimeout(resolve, 1500));

  const fetchedData = {
    id: 0,
    name: "Проект 1",
    filter: {
      regions: ["Москва", "Санкт-Петербург"],
      itemTypes: ["Мясо", "Рыба", "Овощи", "Фрукты"],
      countries: ["США", "Китай", "Япония"],
    },
    lastUpdatedAt: "2022-11-01T14:32:53.448Z",
    createdAt: "2022-11-01T14:32:53.448Z",
  };

  const reformatDate = (date) =>
    new Date(date).toLocaleString("ru", {
      day: "numeric",
      month: "long",
      hour: "numeric",
      minute: "numeric",
    });

  fetchedData.createdAt = reformatDate(fetchedData.createdAt);
  fetchedData.lastUpdatedAt = reformatDate(fetchedData.lastUpdatedAt);
  console.log(dateInRussian);
};
