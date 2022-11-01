import ProjectCard from "../../components/cards/ProjectCard/ProjectCard";
import cl from "./DashboardPage.module.scss";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPlus } from "@fortawesome/free-solid-svg-icons";
import TopNavbar from "../../components/navbars/TopNavbar/TopNavbar";
import SkeletonCardList from "../../components/loading/SkeletonCardList/SkeletonCardList";
import { useState } from "react";
import { useEffect } from "react";
import { doGetProjects } from "../../api/Auth";

const DashboardPage = () => {
  const [projects, setProjects] = useState();
  const [loading, setLoading] = useState(true);

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
      <div className={cl.topnav_container}>
        <TopNavbar pageTitle="Проекты" />
      </div>
      <main className={cl.content}>
        {loading ? (
          <SkeletonCardList />
        ) : (
          <div className={cl.projects_container}>
            <div className={cl.new_project}>
              <FontAwesomeIcon icon={faPlus} />
              <h2>Создать проект</h2>
            </div>
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
        )}
      </main>
    </div>
  );
};

export default DashboardPage;
