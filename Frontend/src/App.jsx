import { AuthProvider } from "./api/AuthContext";
import PrivateRoute from "./api/PrivateRoute";
import {
  BrowserRouter as Router,
  Navigate,
  Route,
  Routes,
} from "react-router-dom";
import SignInPage from "./pages/SignInPage/SignInPage";
import DashboardPage from "./pages/DashboradPage/DashboardPage";

function App() {
  document.body.classList.add("theme");
  return (
    <AuthProvider>
      <Router>
        <Routes>
          <Route path="/" element={<Navigate to="/dashboard" />} />
          <Route path="/login" element={<Navigate to="/signin" />} />
          <Route path="/signin" element={<SignInPage />} />
          <Route
            path="/dashboard"
            element={
              <PrivateRoute>
                <DashboardPage />
              </PrivateRoute>
            }
          />
        </Routes>
      </Router>
    </AuthProvider>
  );
}

export default App;
