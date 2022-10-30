export const doSignIn = (email, password) => {
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

export const doCheckAuth = (token) => {
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
