import React, { useState, useEffect } from "react";
import SkeletonPage from "../components/loading/SkeletonPage/SkeletonPage";
import { doCheckAuth, doLogout, doSignIn } from "./Auth";

export const AuthContext = React.createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  const logout = () => {
    setUser(null);
    doLogout();
  };

  useEffect(() => {
    const token = localStorage.getItem("token");
    doCheckAuth(token).then((response) => {
      if (response.success) {
        setUser(response.data.user);
      } else {
        setUser(null);
      }
      setLoading(false);
    });
  }, []);

  return (
    <AuthContext.Provider value={{ user, logout }}>
      {loading ? <SkeletonPage /> : children}
    </AuthContext.Provider>
  );
};
