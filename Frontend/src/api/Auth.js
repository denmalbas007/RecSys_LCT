export const doSignIn = async (email, password) => {
  await new Promise((resolve) => setTimeout(resolve, 1500));
  localStorage.setItem("token", "12345678");

  return {
    success: true,
    data: {
      token: "1234567890",
      user: {
        id: 1,
        firstName: "Joe",
        lastName: "Mama",
        email: "test@test.com",
      },
    },
  };
};

export const doCheckAuth = async () => {
  await new Promise((resolve) => setTimeout(resolve, 1500));
  // const token = localStorage.getItem("token");

  return {
    success: true,
    data: {
      token: "1234567890",
      user: {
        id: 1,
        firstName: "Joe",
        lastName: "Mama",
        email: "test@test.com",
      },
    },
  };
};

export const doLogout = () => {
  localStorage.setItem("token", null);
};
