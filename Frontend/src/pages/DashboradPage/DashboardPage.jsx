import ProjectCard from "../../components/cards/ProjectCard/ProjectCard";
import cl from "./DashboardPage.module.scss";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPlus } from "@fortawesome/free-solid-svg-icons";
import TopNavbar from "../../components/navbars/TopNavbar/TopNavbar";
import { SkeletonCardList } from "../../components/loading/SkeletonCardList";
import { useState, useEffect, useContext } from "react";
import { doCreateProject } from "../../api/Auth";
import { ReportCard } from "../../components/cards/ReportCard/ReportCard";
import CreateProjectDialog from "../../components/dialogs/CreateProjectDialog/CreateProjectDialog";
import { AuthContext } from "../../api/AuthContext";
import { useNavigate } from "react-router-dom";

const DashboardPage = ({ page }) => {
  const [projectsCopy, setProjectsCopy] = useState([]);
  const [reportsCopy, setReportsCopy] = useState([]);
  const [loading, setLoading] = useState(true);
  const [createPopupShown, setCreatePopupShown] = useState(false);
  const [createLoading, setCreateLoading] = useState(false);
  const [createError, setCreateError] = useState("");
  const navigate = useNavigate();
  const { projects, reports, updateProjects, updateReports } =
    useContext(AuthContext);

  const createProject = async (title) => {
    setCreateLoading(true);
    const response = await doCreateProject(title);
    if (response.success) {
      setCreatePopupShown(false);
      updateProjects();
      setTimeout(() => {
        navigate(`/dashboard`);
      }, 1000);
    } else {
      setCreateError(response.errorMessage);
    }
    setCreateLoading(false);
  };

  useEffect(() => {
    updateProjects()
      .then(() => {
        setLoading(false);
      })
      .catch((error) => {
        console.log(error);
      });
    updateReports();
  }, []);

  useEffect(() => {
    setProjectsCopy(projects);
  }, [projects]);

  useEffect(() => {
    setReportsCopy(reports);
  }, [reports]);

  return (
    <div className={cl.dashboard_container}>
      {createPopupShown && (
        <CreateProjectDialog
          disabled={createLoading}
          error={createError}
          onCreate={createProject}
          onCancel={() => {
            setCreatePopupShown(false);
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
              onClick={() => setCreatePopupShown(true)}
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
              <h2>
                В обработке
                <span className={cl.helper_text}>
                  (автообновление отключено)
                </span>
              </h2>
              <div className={cl.items}>
                {reportsCopy &&
                  reportsCopy
                    .filter((report) => !report.isReady)
                    .map((report) => (
                      <ReportCard
                        key={report.id}
                        id={report.id}
                        name={report.name}
                        createDate={report.createdAt}
                        pending
                      />
                    ))}
              </div>
            </div>
            <div className={cl.reports_ready}>
              <h2>Готовые отчеты</h2>
              <div className={cl.items}>
                {reportsCopy &&
                  reportsCopy
                    .filter((report) => report.isReady)
                    .map((report) => (
                      <ReportCard
                        key={report.id}
                        id={report.id}
                        name={report.name}
                        createDate={report.createdAt}
                        pdfUrl={report.pdfUrl}
                        excelUrl={report.excelUrl}
                      />
                    ))}
              </div>
            </div>
          </div>
        )}
      </main>
    </div>
  );
};

export default DashboardPage;
