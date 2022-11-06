import React, { useState, useEffect } from "react";
import { SkeletonPage } from "../components/loading/SkeletonPage";
import {
  doCheckAuth,
  doGetProjectsByIds,
  doGetUserInfo,
  doLogout,
} from "./Auth";

export const AuthContext = React.createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [projects, setProjects] = useState([]);
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
      value={{ user, projects, updateProjects, onAuthStateChanged, logout }}
    >
      {loading ? <SkeletonPage /> : children}
    </AuthContext.Provider>
  );
};
