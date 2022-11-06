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
    const projects = await doGetProjectsByIds(user.layoutIds);
    setProjects(projects);
  };

  const updateReports = async () => {
    const reports = await doGetReportsByIds(user.reportIds);
    setReports(reports);
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
      }}
    >
      {loading ? <SkeletonPage /> : children}
    </AuthContext.Provider>
  );
};
