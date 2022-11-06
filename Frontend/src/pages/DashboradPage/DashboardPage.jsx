import ProjectCard from "../../components/cards/ProjectCard/ProjectCard";
import cl from "./DashboardPage.module.scss";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPlus } from "@fortawesome/free-solid-svg-icons";
import TopNavbar from "../../components/navbars/TopNavbar/TopNavbar";
import { SkeletonCardList } from "../../components/loading/SkeletonCardList";
import { useState, useEffect, useContext } from "react";
import { doCreateProject, doGetProjectsByIds } from "../../api/Auth";
import { ReportCard } from "../../components/cards/ReportCard/ReportCard";
import CreateProjectDialog from "../../components/dialogs/CreateProjectDialog/CreateProjectDialog";
import { AuthContext } from "../../api/AuthContext";

const DashboardPage = ({ page }) => {
  const [projectsCopy, setProjectsCopy] = useState([]);
  const [loading, setLoading] = useState(true);
  const [popupShown, setPopupShown] = useState(false);
  const [createLoading, setCreateLoading] = useState(false);
  const [createError, setCreateError] = useState("");
  const { projects, updateProjects } = useContext(AuthContext);

  const createProject = async (title) => {
    setCreateLoading(true);
    const response = await doCreateProject(title);
    if (response.success) {
      setPopupShown(false);
      setCreateLoading(false);
      updateProjects();
    } else {
      setCreateError(response.errorMessage);
      setCreateLoading(false);
    }
  };

  useEffect(() => {
    updateProjects()
      .then(() => {
        setLoading(false);
      })
      .catch((error) => {
        console.log(error);
      });
  }, []);

  useEffect(() => {
    setProjectsCopy(projects);
  }, [projects]);

  return (
    <div className={cl.dashboard_container}>
      {popupShown && (
        <CreateProjectDialog
          disabled={createLoading}
          error={createError}
          onCreate={createProject}
          onCancel={() => {
            setPopupShown(false);
          }}
        />
      )}
      <div className={cl.topnav_container}>
        <TopNavbar pageTitle="Проекты" />
      </div>
      <main className={cl.content}>
        {loading ? (
          <div className={cl.skeleton_container}>
            <SkeletonCardList />
          </div>
        ) : page === "projects" ? (
          <div className={cl.projects_container}>
            <button
              className={cl.new_project}
              onClick={() => setPopupShown(true)}
            >
              <FontAwesomeIcon icon={faPlus} />
              <h2>Создать проект</h2>
            </button>
            {projectsCopy &&
              projectsCopy.map((project) => (
                <ProjectCard
                  key={project.id}
                  id={project.id}
                  name={project.name}
                  filters={project.filter}
                  lastUpdate={project.updatedAt}
                />
              ))}
          </div>
        ) : (
          <div className={cl.reports_container}>
            <div className={cl.reports_pending}>
              <h2>В обработке</h2>
              <ReportCard projectTitle={"Project 1"} pending />
            </div>
            <div className={cl.reports_ready}>
              <h2>Готовые отчеты</h2>
              <ReportCard projectTitle={"Project 1"} />
              <ReportCard projectTitle={"Project 1"} />
              <ReportCard projectTitle={"Project 1"} />
            </div>
          </div>
        )}
      </main>
    </div>
  );
};

export default DashboardPage;
