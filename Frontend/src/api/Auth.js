export const doSignIn = (email, password) => {
  return {
    error: "Неверная почта или пароль",
    success: true,
    data: {
      token: "1234567890",
      user: {
        id: 1,
        name: "Joe Mama",
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
        name: "Joe Mama",
        email: "test@test.com",
      },
    },
  };
};
