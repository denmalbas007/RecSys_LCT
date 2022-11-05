import ProjectCard from "../../components/cards/ProjectCard/ProjectCard";
import cl from "./DashboardPage.module.scss";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPlus, faHourglass } from "@fortawesome/free-solid-svg-icons";
import TopNavbar from "../../components/navbars/TopNavbar/TopNavbar";
import { SkeletonCardList } from "../../components/loading/SkeletonCardList";
import { useState, useEffect } from "react";
import { doGetProjects } from "../../api/Auth";
import { ReportCard } from "../../components/cards/ReportCard/ReportCard";
import CreateProjectDialog from "../../components/dialogs/CreateProjectDialog/CreateProjectDialog";

const DashboardPage = ({ page }) => {
  const [projects, setProjects] = useState();
  const [loading, setLoading] = useState(true);
  const [popupShown, setPopupShown] = useState(false);

  useEffect(() => {
    doGetProjects()
      .then((response) => {
        console.log(response);
        setProjects(response.projects);
        setLoading(false);
      })
      .catch((error) => {
        console.log(error);
      });
  }, []);

  return (
    <div className={cl.dashboard_container}>
      {popupShown && (
        <CreateProjectDialog
          onCreate={() => {
            setPopupShown(false);
          }}
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
            {projects.map((project) => (
              <ProjectCard
                key={project.id}
                id={project.id}
                name={project.name}
                filters={project.filters}
                lastUpdate={project.lastUpdatedAt}
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
