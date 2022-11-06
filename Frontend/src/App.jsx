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
import ProjectPage from "./pages/ProjectPage/ProjectPage";

function App() {
  return (
    <AuthProvider>
      <Router>
        <Routes>
          <Route path="/" element={<Navigate to="/projects" />} />
          <Route path="/login" element={<Navigate to="/signin" />} />
          <Route path="/signin" element={<SignInPage />} />
          <Route
            path="/projects"
            element={
              <PrivateRoute>
                <DashboardPage page="projects" />
              </PrivateRoute>
            }
          />
          <Route
            path="/reports"
            element={
              <PrivateRoute>
                <DashboardPage page="reports" />
              </PrivateRoute>
            }
          />
          <Route
            path="/project/:id"
            element={
              <PrivateRoute>
                <ProjectPage />
              </PrivateRoute>
            }
          />
          <Route path="*" element={<Navigate to="/projects" />} />
        </Routes>
      </Router>
    </AuthProvider>
  );
}

export default App;
