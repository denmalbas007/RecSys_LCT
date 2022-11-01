import React, { useState, useEffect } from "react";
import { SkeletonPage } from "../components/loading/SkeletonPage";
import { doCheckAuth, doLogout, doSignIn } from "./Auth";

export const AuthContext = React.createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  const logout = () => {
    setUser(null);
    doLogout();
  };

  const onAuthStateChanged = (newUser) => {
    console.log("triggered");
    setUser(newUser);
    console.log(user);
    setLoading(false);
  };

  useEffect(() => {
    doCheckAuth().then((response) => {
      if (response.success) {
        setUser(response.user);
      } else {
        setUser(null);
      }
      setLoading(false);
    });
  }, []);

  return (
    <AuthContext.Provider value={{ user, setUser, logout, onAuthStateChanged }}>
      {loading ? <SkeletonPage /> : children}
    </AuthContext.Provider>
  );
};
