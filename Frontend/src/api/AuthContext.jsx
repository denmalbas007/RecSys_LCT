import React, { useState, useEffect } from "react";
import { SkeletonPage } from "../components/loading/SkeletonPage";
import {
  doCheckAuth,
  doGetProjectsByIds,
  doGetReportsByIds,
  doGetUserInfo,
  doLogout,
} from "./Auth";

export const AuthContext = React.createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [projects, setProjects] = useState([]);
  const [reports, setReports] = useState([]);
  const [loading, setLoading] = useState(true);

  const logout = () => {
    setUser(null);
    doLogout();
  };

  const onAuthStateChanged = (newUser) => {
    setUser(newUser);
    setLoading(false);
  };

  const updateProjects = async () => {
    const newProjects = await doGetProjectsByIds(user.layoutIds);
    setProjects(newProjects);
  };

  const updateReports = async () => {
    const reports = await doGetReportsByIds(user.reportIds);
    setReports(reports);
  };

  // чтобы не обновлять всего пользователя, мы локально будем хранить ID проектов
  // они все равно обновляются на сервере
  const addLayoutId = (id) => {
    const newLayoutIds = [...user.layoutIds, id];
    const newUser = { ...user, layoutIds: newLayoutIds };
    setUser(newUser);
  };

  const addReportId = (id) => {
    const newReportIds = [...user.reportIds, id];
    const newUser = { ...user, reportIds: newReportIds };
    setUser(newUser);
  };

  useEffect(() => {
    // setUser({ projects: [] });
    // setLoading(false);
    // return;
    doCheckAuth().then((success) => {
      if (success) {
        doGetUserInfo().then((response) => {
          onAuthStateChanged(response.user);
          setLoading(false);
        });
        doGetProjectsByIds().then((response) => {
          setProjects(response.projects);
        });
      } else {
        setUser(null);
        setLoading(false);
      }
    });
  }, []);

  return (
    <AuthContext.Provider
      value={{
        user,
        projects,
        reports,
        updateProjects,
        updateReports,
        onAuthStateChanged,
        logout,
        addLayoutId,
        addReportId,
      }}
    >
      {loading ? <SkeletonPage /> : children}
    </AuthContext.Provider>
  );
};
