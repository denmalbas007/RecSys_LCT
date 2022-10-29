import ProjectCard from "../../components/cards/ProjectCard/ProjectCard";
import cl from "./DashboardPage.module.scss";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPlus } from "@fortawesome/free-solid-svg-icons";
import { Link } from "react-router-dom";

const projectsExample = {
  id: 1,
  name: "Project 1",
  created_at: "2022-10-28",
  updated_at: "2022-10-29",
  filters: [
    {
      name: "Filter Filter Filter",
    },
    {
      name: "Filter 1",
    },
    {
      name: "Filter 1",
    },
    {
      name: "Filter 1",
    },
    {
      name: "Filter 1",
    },
  ],
  items: [
    {
      name: "Item 1",
    },
    {
      name: "Item 2",
    },
    {
      name: "Item 2",
    },
    {
      name: "Item 2",
    },
    {
      name: "Item 2",
    },
  ],
};

const DashboardPage = () => {
  return (
    <div className={cl.dashboard_container}>
      <nav>navbar</nav>
      <main className={cl.content}>
        <button className={cl.project_create}>
          <FontAwesomeIcon icon={faPlus} />
          <h2>Создать проект</h2>
        </button>
        <div className={cl.projects_container}>
          <ProjectCard project={projectsExample} />
          <ProjectCard project={projectsExample} />
          <ProjectCard project={projectsExample} />
          <ProjectCard project={projectsExample} />
          <ProjectCard project={projectsExample} />

          {/* {projectsExample.map((project) => (
            <ProjectCard project={project} />
          ))} */}
        </div>
      </main>
    </div>
  );
};

export default DashboardPage;
