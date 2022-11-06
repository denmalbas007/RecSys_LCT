import React, { useState, useEffect } from "react";
import { SkeletonPage } from "../components/loading/SkeletonPage";
import { doCheckAuth, doGetUserInfo, doLogout } from "./Auth";

export const AuthContext = React.createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  const logout = () => {
    setUser(null);
    doLogout();
  };

  const onAuthStateChanged = (newUser) => {
    console.log("onAuthStateChanged", newUser);
    setUser(newUser);
    setLoading(false);
  };

  useEffect(() => {
    // setUser({ username: "admin", role: "admin" });
    // setLoading(false);
    // return;
    doCheckAuth().then((success) => {
      if (success) {
        doGetUserInfo().then((response) => {
          onAuthStateChanged(response.user);
          setLoading(false);
        });
      } else {
        setUser(null);
        setLoading(false);
      }
    });
  }, []);

  return (
    <AuthContext.Provider value={{ user, setUser, logout, onAuthStateChanged }}>
      {loading ? <SkeletonPage /> : children}
    </AuthContext.Provider>
  );
};
