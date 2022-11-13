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
  try {
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
  } catch {
    return {
      success: false,
      errorMessage: "Возможно сервер недоступен",
    };
  }
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
};

export const doFetchCountries = async () => {
  const url = API_URL + "filters/countries";
  const response = await axios.get(url);

  return response.data.countries;
};

export const doFetchRegions = async () => {
  const url = API_URL + "filters/regions";
  const response = await axios.get(url);

  return response.data.regions;
};

export const doUpdateProject = async (project) => {
  const url = API_URL + "layouts";
  const response = await axios.put(
    url,
    {
      layout: {
        ...project,
      },
    },
    {
      headers: getHeaders(),
    }
  );
  if (response.status === 200) {
    return {
      success: true,
      errorMessage: "",
    };
  }
  return {
    success: false,
    errorMessage: "Ошибка при обновлении проекта",
  };
};

export const doGetTable = async (filters) => {
  const url = API_URL + "customs/by-filter";
  const response = await axios.post(
    url,
    {
      filter: {
        ...filters,
      },
      pagination: {
        offset: 0,
        count: 30,
      },
    },
    {
      headers: getHeaders(),
    }
  );

  return response.data.customsElements;
};

export const doCreateReport = async (name, filters) => {
  const url = API_URL + "reports";

  const response = await axios.post(
    url,
    {
      filter: {
        ...filters,
      },
      name: name,
    },
    {
      headers: getHeaders(),
    }
  );

  console.log(response);
};

export const doGetReportsByIds = async (reportIds) => {
  const url = API_URL + "reports/by-ids";
  if (!reportIds) {
    return [];
  }
  const reports = await axios.post(
    url,
    { ids: [...reportIds] },
    {
      headers: getHeaders(),
    }
  );
  const reformatDate = (date) =>
    new Date(date).toLocaleString("ru", {
      day: "numeric",
      month: "short",
      hour: "numeric",
      minute: "numeric",
    });

  return reports.data.reportsMetas.map((report) => ({
    ...report,
    createdAt: reformatDate(report.createdAt),
  }));
};

/**
 * In progress
 */

export const doDownloadPDF = async (link) => {
  const url = API_URL + link;
  const response = await axios.get(url, {
    headers: getHeaders(),
    responseType: "blob",
  });

  const blob = new Blob([response.data], { type: "application/pdf" });
  const linkPDF = document.createElement("a");
  linkPDF.href = window.URL.createObjectURL(blob);
  linkPDF.download = "report.pdf";
  linkPDF.click();
};

export const doDownloadExcel = async (link) => {
  const url = API_URL + link;
  const response = await axios.get(url, {
    headers: getHeaders(),
    responseType: "blob",
  });

  const blob = new Blob([response.data], { type: "application/vnd.ms-excel" });
  const linkPDF = document.createElement("a");
  linkPDF.href = window.URL.createObjectURL(blob);
  linkPDF.download = "report.xlsx";
  linkPDF.click();
};

/**
 * To do
 */
